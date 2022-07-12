using Foundation;
using UIKit;
using SAMU192iOS.ViewControllers;
using SAMU192iOS.FacadeStub;
using System;
using SAMU192Core.Exceptions;

namespace SAMU192iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        public RootViewController RootViewController { get { return Window.RootViewController as RootViewController; } }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                StubAppCenter.Inicializa();

                AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
                    try
                    {
                        var exception = ((Exception)e.ExceptionObject).GetBaseException();
                        Exception ex = new Exception("*** Ocorreu uma exceção não tratada. Detalhes: \n\n", exception);
                        Utils.Mensagem.Erro(ex);
                    }
                    catch (Exception ex)
                    {
                        Exception ex2 = new Exception("*** Ocorreu uma exceção não tratada. Detalhes: \n\n", ex);
                        Utils.Mensagem.Erro(ex2);
                    }
                };

                StubPushNotifications.ServicoDisponivel();

                string fcmToken = StubPushNotifications.Token();

                GravaInstanceID();

                UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = Utils.Colors.Laranja, TextShadowColor = UIColor.Clear });
                UINavigationBar.Appearance.TintColor = Utils.Colors.Laranja;

                Window = new UIWindow(UIScreen.MainScreen.Bounds);
                Window.RootViewController = new RootViewController();
                Window.MakeKeyAndVisible();

                StubUtilidades.SetCultureInfo();

                return true;
            }
            catch (ValidationException vex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void GravaInstanceID()
        {
            try
            {
                string id;
                var ia = new Firebase.InstanceID.InstanceIdHandler((a, b) =>
                {
                    try
                    {
                        StubUtilidades.SalvaInstanceID(a);
                    }
                    catch (Exception)
                    {

                    }
                });
                Firebase.InstanceID.InstanceId.SharedInstance.GetId(ia);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            try
            {
                StubPushNotifications.DesabilitaDirectChannel();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            try
            {

            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Erro(vex);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }


        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            try
            {
                StubPushNotifications.RegisteredForRemoteNotifications(application, deviceToken);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public void DidRefreshRegistrationToken(Firebase.CloudMessaging.Messaging msg, string str)
        {
            try
            {
                StubPushNotifications.AtualizaToken(msg.FcmToken);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        //Fire when background received notification is clicked
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                StubPushNotifications.NotificacaoRecebida(application, userInfo, completionHandler);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        // Receive data message on iOS 10 devices.
        public void ApplicationReceivedRemoteMessage(Firebase.CloudMessaging.RemoteMessage remoteMessage)
        {
            try
            {
                StubPushNotifications.ApplicationReceivedRemoteMessage(remoteMessage);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            try
            {
                Exception ex = new Exception("Falha ao registrar Notificações PUSH. Detalhes: " + error.LocalizedDescription);
                Utils.Mensagem.Erro(ex);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void OnResignActivation(UIApplication application)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void WillEnterForeground(UIApplication application)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void WillTerminate(UIApplication application)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void DidFailToContinueUserActivitiy(UIApplication application, string userActivityType, NSError error)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}


