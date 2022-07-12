using SAMU192Core.Facades;
using SAMU192Droid.Implementations;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubConexao
    {
        internal static void Carrega()
        {
            FacadeConexao.Carrega(new NetworkConnectionImpl());
        }
    }
}