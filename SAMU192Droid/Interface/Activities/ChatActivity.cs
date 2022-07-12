using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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

            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.chat);

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }

            // Create your application here
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }
    }
}