namespace SAMU192Core.Interfaces
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        bool CheckNetworkConnection();
        string GetWifiStatus();
    }
}
