using SAMU192Core.Facades;
using SAMU192Core.Interfaces;
using SAMU192Droid.Implementaitons;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubTelefonia
    {
        static TelefoniaImpl telefonia;
        internal static void Carrega(TelefoniaAbstract.LigacaoConectada ligacaoConectada, TelefoniaAbstract.LigacaoDesconectada ligacaoDesconectada, object phoneManager, bool force = false)
        {
            if (force || telefonia == null || telefonia.PhoneManager == null)
            {
                telefonia = new TelefoniaImpl(ligacaoConectada, ligacaoDesconectada, phoneManager);
                FacadeTelefonia.Carregar(telefonia);
            }
        }

        internal static void Descarrega()
        {
            telefonia = new TelefoniaImpl(null, null, null);
            FacadeTelefonia.Carregar(telefonia);
        }

        public static bool FazerLigacao(object activity)
        {
            return FacadeTelefonia.FazerLigacao(activity);
        }

        internal static bool VerificarPermissao(object activity)
        {
            return FacadeTelefonia.VerificarPermissao(activity);
        }
    }
}