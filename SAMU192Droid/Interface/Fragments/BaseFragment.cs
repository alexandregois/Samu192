using Android.App;

namespace SAMU192Droid.Interface.Fragments
{
    public abstract class BaseFragment : Fragment
    {
        /// <summary>
        /// Método que seta importância para acessibilidade (TalkBack) como NO e dependendo do caso Invisível
        /// </summary>
        public abstract void HideDescendantsForAccessibility();

        /// <summary>
        /// Método que seta importância para acessibilidade (TalkBack) como YES e dependendo do caso Visível
        /// </summary>
        public abstract void ShowDescendantsForAccessibility();
    }
}