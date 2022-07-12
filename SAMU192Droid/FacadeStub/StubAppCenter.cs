using System;
using SAMU192Core.Facades;
using SAMU192Core.Utils;
using SAMU192Droid.Implementations;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubAppCenter
    {
        internal static void Inicializa()
        {
            FacadeAppCenter.Inicializa(new NetworkConnectionImpl(), Constantes.APP_CENTER_ANDROID);
        }

        internal static void AppCrash(Exception ex, string tela = "", string detalhes = "")
        {
            FacadeAppCenter.AppCrash(ex, tela, detalhes);
        }

        internal static void AppCrash(Java.Lang.Exception ex, string tela = "", string detalhes = "")
        {
            Exception exception = new Exception(ex.Message, ex.InnerException);
            FacadeAppCenter.AppCrash(exception, tela, detalhes);
        }

        internal static void AppAnalytic(string msg, string tela = "", string detalhes = "")
        {
            FacadeAppCenter.AppAnalytic(msg, tela, detalhes);
        }
    }
}