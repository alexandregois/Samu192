using System;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;

namespace SAMU192Core.Facades
{
    public static class FacadeGPS
    {
        static GPSAbstract gps;

        public static void Carregar(GPSAbstract _gps, object args = null, GPSAbstract.AfterNotify afterNotify_event = null, bool inWalkthrough = false)
        {
            gps = _gps;
            gps.Carrega(args, afterNotify_event, inWalkthrough);
        }

        public static CoordenadaDTO GetLastLocation()
        {
            return gps?.GetLastLocation();
        }

        public static DateTime? GetTimeOfLastLocation()
        {
            return gps?.GetTimeOfLastLocation();
        }

        public static void StopLocationManager()
        {
            if (gps != null)
                gps.StopLocationManager();
        }

        public static void Dispose()
        {
            if (gps != null)
                gps.Dispose();
            gps = null;
        }

        public static string GetProviders()
        {
            return gps.GetProviders();
        }
    }
}