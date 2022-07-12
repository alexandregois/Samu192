using SAMU192Core.Facades;
using SAMU192Core.Interfaces;
using SAMU192iOS.Implementaitons;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubTelefonia
    {
        static TelefoniaImpl telefonia;
        internal static void Carrega(TelefoniaAbstract.LigacaoConectada ligacaoConectada, TelefoniaAbstract.LigacaoDesconectada ligacaoDesconectada, bool force = false)
        {
            if (force || telefonia == null || telefonia.PhoneManager == null)
            {
                telefonia = new TelefoniaImpl(ligacaoConectada, ligacaoDesconectada);
                FacadeTelefonia.Carregar(telefonia);
            }
        }

        public static bool FazerLigacao()
        {
            return FacadeTelefonia.FazerLigacao();
        }

        public static bool VerificarPermissao()
        {
            return FacadeTelefonia.VerificarPermissao(null);
        }
    }
}