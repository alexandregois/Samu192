﻿using System;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192InterfaceService.Utils;

namespace SAMU192iOS.FacadeStub
{
    public static class StubWebService
    {
        static ServidorDTO _servidor;
        public static ServidorDTO Servidor { get => _servidor; set => _servidor = value; }

        public static void Carrega(Action _solicitarAtendimentoCallBack, Action<ServidorDTO> _solicitarAutorizacaoMidiaCallBack)
        {
            FacadeWebService.Carrega(_solicitarAtendimentoCallBack, _solicitarAutorizacaoMidiaCallBack);
        }

        public static void EnviaMidia(ServidorDTO servidor, Enums.TipoMidia tipoMidia, byte[] midia, Action envioDeMidiaCallBack)
        {
            EnviarMidiaDTO dados = new EnviarMidiaDTO();
            dados.TipoMidia = tipoMidia;
            dados.Identificador = StubUtilidades.RecuperaInstanceID();
            dados.NomeArquivo = MontaNomeArquivo(tipoMidia);
            FacadeWebService.EnviarMidia(servidor, dados, midia, envioDeMidiaCallBack);
        }

        private static string MontaNomeArquivo(Enums.TipoMidia tipoMidia)
        {
            string extensao = (tipoMidia == SAMU192InterfaceService.Utils.Enums.TipoMidia.Foto ? "png" : "mp4");
            string nomeArquivo = string.Format("{0}_{1}.{2}", StubUtilidades.RecuperaInstanceID(), DateTime.Now.TimeOfDay.Ticks.ToString(), extensao);

            return nomeArquivo;
        }

        public static void SolicitarAtendimento(SolicitarAtendimentoDTO dados)
        {
            FacadeWebService.SolicitarAtendimento(dados);
        }

        public static SolicitarAtendimentoDTO MontaPacote(bool outraPessoa, string queixa, CadastroDTO cadastro, EnderecoDTO endereco)
        {
            var pacote = FacadeWebService.MontaPacote(2, outraPessoa, queixa, cadastro, endereco);
            return pacote;
        }

        public static SolicitarAutorizacaoMidiaDTO MontaSolicitacaoMidia()
        {
            var solicitacaoMidia = FacadeWebService.MontaSolicitacaoMidia();
            return solicitacaoMidia;
        }
    }
}