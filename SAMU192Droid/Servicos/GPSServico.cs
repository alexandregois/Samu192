using System;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;

namespace SAMU192Droid.Servicos
{
    [Service]
    public class GPSServico : Service, Android.Locations.ILocationListener
    {
        string locationProvider;
        LocationManager locMgr;
        FusedLocationProviderClient fusedLocationProviderClient;
        FusedLocationProviderCallback fusedLocationCallback;

        public static bool IsStarted = false;
        public static bool Erro = false;
        public static string ErroMensagem = string.Empty;

        const int MAXLOTE = 100; // Tamanho máximo de entradas de coordenadas de GPS enviadas em uma transmissão.
        const int MAXLEITURAS = 3000; // Quantidade máxima de leituras que o aparelho armazena em memória.
        const long INTERVAL_GPS = 1 * 1000; // Intervalo entre atualizações de GPS (ciclos)

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (Build.VERSION.Release == "7.1.1")
                LigaFusedLocationProvider();
            else
                LigaDefaultLocationProvider();

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (Build.VERSION.Release == "7.1.1")
                DesligaFusedLocationProvider();
            else
                DesligaDefaultLocationProvider();

            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public void OnLocationChanged(Location location)
        {
            try
            {
                SendBroadCastGPS(location);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("OnLocationChanged {0}{1}", ex.ToString(), System.Environment.NewLine));
            }
        }

        public void OnProviderDisabled(string provider)
        {
            SendBroadCastGPSStatus(false);
        }

        public void OnProviderEnabled(string provider)
        {
            SendBroadCastGPSStatus(true);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        private void LigaDefaultLocationProvider()
        {
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            Criteria locationCriteria = new Criteria();
            locationCriteria.Accuracy = Accuracy.Fine;
            locationCriteria.PowerRequirement = Power.NoRequirement;

            locationProvider = locMgr.GetBestProvider(locationCriteria, true);

            if (!string.IsNullOrEmpty(locationProvider))
                locMgr.RequestLocationUpdates(locationProvider, INTERVAL_GPS, 0, this);

            locMgr.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
            locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, this);

            Location myLocation = locMgr.GetLastKnownLocation(LocationManager.PassiveProvider);

            if (myLocation != null)
                SendBroadCastGPS(myLocation);

            IsStarted = true;
        }

        private async void LigaFusedLocationProvider()
        {
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this.ApplicationContext);

            Android.Gms.Location.LocationRequest locationRequest = new Android.Gms.Location.LocationRequest()
                                    .SetPriority(Android.Gms.Location.LocationRequest.PriorityHighAccuracy)
                                    .SetInterval(INTERVAL_GPS)
                                    .SetFastestInterval(INTERVAL_GPS);

            fusedLocationCallback = new FusedLocationProviderCallback(this);

            await fusedLocationProviderClient.RequestLocationUpdatesAsync(locationRequest, fusedLocationCallback);

            Location myLocation = await fusedLocationProviderClient.GetLastLocationAsync();

            if (myLocation != null)
                SendBroadCastGPS(myLocation);

            IsStarted = true;
        }

        private async void DesligaFusedLocationProvider()
        {
            await fusedLocationProviderClient.RemoveLocationUpdatesAsync(fusedLocationCallback);
            fusedLocationProviderClient.Dispose();
            fusedLocationCallback.Dispose();
            IsStarted = false;
        }

        private void DesligaDefaultLocationProvider()
        {
            locMgr?.RemoveUpdates(this);
            locMgr?.Dispose();
            IsStarted = false;
        }

        private void SendBroadCastGPS(Location myLocation)
        {
            Intent intent = new Intent("GPS_UPDATE");
            intent.PutExtra("tipo", (int)SAMU192Core.Utils.Enums.BroadcastType.position);
            intent.PutExtra("latitude", (double)myLocation.Latitude);
            intent.PutExtra("longitude", (double)myLocation.Longitude);
            SendBroadcast(intent);
            LocalBroadcastManager.GetInstance(this).SendBroadcast(intent);
        }

        private void SendBroadCastGPSStatus(bool ativo)
        {
            Intent intent = new Intent("GPS_UPDATE");
            intent.PutExtra("tipo", (int)SAMU192Core.Utils.Enums.BroadcastType.status);
            intent.PutExtra("ativo", ativo);
            LocalBroadcastManager.GetInstance(this).SendBroadcast(intent);
        }

        public class FusedLocationProviderCallback : LocationCallback
        {
            GPSServico gpsServico;
            public FusedLocationProviderCallback(GPSServico _gpsServico)
            {
                gpsServico = _gpsServico;
            }

            public override void OnLocationAvailability(LocationAvailability locationAvailability)
            { }

            public override void OnLocationResult(LocationResult result)
            {
                try
                {
                    if (result.Locations != null && result.Locations.Count > 0)
                    {
                        gpsServico.SendBroadCastGPS(result.Locations[0]);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("OnLocationResult {0}{1}", ex.ToString(), System.Environment.NewLine));
                }
            }
        }
    }
}