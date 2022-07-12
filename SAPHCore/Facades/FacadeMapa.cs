using System;
using System.Threading;
using System.Threading.Tasks;
using SAMU192Core.BLL;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;

namespace SAMU192Core.Facades
{
    public static class FacadeMapa
    {
        static IMapa mapa;
        static ThreadSafeList<EnderecoDTO> Cache = new ThreadSafeList<EnderecoDTO>();
        static bool aguardandoCoordenada = false;
        static bool aguardandoEndereco = false;

        public static void Carregar(IMapa _mapa, object args = null, bool overrideDelegate = false)
        {
            mapa = _mapa;
            mapa.Carrega(args, CoordenadaDefault(), overrideDelegate);
        }

        public static CoordenadaDTO CoordenadaDefault()
        {
            return new CoordenadaDTO(Constantes.DEFAULT_LATITUDE, Constantes.DEFAULT_LONGITUDE);
        }

        public static CoordenadaDTO PosicionaMapa(CoordenadaDTO coordenada, float? zoom = null)
        {
            return mapa.PosicionaMapa(coordenada, zoom);
        }

        public static async Task<EnderecoDTO> ReverterCoordenada(CoordenadaDTO coordenada, object args = null)
        {
            EnderecoDTO enderecoCache = null;

            ValidaConexao();
            bool retry = false;
            if (coordenada == null)
                return null;
            var coo = new Coordenada(coordenada);

            const double limit = 0.010;
            double menor = limit;
            Func<EnderecoDTO, bool> condicao = (w => w != null && ((w.Coordenada.Latitude == coordenada.Latitude && w.Coordenada.Longitude == coordenada.Longitude) || coo.DistanciaKm(new Coordenada(w.Coordenada)) < limit));
            EnderecoDTO foundInCache = null;
            try
            {
                foreach (EnderecoDTO end in Cache.Where(condicao))
                {
                    if (foundInCache == null)
                    {
                        menor = coo.DistanciaKm(new Coordenada(end.Coordenada));
                        foundInCache = end;
                    }
                    else
                    {
                        double dist = coo.DistanciaKm(new Coordenada(end.Coordenada));
                        if (menor > dist)
                        {
                            menor = coo.DistanciaKm(new Coordenada(end.Coordenada));
                            foundInCache = end;
                        }
                    }
                }
            }
            catch (Exception)
            {
                foundInCache = null;
            }

            if (foundInCache != null)
            {
                foundInCache.EmCache = true;
                foundInCache.Coordenada = coordenada;//Retorna a mesma posição (coordenada) do mapa, pois está no range do cache, evitando o efeito elástico.
                return foundInCache;
            }

            if (!aguardandoCoordenada)
            {
                aguardandoCoordenada = true;
                try
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    source.CancelAfter(TimeSpan.FromSeconds(30));
                    Task<EnderecoDTO> task = Task.Run(() => mapa.ReverterCoordenada(coordenada, args), source.Token);
                    enderecoCache = await task;
                    enderecoCache.EmCache = false;
                    enderecoCache.EnderecoJaPesquisado = ToSearchString(task.ToString());
                    Cache.Add(enderecoCache);
                }
                catch
                {
                    retry = true;
                }
                finally
                {
                    aguardandoCoordenada = false;
                }
                if (retry)
                {
                    aguardandoCoordenada = true;
                    try
                    {
                        CancellationTokenSource source2 = new CancellationTokenSource();
                        source2.CancelAfter(TimeSpan.FromSeconds(30));
                        Task<EnderecoDTO> task2 = Task.Run(() => mapa.ReverterCoordenada(coordenada, args), source2.Token);
                        enderecoCache = await task2;
                        enderecoCache.EmCache = false;
                        enderecoCache.EnderecoJaPesquisado = ToSearchString(task2.ToString());
                        Cache.Add(enderecoCache);
                    }
                    catch (Exception ex2)
                    {
                        throw new ValidationException("O tempo limite de espera para retornar o endereço se esgotou! Tente novamente.", ex2);
                    }
                    finally
                    {
                        aguardandoCoordenada = false;
                    }
                }
            }
            return enderecoCache;
        }

        public static async Task<EnderecoDTO> ReverterEndereco(string enderecoLegivel, object args = null)
        {
            EnderecoDTO enderecoCache = null;

            if (string.IsNullOrEmpty(enderecoLegivel))
                throw new ValidationException("Informe um endereço para Busca.");
            bool retry = false;
            string enderecoPesquisado = ToSearchString(enderecoLegivel);
            EnderecoDTO foundInCache = new EnderecoDTO();
            try
            {
                foundInCache = Cache.FirstOrDefault(w => enderecoPesquisado == w.EnderecoJaPesquisado || ToSearchString(w.ToString()) == enderecoPesquisado);
            }
            catch (Exception)
            {
                foundInCache = null;
            }

            if (foundInCache != null)
            {
                foundInCache.EmCache = true;
                return foundInCache;
            }

            if (!aguardandoEndereco)
            {
                aguardandoEndereco = true;
                try
                {
                    ValidaConexao();
                    CancellationTokenSource source = new CancellationTokenSource();
                    source.CancelAfter(TimeSpan.FromSeconds(30));
                    Task<EnderecoDTO> task = Task.Run(() => mapa.ReverterEndereco(enderecoLegivel, args), source.Token);
                    enderecoCache = await task;
                    enderecoCache.EmCache = false;
                    enderecoCache.EnderecoJaPesquisado = enderecoPesquisado;
                    Cache.Add(enderecoCache);
                }
                catch
                {
                    retry = true;
                }

                if (retry)
                {
                    try
                    {
                        ValidaConexao();
                        CancellationTokenSource source2 = new CancellationTokenSource();
                        source2.CancelAfter(TimeSpan.FromSeconds(30));
                        Task<EnderecoDTO> task2 = Task.Run(() => mapa.ReverterEndereco(enderecoLegivel, args), source2.Token);
                        enderecoCache = await task2;
                        enderecoCache.EmCache = false;
                        enderecoCache.EnderecoJaPesquisado = enderecoPesquisado;
                        Cache.Add(enderecoCache);
                    }
                    catch (Exception ex2)
                    {
                        aguardandoEndereco = false;
                        throw new ValidationException("Endereço não encontrado! Tente novamente.", ex2);
                    }
                }
                aguardandoEndereco = false;
            }
            return enderecoCache;
        }

        static string ToSearchString(string value)
        {
            return value.Replace("  ", " ").Replace(",", "").Replace(".", "").Replace("-", "").Replace("_", "").Replace("/", "").Replace(@"\", "").Replace(";", "").Replace(":", "").ToUpper();
        }

        public static object ConfigurarMapa()
        {
            return mapa.ConfigurarMapa(null);

        }

        public static bool ValidarEndereco(EnderecoDTO endereco)
        {
            return true;
        }

        private static void ValidaConexao()
        {
            if (!FacadeConexao.GetNetworkConnection().CheckNetworkConnection())
                throw new ConnectionException("Esta funcionalidade necessita do uso de internet. Por favor, verifique seu sinal de internet.");
        }
    }
}
