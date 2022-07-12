using UIKit;
using CoreLocation;
using Foundation;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using SAMU192iOS.ViewControllers;
using SAMU192Core.Utils;
using SAMU192iOS.FacadeStub;

namespace SAMU192iOS.Implementaitons
{
    internal class GPSImpl : GPSAbstract
    {
        static MyLocationManager location;
        bool notifiedAccessDenied = false;
        public bool NotifiedAccessDenied { get => notifiedAccessDenied; set => notifiedAccessDenied = value; }

        public override void Carrega(object arg = null, GPSAbstract.AfterNotify afterNotify_event = null, bool inWalkthrough = false)
        {
            bool bypassPermission = false;
            if (location != null)
            {
                location.Dispose();
                location = null;
                bypassPermission = true;
            }

            location = new MyLocationManager(this, afterNotify_event, inWalkthrough, bypassPermission);
        }

        public override void StopLocationManager()
        {
            location.LocationsUpdated -= null;
            location.StopUpdatingLocation();
        }

        public override void Dispose()
        {
            location.Dispose();
            location = null;
        }

        public override string GetProviders()
        {
            return string.Format("LocationServicesEnabled: {0} Status: {1}", 
                CLLocationManager.LocationServicesEnabled.ToString() , 
                GetStatusDescription(CLLocationManager.Status));
        }

        private string GetStatusDescription(CLAuthorizationStatus status)
        {
            string val = "Indeterminado";
            try
            {                
                switch (status)
                {
                    case CLAuthorizationStatus.NotDetermined:
                        val = "Indeterminado";
                        break;
                    case CLAuthorizationStatus.Restricted:
                        val = "Restrito";
                        break;
                    case CLAuthorizationStatus.Denied:
                        val = "Não Autorizado";
                        break;
                    case CLAuthorizationStatus.Authorized | CLAuthorizationStatus.AuthorizedAlways:
                        val = "Autorizado";
                        break;
                    case CLAuthorizationStatus.AuthorizedWhenInUse:
                        val = "Autorizado quando em uso";
                        break;
                }
            }
            catch { /*dummy*/ }
            return val;
        }

        internal class MyLocationManager : CLLocationManager
        {
            GPSImpl gpsImpl;
            AfterNotify AfterNotify_event;
            bool InWalkthrough = false;
            CLAuthorizationStatus LastAuthStatus = CLAuthorizationStatus.NotDetermined;

            public MyLocationManager(GPSImpl gpsImpl, GPSAbstract.AfterNotify afterNotify_event, bool inWalkthrough, bool bypassPermission)
            {
                this.gpsImpl = gpsImpl;
                InWalkthrough = inWalkthrough;
                this.DistanceFilter = CLLocationDistance.FilterNone;
                this.DesiredAccuracy = 1;
                this.LocationsUpdated += MyLocationManager_LocationsUpdated;
                this.Failed += MyLocationManager_Failed;
                this.MonitoringFailed += MyLocationManager_MonitoringFailed;
                this.AuthorizationChanged += MyLocationManager_AuthorizationChanged;
                this.AfterNotify_event = afterNotify_event;
                if (!bypassPermission)
                    this.CheckPermissions();
                LastAuthStatus = Status;
            }

            private void CheckPermissions()
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                    this.RequestWhenInUseAuthorization();
                //if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
                //    this.AllowsBackgroundLocationUpdates = true;
            }

            private void NotificaUsuarioSobrePermissaoNegada()
            {
                if (!gpsImpl.NotifiedAccessDenied || InWalkthrough)
                {
                    InvokeOnMainThread(() =>
                    {
                        Utils.Mensagem.Alerta(
                           "O Aplicativo CHAMAR 192 necessita permissão para acessar o Serviço de Localização do Aparelho. Verifique.",
                           (o) =>
                           {
                               UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
                           });
                    });
                }

                gpsImpl.NotifiedAccessDenied = true;
            }

            private void MyLocationManager_AuthorizationChanged(object sender, CLAuthorizationChangedEventArgs e)
            {
                switch (e.Status)
                {
                    case CLAuthorizationStatus.Restricted:
                    case CLAuthorizationStatus.Denied:
                        this.DoDeniedAnalytic(e.Status);
                        this.NotificaUsuarioSobrePermissaoNegada();
                        this.AfterNotify_event(false);
                        break;
                    case CLAuthorizationStatus.NotDetermined:
                        gpsImpl.NotifiedAccessDenied = false;
                        this.AfterNotify_event(false);
                        break;
                    default:
                        gpsImpl.NotifiedAccessDenied = false;
                        this.AfterNotify_event(true);
                        this.StartUpdatingLocation();
                        this.DoAllowedAnalytics(e.Status);
                        break;
                }
                LastAuthStatus = e.Status;
            }

            private void DoAllowedAnalytics(CLAuthorizationStatus status)
            {
                if (LastAuthStatus != status)
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.GPS_Aceito_Permissao.Value);
            }

            private void DoDeniedAnalytic(CLAuthorizationStatus status)
            {
                if (LastAuthStatus != status)
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.GPS_Negado_Permissao.Value);
            }

            private void MyLocationManager_Failed(object sender, NSErrorEventArgs e)
            {
                //dummy
            }

            private void MyLocationManager_MonitoringFailed(object sender, CLRegionErrorEventArgs e)
            {
                //dummy
            }

            private void MyLocationManager_LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
            {
                var coordenadaTmp = e.Locations[0].Coordinate;
                this.gpsImpl.OnGPSLocationChanged(new CoordenadaDTO(coordenadaTmp.Latitude, coordenadaTmp.Longitude));
            }
        }
    }
}