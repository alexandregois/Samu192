using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using System.Collections.Generic;

namespace SAMU192Core.Facades
{
    public static class FacadePushNotifications
    {
        static IPushNotification pushNotification;
        static string token;
        static List<int> pendentes = new List<int>();
        public static void Carregar(IPushNotification _pushNotification)
        {
            pushNotification = _pushNotification;
        }

        public static string ServicoDisponivel(object activity = null)
        {
            return pushNotification.ServicoDisponivel(activity);
        }

        public static void AtualizaToken(string newToken)
        {
            token = newToken;
            FCMTokenDTO fcmToken = new FCMTokenDTO(newToken);
            FacadeCadastro.Salvar<FCMTokenDTO>(fcmToken);
        }

        public static string Token()
        {
            FCMTokenDTO fcmToken = FacadeCadastro.Recuperar<FCMTokenDTO>();
            return fcmToken.Token;
        }

        public static void ReceberMensagem(object message, object args)
        {
            var notificacao = pushNotification.ReceberMensagem(message, args);
            TratamentoNotificacao(notificacao, args);
        }

        private static void TratamentoNotificacao(NotificacaoDTO notificacao, object args)
        {
            switch (notificacao.Modo)
            {
                case ModoNotificacao.Avaliação:
                    ExecutaNotificacao(notificacao, args, 3); //ExecutaNotificacao(notificacao, args);
                    break;
                case ModoNotificacao.Boletim:
                    ExecutaNotificacao(notificacao, args, 3);
                    break;
                case ModoNotificacao.Abertura:
                    ExecutaNotificacao(notificacao, args, 2);
                    break;
            }
        }

        private static void ExecutaNotificacao(NotificacaoDTO notificacao, object args)
        {
            if (notificacao.FromBackground)
                pushNotification.ExecutaNotificacaoBackground(notificacao, args);
            else
                pushNotification.ExecutaNotificacaoForeground(notificacao, args);
        }

        private static void ExecutaNotificacao(NotificacaoDTO notificacao, object args, int idxTabForBadge)
        {
            pushNotification.ExecutaNotificacaoBadge(notificacao, args, idxTabForBadge);
            RemoveNotificacaoPendente((int)notificacao.Modo);
            pendentes.Add((int)notificacao.Modo);
        }

        public static void RemoveNotificacaoPendente(int modo)
        {
            if (pendentes.Contains(modo))
                pendentes.Remove(modo);
        }

        public static List<int> NotificacoesPendentes()
        {
            return pendentes;
        }
    }
}
