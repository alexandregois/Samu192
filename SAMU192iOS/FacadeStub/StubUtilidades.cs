using SAMU192Core.DTO;
using SAMU192Core.Facades;
using System;

namespace SAMU192iOS.FacadeStub
{
    internal static class StubUtilidades
    {
        public static void SetCultureInfo()
        {
            FacadeUtilidades.SetCultureInfo();
        }

        public static void ValidarCodigoPIN(DateTime dtNow, string pin)
        {
            FacadeUtilidades.ValidarCodigoPIN(dtNow, pin);
        }

        public static string Gerar(DateTime dtNow)
        {
            return FacadeUtilidades.Gerar(dtNow);
        }

        internal static string RecuperaInstanceID()
        {
            string instanceID = FacadeUtilidades.RecuperaInstanceID();
            return instanceID;
        }

        internal static void SalvaInstanceID(string id)
        {
            FacadeUtilidades.SalvaInstanceID(id);
        }

        internal static void LigaServico(Action<ServidorDTO> callback)
        {
            SolicitarAutorizacaoMidiaDTO solicitacao = StubWebService.MontaSolicitacaoMidia();
            FacadeUtilidades.LigaServico(callback, solicitacao);
        }

        internal static void DesligaServico()
        {
            FacadeUtilidades.DesligaServico();
        }

        internal static bool AppEmProducao()
        {
            return FacadeUtilidades.AppEmProducao();
        }
    }
}