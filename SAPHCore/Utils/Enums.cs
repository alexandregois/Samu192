namespace SAMU192Core.Utils
{
    public static class Enums
    {
        public enum PhoneCallState
        {
            Disconnected = 0,
            Connected = 1
        }

        public enum FileName
        {
            Indefinido = 0,
            Compartilhar = 1
        }

        public enum Broadcast
        {
            Indefinido,
            TermosCondicoesBaixado,
            TermosCondicoesExibir,
            TermosCondicoesAceitar,
            BoletimBaixado,
            AcompanhamentoAtualizacao,
            AcompanhamentoRetornoEnviar,
            Favoritos,
            EnderecoSelecionado,
            EnderecoGravado,
            AcompanhamentoDisponivel,
            BoletimDisponivel,
            PesquisaDisponivel,
            ChamadoAbertura,
            Ligar,
            CadastroSalvo,
            Storage,
            TermosCondicoesDownloadErro,
            RetornoMapa
        }

        public enum BroadcastType
        {
            position = 99,
            status = 55
        }

        public enum ImagemPosicionarNoMapa
        {
            Indefinido = 0,
            Ambulancia = 1
        }

        public enum Gravidade
        {
            SemLesao,
            Pequena,
            Media,
            Severa,
            Morte,
            Indeterminada
        }

        public enum AcompanhamentoOrigemImagens
        {
            Camera,
            CameraGaleria,
        }

        public enum AcompanhamentoTipoImagens
        {
            Nenhum,
            Foto,
            FotoVideo,
        }


        public enum RequestPermissionCode
        {
            GPS,
            Telefone,
            Storage,
            Camera
        }

        public enum ActivityResult
        {
            Foto = 37532,
            Video = 37533,
            Galeria = 37534,
            GPS = 37535
        }

        public class AnalyticsType
        {
            public string Value { get; private set; }

            private AnalyticsType(string value) { this.Value = value; }

            public static AnalyticsType Indefinido { get { return new AnalyticsType("Indefinido"); } }

            public static AnalyticsType WalkThrough_Iniciado { get { return new AnalyticsType("WalkThrough_Iniciado"); } }
            public static AnalyticsType WalkThrough_Termo_Aceite { get { return new AnalyticsType("WalkThrough_Termo_Aceite"); } }
           
            public static AnalyticsType GPS_Pedido_Permissao { get { return new AnalyticsType("GPS_Pedido_Permissao"); } }
            public static AnalyticsType GPS_Negado_Permissao { get { return new AnalyticsType("GPS_Negado_Permissao"); } }
            public static AnalyticsType GPS_Aceito_Permissao { get { return new AnalyticsType("GPS_Negado_Permissao"); } }

            public static AnalyticsType Telefonia_Aceito_Permissao { get { return new AnalyticsType("WalkThrough_Telefonia_Aceite_Permissao"); } }
            public static AnalyticsType Telefonia_Negado_Permissao { get { return new AnalyticsType("WalkThrough_Telefonia_Negado_Permissao"); } }

            public static AnalyticsType EnvioPacote_Validacao { get { return new AnalyticsType("EnvioPacote_Validacao"); } }

            public static AnalyticsType Ligar { get { return new AnalyticsType("Ligar"); } }
            public static AnalyticsType Ligar_ParaMim { get { return new AnalyticsType("Ligar_ParaMim"); } }
            public static AnalyticsType Ligar_ParaOutraPessoa { get { return new AnalyticsType("Ligar_ParaOutraPessoa"); } }

            public static AnalyticsType Chat_ParaMim { get { return new AnalyticsType("Ligar_ParaMim"); } }
            public static AnalyticsType Chat_ParaOutraPessoa { get { return new AnalyticsType("Ligar_ParaOutraPessoa"); } }

            public static AnalyticsType Foto_Click { get { return new AnalyticsType("Foto_Click"); } }
            public static AnalyticsType Foto_ExibirBotao { get { return new AnalyticsType("Foto_ExibirBotao"); } }

            public static AnalyticsType Cadastro_Efetuado { get { return new AnalyticsType("Cadastro_Efetuado"); } }
            public static AnalyticsType Favorito_Cadastrado { get { return new AnalyticsType("Favorito_Cadastrado"); } }

            public static AnalyticsType ForaDeArea { get { return new AnalyticsType("ForaDeArea"); } }

            public static AnalyticsType WalkThrough_Completado { get { return new AnalyticsType("WalkThrough_Completado"); } }

            public static AnalyticsType WalkThrough_Fechado { get { return new AnalyticsType("WalkThrough_Fechado"); } }
        }

    }
}