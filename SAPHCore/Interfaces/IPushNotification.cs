using SAMU192Core.DTO;

namespace SAMU192Core.Interfaces
{
    public interface IPushNotification
    {
        string ServicoDisponivel(object arg1 = null);
        
        NotificacaoDTO ReceberMensagem(object message, object arg1);

        void ExecutaNotificacaoForeground(NotificacaoDTO notificacao, object args);

        void ExecutaNotificacaoBackground(NotificacaoDTO notificacao, object args);

        void ExecutaNotificacaoBadge(NotificacaoDTO notificacao, object args, int idxTab);

        string SelecionarTela(ModoNotificacao modo);
    }
    public enum ModoNotificacao { Avaliação = 1, Boletim = 2, Abertura = 3 }
}
