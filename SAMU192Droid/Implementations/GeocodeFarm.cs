using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using static SAMU192Core.Utils.Constantes;

namespace SAMU192Droid.Implementations
{
    internal class GeocodeFarm : IMapa
    {
        private GoogleMap oMap;
        private Action<GoogleMap> MapReadyAction;

        private string Key;
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

        internal class QualidadeGeocode
        {
            internal static string Valor2Texto(eQualidadeGeocode valor)
            {
                switch (valor)
                {
                    //TODO Até segunda ordem volta o próprio nome do enumerador
                    case eQualidadeGeocode.Descohecido:
                    case eQualidadeGeocode.Exato:
                    case eQualidadeGeocode.Impreciso:
                    case eQualidadeGeocode.Proximo:
                        return valor.ToString("g");
                }
                return string.Empty;
            }
        }

        public void Carrega(object args, CoordenadaDTO coordenadaDefault, bool overrideDelegate = false)
        {
            oMap = (GoogleMap)args;
            this.Key = GEOCODE_KEY;
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
            var result = ProcessaRevGeocode(Requisicao(URLRevGeocode(coordenada.Latitude, coordenada.Longitude)));

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
            string url = String.Format(GEOCODE_URL_API, enderecoLegivel, Key);
            return url;
        }

        private string URLRevGeocode(double lat, double lng)
        {
            string url = string.Format(GEOCODE_URL_API_REV, lat.ToString().Replace(",", "."), lng.ToString().Replace(",", "."), Key);
            return url;
        }

        private string Requisicao(string url)
        {
            return new WebClient() { Encoding = Encoding.UTF8, Proxy = this.Proxy }.DownloadString(url);
        }

        private GeoCodingResult ProcessaGeocode(string xml)
        {
            XmlDocument x = new XmlDocument();
            x.LoadXml(xml);
            try
            {
                XmlNodeList xs = x.GetElementsByTagName("access");

                if (xs.Count == 0)
                    return new GeoCodingResult() { Erro = "Resposta desconhecida do Geocodefarm!" };

                if (!xs.Item(0).InnerText.EndsWith("ACCESS_GRANTED"))
                    return new GeoCodingResult() { Erro = String.Format("Resposta do Geocodefarm: {0}", xs.Item(0).InnerText) };

                if (x.GetElementsByTagName("status").Item(0).InnerText != "SUCCESS")
                    return new GeoCodingResult() { Qualidade = eQualidadeGeocode.Descohecido };

                return ProcessaResultadosGeocode(x);
            }
            catch
            {
                return new GeoCodingResult() { Erro = "Resposta do Geocodefarm com formatação inválida ou desconhecida!" };
            }
        }

        private GeoCodingResult ProcessaRevGeocode(string xml)
        {
            XmlDocument x = new XmlDocument();
            x.LoadXml(xml);
            try
            {
                XmlNodeList xs = x.GetElementsByTagName("access");

                if (xs.Count == 0)
                    return new GeoCodingResult() { Erro = "Resposta desconhecida do Geocodefarm!" };

                if (!xs.Item(0).InnerText.EndsWith("ACCESS_GRANTED"))
                    return new GeoCodingResult() { Erro = String.Format("Resposta do Geocodefarm: {0}", xs.Item(0).InnerText) };

                if (x.GetElementsByTagName("status").Item(0).InnerText != "SUCCESS")
                    return new GeoCodingResult() { Qualidade = eQualidadeGeocode.Descohecido };

                return ProcessaResultadosGeocode(x);
            }
            catch
            {
                return new GeoCodingResult() { Erro = "Resposta do Geocodefarm com formatação inválida ou desconhecida!" };
            }
        }

        private GeoCodingResult ProcessaResultadosGeocode(XmlDocument doc)
        {
            double nLat = 0;
            double nLng = 0;
            GeoCodingResult resp = null;
            eQualidadeGeocode qualidade = eQualidadeGeocode.Descohecido;
            GisLimites limites = null;
            CoordenadaDTO coordenada = null;

            foreach (XmlElement x in doc.GetElementsByTagName("result"))
            {


                XmlNodeList xs = x.GetElementsByTagName("COORDINATES");
                if (xs.Count == 1)
                {
                    XmlElement xLoc = (XmlElement)xs[0];

                    string aux = ConteudoElemento(xLoc, "latitude");
                    if (!double.TryParse(aux.Replace(".", ","), out nLat))
                        continue;

                    aux = ConteudoElemento(xLoc, "longitude");
                    if (!double.TryParse(aux.Replace(".", ","), out nLng))
                        continue;

                    string sQualidade = x.GetElementsByTagName("accuracy")[0].InnerText;
                    qualidade = DefineQualidade(sQualidade);
                    limites = PegaLimites(x);
                    coordenada = new CoordenadaDTO(nLat, nLng);
                }

                XmlNodeList xBairro = x.GetElementsByTagName("formatted_address");
                string bairroGeoCode = string.Empty;
                if (xBairro.Count == 1)
                {
                    XmlElement xLoc = (XmlElement)xBairro[0];
                    string todoEndereco = x.GetElementsByTagName("formatted_address")[0].InnerText;
                    if (!string.IsNullOrEmpty(todoEndereco))
                    {
                        var splitValor = todoEndereco.Split(',');
                        if (splitValor.Count() >= 2)
                        {
                            var splitBairro = splitValor[1].Split('-');
                            if (splitBairro.Count() >= 2)
                                bairroGeoCode = splitBairro[1].TrimStart();


                        }
                    }
                }
                XmlNodeList xs2 = x.GetElementsByTagName("ADDRESS");
                if (xs2.Count == 1)
                {
                    XmlElement endereco = (XmlElement)xs2[0];
                    string sQualidade = x.GetElementsByTagName("accuracy")[0].InnerText;
                    qualidade = DefineQualidade(sQualidade);
                    limites = PegaLimites(x);

                    resp = new GeoCodingResult()
                    {
                        Qualidade = qualidade,
                        Endereco = new EnderecoDTO(
                            TrataEndereco(ConteudoElemento(endereco, "street_name")),
                            PegaNumeral(endereco),
                            "",
                            bairroGeoCode,
                            ConteudoElemento(endereco, "locality"),
                            ConteudoElemento(endereco, "admin_1"),
                            coordenada),
                        Limites = PegaLimites(x)
                    };

                    resp.Endereco.Estado = EstadoSigla(resp.Endereco.Estado);


                }

            }
            return resp;
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

        private eQualidadeGeocode DefineQualidade(string qualidade)
        {
            switch (qualidade.ToUpper())
            {
                case "EXACT_MATCH":
                    return eQualidadeGeocode.Exato;
                case "HIGH_ACCURACY":
                    return eQualidadeGeocode.Proximo;
                case "MEDIUM_ACCURACY":
                case "UNKNOWN_ACCURACY":
                    return eQualidadeGeocode.Impreciso;
                default:
                    return eQualidadeGeocode.Impreciso;
            }
        }

        private string ConteudoElemento(XmlElement element, string name)
        {
            XmlNodeList xs = element.GetElementsByTagName(name);
            if (xs.Count == 1)
                return xs[0].InnerText;
            else
                return string.Empty;
        }

        private string PegaNumeral(XmlElement address)
        {
            string numeral = ConteudoElemento(address, "street_number");
            if (numeral.Contains("-"))
                return numeral.Substring(0, numeral.IndexOf("-"));
            else
                return numeral;
        }

        private GisLimites PegaLimites(XmlElement x)
        {
            XmlNodeList xs = x.GetElementsByTagName("BOUNDARIES");
            if (xs.Count == 1)
            {
                XmlElement boundaries = (XmlElement)xs[0];
                GisLimites resp = new GisLimites();
                double value;
                string aux = ConteudoElemento(boundaries, "northeast_latitude");
                if (double.TryParse(aux.Replace(".", ","), out value))
                    resp.y1 = value;
                else
                    return null;
                aux = ConteudoElemento(boundaries, "northeast_longitude");
                if (double.TryParse(aux.Replace(".", ","), out value))
                    resp.x2 = value;
                else
                    return null;
                aux = ConteudoElemento(boundaries, "southwest_latitude");
                if (double.TryParse(aux.Replace(".", ","), out value))
                    resp.y2 = value;
                else
                    return null;
                aux = ConteudoElemento(boundaries, "southwest_longitude");
                if (double.TryParse(aux.Replace(".", ","), out value))
                    resp.x1 = value;
                else
                    return null;
                return resp;
            }
            return null;
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