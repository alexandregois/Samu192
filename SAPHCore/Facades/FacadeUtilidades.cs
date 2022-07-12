using SAMU192Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;

namespace SAMU192Core.Facades
{
    public static class FacadeUtilidades
    {
        private static string InstanceID = null;
        private static Servicos.Local localService;
        private static SolicitarAutorizacaoMidiaDTO solicitacao;

        public static void SetCultureInfo()
        {
            var userSelectedCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = userSelectedCulture;
        }

        public static void SalvaInstanceID(string id)
        {
            InstanceID = id;
            FCMInstanceIdDTO fcmInstanceID = new FCMInstanceIdDTO(InstanceID);
            FacadeCadastro.Salvar<FCMInstanceIdDTO>(fcmInstanceID);
        }

        public static string RecuperaInstanceID()
        {
            FCMInstanceIdDTO fcmInstanceID = FacadeCadastro.Recuperar<FCMInstanceIdDTO>();
            InstanceID = fcmInstanceID.ID;
            return InstanceID;
        }

        private static readonly char[] BaseChars = "FG1H3024598IJLKMWN76OPSRQTVUACDYBZEX".ToCharArray();
        private static readonly Dictionary<char, int> CharValues = BaseChars
                   .Select((c, i) => new { Char = c, Index = i })
                   .ToDictionary(c => c.Char, c => c.Index);

        private static string LongToBase(long value)
        {
            long targetBase = BaseChars.Length;
            char[] buffer = new char[Math.Max(
                       (int)Math.Ceiling(Math.Log(value + 1, targetBase)), 1)];

            var i = (long)buffer.Length;
            do
            {
                buffer[--i] = BaseChars[value % targetBase];
                value = value / targetBase;
            }
            while (value > 0);
            return new string(buffer);
        }

        private static long BaseToLong(string number)
        {
            char[] chrs = number.ToCharArray();
            int m = chrs.Length - 1;
            int n = BaseChars.Length, x;
            long result = 0;
            for (int i = 0; i < chrs.Length; i++)
            {
                x = CharValues[chrs[i]];
                result += x * (long)Math.Pow(n, m--);
            }
            return result;
        }

        public static string Gerar(DateTime dtNow)
        {
            string valorMontado = dtNow.Hour.ToString("00") + dtNow.Minute.ToString("00"); //hhmm
            long numb = long.Parse(valorMontado);

            string code = LongToBase(numb);
            string check = LongToBase(dtNow.DayOfYear);

            return code + "-" + check;
        }

        public static void ValidarCodigoPIN(DateTime dtNow, string pin)
        {
            try
            {
                if (!pin.Contains("-"))
                    throw new ValidationException("Código Inválido");

                string[] splited = pin.Split('-');
                long valorCheck = BaseToLong(splited[1]);
                long valorPIN = BaseToLong(splited[0]);

                DateTime dateCheck = new DateTime(DateTime.Now.Year, 1, 1).AddDays(valorCheck - 1);

                string valorMontado = valorPIN.ToString("0000");
                DateTime dtTmp = new DateTime(
                    dateCheck.Year,
                    dateCheck.Month,
                    dateCheck.Day,
                    Int16.Parse(valorMontado.Substring(0, 2)),
                    Int16.Parse(valorMontado.Substring(2, 2)),
                    0);

                DateTime dtTmpMin = dtTmp.AddMinutes(-5);
                DateTime dtTmpMax = dtTmp.AddMinutes(5);

                if (dtNow < dtTmpMin || dtNow > dtTmpMax)
                    throw new ValidationException("Código Inválido");
            }
            catch
            {
                throw new ValidationException("Código Inválido");
            }
        }

        public static bool LigaServico(Action<ServidorDTO> callBack, SolicitarAutorizacaoMidiaDTO _solicitacao)
        {
            if (localService == null)
            {
                localService = new Servicos.Local();
                localService.Liga(callBack, ExecutarServico);
            }
            solicitacao = _solicitacao;
            return true;
        }

        public static ServidorDTO ExecutarServico()
        {
            FacadeWebService.SolicitarAutorizacaoMidia(solicitacao);
            return null;
        }

        public static bool DesligaServico()
        {
            localService?.Desliga();
            localService = null;
            solicitacao = null;
            return true;
        }

        public static bool AppEmProducao()
        {
            return Utils.Constantes.APP_EM_PRODUCAO;
        }

        public static string GetWifiStatus(INetworkConnection networkConnection)
        {
            return networkConnection.GetWifiStatus();
        }
    }
}
