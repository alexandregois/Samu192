using Android.App;
using Android.Content;
using Android.Net;
using SAMU192Core.Interfaces;

namespace SAMU192Droid.Implementations
{
    public class NetworkConnectionImpl : INetworkConnection
    {
        public bool IsConnected
        {
            get
            {
                return CheckNetworkConnection();
            }
        }

        public bool CheckNetworkConnection()
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            return activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting;
        }

        private bool wifiOn = false, isWifiConnected = false;
        const string wifiStatusFormat = "WifiActivated: {0} | WifiConnected: {1}";
        private string wifiStatus = string.Format(wifiStatusFormat, false.ToString(), false.ToString());
        public string GetWifiStatus()
        {
            var wifiManager = Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService) as Android.Net.Wifi.WifiManager;
            if (wifiManager != null)
            {
                wifiOn = wifiManager.IsWifiEnabled;
                isWifiConnected = wifiManager.IsWifiEnabled && (wifiManager.ConnectionInfo.NetworkId != -1
                    && wifiManager.ConnectionInfo.SSID != "<unknown SSID>");

                wifiStatus = string.Format(wifiStatusFormat, wifiOn.ToString(), isWifiConnected.ToString());
            }
            return wifiStatus;
        }
    }
}