using System;
using static SAMU192InterfaceService.Utils.Enums;

namespace SAMU192InterfaceService.DataContracts
{
    public class DCEnviarMidiaV1
    {

        enum Colunas
        {
            versao = 0,
            tipoMidia = 1,
            identificador = 2,
            nomeArquivo = 3
        }

        const int NUM_COLUNAS = 4;
        public const string VERSAO = "DCEnviarMidiaV1";

        public DCEnviarMidiaV1()
        {

        }

        public DCEnviarMidiaV1(string[] dados)
        {

            if (dados.Length != NUM_COLUNAS)
                throw new ApplicationException("Envio de Mídia: Quantidade incorreta de informações!");

            if (dados[(int)Colunas.versao] != VERSAO)
                throw new ApplicationException("Envio de Mídia: Versão incorreta de informações!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.identificador]))
                throw new ApplicationException("Envio de Mídia: Identificador do dispositivo não informado!");

            if (string.IsNullOrEmpty(dados[(int)Colunas.nomeArquivo ]))
                throw new ApplicationException("Envio de Mídia: Nome do arquivo não informado!");

            Identificador = dados[(int)Colunas.identificador];
            TipoMidiaStr = dados[(int)Colunas.tipoMidia];
            NomeArquivo = dados[(int)Colunas.nomeArquivo];

        }


        public string Identificador { get; set; }
        public string NomeArquivo { get; set; }
        public TipoMidia TipoMidia { get; set; }
        public string TipoMidiaStr
        {
            get
            {
                return ((int)TipoMidia).ToString();
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        TipoMidia = (TipoMidia)int.Parse(value);
                    }
                    catch
                    {
                        TipoMidia = TipoMidia.Indefinido;
                    }
                }
                else
                    TipoMidia = TipoMidia.Indefinido;
            }
        }
        public string[] Dados
        {
            get
            {
                string[] resp = new string[NUM_COLUNAS];

                resp[(int)Colunas.identificador] = Identificador;
                resp[(int)Colunas.nomeArquivo] = NomeArquivo;
                resp[(int)Colunas.tipoMidia] = TipoMidiaStr;
                resp[(int)Colunas.versao] = VERSAO;

                return resp;
            }
        }

    }
}
