using System;
using System.Collections.Generic;
using CoreGraphics;
using SAMU192Core.DTO;
using SAMU192iOS.FacadeStub;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class FavoritoListaViewController : BaseViewController
    {

        List<EnderecoDTO> enderecos;
        List<EnderecoDTO> Enderecos { get => enderecos; set => enderecos = value; }
        List<UIView> controls = new List<UIView>();
        FavoritoExcluirViewController favoritosVC = null;
        nfloat width;
        bool modoSelecao = false;
        public bool ModoSelecao { get => modoSelecao; set => modoSelecao = value; }

        bool fromWalkThrough = false;
        public bool FromWalkThrough { get => fromWalkThrough; set => fromWalkThrough = value; }

        public FavoritoListaViewController(IntPtr handle) : base(handle)
        {
            this.NavigationItem.Title = "Favoritos";
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                View = Utils.Interface.ConfiguraView(View, 800);
                Utils.Interface.ConfiguraScrollView(scrollviewone, View, 800);
                btnAdd.TouchUpInside += BtnAdd_TouchUpInside;
                width = this.View.Frame.Width - (nfloat)20;
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
                width = this.View.Frame.Width - (nfloat)20;
                btnAdd.Frame = new CGRect(54, 80, width > 320 ? width - 100 : 220, 42);
                CarregaListaEnderecos();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private float CarregaListaEnderecos()
        {
            Enderecos = StubCadastro.RecuperaEnderecos();
            nfloat initialY = 65;
            ClearView();
            foreach (var end in Enderecos)
            {
                initialY = MontaItemEndereco(end, initialY);
            }
            return (float)initialY;
        }

        private void ClearView()
        {
            btnAdd.Frame = new CGRect(54, 80, width > 320 ? width - 100 : 220, 42);
            if (controls == null || controls.Count == 0)
                return;

            foreach (UIView view in controls)
            {
                view.RemoveFromSuperview();
            }
            controls.Clear();
        }

        private nfloat MontaItemEndereco(EnderecoDTO endereco, nfloat initialY)
        {
            UITableViewCell table = new UITableViewCell();
            table.Frame = new CGRect(10, initialY + 10, width, 110);
            table.BackgroundColor = UIColor.White;
            table.Layer.CornerRadius = 20;
            table.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
            controls.Add(table);

            UILabel lblTitulo = new UILabel();
            lblTitulo.Text = endereco.Nome;
            lblTitulo.Frame = new CGRect(20, initialY + 20, 200, 20);
            lblTitulo.Font = UIFont.FromName("Helvetica Bold", 17);
            lblTitulo.TextColor = Utils.Colors.Laranja;
            controls.Add(lblTitulo);

            UILabel lblEndereco = new UILabel();
            lblEndereco.Text = endereco.ToString();
            lblEndereco.Frame = new CGRect(20, initialY + 50, width - 40, 40);
            lblEndereco.Font = UIFont.FromName("Helvetica", 15);
            lblEndereco.Lines = 2;
            controls.Add(lblEndereco);

            UILabel lblReferecia = null;
            if (!string.IsNullOrEmpty(endereco.Referencia))
            {
                lblReferecia = new UILabel();
                lblReferecia.Text = "Referência: " + endereco.Referencia;
                lblReferecia.Frame = new CGRect(20, initialY + 90, width - 40, 20);
                lblReferecia.Font = UIFont.FromName("Helvetica Bold", 15);
                controls.Add(lblReferecia);
            }

            UIButton btnExlcuir = new UIButton();
            string textLabel = (ModoSelecao ? "Selecionar" : "Excluir");                        
            btnExlcuir.SetTitle(textLabel, UIControlState.Normal);
            btnExlcuir.SetImage(UIImage.FromBundle(ModoSelecao ? "check" : "botaoDelete"), UIControlState.Normal);
            btnExlcuir.TintColor = UIColor.White;
            btnExlcuir.TintColorDidChange();
            btnExlcuir.TouchUpInside += (s, e) => 
            {
                try
                {
                    if (ModoSelecao)
                    {

                        Utils.Interface.VoltarViewController(false, true, this);
                        var window = UIApplication.SharedApplication.KeyWindow;
                        var controller = ((RootViewController)window.RootViewController).NavController;
                        if (controller != null && controller.TopViewController is PrincipalViewController)
                        {
                            if (endereco != null)
                            {
                                ((PrincipalViewController)controller.TopViewController).EnderecoCriadoUsuario = endereco;
                            }
                        }
                    }
                    else
                    {
                        if (favoritosVC == null)
                            favoritosVC = this.Storyboard.InstantiateViewController("FavoritoExcluirViewController") as FavoritoExcluirViewController;
                        favoritosVC.EnderecoRemover = endereco;
                        favoritosVC.Enderecos = Enderecos;
                        this.NavigationController.PushViewController(favoritosVC, true);
                    }
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            };
            btnExlcuir.BackgroundColor = Utils.Colors.Laranja;
            btnExlcuir.BackgroundColor.ColorWithAlpha((nfloat)255);
            btnExlcuir.SetTitleColor(UIColor.White, UIControlState.Normal);
            btnExlcuir.Font = UIFont.FromName("Helvetica", 16);
            btnExlcuir.Layer.CornerRadius = 10;
            btnExlcuir.Frame = new CGRect(this.View.Frame.Width - 140, initialY + 20, 120, 30);
            controls.Add(btnExlcuir);

            View.AddSubview(table);
            View.AddSubview(lblTitulo);
            View.AddSubview(lblEndereco);
            if (lblReferecia != null)
                View.AddSubview(lblReferecia);
            View.AddSubview(btnExlcuir);

            btnAdd.Frame = new CGRect(54, table.Frame.Y + table.Frame.Height + 10, width > 320 ? width - 100 : 220, 42);
            return (table.Frame.Y + table.Frame.Height);
        }

        private void BtnAdd_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                MapaViewController mapa = this.Storyboard.InstantiateViewController("MapaViewControllerFavorito") as MapaViewController;
                mapa.FromWalkThrough = FromWalkThrough;
                this.NavigationController.PushViewController(mapa, true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnFechar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                this.DismissModalViewController(true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}