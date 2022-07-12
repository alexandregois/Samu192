using System;
using Android.App;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192Core.Interfaces;
using SAMU192Droid.Implementaitons;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubGPS
    {
        static GPSImpl gps = null;

        internal static void Carrega(Activity args, GPSAbstract.AtualizaDadosGPS atualizaDadosGPS, GPSAbstract.AfterNotify afterNotify_event)
        {
            if (gps == null)
                gps = new GPSImpl();
            gps.AtualizaDadosGPS_Event = atualizaDadosGPS;
            FacadeGPS.Carregar(gps, args, afterNotify_event);
        }

        internal static void Descarrega()
        {
            FacadeGPS.Dispose();
        }

        public static DateTime? GetTimeOfLastLocation()
        {
            return FacadeGPS.GetTimeOfLastLocation();
        }

        public static CoordenadaDTO GetLastLocation()
        {
            return FacadeGPS.GetLastLocation();
        }

        public static void StopLocationManager()
        {
            FacadeGPS.StopLocationManager();
        }

        public static void Dispose()
        {
            FacadeGPS.Dispose();
        }

        public static string GetProviders()
        {
            return FacadeGPS.GetProviders();
        }
    }
}