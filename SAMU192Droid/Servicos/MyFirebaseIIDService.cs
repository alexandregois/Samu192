using Android.App;
using Android.Content;
using Firebase.Iid;
using SAMU192Droid.FacadeStub;

namespace SAMU192Droid.Servicos
{
    [Service(Exported = false)]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            StubCadastro.Carrega();
            StubPushNotifications.AtualizaToken(FirebaseInstanceId.Instance.Token);
        }

        void SendRegistrationToAppServer(string token)
        {
            // Add custom implementation here as needed.
        }
    }    
}