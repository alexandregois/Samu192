using System;
using System.Threading.Tasks;
using Android.Gms.Maps;
using SAMU192Droid.Implementations;
using SAMU192Core.DTO;
using SAMU192Core.Facades;

namespace SAMU192Droid.FacadeStub
{
    public static class StubMapa
    {
        public static void Carrega(object args)
        {
            HereApi mapa = new HereApi(); //GeocodeFarm mapa = new GeocodeFarm();
            FacadeMapa.Carregar(mapa, args);
        }

        internal async static Task<EnderecoDTO> ReverterCoordenada(CoordenadaDTO coordenada, object args)
        {
            return await FacadeMapa.ReverterCoordenada(coordenada, args);
        }

        internal async static Task<EnderecoDTO> ReverterEndereco(string enderecoLegivel, object args)
        {
            return await FacadeMapa.ReverterEndereco(enderecoLegivel, args);
        }

        public static CoordenadaDTO PosicionaMapa(CoordenadaDTO coordenada, float? zoom)
        {
            return FacadeMapa.PosicionaMapa(coordenada, zoom);
        }        

        internal static CoordenadaDTO SetaLocalizacaoGPSNoMapa()
        {
            return FacadeMapa.PosicionaMapa(StubGPS.GetLastLocation());
        }

        internal static Action<GoogleMap> ConfigurarMapa()
        {            
            return (Action<GoogleMap>)FacadeMapa.ConfigurarMapa();
        }

        internal static CoordenadaDTO SetaLocalizacaoGPSNoMapa(CoordenadaDTO coordenada, float? zoom = null)
        {
            return FacadeMapa.PosicionaMapa(coordenada != null ? coordenada : StubGPS.GetLastLocation(), zoom);
        }

        internal static CoordenadaDTO CoordenadaDefault()
        {
            return FacadeMapa.CoordenadaDefault();
        }
    }
}