using System.Threading.Tasks;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192iOS.Implementaions;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubMapa
    {
        internal static void Carrega(object args, bool overrideDelegate)
        {
            MapaImpl mapa = new MapaImpl();
            FacadeMapa.Carregar(mapa, args, overrideDelegate);
        }

        internal static CoordenadaDTO CoordenadaDefault()
        {
            return FacadeMapa.CoordenadaDefault();
        }

        internal async static Task<EnderecoDTO> ReverterCoordenada(CoordenadaDTO coordenada, object args)
        {
            //Evita "Paranatinga-MT" Coordenada default do mapa iOS
            if (coordenada.Latitude.ToString() == "-14,36688" && coordenada.Longitude.ToString() == "-54,492188")
            {
                coordenada = StubGPS.GetLastLocation();
                if (coordenada == null)
                    return new EnderecoDTO();
            }

            return await FacadeMapa.ReverterCoordenada(coordenada, args);
        }

        internal async static Task<EnderecoDTO> ReverterEndereco(string enderecoLegivel, object args)
        {
            return await FacadeMapa.ReverterEndereco(enderecoLegivel, args);
        }

        internal static CoordenadaDTO PosicionaMapa(CoordenadaDTO coordenada)
        {
            return FacadeMapa.PosicionaMapa(coordenada); ;
        }

        internal static CoordenadaDTO SetaLocalizacaoGPSNoMapa(CoordenadaDTO coordenada)
        {
            return FacadeMapa.PosicionaMapa(coordenada != null ? coordenada : StubGPS.GetLastLocation());
        }
    }
}