using System.Collections.Generic;
using Android.App;
using Firebase.Messaging;
using SAMU192Core.DTO;
using SAMU192Core.Facades;
using SAMU192Droid.Implementations;
using SAMU192Droid.Servicos;

namespace SAMU192Droid.FacadeStub
{
    internal static class StubPushNotifications
    {
        static PushNotificationImpl pushNotificatiion;

        internal static void Carregar(bool force = false)
        {
            if (force || pushNotificatiion == null)
                pushNotificatiion = new PushNotificationImpl();
            FacadePushNotifications.Carregar(pushNotificatiion);
        }

        internal static string ServicoDisponivel(Activity activity)
        {
            Carregar();
            return FacadePushNotifications.ServicoDisponivel(activity);
        }

        internal static void AtualizaToken(string newToken)
        {
            FacadePushNotifications.AtualizaToken(newToken);
        }

        internal static string Token()
        {
            return FacadePushNotifications.Token();
        }

        internal static void ReceberMensagem(RemoteMessage message, MyFirebaseMessagingService myFirebaseMessagingService)
        {
            FacadePushNotifications.ReceberMensagem(message, myFirebaseMessagingService);
        }

        internal static void ReceberMensagem(NotificacaoDTO message, Activity activity)
        {
            FacadePushNotifications.ReceberMensagem(message, activity);
        }
    }
}