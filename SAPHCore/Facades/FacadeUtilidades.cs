using SAMU192Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;
using Newtonsoft.Json;

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

        public static string[] DeserializaPacote(String s, bool isObjeto)
        {
            String[] retorno;

            if (isObjeto == true)
            {
                var Json = JsonConvert.DeserializeObject<List<object>>(s);
                retorno = Json.Select(x => x.ToString()).ToArray();
            }
            else
            {
                var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
                retorno = new string[2];
                foreach (KeyValuePair<string, string> chave in result)
                {
                    retorno[0] = chave.Key;
                    retorno[1] = chave.Value;
                }
                
            }

            return retorno;
        }

        public static String[] MontaPacoteBuscaMensagem(Enums.BuscarMensagens buscarMensagens, DateTime data)
        {
            String[] retorno = new string[18];

            switch (buscarMensagens)
            {
                case Enums.BuscarMensagens.MensagensNovas:
                    {
                        retorno = new string[15];
                        retorno[0] = "DCBuscarMensagemV1";
                        retorno[1] = FacadeUtilidades.RecuperaInstanceID().ToString();
                        retorno[2] = "5";
                        retorno[3] = "1";
                        retorno[4] = "2";
                        retorno[5] = null;
                        retorno[6] = null;
                        retorno[7] = null;
                        retorno[8] = null;
                        retorno[9] = null;
                        retorno[10] = null;
                        retorno[11] = null;
                        retorno[12] = null;
                        retorno[13] = null;
                        retorno[14] = null;

                        break;
                    }
                case Enums.BuscarMensagens.MensagensNaoLidas:
                    {
                        retorno = new string[15];
                        retorno[0] = "DCBuscarMensagemV1";
                        retorno[1] = FacadeUtilidades.RecuperaInstanceID().ToString();
                        retorno[2] = "5";
                        retorno[3] = "1";
                        retorno[4] = "1";
                        retorno[5] = "";
                        retorno[6] = "";
                        retorno[7] = "";
                        retorno[8] = "";
                        retorno[9] = "";
                        retorno[10] = "";
                        retorno[11] = "";
                        retorno[12] = "";
                        retorno[13] = "";
                        retorno[14] = "";

                        break;
                    }
                case Enums.BuscarMensagens.MensagensRecebidas:
                    {
                        retorno = new string[15];
                        retorno[0] = "DCBuscarMensagemV1";
                        retorno[1] = FacadeUtilidades.RecuperaInstanceID().ToString();
                        retorno[2] = "5";
                        retorno[3] = "1";
                        retorno[4] = "1";
                        retorno[5] = "";
                        retorno[6] = "";
                        retorno[7] = "";
                        retorno[8] = "";
                        retorno[9] = "";
                        retorno[10] = "";
                        retorno[11] = "";
                        retorno[12] = "";
                        retorno[13] = "";
                        retorno[14] = "";

                        break;
                    }
                default: break;
            }

            return retorno;
        }

        public static String[] MontaPacotePermiteChat()
        {
            String[] retorno = new string[2];

            retorno[0] = "DCConsultarParametrizacaoV1";
            retorno[1] = FacadeUtilidades.RecuperaInstanceID().ToString();

            return retorno;
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
