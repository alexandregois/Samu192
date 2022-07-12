using System;
using System.Collections.Generic;
using Firebase.CloudMessaging;
using Foundation;
using SAMU192Core.Facades;
using SAMU192iOS.Implementaions;
using UIKit;
using UserNotifications;

namespace SAMU192iOS.FacadeStub
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

        internal static void ServicoDisponivel()
        {
            Carregar();
            FacadePushNotifications.ServicoDisponivel();
        }

        internal static void NotificacaoRecebida(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            FacadePushNotifications.ReceberMensagem(userInfo, application);
        }

        internal static void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Messaging.SharedInstance.ApnsToken = deviceToken;
            pushNotificatiion.Connectar();
            Firebase.InstanceID.InstanceId.SharedInstance.SetApnsToken(deviceToken, Firebase.InstanceID.ApnsTokenType.Unknown);
        }

        internal static void DesabilitaDirectChannel()
        {
            Messaging.SharedInstance.ShouldEstablishDirectChannel = false;
        }

        internal static void AtualizaToken(string newToken)
        {
            FacadePushNotifications.AtualizaToken(newToken);
        }

        internal static string Token()
        {
            return FacadePushNotifications.Token();
        }

        internal static void ArmazenaNavigationControler(UINavigationController navigationController)
        {
            pushNotificatiion.ArmazenaNavigationControler(navigationController);
        }

        internal static void ArmazenaTabControler(UITabBarController tabBarController)
        {
            pushNotificatiion.ArmazenaTabControler(tabBarController);
        }

        // Receive data message on iOS 10 devices.
        internal static void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
        {
            //Console.WriteLine(remoteMessage.AppData);
        }

        internal static List<int> NotificacoesPendentes()
        {
            return FacadePushNotifications.NotificacoesPendentes();
        }

        internal static void RemoveNotificacaoPendente(int modo)
        {
            FacadePushNotifications.RemoveNotificacaoPendente(modo);
        }
    }
}