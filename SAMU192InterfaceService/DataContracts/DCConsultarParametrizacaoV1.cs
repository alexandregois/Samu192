using System.Runtime.Serialization;
using System;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCConsultarParametrizacaoV1
    {
        enum Colunas
        {
            Versao = 0,
            Identificador = 1
        }

        const int NUM_COLUNAS = 2;
        public const string VERSAO = "DCConsultarParametrizacaoV1";

        public DCConsultarParametrizacaoV1()
        {

        }

        public DCConsultarParametrizacaoV1(string[] dados)
        {
            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Consulta de Parametrização: Quantidade incorreta de linhas!");

            if (dados[(int)Colunas.Versao] != VERSAO)
                throw new ApplicationException("Consulta de Parametrização: Versão incorreta de dados!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.Identificador]))
                throw new ApplicationException("Consulta Parametrização: Identificador do dispositivo não informado!");

            Identificador = dados[(int)Colunas.Identificador];

        }

        public string Identificador { get; set; }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;
                resp[(int)Colunas.Identificador] = Identificador;

                return resp;
            }
        }

        public class Parametrizacao
        {
            public enum eSituacaoAtendimentoChat
            {
                NaoPermitido = 0,
                Permitido = 1,
                EmAtendimento = 2
            }

            /// <summary>
            /// 0 - Não permite chat
            /// 1 - Permite chat
            /// 2 - Dispositivo em atendimento por chat
            /// </summary>
            public eSituacaoAtendimentoChat PermiteAtendimentoChat { get; set; }
        }
    }
}