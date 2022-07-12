using System;
using CoreLocation;
using Foundation;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192Core.Interfaces;
using SAMU192iOS.Implementaitons;
using SAMU192iOS.ViewControllers;
using UIKit;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubGPS
    {
        static GPSImpl gps = null;
        internal static void Carrega(GPSAbstract.AtualizaDadosGPS atualizaDadosGPS, GPSAbstract.AfterNotify afterNotify_event, bool inWalkthrough = false)
        {
            if (gps == null)
                gps = new GPSImpl();
            gps.AtualizaDadosGPS_Event = atualizaDadosGPS;
            FacadeGPS.Carregar(gps, null, afterNotify_event, inWalkthrough);
        }

        internal static void Descarrega()
        {
            StopLocationManager();
            gps.Dispose();
        }

        internal static void Recarrega(GPSAbstract.AtualizaDadosGPS atualizaDadosGPS, GPSAbstract.AfterNotify afterNotify_event)
        {
            Carrega(atualizaDadosGPS, afterNotify_event);
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

        public static bool StatusGPS()
        {
            if (CLLocationManager.Status == CLAuthorizationStatus.Restricted
                || CLLocationManager.Status == CLAuthorizationStatus.Denied
                || CLLocationManager.Status == CLAuthorizationStatus.NotDetermined)
            {
                return false;
            }
            else
                return true;
        }

        public static bool NotificaAtivacao(GPSAbstract.AfterNotify afterNotify_event)
        {
            bool result = false;
            if (!CoreLocation.CLLocationManager.LocationServicesEnabled)
            {
                Utils.Mensagem.Alerta(
                    "O Aplicativo CHAMAR 192 necessita que o Serviço de Localização do Aparelho esteja ativo para seu correto funcionamento.",
                    (object val) => { UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Privacy&path=LOCATION:")); });
                result = true;
            }
            else
            {
                StubGPS.Carrega((CoordenadaDTO coo) => { }, afterNotify_event, true);
                result = false;
            }
            return result;
        }

        public static string GetProviders()
        {
            return FacadeGPS.GetProviders();
        }
    }
}