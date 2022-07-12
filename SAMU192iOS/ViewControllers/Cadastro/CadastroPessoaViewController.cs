using System;
using UIKit;
using SAMU192Core.Exceptions;
using SAMU192iOS.FacadeStub;
using SAMU192Core.DTO;
using SAMU192Core.Utils;

namespace SAMU192iOS.ViewControllers
{
    public partial class CadastroPessoaViewController : BaseViewController
    {
        CadastroDTO cadastro;

        public CadastroDTO Cadastro { get => cadastro; set => cadastro = value; }

        string PLACE_HOLDER_HISTORICO = "(Diabetes, marca-passo, etc.)";

        public CadastroPessoaViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                View = Utils.Interface.ConfiguraView(View, 620);
                Utils.Interface.ConfiguraScrollView(scrollviewone, View, 620);
                btnSalvar.TouchUpInside += BtnSalvar_TouchUpInside;
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

                Utils.Interface.ConfiguraTextField(txtNome, scrollviewone, 50, 20);
                float y = (View.Frame.Height == 480 ? 350 : 120);
                btnSalvar.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin;

                CarregaCadastro();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CarregaCadastro()
        {
            if (Cadastro != null)
            {
                txtNome.Text = Cadastro.Nome;
                txtNome.Text = Cadastro.Nome;
                dtpDtNasc.Date = Utils.Conversores.DateTimeToNSDate(Cadastro.DtNasc.HasValue ? Cadastro.DtNasc.Value : DateTime.Now);
                segGenero.SelectedSegment = (Cadastro.Sexo.HasValue && Cadastro.Sexo.Value == 'M' ? (nint)1 : (nint)0);
            }
        }

        private async void BtnSalvar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                char sexo = segGenero.SelectedSegment == 1 ? 'M' : 'F';
                DateTime dtNasc = Utils.Conversores.NSDateToDateTime(dtpDtNasc.Date);

                CadastroDTO temp = Cadastro;
                CadastroDTO cadastro = new CadastroDTO(txtNome.Text, dtNasc, sexo, string.Empty, temp.Telefones);
                StubCadastro.ValidaCadastro(cadastro);

                if (!StubCadastro.ExisteCadastro())
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.Cadastro_Efetuado.Value);

                StubCadastro.SalvaCadastro(cadastro);

                //Abrir Favorito??
                if (!StubCadastro.ExisteEnderecos())
                {
                    var window = UIApplication.SharedApplication.KeyWindow;
                    UINavigationController navController = ((RootViewController)window.RootViewController).NavController;
                    int button = await Utils.Mensagem.Questao("Abrir Favorito?", "Deseja cadastrar endereços Favoritos agora?",
                        (n) => {
                            Utils.Interface.VoltarViewController(false, true, n);
                            Utils.Interface.VoltarViewController(false, true, n);
                        },
                        navController,
                        "Agora", 
                        "Mais tarde");
                    if (button == 0)
                    {
                        FavoritoListaViewController favoritos = this.Storyboard.InstantiateViewController("FavoritoListaViewController") as FavoritoListaViewController;
                        favoritos.FromWalkThrough = true;
                        this.NavigationController.PushViewController(favoritos, true);
                        return;
                    }                    
                }
                else
                {
                    Utils.Interface.VoltarViewController(false, true, this.NavigationController);
                    Utils.Interface.VoltarViewController(false, true, this.NavigationController);
                }
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