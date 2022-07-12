using System;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class MenuViewController : BaseViewController
    {

        public MenuViewController() : base()
        { }

        public MenuViewController(IntPtr handle) : base(handle)
        { }

        protected SidebarNavigation.SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.SidebarController;
            }
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                btnChamarSAMU.TouchUpInside += BtnChamarSAMU_TouchUpInside;
                btnCadastro.TouchUpInside += BtnCadastro_TouchUpInside;
                btnFavoritos.TouchUpInside += BtnFavoritos_TouchUpInside;
                btnQuandoAcionar.TouchUpInside += BtnQuandoAcionar_TouchUpInside;
                btnSobre.TouchUpInside += BtnSobre_TouchUpInside;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnSobre_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                SidebarController.CloseMenu();
                Utils.Interface.EmpurrarViewController(this, "SobreViewController");
                HideForAcessibility();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnQuandoAcionar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                SidebarController.CloseMenu();
                Utils.Interface.EmpurrarViewController(this, "QuandoAcionarViewController");
                HideForAcessibility();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnFavoritos_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                SidebarController.CloseMenu();
                Utils.Interface.EmpurrarViewController(this, "FavoritoListaViewController");
                HideForAcessibility();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnCadastro_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                SidebarController.CloseMenu();
                Utils.Interface.EmpurrarViewController(this, "NumeroTelefoneViewController");
                HideForAcessibility();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnChamarSAMU_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                SidebarController.CloseMenu();
                HideForAcessibility();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public void HideForAcessibility()
        {
            this.btnChamarSAMU.AccessibilityElementsHidden =
                this.btnCadastro.AccessibilityElementsHidden =                
                this.btnFavoritos.AccessibilityElementsHidden =
                this.btnQuandoAcionar.AccessibilityElementsHidden =
                this.btnSobre.AccessibilityElementsHidden = true;
        }

        public void ShowForAcessibility()
        {
            this.btnChamarSAMU.AccessibilityElementsHidden =
                this.btnCadastro.AccessibilityElementsHidden =
                this.btnFavoritos.AccessibilityElementsHidden =
                this.btnQuandoAcionar.AccessibilityElementsHidden =
                this.btnSobre.AccessibilityElementsHidden = false;
        }
    }
}