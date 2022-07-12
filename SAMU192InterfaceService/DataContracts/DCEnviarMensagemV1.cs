using System;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCEnviarMensagemV1
    {
        enum Colunas
        {
            Versao = 0,
            FCMRegistration = 1,
            HorarioRegistro = 2,
            Mensagem = 3
        }

        const int NUM_COLUNAS = 4;
        public const string VERSAO = "DCEnviarMensagemV1";

        public DCEnviarMensagemV1()
        {

        }

        public DCEnviarMensagemV1(string[] dados)
        {
            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Envio de Mensagem: Quantidade incorreta de linhas!");

            if (dados[(int)Colunas.Versao] != VERSAO)
                throw new ApplicationException("Envio de Mensagem: Versão incorreta de dados!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.FCMRegistration]))
                throw new ApplicationException("Envio de Mensagem: FCM não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.HorarioRegistro]))
                throw new ApplicationException("Envio de Mensagem: Horário de registro da mensagem não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.Mensagem]))
                throw new ApplicationException("Envio de Mensagem: Mensagem não informada!");

            FCMRegistration = dados[(int)Colunas.FCMRegistration];
            HorarioRegistroStr = dados[(int)Colunas.HorarioRegistro];
            Mensagem = dados[(int)Colunas.Mensagem];
        }

        public string FCMRegistration { get; set; }
        public string Mensagem { get; set; }
        public DateTime? HorarioRegistro { get; set; }
        public string HorarioRegistroStr
        {
            get
            {
                if (HorarioRegistro.HasValue)
                {
                    return HorarioRegistro.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime aux))
                    {
                        HorarioRegistro = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Registro deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioRegistro = null;
            }
        }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.FCMRegistration] = FCMRegistration;
                resp[(int)Colunas.HorarioRegistro] = HorarioRegistroStr;
                resp[(int)Colunas.Mensagem] = Mensagem;

                return resp;
            }
        }
    }
}