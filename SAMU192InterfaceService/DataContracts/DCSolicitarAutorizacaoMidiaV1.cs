using System;
using static SAMU192InterfaceService.Utils.Enums;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCSolicitarAutorizacaoMidiaV1
    {

        enum Colunas
        {
            versao = 0,
            identificador = 1
        }

        const int NUM_COLUNAS = 2;
        public const string VERSAO = "DCSolicitarAutorizacaoMidiaV1";

        public DCSolicitarAutorizacaoMidiaV1()
        {

        }

        public DCSolicitarAutorizacaoMidiaV1(string[] dados)
        {

            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Autorização de Mídia: Quantidade incorreta de informações!");

            if (dados[(int)Colunas.versao] != VERSAO)
                throw new ApplicationException("Autorização de Mídia: Versão incorreta de informações!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.identificador]))
                throw new ApplicationException("Autorização de Midia: Identificador do dispositivo não informado!");

            Identificador = dados[(int)Colunas.identificador];

        }


        public string Identificador { get; set; }
        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.identificador] = Identificador;
                resp[(int)Colunas.versao] = VERSAO;

                return resp;
            }
        }

    }
}
