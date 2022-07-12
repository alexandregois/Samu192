using System.Runtime.Serialization;
using System;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCConsultarParametrizacaoV1
    {
        enum Colunas
        {
            Versao = 0
        }

        const int NUM_COLUNAS = 1;
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
        }

        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.Versao] = VERSAO;

                return resp;
            }
        }

        public class Parametrizacao
        {
            public bool? PermiteAtendimentoChat { get; set; }
        }
    }
}