using SAMU192Core.Facades;
using SAMU192iOS.Implementations;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubConexao
    {
        internal static void Carrega()
        {
            FacadeConexao.Carrega(new NetworkConnectionImpl());
        }
    }
}