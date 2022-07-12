using System;
using SAMU192Core.Facades;
using SAMU192Core.Utils;
using SAMU192iOS.Implementations;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubAppCenter
    {
        internal static void Inicializa()
        {
            FacadeAppCenter.Inicializa(new NetworkConnectionImpl(), Constantes.APP_CENTER_IOS);
        }

        internal static void AppCrash(Exception ex, string tela = "", string detalhes = "")
        {
            FacadeAppCenter.AppCrash(ex, tela, detalhes);
        }

        internal static void AppCrash(Foundation.NSErrorException ex, string tela = "", string detalhes = "")
        {
            Exception exception = new Exception(ex.Message, new Exception("NSErrorException: " + ex.InnerException));
            FacadeAppCenter.AppCrash(exception, tela, detalhes);
        }

        internal static void AppAnalytic(string msg, string tela = "", string detalhes = "")
        {
            FacadeAppCenter.AppAnalytic(msg, tela, detalhes);
        }
    }
}