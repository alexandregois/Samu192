using System;
using SAMU192InterfaceService.DataContracts;
using SAMU192InterfaceService.Utils;

namespace SAMU192Core.DTO
{
    public class SolicitarAtendimentoDTO
    {
        private DCSolicitarAtendimentoV1 solicitacao;

        internal DCSolicitarAtendimentoV1 Solicitacao { get { return solicitacao; } }

        public SolicitarAtendimentoDTO() { 
            solicitacao = new DCSolicitarAtendimentoV1();
        }

        public SolicitarAtendimentoDTO(DCSolicitarAtendimentoV1 solicitacao)
        {
            if (solicitacao == null) solicitacao = new DCSolicitarAtendimentoV1();
            this.solicitacao = solicitacao;
        }

        public string Identificador { get { return solicitacao.Identificador; } set { solicitacao.Identificador = value; } }
        public string FCMRegistration { get { return solicitacao.FCMRegistration; } set { solicitacao.FCMRegistration = value; } }
        public string Bairro { get { return solicitacao.Bairro; } set { solicitacao.Bairro = value; } }
        public string Cidade { get { return solicitacao.Cidade; } set { solicitacao.Cidade = value; } }
        public string Complemento { get { return solicitacao.Complemento; } set { solicitacao.Complemento = value; } }
        public DateTime? DataNascimento { get { return solicitacao.DataNascimento; } set { solicitacao.DataNascimento = value; } }
        public Double? Latitude { get { return solicitacao.Latitude; } set { solicitacao.Latitude = value; } }
        public Double? LatitudeApp { get { return solicitacao.LatitudeApp; } set { solicitacao.LatitudeApp = value; } }
        public string Logradouro { get { return solicitacao.Logradouro; } set { solicitacao.Logradouro = value; } }
        public Double? Longitude { get { return solicitacao.Longitude; } set { solicitacao.Longitude = value; } }
        public Double? LongitudeApp { get { return solicitacao.LongitudeApp; } set { solicitacao.LongitudeApp = value; } }
        public string Nome { get { return solicitacao.Nome; } set { solicitacao.Nome = value; } }
        public string Numero { get { return solicitacao.Numero; } set { solicitacao.Numero = value; } }
        public bool? OutraPessoa { get { return solicitacao.OutraPessoa; } set { solicitacao.OutraPessoa = value; } }
        public string Queixa { get { return solicitacao.Queixa; } set { solicitacao.Queixa = value; } }
        public string Referencia { get { return solicitacao.Referencia; } set { solicitacao.Referencia = value; } }
        public string Sexo { get { return solicitacao.Sexo; } set { solicitacao.Sexo = value; } }
        public int Sistema { get { return (int)solicitacao.Sistema; } set { solicitacao.Sistema = (Enums.SistemaOperacional)value; } }
        public string Telefone1 { get { return solicitacao.Telefone1; } set { solicitacao.Telefone1 = value; } }
        public string Telefone2 { get { return solicitacao.Telefone2; } set { solicitacao.Telefone2 = value; } }
        public string UF { get { return solicitacao.UF; } set { solicitacao.UF = value; } }

    }
}
