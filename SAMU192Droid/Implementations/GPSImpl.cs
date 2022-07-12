using Android.App;
using Android.Content;
using SAMU192Droid.Servicos;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using Android.Support.V4.Content;

namespace SAMU192Droid.Implementaitons
{
    internal class GPSImpl : GPSAbstract
    {
        static GPSServicoReceiver receiver;
        static Activity activity;

        public override void Carrega(object args = null, AfterNotify afterNotify_event = null, bool inWalkthrough = false)
        {
            activity = (Activity)args;

            var intent = new Intent(activity, typeof(GPSServico));

            if (GPSServico.IsStarted)
                activity.StopService(intent);

            activity.StartService(intent);

            LoadReceiver();
        }

        private void LoadReceiver()
        {
            if (receiver == null)
            {
                receiver = new GPSServicoReceiver(this);
                IntentFilter intentFilterUpdate = new IntentFilter("GPS_UPDATE");
                intentFilterUpdate.Priority = (int)IntentFilterPriority.HighPriority;
                LocalBroadcastManager.GetInstance(activity).RegisterReceiver(receiver, intentFilterUpdate);
            }
        }

        public override void StopLocationManager()
        {
            if (receiver != null)
            {
                LocalBroadcastManager.GetInstance(activity).UnregisterReceiver(receiver);
                
                receiver.Dispose();
                receiver = null;
            }
        }

        public override void Dispose()
        {
            StopLocationManager();
            var intent = new Intent(activity, typeof(GPSServico));
            activity.StopService(intent);
        }

        private static string gpsProviders = string.Empty;
        public override string GetProviders()
        {
            var locationManager = Application.Context.GetSystemService(Android.Content.Context.LocationService) as Android.Locations.LocationManager;
            if (locationManager != null)
            {
                string activedProviders = string.Empty;
                if (locationManager?.AllProviders.Count > 0)
                {
                    foreach (var provider in locationManager.AllProviders)
                    {
                        activedProviders = (string.IsNullOrEmpty(activedProviders) ? provider : activedProviders + ", " + provider);
                    }
                }
                gpsProviders = activedProviders;
            }
            return gpsProviders;
        }

        internal class GPSServicoReceiver : BroadcastReceiver
        {
            GPSImpl gpsImpl;
            const int MIN_FREQUENCIA_GPS = 10;
            public GPSServicoReceiver(GPSImpl gpsImpl)
            {
                this.gpsImpl = gpsImpl;
            }

            System.DateTime ultimaCoord = new System.DateTime(2010, 1, 1);

            public override void OnReceive(Context context, Intent intent)
            {
                if (System.DateTime.Now.Subtract(ultimaCoord).TotalSeconds > MIN_FREQUENCIA_GPS)
                {
                    int tipo = intent.GetIntExtra("tipo", 0);

                    switch ((SAMU192Core.Utils.Enums.BroadcastType)tipo)
                    {
                        case SAMU192Core.Utils.Enums.BroadcastType.position:
                            double lat = intent.GetDoubleExtra("latitude", 0);
                            double lng = intent.GetDoubleExtra("longitude", 0);
                            Android.Util.Log.Info("SAMU192", string.Format("Coordenada: {0}, {1}", lat, lng));
                            this.gpsImpl.OnGPSLocationChanged(new CoordenadaDTO(lat, lng));
                            break;
                        default:
                            break;
                    }
                    ultimaCoord = System.DateTime.Now;
                }
            }            
        }
    }
}