using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Firebase.Iid;
using Firebase.Messaging;
using SAMU192Core.DTO;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;
using SAMU192Droid.Interface;
using SAMU192Droid.Interface.Activities;
using SAMU192Droid.Servicos;

namespace SAMU192Droid.Implementations
{
    internal class PushNotificationImpl : IPushNotification
    {
        public void EnviarNotificacao(string msg, object dados, object svc)
        {
            IDictionary<string, string> data = (IDictionary<string, string>)dados;
            MyFirebaseMessagingService service = (MyFirebaseMessagingService)svc;

            var intent = new Intent(service, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (string key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }
            var pendingIntent = PendingIntent.GetActivity(service, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new Notification.Builder(service)
                .SetSmallIcon(Resource.Drawable.Icon)//icon_notification
                .SetContentTitle("CHAMAR 192")
                .SetContentText(msg)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManager.FromContext(service);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        public void ExecutaNotificacaoBackground(NotificacaoDTO notificacao, object args)
        {
            ExecutaNotificacaoForeground(notificacao, args);
        }

        public void ExecutaNotificacaoBadge(NotificacaoDTO notificacao, object args, int idxTab)
        {
            ExecutaNotificacaoForeground(notificacao, args);
        }

        public void ExecutaNotificacaoForeground(NotificacaoDTO notificacao, object args)
        {
            Context context = (Context)args;
            Enums.Broadcast bc = Enums.Broadcast.Indefinido;
            switch (notificacao.Modo)
            {
                case ModoNotificacao.Avaliação:
                    bc = Enums.Broadcast.PesquisaDisponivel;
                    break;
                case ModoNotificacao.Boletim:
                    bc = Enums.Broadcast.BoletimDisponivel;
                    break;
                case ModoNotificacao.Abertura:
                    bc = Enums.Broadcast.ChamadoAbertura;
                    break;
                default:
                    break;
            }
            Utils.Ferramentas.EnviaBroadcast((int)bc, notificacao.CodChamado, notificacao.FromBackground);
        }

        public string SelecionarTela(ModoNotificacao modo)
        {
            throw new NotImplementedException();
        }

        public string ServicoDisponivel(object arg1 = null)
        {
            Activity activity = (Activity)arg1;
            string msgText;
            var token = FirebaseInstanceId.Instance.Token;
            FacadeStub.StubPushNotifications.AtualizaToken(token);
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(activity);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    msgText = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                }
                else
                {
                    msgText = "Este dispositivo não é suportado.";
                }
            }
            else
            {
                msgText = "Google Play Services está disponível.";
            }
            return msgText;
        }

        public NotificacaoDTO ReceberMensagem(object msg, object arg1)
        {
            Context context;
            NotificacaoDTO notificacao;
            try
            {
                RemoteMessage message = (RemoteMessage)msg;
                context = (MyFirebaseMessagingService)arg1;

                notificacao = new NotificacaoDTO();
                notificacao.FromBackground = false;
                foreach (var item in message.Data)
                {
                    switch (item.Key)
                    {
                        case "Modo":
                            notificacao.Modo = (ModoNotificacao)Convert.ToInt32(item.Value);
                            break;
                        case "CodChamado":
                            notificacao.CodChamado = Convert.ToInt32(item.Value);
                            break;
                        default:
                            break;
                    }
                }
                var notification = message.GetNotification();
                notificacao.Titulo = notification.Title;
                notificacao.Mensagem = notification.Body;
            }
            catch (InvalidCastException)
            {
                notificacao = (NotificacaoDTO)msg;
                context = (Activity)arg1;
            }                                
            return notificacao;
        }
    }
}