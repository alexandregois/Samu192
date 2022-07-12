using SAMU192Core.Interfaces;

namespace SAMU192Core.DTO
{
    public class NotificacaoDTO
    {
        public NotificacaoDTO()
        { }

        public NotificacaoDTO(string titulo, string mensagem, int codChamado, ModoNotificacao modo, bool fromBackground)
        {
            this.Titulo = titulo;
            this.Mensagem = mensagem;
            this.Modo = modo;
            this.CodChamado = codChamado;
            this.FromBackground = fromBackground;
        }

        string titulo;
        string mensagem;
        int codChamado;
        ModoNotificacao modo;
        bool fromBackground;

        public string Mensagem { get => mensagem; set => mensagem = value; }
        public int CodChamado { get => codChamado; set => codChamado = value; }
        public ModoNotificacao Modo { get => modo; set => modo = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public bool FromBackground { get => fromBackground; set => fromBackground = value; }

        public override string ToString()
        {
            return string.Format("Notificação{ Título: " + Titulo + " Mensagem: " + Mensagem + " Modo: " + modo.ToString() + " CodChamado: " + CodChamado + " Background: " + FromBackground.ToString() + " }");
        }
    }
}
