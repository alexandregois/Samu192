using System;
using System.Collections;
using System.Linq;
using Android.App;
using Android.Content;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192InterfaceService.Utils;

namespace SAMU192Droid.FacadeStub
{
    public static class StubWebService
    {
        static ServidorDTO _servidor;
        public static ServidorDTO Servidor { get => _servidor; set => _servidor = value; }

        public static void Carrega(Action _solicitarAtendimentoCallBack, Action<ServidorDTO> _solicitarAutorizacaoMidiaCallBack)
        {
            FacadeWebService.Carrega(_solicitarAtendimentoCallBack, _solicitarAutorizacaoMidiaCallBack);
        }

        public static void EnviaMidia(ServidorDTO servidor, SAMU192InterfaceService.Utils.Enums.TipoMidia tipoMidia, byte[] midia, Action envioDeMidiaCallBack)
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

        public static bool SolicitarAtendimentoChat(SolicitarAtendimentoChatDTO dados)
        {
            return FacadeWebService.SolicitarAtendimentoChat(dados);
        }

        public static string RetornaConsultaParametrizacao(string[] dados)
        {
            return FacadeWebService.RetornaConsultaParametrizacao(dados);
        }

        public static string BuscaMensagens(string[] dados)
        {
            return FacadeWebService.BuscarMensagens(dados);
        }

        public static string EnviarMensagens(string[] dados)
        {
            return FacadeWebService.EnviarMensagens(dados);
        }

        public static SolicitarAtendimentoDTO MontaPacote(bool outraPessoa, string queixa, CadastroDTO cadastro, EnderecoDTO endereco)
        {

            var pacote = FacadeWebService.MontaPacote(1, outraPessoa, queixa, cadastro, endereco);

            return pacote;
        }

        public static SolicitarAtendimentoChatDTO MontaPacoteChat(bool outraPessoa, string queixa, CadastroDTO cadastro, EnderecoDTO endereco)
        {

            var pacote = FacadeWebService.MontaPacoteChat(1, outraPessoa, queixa, cadastro, endereco);

            //StubUtilidades.MontaPacoteShared(pacote);

            return pacote;
        }

        public static SolicitarAutorizacaoMidiaDTO MontaSolicitacaoMidia()
        {
            var solicitacaoMidia = FacadeWebService.MontaSolicitacaoMidia();
            return solicitacaoMidia;
        }
    }
}