using Android.App;
using Android.Content;
using Firebase.Messaging;
using System.Collections.Generic;
using SAMU192Droid.FacadeStub;

namespace SAMU192Droid.Servicos
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMessagingService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            StubPushNotifications.ReceberMensagem(message, this);
        }

    }
}