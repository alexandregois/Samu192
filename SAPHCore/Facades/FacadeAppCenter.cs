using System;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;

namespace SAMU192Core.Facades
{
    public static class FacadeAppCenter
    {
        private static bool AppCenterIsEnabled = false;
        private static bool AppCenterAnalyticsIsActivated = true, AppCenterCrashesIsActivated = true;
        private static INetworkConnection NetConn;
        private static string AppCenterSecret = string.Empty;
       
        public static void Inicializa(INetworkConnection netConn, string appCenterSecret)
        {
            try
            { 
                NetConn = netConn;
                AppCenterSecret = appCenterSecret;
                AppCenterAnalyticsIsActivated = Constantes.APPCENTER_ANALYTICS_ACTIVATED;
                AppCenterCrashesIsActivated = Constantes.APPCENTER_CRASHES_ACTIVATED;

                if (!AppCenterIsEnabled)
                {
                    AppCenter.Start(AppCenterSecret, typeof(Analytics), typeof(Crashes));
                    Habilita();
                }
            }
            catch { /*dummy - APP CENTER NUNCA PODE PARAR A APP MESMO SE ERRO*/ }
        }

        private static async void Habilita()
        {
            try
            {
                AppCenterIsEnabled = await Crashes.IsEnabledAsync();
            }
            catch { /*dummy - APP CENTER NUNCA PODE PARAR A APP MESMO SE ERRO*/ }
        }

        public static void AppCrash(Exception ex, string tela = "", string detalhes = "")
        {
            try
            {
                if (!AppCenterIsEnabled)
                    return;

                if (!AppCenterCrashesIsActivated)
                    return;

                string wifiStatus = FacadeUtilidades.GetWifiStatus(NetConn);
                string gpsProviders = FacadeGPS.GetProviders();

                Dictionary<string, string> properties = null;
                if (!string.IsNullOrEmpty(wifiStatus) || !string.IsNullOrEmpty(gpsProviders) || !string.IsNullOrEmpty(tela) || !string.IsNullOrEmpty(detalhes))
                {
                    properties = new Dictionary<string, string>
                    {
                        //Mais informações
                        { "WifiStatus", wifiStatus ?? "" },
                        { "GPSProviders", gpsProviders ?? "" },
                        { "Tela", tela ?? "" },
                        { "Detalhes", detalhes ?? ""}
                    };
                }

                Crashes.TrackError(ex, properties);
            }
            catch { /*dummy - APP CENTER NUNCA PODE PARAR A APP MESMO SE ERRO*/ }
        }

        public static void AppAnalytic(string eventName, string tela = "", string detalhes = "")
        {
            try
            {
                if (!AppCenterIsEnabled)
                    return;

                if (!AppCenterAnalyticsIsActivated)
                    return;

                Dictionary<string, string> properties = null;
                if (!string.IsNullOrEmpty(tela) || !string.IsNullOrEmpty(detalhes))
                {
                    properties = new Dictionary<string, string>
                    {
                        //Mais informações
                        { "Tela", tela ?? "" },
                        { "Detalhes", detalhes ?? ""}
                    };
                }

                Analytics.TrackEvent(eventName, properties);
            }
            catch { /*dummy - APP CENTER NUNCA PODE PARAR A APP MESMO SE ERRO*/ }
        }
    }
}
