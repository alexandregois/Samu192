using System;
using SAMU192InterfaceService.DataContracts;
using SAMU192InterfaceService.Utils;

namespace SAMU192Core.DTO
{
    public class SolicitarAutorizacaoMidiaDTO
    {
        private DCSolicitarAutorizacaoMidiaV1 solicitacao;

        internal DCSolicitarAutorizacaoMidiaV1 Solicitacao { get { return solicitacao; } }

        public SolicitarAutorizacaoMidiaDTO() { 
            solicitacao = new DCSolicitarAutorizacaoMidiaV1();
        }

        public SolicitarAutorizacaoMidiaDTO(DCSolicitarAutorizacaoMidiaV1 solicitacao)
        {
            if (solicitacao == null) solicitacao = new DCSolicitarAutorizacaoMidiaV1();
            this.solicitacao = solicitacao;
        }

        public string Identificador { get { return solicitacao.Identificador; } set { solicitacao.Identificador = value; } }

    }
}
