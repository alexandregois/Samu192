
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SAMU192Droid.Interface.Fragments
{
    public class InformaComunicacaoFragment : BaseFragment
    {
        public static string TAG = "Comunicação";

        View view;
        Button btn_região_atendimento;
        Button btn_ok;

        public override void HideDescendantsForAccessibility()
        {
            btn_região_atendimento.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            btn_região_atendimento.ImportantForAccessibility = ImportantForAccessibility.Yes;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                view = inflater.Inflate(Resource.Layout.InformaComunicacao, null);
                //btn_região_atendimento = (Button)view.FindViewById(Resource.Id.btn_regiao_atendimento);
                //btn_região_atendimento.Click += Btn_região_atendimento_Click;

                btn_ok = (Button)view.FindViewById(Resource.Id.ok_cmd);
                btn_ok.Click += Btn_ok_Click;

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            this.Activity.OnBackPressed();
        }

        private void Btn_região_atendimento_Click(object sender, System.EventArgs e)
        {
            try
            {
                string url = "http://www.true.com.br/aplicativo-samu-192/";

                Intent i = new Intent(Intent.ActionView);
                i.SetData(Android.Net.Uri.Parse(url));
                StartActivity(i);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}
