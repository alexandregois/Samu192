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
            Identificador = 1,
            CodConversaMensagem = 2,
            AtualizaHorarioRecebida = 3,
            AtualizaHorarioLida = 4
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

            if (string.IsNullOrEmpty(dados[(int)Colunas.Identificador]))
                throw new ApplicationException("Atualização de Mensagem: Identificador não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.CodConversaMensagem]))
                throw new ApplicationException("Atualização de Mensagem: CodConversaMensagem não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.AtualizaHorarioRecebida]) && string.IsNullOrEmpty(dados[(int)Colunas.AtualizaHorarioLida]))
                throw new ApplicationException("Atualização de Mensagem: Horário de recebimento OU horário de leitura da mensagem devem ser informados!");

            Identificador = dados[(int)Colunas.Identificador];
            CodConversaMensagemStr = dados[(int)Colunas.CodConversaMensagem];
            AtualizaHorarioRecebidaStr = dados[(int)Colunas.AtualizaHorarioRecebida];
            AtualizaHorarioLidaStr = dados[(int)Colunas.AtualizaHorarioLida];
        }

        public string Identificador { get; set; }

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

        public bool? AtualizaHorarioRecebida { get; set; }
        public string AtualizaHorarioRecebidaStr
        {
            get
            {
                if (AtualizaHorarioRecebida.HasValue)
                {
                    return AtualizaHorarioRecebida.Value ? "1" : "0";
                }
                else
                    return "1";
            }
            set
            {
                AtualizaHorarioRecebida = true;

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    AtualizaHorarioRecebida = (value == "1");
                }
            }
        }

        public bool? AtualizaHorarioLida { get; set; }
        public string AtualizaHorarioLidaStr
        {
            get
            {
                if (AtualizaHorarioLida.HasValue)
                {
                    return AtualizaHorarioLida.Value ? "1" : "0";
                }
                else
                    return "1";
            }
            set
            {
                AtualizaHorarioLida = true;

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    AtualizaHorarioLida = (value == "1");
                }
            }
        }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.Identificador] = Identificador;
                resp[(int)Colunas.CodConversaMensagem] = CodConversaMensagemStr;
                resp[(int)Colunas.AtualizaHorarioRecebida] = AtualizaHorarioRecebidaStr;
                resp[(int)Colunas.AtualizaHorarioLida] = AtualizaHorarioLidaStr;

                return resp;
            }
        }
    }
}
