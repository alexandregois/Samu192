using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace SAMU192Droid.Interface.Fragments
{
    public class ChatFragment : BaseFragment
    {
        public static string TAG = "Chat";

        View view;
        Button btn_sendmessage;

        public override void HideDescendantsForAccessibility()
        {
            
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                view = inflater.Inflate(Resource.Layout.chat, null);
                btn_sendmessage = (Button)view.FindViewById(Resource.Id.btn_sendmessage);
                btn_sendmessage.Click += Btn_sendmessage_Click;

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        private void Btn_sendmessage_Click(object sender, EventArgs e)
        {
            
        }

        public override void ShowDescendantsForAccessibility()
        {
            
        }
    }
}