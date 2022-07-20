using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SAMU192Core.Facades;
using SAMU192Core.Utils;
using SAMU192Droid.FacadeStub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMU192Droid.Interface.Activities
{
    [Activity(Label = "ChatActivity")]
    public class ChatActivity : Activity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.chat);

            var resultMessages = StubWebService.BuscaMensagens(StubUtilidades.MontaPacoteBuscaMensagem(Enums.BuscarMensagens.MensagensNovas, DateTime.Now));

            // Create your application here
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }
    }
}