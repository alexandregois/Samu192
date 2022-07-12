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
    [Activity(Label = "InformaActivity")]
    public class InformaNaoChatActivity : Activity
    {
        Button btn_ok;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.naopermitechat);

                btn_ok = FindViewById<Button>(Resource.Id.ok_cmd);
                btn_ok.Click += Btn_ok_Click;

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