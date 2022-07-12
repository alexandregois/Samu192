using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCAtualizarMensagemV1
    {
        enum Colunas
        {
            Versao = 0,
            FCMRegistration = 1,
            CodConversaMensagem = 2,
            HorarioRecebida = 3,
            HorarioLida = 4
        }

        const int NUM_COLUNAS = 5;
        public const string VERSAO = "DCAtualizarMensagemV1";

        public DCAtualizarMensagemV1()
        {

        }

        public DCAtualizarMensagemV1(string[] dados)
        {
            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Atualização de Mensagem: Quantidade incorreta de linhas!");

            if (dados[(int)Colunas.Versao] != VERSAO)
                throw new ApplicationException("Atualização de Mensagem: Versão incorreta de dados!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.FCMRegistration]))
                throw new ApplicationException("Atualização de Mensagem: FCM não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.CodConversaMensagem]))
                throw new ApplicationException("Atualização de Mensagem: CodConversaMensagem não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.HorarioRecebida]) && string.IsNullOrEmpty(dados[(int)Colunas.HorarioLida]))
                throw new ApplicationException("Atualização de Mensagem: Horário de recebimento ou horário de leitura da mensagem devem ser informados!");

            FCMRegistration = dados[(int)Colunas.FCMRegistration];
            CodConversaMensagemStr = dados[(int)Colunas.CodConversaMensagem];
            HorarioRecebidaStr = dados[(int)Colunas.HorarioRecebida];
            HorarioLidaStr = dados[(int)Colunas.HorarioLida];
        }

        public string FCMRegistration { get; set; }

        public Guid? CodConversaMensagem { get; set; }
        public string CodConversaMensagemStr
        {
            get
            {
                if (CodConversaMensagem.HasValue)
                {
                    return CodConversaMensagem.Value.ToString();
                }
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    if (Guid.TryParse(value, out Guid aux))
                    {
                        CodConversaMensagem = aux;
                    }
                    else
                        throw new ApplicationException("CodConversaMensagem deve ser um UUID.");
                }
                else
                    CodConversaMensagem = null;
            }
        }

        public DateTime? HorarioRecebida { get; set; }
        public string HorarioRecebidaStr
        {
            get
            {
                if (HorarioRecebida.HasValue)
                {
                    return HorarioRecebida.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
                        HorarioRecebida = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Registro deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioRecebida = null;
            }
        }

        public DateTime? HorarioLida { get; set; }
        public string HorarioLidaStr
        {
            get
            {
                if (HorarioLida.HasValue)
                {
                    return HorarioLida.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
                        HorarioLida = aux;
                    }
                    else
                        throw new ApplicationException("Horario de Leitura deve ser especificada no formato 'yyyy-MM-dd HH:mm:ss.fff'.");
                }
                else
                    HorarioLida = null;
            }
        }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.FCMRegistration] = FCMRegistration;
                resp[(int)Colunas.CodConversaMensagem] = CodConversaMensagemStr;
                resp[(int)Colunas.HorarioRecebida] = HorarioRecebidaStr;
                resp[(int)Colunas.HorarioLida] = HorarioLidaStr;

                return resp;
            }
        }
    }
}
