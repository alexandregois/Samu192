using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Newtonsoft.Json.Linq;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using static SAMU192Core.Utils.Constantes;
namespace SAMU192Droid.Implementations
{
    internal class HereApi : IMapa
    {
        private GoogleMap oMap;
        private Action<GoogleMap> MapReadyAction;
        private string AppID;
        private string AppCode;
        private IWebProxy Proxy = new WebProxy();// (new Uri("http://10.0.255.1:3128")) { Credentials = new NetworkCredential("laboratorio", "Lab-2018") };
        public bool GpsPermitido;
        internal class GeoCodingResult
        {
            internal String Erro;
            internal eQualidadeGeocode Qualidade;
            internal EnderecoDTO Endereco;
            internal GisLimites Limites;
        }
        internal class GisLimites
        {
            internal double x1;
            internal double y1;
            internal double x2;
            internal double y2;
        }
        internal enum eQualidadeGeocode
        {
            Descohecido = 0,
            Impreciso = 1,
            Proximo = 2,
            Exato = 3
        }
        public void Carrega(object args, CoordenadaDTO coordenadaDefault, bool overrideDelegate = false)
        {
            oMap = (GoogleMap)args;
            this.AppID = GEOCODEHERE_APPID;
            this.AppCode = GEOCODEHERE_APPCODE;
        }
        public CoordenadaDTO PosicionaMapa(CoordenadaDTO coordenada, float? zoom = null)
        {
            if (coordenada != null)
            {
                LatLng coordenadaTmp = new LatLng(coordenada.Latitude, coordenada.Longitude);
                if (oMap != null && LocalizacaoValida(coordenadaTmp))
                {
                    if (!zoom.HasValue)
                        zoom = oMap.CameraPosition.Zoom;
                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(coordenadaTmp, zoom.Value);
                    oMap.MoveCamera(cameraUpdate);
                }
            }
            return coordenada;
        }
        public async Task<EnderecoDTO> ReverterCoordenada(CoordenadaDTO coordenada, object args)
        {
            var result = ProcessaGeocodeReverso(Requisicao(URLRevGeocode(coordenada.Latitude, coordenada.Longitude)));
            if (result.Endereco != null)
            {
                result.Endereco.Coordenada = coordenada;
                result.Limites = new GisLimites() { x1 = coordenada.Longitude, x2 = coordenada.Longitude, y1 = coordenada.Latitude, y2 = coordenada.Latitude };
            }
            return result.Endereco;
        }
        public async Task<EnderecoDTO> ReverterEndereco(string enderecoLegivel, object args)
        {
            if (!enderecoLegivel.ToLower().Trim().EndsWith("Brasil"))
                enderecoLegivel += ", Brasil";
            var result = ProcessaGeocode(Requisicao(URLGeocode(enderecoLegivel)));
            return result.Endereco;
        }
        public object ConfigurarMapa(object view = null)
        {
            if (oMap == null) return null;
            MapReadyAction += delegate (GoogleMap googleMap)
            {
                googleMap.MyLocationEnabled = GpsPermitido;
                googleMap.UiSettings.ZoomControlsEnabled = true;
                googleMap.UiSettings.TiltGesturesEnabled = false;
                oMap = googleMap;
                CoordenadaDTO coordenada = new CoordenadaDTO(googleMap.CameraPosition.Target.Latitude, googleMap.CameraPosition.Target.Longitude);
                PosicionaMapa(coordenada, 17);
            };
            return MapReadyAction;
        }
        private bool LocalizacaoValida(LatLng localization)
        {
            return (localization != null && (localization.Latitude != 0 || localization.Longitude != 0));
        }
        public void Dispose()
        {
            oMap.Dispose();
            oMap = null;
        }
        private string URLGeocode(string enderecoLegivel)
        {
            return String.Format(GEOCODEHERE_URL_API, AppID, AppCode, enderecoLegivel);
        }
        private string URLRevGeocode(double lat, double lng)
        {
            return string.Format(GEOCODEHERE_URL_API_REV, AppID, AppCode, lat.ToString().Replace(",", "."), lng.ToString().Replace(",", "."), GEOCODEHERE_DISTANCIA_REV);
        }
        private string Requisicao(string url)
        {
            return new WebClient() { Encoding = Encoding.UTF8, Proxy = this.Proxy }.DownloadString(url);
        }
        private GeoCodingResult ProcessaGeocode(string json)
        {
            try
            {
                JObject doc = JObject.Parse(json);
                JToken view = doc["Response"]["View"];
                if (view == null)
                {
                    JToken tipo = doc["type"];
                    if (tipo != null)
                        throw new ApplicationException("Resposta de exceção em GeocoderHere");
                    else
                        throw new ApplicationException("Resposta sem tratamento em GeocoderHere");
                }
                else
                {
                    if (!view.HasValues || !view[0]["Result"].HasValues)
                        return new GeoCodingResult() { Qualidade = eQualidadeGeocode.Descohecido };
                    else
                        return ProcessaResultadosGeocode(view[0], string.Empty);
                }
            }
            catch (Exception ex)
            {
                return new GeoCodingResult() { Erro = ex.Message };
                //return new GeoCodingResult() { Erro = "Resposta do GeocodeHere com formatação inválida ou desconhecida!" };
            }
        }
        private GeoCodingResult ProcessaGeocodeReverso(string json)
        {
            try
            {
                JObject doc = JObject.Parse(json);
                JToken view = doc["Response"]["View"];
                if (view == null)
                {
                    JToken tipo = doc["type"];
                    if (tipo != null)
                        throw new ApplicationException("Resposta de exceção em GeocoderHere");
                    else
                        throw new ApplicationException("Resposta sem tratamento em GeocoderHere");
                }
                else
                {
                    if (!view.HasValues)
                        return new GeoCodingResult() { Qualidade = eQualidadeGeocode.Descohecido };
                    else
                    {
                        GeoCodingResult resp = ProcessaResultadosGeocode(view[0], "houseNumber");
                        if (resp.Qualidade == eQualidadeGeocode.Descohecido || resp.Qualidade == eQualidadeGeocode.Impreciso)
                            return ProcessaResultadosGeocode(view[0], string.Empty);
                        else
                            return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                return new GeoCodingResult() { Erro = ex.Message };
                //return new GeoCodingResult() { Erro = "Resposta do GeocodeHere com formatação inválida ou desconhecida!" };
            }
        }
        private GeoCodingResult ProcessaResultadosGeocode(JToken view, string matchLevelEsperado)
        {
            foreach (JToken result in view["Result"])
            {
                //JToken result = view[i];
                if (result != null && result["Location"]["Address"].Value<String>("Country") == "BRA")
                {
                    if (string.IsNullOrEmpty(matchLevelEsperado) || matchLevelEsperado.ToUpper() == result.Value<string>("MatchLevel").ToUpper())
                    {
                        eQualidadeGeocode qualidade = eQualidadeGeocode.Exato;
                        CoordenadaDTO coordenada = null;
                        GisLimites limites = null;
                        JToken ponto = result["Location"]["NavigationPosition"];
                        if (ponto != null)
                        {
                            //Se não tem ponto é georef reverso
                            double latitude = ponto[0].Value<double>("Latitude");
                            double longitude = ponto[0].Value<double>("Longitude");
                            coordenada = new CoordenadaDTO() { Latitude = latitude, Longitude = longitude };
                            limites = new GisLimites() { x1 = longitude, x2 = longitude, y1 = latitude, y2 = latitude };
                            qualidade = DefineQualidade(result.Value<String>("MatchLevel"), result["MatchQuality"]);
                        }
                        EnderecoDTO endereco = new EnderecoDTO()
                        {
                            Coordenada = coordenada,
                            Bairro = result["Location"]["Address"].Value<String>("District"),
                            Cidade = result["Location"]["Address"].Value<String>("City"),
                            Estado = result["Location"]["Address"].Value<String>("State"),
                            Logradouro = result["Location"]["Address"].Value<String>("Street"),
                            Numero = result["Location"]["Address"].Value<String>("HouseNumber")
                        };
                        return new GeoCodingResult()
                        {
                            Qualidade = qualidade,
                            Endereco = endereco,
                            Limites = limites
                        };
                    }
                }
            }
            return new GeoCodingResult() { Qualidade = eQualidadeGeocode.Descohecido };
        }
        private eQualidadeGeocode DefineQualidade(String matchLevel, JToken matchQuality)
        {
            switch (matchLevel.ToUpper())
            {
                case "STREET":
                    if (MenorQualidade(matchQuality) >= 0.75M)
                        return eQualidadeGeocode.Proximo;
                    else
                        return eQualidadeGeocode.Impreciso;
                case "HOUSENUMBER":
                    decimal menor = MenorQualidade(matchQuality);
                    if (menor >= 0.95M)
                        return eQualidadeGeocode.Exato;
                    else
                        if (menor >= 0.75M)
                        return eQualidadeGeocode.Proximo;
                    else
                        return eQualidadeGeocode.Impreciso;
                default:
                    return eQualidadeGeocode.Impreciso;
            }
        }
        private decimal MenorQualidade(JToken matchQuality)
        {
            decimal menor = 1M;
            foreach (JToken child in matchQuality.Children())
            {
                if (child is JProperty)
                {
                    JProperty x = (JProperty)child;
                    if (x.Value.Type is JTokenType.Array)
                    {
                        foreach (decimal valor in x.Value.Values<decimal>())
                            if (menor > valor)
                                menor = valor;
                    }
                    else
                        if (menor > (decimal)x.Value)
                        menor = (decimal)x.Value;
                }
            }
            return menor;
        }
        private string TrataEndereco(string endereco)
        {
            endereco = endereco.Trim();
            if (endereco.EndsWith(","))
                endereco = endereco.Substring(0, endereco.Length - 1);
            return endereco;
        }
        private string EstadoSigla(string Estado)
        {
            switch (Estado.ToUpper())
            {
                case "ACRE":
                    return "AC";
                case "ALAGOAS":
                    return "AL";
                case "AMAPÁ":
                    return "AP";
                case "AMAZONAS":
                    return "AM";
                case "BAHIA":
                    return "BA";
                case "CEARÁ":
                    return "CE";
                case "DISTRITO FEDERAL":
                    return "DF";
                case "ESPÍRITO SANTO":
                    return "ES";
                case "GOIÁS":
                    return "GO";
                case "MARANHÃO":
                    return "MA";
                case "MATO GROSSO":
                    return "MT";
                case "MATO GROSSO DO SUL":
                    return "MS";
                case "MINAS GERAIS":
                    return "MG";
                case "PARÁ":
                    return "PA";
                case "PARAÍBA":
                    return "PB";
                case "PARANÁ":
                    return "PR";
                case "PERNAMBUCO":
                    return "PE";
                case "PIAUÍ":
                    return "PI";
                case "RIO DE JANEIRO":
                    return "RJ";
                case "RIO GRANDE DO NORTE":
                    return "RN";
                case "RIO GRANDE DO SUL":
                    return "RS";
                case "RONDÔNIA":
                    return "RO";
                case "RORAIMA":
                    return "RR";
                case "SANTA CATARINA":
                    return "SC";
                case "SÃO PAULO":
                    return "SP";
                case "SERGIPE":
                    return "SE";
                case "TOCANTINS":
                    return "TO";
                default:
                    return Estado;
            }
        }
        public CoordenadaDTO PosicionaImagemNoMapa(CoordenadaDTO coordenada, string imagemPath, object tela, int index, string nomeEquipe)
        {
            throw new NotImplementedException();
        }
        public void AjustaCentralizacaoMapa(CoordenadaDTO coordenada, CoordenadaDTO coordenadaAmbulancia, double fator, bool alterouDestino)
        {
            throw new NotImplementedException();
        }
        public void LimparMapa()
        {
            throw new NotImplementedException();
        }
    }
}