using System;
using Android.OS;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;
using SAMU192Droid.FacadeStub;

namespace SAMU192Droid.Interface.Fragments
{
    public class BaseWalkthroughFragment : WalkthroughFragment
    {
        protected TextView tv_1;
        protected TextView tv_2;
        protected ImageView iv_1;
        protected Button walknext_cmd;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                var view = base.OnCreateView(inflater, container, savedInstanceState);
                if (StubWalkThrough.GetWalkThrough())
                {
                    LoadScreenControls();
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Handler h = new Handler();
            System.Action myAction = () =>
            {
                if (StubWalkThrough.GetWalkThrough())
                {
                    if (tv_1 != null)
                    {
                        tv_1.Focusable = true;
                        tv_1.RequestFocus();
                        tv_1.SendAccessibilityEvent(EventTypes.ViewAccessibilityFocused);
                    }
                }
            };

            h.PostDelayed(myAction, 1000);
        }

        private void LoadScreenControls()
        {
            tv_1 = view.FindViewById<TextView>(Resource.Id.tv_1);
            tv_2 = view.FindViewById<TextView>(Resource.Id.tv_2);
            iv_1 = view.FindViewById<ImageView>(Resource.Id.iv_1);
            walknext_cmd = view.FindViewById<Button>(Resource.Id.walknext_cmd);
        }

        public override void HideDescendantsForAccessibility()
        {
            tv_1.ImportantForAccessibility =
            tv_2.ImportantForAccessibility =
            iv_1.ImportantForAccessibility =
            walknext_cmd.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            if (StubWalkThrough.GetWalkThrough())
            {
                tv_1.ImportantForAccessibility =
                tv_2.ImportantForAccessibility =
                iv_1.ImportantForAccessibility =
                walknext_cmd.ImportantForAccessibility = ImportantForAccessibility.Yes;
            }
        }
    }
}