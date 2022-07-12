using UIKit;
using System;
using Foundation;
using UserNotifications;
using Firebase.CloudMessaging;
using SAMU192Core.Interfaces;
using SAMU192iOS.FacadeStub;
using SAMU192Core.DTO;
using SAMU192iOS.ViewControllers;

namespace SAMU192iOS.Implementaions
{
    internal class PushNotificationImpl : IPushNotification
    {
        public class NotifyDelegate: UNUserNotificationCenterDelegate
        {

        }
        NotifyDelegate notifyDelegate;
        UINavigationController navigationController;
        UITabBarController tabBarController;

        public string ServicoDisponivel(object arg1 = null)
        {
            //iOS 10 ou posterior
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    if (granted)
                    {
                        new NSObject().InvokeOnMainThread(() => {
                            UIApplication.SharedApplication.RegisterForRemoteNotifications();
                        });
                    }
                });
                // APNS
                notifyDelegate = new NotifyDelegate();
                UNUserNotificationCenter.Current.Delegate = (IUNUserNotificationCenterDelegate)this.notifyDelegate;
            }
            else
            {
                // iOS 9 ou anterior
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                new NSObject().InvokeOnMainThread(() => {
                    UIApplication.SharedApplication.RegisterForRemoteNotifications();
                });
            }            

            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh((sender, e) =>
            {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                StubPushNotifications.AtualizaToken(newToken);
                Connectar();
            });

            return "";
        }

        internal void ArmazenaNavigationControler(UINavigationController _navigationController)
        {
            navigationController = _navigationController;
        }

        internal void ArmazenaTabControler(UITabBarController _tabBarController)
        {
            tabBarController = _tabBarController;
        }


        public NotificacaoDTO ReceberMensagem(object message, object arg1)
        {
            UIApplication application = (UIApplication)arg1;
            NSDictionary userInfo = (NSDictionary)message;
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            bool fromBackground = (application.ApplicationState != UIApplicationState.Active);
            
            var aps_d = userInfo["aps"] as NSDictionary;
            var alert_d = aps_d["alert"] as NSDictionary;
            //var body = userInfo["google.c.a.c_l"] as NSString;
            var body = alert_d["body"];
            var title = alert_d["title"];

            NSObject obj;
            ModoNotificacao modo;

            if (userInfo.TryGetValue(new NSString("Modo"), out obj))
                modo = (ModoNotificacao)int.Parse(obj.Description);
            else
                modo = (ModoNotificacao)int.Parse(userInfo.Values[1].Description != null ? userInfo.Values[1].Description : "-1");
            int codChamado = int.Parse(userInfo.Values[2].Description != null ? userInfo.Values[2].Description : "-1");
            string titulo = !string.IsNullOrEmpty(title.Description) ? title.Description : string.Empty;
            string mensagem = !string.IsNullOrEmpty(body.Description) ? body.Description : string.Empty;

            NotificacaoDTO notificacao = new NotificacaoDTO(titulo, mensagem, codChamado, modo, fromBackground);

            return notificacao;
        }

        internal void Connectar()
        {
            try
            {
                if (Firebase.Core.App.DefaultInstance == null)
                    Firebase.Core.App.Configure();
            }
            catch
            {
                //dummy
            }
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }

        public async void ExecutaNotificacaoForeground(NotificacaoDTO notificacao, object args)
        {
            try
            {
                string tela = SelecionarTela(notificacao.Modo);
                UIApplication app = (UIApplication)args;
                var navController = (navigationController == null ? Utils.Interface.GetNavigationController(app.KeyWindow.RootViewController) : navigationController);
                int button2 = await Utils.Mensagem.Questao("Notificação", notificacao.Mensagem, (n) => { }, navController, "Descartar", "Acessar");
                if (button2 > 0)
                    Utils.Interface.EmpurrarViewController(app.KeyWindow.RootViewController, tela, navController);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public void ExecutaNotificacaoBackground(NotificacaoDTO notificacao, object args)
        {
            string tela = SelecionarTela(notificacao.Modo);
            UIApplication app = (UIApplication)args;
            var navController = (navigationController == null ? Utils.Interface.GetNavigationController(app.KeyWindow.RootViewController) : navigationController);
            Utils.Interface.EmpurrarViewController(app.KeyWindow.RootViewController, tela, navController);
        }

        public string SelecionarTela(ModoNotificacao modo)
        {
            string tela = string.Empty;
            switch (modo)
            {
                case ModoNotificacao.Avaliação:
                    tela = "PesquisaSatisfacaoViewController";
                    break;
                case ModoNotificacao.Boletim:
                    tela = "BoletimOnlineViewController";
                    break;
            }
            return tela;
        }

        public void ExecutaNotificacaoBadge(NotificacaoDTO notificacao, object args, int idxTab)
        {
            throw new NotImplementedException();
        }
    }
}