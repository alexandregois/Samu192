using System;
using SAMU192Core.Interfaces;

namespace SAMU192Core.Facades
{
    public static class FacadeTelefonia
    {
        static TelefoniaAbstract telefonia;

        public static void Carregar(TelefoniaAbstract _telefonia)
        {
            telefonia = _telefonia;
        }

        public static bool FazerLigacao(object args = null)
        {
            return telefonia.MakeCall(Utils.Constantes.NUMERO_TELEFONE, args);
        }

        public static bool VerificarPermissao(object activity)
        {
            return telefonia.VerificaPermissao(activity);
        }
    }
}
