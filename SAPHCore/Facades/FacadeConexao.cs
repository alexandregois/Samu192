using SAMU192Core.Interfaces;

namespace SAMU192Core.Facades
{
    public static class FacadeConexao
    {
        static INetworkConnection networkConnection;
        public static void Carrega(INetworkConnection _networkConnection)
        {
            networkConnection = _networkConnection;
        }

        public static INetworkConnection GetNetworkConnection()
        {
            return networkConnection;
        }
    }
}
