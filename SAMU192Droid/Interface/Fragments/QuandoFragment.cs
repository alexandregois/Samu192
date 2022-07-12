using Android.OS;
using Android.Views;
using Android.Widget;

namespace SAMU192Droid.Interface.Fragments
{
    public class QuandoFragment : BaseFragment
    {
        public static string TAG = "Quando";

        View view;
        TextView quando_tv;

        public override void HideDescendantsForAccessibility()
        {
            quando_tv.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            quando_tv.ImportantForAccessibility = ImportantForAccessibility.Yes;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = inflater.Inflate(Resource.Layout.quando, null);
            quando_tv = view.FindViewById<TextView>(Resource.Id.quando_tv);
            quando_tv.ImportantForAccessibility = ImportantForAccessibility.Yes;
            return view;
        }        
    }
}