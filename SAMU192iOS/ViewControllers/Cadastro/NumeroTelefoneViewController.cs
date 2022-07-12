using System;
using UIKit;
using SAMU192iOS.FacadeStub;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;

namespace SAMU192iOS.ViewControllers
{
    public partial class NumeroTelefoneViewController : BaseViewController
    {
        protected SidebarNavigation.SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.SidebarController;
            }
        }

        CadastroDTO cadastro = null;

        public NumeroTelefoneViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                btnAvancar.TouchUpInside += BtnAvancar_TouchUpInside;
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
                Utils.Interface.ConfiguraTextField(txtNumeroTelefone, scrollviewone, 9, 20);
                Utils.Interface.ConfiguraTextField(txtDDD, scrollviewone, 3, 20);

                CarregaCadastro();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CarregaCadastro()
        {
            cadastro = StubCadastro.RecuperaCadastro();
            if (cadastro.Telefones != null && cadastro.Telefones.Length > 0)
            {
                txtDDD.Text = cadastro.Telefones[0].Ddd;
                txtNumeroTelefone.Text = cadastro.Telefones[0].Numero;
            }
        }

        private void BtnAvancar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                TelefoneDTO[] telefones = new TelefoneDTO[1];
                telefones[0] = new TelefoneDTO(txtDDD.Text, txtNumeroTelefone.Text, 0);
                CadastroDTO cadastroSalva = new CadastroDTO(cadastro.Nome, cadastro.DtNasc, cadastro.Sexo, cadastro.Historico, telefones);
                StubCadastro.ValidaTelefone(cadastroSalva);

                CadastroPessoaViewController cadastroVC = this.Storyboard.InstantiateViewController("CadastroPessoaViewController") as CadastroPessoaViewController;
                cadastroVC.Cadastro = cadastroSalva;
                this.NavigationController.PushViewController(cadastroVC, true);
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Alerta(vex.Message, (o) => { });
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}