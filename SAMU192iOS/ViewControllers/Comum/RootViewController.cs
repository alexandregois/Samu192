using System;
using UIKit;
using SidebarNavigation;
using System.Threading;
using System.Globalization;

namespace SAMU192iOS.ViewControllers
{
	public partial class RootViewController : BaseViewController
	{
        private UIStoryboard _storyboard;
		
		public SidebarNavigation.SidebarController SidebarController { get; private set; }

        public UINavigationController NavController { get; private set; }
        public MenuViewController MenuController { get; private set; }        

        public override UIStoryboard Storyboard {
			get
            {
				if (_storyboard == null)
					_storyboard = UIStoryboard.FromName("MainStoryboard", null);

                return _storyboard;
			}
		}

        public RootViewController(IntPtr handle) : base(handle)
        {
        }

        public RootViewController() : base()
		{
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void ViewDidLoad()
		{
            try
            {
                base.ViewDidLoad();

                NavController = new UINavigationController();
                MenuController = (MenuViewController)Storyboard.InstantiateViewController("MenuViewController");
                NavController.PushViewController((PrincipalViewController)Storyboard.InstantiateViewController("PrincipalViewController"), false);
                SidebarController = new SidebarController(this, NavController, MenuController);
                MenuController.HideForAcessibility();

                SidebarController.MenuWidth = 220;
                SidebarController.MenuLocation = MenuLocations.Left;
                SidebarController.HasDarkOverlay = true;
                SidebarController.ReopenOnRotate = false;

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

    }
}