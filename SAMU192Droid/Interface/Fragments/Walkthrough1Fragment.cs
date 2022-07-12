using Android.OS;
using Android.Views;
using SAMU192Core.Utils;
using SAMU192Droid.FacadeStub;

namespace SAMU192Droid.Interface.Fragments
{
    public class Walkthrough1Fragment : BaseWalkthroughFragment
    {
        public Walkthrough1Fragment()
        {
            TAG = "Walkthrough1Fragment";
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Iniciado.Value);
            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}