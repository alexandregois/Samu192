using System;
using UIKit;
using Foundation;
using SidebarNavigation;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192iOS.FacadeStub;
using SAMU192iOS.Implementations;
using System.Threading;
using SAMU192Core.Utils;

namespace SAMU192iOS.ViewControllers
{
    public partial class PrincipalViewController : BaseViewController
    {
        CadastroDTO CADASTRO;
        SolicitarAtendimentoDTO PACOTE;
        bool alreadyPassToOpenWalkThrough = false;
        FavoritoListaViewController favoritosLista = null;
        MapaViewController mapa = null;
        bool emLigacao = false, paraOutraPessoa = false;
        string PLACE_HOLDER_QUEIXA = "Principal motivo que leva a acionar o SAMU.";
        string QUEIXA = string.Empty;

        EnderecoDTO enderecoCriadoUsuario, enderecoGPS;
        internal EnderecoDTO EnderecoCriadoUsuario { get => enderecoCriadoUsuario; set => enderecoCriadoUsuario = value; }
        internal EnderecoDTO EnderecoGPS { get => enderecoGPS; set => enderecoGPS = value; }

        protected SidebarNavigation.SidebarController SidebarController
        {
            get
            {
                return (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.SidebarController;
            }
        }
        
        public PrincipalViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                LoadScreenControls();

                CADASTRO = StubCadastro.RecuperaCadastro();

                LoadStubFacades();
                LoadControlValues();
                StubWalkThrough.SetInWalkThrough(false);
                var termo = StubCadastro.RecuperaAceiteTermo();
                if (termo.Aceite)
                {
                    CADASTRO = StubCadastro.RecuperaCadastro();
                    if (!StubCadastro.ExisteCadastro(CADASTRO) && !alreadyPassToOpenWalkThrough)
                    {
                        alreadyPassToOpenWalkThrough = true;
                        StubWalkThrough.SetInWalkThrough(true);
                        Utils.Interface.OpenWalkThrough(this);
                    }
                }
                else
                {
                    StubWalkThrough.SetInWalkThrough(true);
                    Utils.Interface.OpenWelcome(this);                    
                }
                AtivaGPS();

                View = Utils.Interface.ConfiguraView(View, 850);
                Utils.Interface.ConfiguraScrollView(scrollviewone, View, 850);

                if (SidebarController != null)
                {
                    btnMenu.Clicked += BtnMenu_Clicked;
                }
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Erro(vex);
            }
            catch (Exception ex)
            {                
                Utils.Mensagem.Erro(ex);
            }
        }

        private void TxtQueixa_Changed(object sender, EventArgs e)
        {
            try
            {
                if (txtQueixa.Text.Length > 100)
                    txtQueixa.Text = txtQueixa.Text.Substring(0, 100);
            }
            catch (Exception ex)
            {

            }
        }

        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);
                float y = (View.Frame.Height == 480 ? 445 : 150);
                Utils.Interface.ConfiguraTextView(txtQueixa, scrollviewone, y, PLACE_HOLDER_QUEIXA);

                StubTelefonia.Carrega(UpdatePhoneCallConnected, UpdatePhoneCallDisconnected);
                StubGPS.Carrega(AtualizaDadosGPS, (bool val) => { });
                InvokeOnMainThread(() =>
                {
                    HabilitaBotaoFoto();
                    if (emLigacao)
                        VisibilidadeBotoesTelefone(false);
                });
                CADASTRO = StubCadastro.RecuperaCadastro();
                if (!emLigacao)
                    LoadControlValues();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        internal async void LiberarBotaoFoto_Callback(ServidorDTO servidor)
        {
            InvokeOnMainThread(() => {
                try
                {
                    StubWebService.Servidor = servidor;
                    if (servidor != null)
                    {
                        StubUtilidades.DesligaServico();
                        VisibilidadeBotoesTelefone(false);
                    }

                    HabilitaBotaoFoto();
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }

        internal async void LoadStubFacades()
        {
            try
            {
                StubTelefonia.Carrega(UpdatePhoneCallConnected, UpdatePhoneCallDisconnected, true);
                StubMapa.Carrega(new MapKit.MKMapView(), false);
                StubLocalizacao.Carrega("Assets");
                StubWebService.Carrega(null, LiberarBotaoFoto_Callback);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        internal void LoadScreenControls()
        {
            try
            {
                //float y = (View.Frame.Height == 480 ? 445 : 150);
                //Utils.Interface.ConfiguraTextView(txtQueixa, scrollviewone, 500, PLACE_HOLDER_QUEIXA);
                Utils.Interface.ResizeLabelFontSize(lbl00, 25, UIFontWeight.Bold);
                Utils.Interface.ResizeLabelFontSize(lbl01, 21, UIFontWeight.Bold);
                Utils.Interface.ResizeLabelFontSize(lbl001, 21, UIFontWeight.Bold);

                Utils.Interface.AlteraCorButton(btnSelMapa, false);
                Utils.Interface.AlteraCorButton(btnSelFavoritos, false);
                Utils.Interface.AlteraCorButton(btnSelGPS, true);

                btnChamarSAMU.TouchUpInside += BtnChamarSAMU_TouchUpInside;
                btnChamarParaMim.TouchUpInside += BtnChamarParaMim_TouchUpInside;
                btnChamarOutraPessoa.TouchUpInside += BtnChamarOutraPessoa_TouchUpInside;

                btnSelFavoritos.TouchUpInside += BtnSelFavoritos_TouchUpInside;
                btnSelMapa.TouchUpInside += BtnSelMapa_TouchUpInside;
                btnSelGPS.TouchUpInside += BtnSelGPS_TouchUpInside;
                btnQuandoAcionar.TouchUpInside += BtnQuandoAcionar_TouchUpInside;
                btnQdoAcionarForaArea.TouchUpInside += BtnQuandoAcionar_TouchUpInside;

                txtQueixa.Changed += TxtQueixa_Changed;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        internal void LoadControlValues()
        {
            try
            {
                VisibilidadeBotoesTelefone(!emLigacao);
                VisibilidadePadrao(true);
                if (emLigacao) HabilitaBotaoFoto();
                CarregaCadastro(StubCadastro.ExisteCadastro(CADASTRO));
                CarregaEnderecos();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CarregaEnderecos()
        {
            if (EnderecoCriadoUsuario == null)
            {
                if (StubCadastro.ExisteCadastro(CADASTRO))
                    StubGPS.Recarrega(AtualizaDadosGPS, (bool val) => { });

                var enderecos = StubCadastro.RecuperaEnderecos();
                if (enderecos != null && enderecos.Count > 0)
                {
                    lblEndereco.Text = string.IsNullOrEmpty(enderecos[0].Nome) ? enderecos[0].ToString() : enderecos[0].Nome.ToUpper() + " - " + enderecos[0].ToString();
                }
            }
            else
            {
                lblEndereco.Text = string.IsNullOrEmpty(EnderecoCriadoUsuario.Nome) ? EnderecoCriadoUsuario.ToString() : EnderecoCriadoUsuario.Nome.ToUpper() + " - " + EnderecoCriadoUsuario.ToString();
                aivGPSProgress.StopAnimating();
            }
        }

        private void LigarEnviarPacote(bool outraPessoa)
        {
            try
            {
                QUEIXA = (txtQueixa.Text == PLACE_HOLDER_QUEIXA ? string.Empty : txtQueixa.Text);
                paraOutraPessoa = outraPessoa;
                //Dispara assíncrono o envio de pacote e efetua a ligação.
                BackgroundTask bgPacote = new BackgroundTask(this.View, false, PreExecute_Pacote, RunInBackGround_Pacote, PostExecute_Pacote, OnCancel_Pacote, OnError_Pacote, OnValidationException_Pacote, "Enviando dados. Aguarde...", 30000, this.scrollviewone);
                bgPacote.Execute();

                StubTelefonia.FazerLigacao();

            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void RunInBackGround_Pacote(System.Threading.CancellationToken ct)
        {
            try
            {
                StubWebService.SolicitarAtendimento(PACOTE);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }

        }

        private void PreExecute_Pacote()
        {
            PACOTE = StubWebService.MontaPacote(paraOutraPessoa, QUEIXA, CADASTRO, EnderecoCriadoUsuario ?? EnderecoGPS);
        }

        private void PostExecute_Pacote()
        { }

        private void OnCancel_Pacote()
        {
            VisibilidadeBotoesTelefone(true);
            VisibilidadePadrao(true);
            VisibilidadeTirarFoto(false);
        }

        private void OnError_Pacote(Exception ex)
        {
            VisibilidadeBotoesTelefone(true);
            VisibilidadePadrao(true);
            VisibilidadeTirarFoto(false);
            StubAppCenter.AppCrash(ex, "PrincipalViewController");
        }

        private void OnValidationException_Pacote(ValidationException vex)
        {
            VisibilidadeBotoesTelefone(true);
            VisibilidadePadrao(true);
            VisibilidadeTirarFoto(false);
            StubAppCenter.AppAnalytic(Enums.AnalyticsType.EnvioPacote_Validacao.Value, "PrincipalViewController", vex.Message);
        }

        private void BtnSelMapa_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                aivGPSProgress.StopAnimating();
                aivGPSProgress.Hidden = true;

                Utils.Interface.AlteraCorButton(btnSelFavoritos, false);
                Utils.Interface.AlteraCorButton(btnSelGPS, false);
                Utils.Interface.AlteraCorButton(btnSelMapa, true);

                if (mapa == null)
                    mapa = this.Storyboard.InstantiateViewController("MapaViewControllerGPS") as MapaViewController;
                mapa.EnderecoCriadoUsuario = enderecoCriadoUsuario;
                this.NavigationController.PushViewController(mapa, true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnSelFavoritos_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                aivGPSProgress.StopAnimating();
                aivGPSProgress.Hidden = true;

                Utils.Interface.AlteraCorButton(btnSelMapa, false);
                Utils.Interface.AlteraCorButton(btnSelGPS, false);
                Utils.Interface.AlteraCorButton(btnSelFavoritos, true);

                if (favoritosLista == null)
                    favoritosLista = this.Storyboard.InstantiateViewController("FavoritoListaViewController") as FavoritoListaViewController;
                favoritosLista.ModoSelecao = true;
                this.NavigationController.PushViewController(favoritosLista, true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnSelGPS_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.AlteraCorButton(btnSelMapa, false);
                Utils.Interface.AlteraCorButton(btnSelFavoritos, false);
                Utils.Interface.AlteraCorButton(btnSelGPS, true);

                EnderecoCriadoUsuario = null;
                if (mapa != null)
                    mapa.EnderecoCriadoUsuario = null;
                AtivaGPS();
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
                Utils.Interface.EmpurrarViewController(this, "QuandoAcionarViewController");
            }
            catch(Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnChamarSAMU_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Ligar.Value);
                LigarEnviarPacote(false);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnChamarOutraPessoa_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Ligar_ParaOutraPessoa.Value);
                LigarEnviarPacote(true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnChamarParaMim_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Ligar_ParaMim.Value);
                LigarEnviarPacote(false);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void HabilitaBotaoFoto()
        {
            InvokeOnMainThread(() =>
            {
                bool habilita = StubWebService.Servidor != null;
                VisibilidadeTirarFoto(habilita);
            });
        }

        internal void UpdatePhoneCallConnected()
        {
            InvokeOnMainThread(() =>
            {
                try
                {
                    VisibilidadeBotoesTelefone(false);
                    VisibilidadeTirarFoto(true);
                    emLigacao = true;
                    StubUtilidades.LigaServico(LiberarBotaoFoto_Callback);
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }

        internal void UpdatePhoneCallDisconnected()
        {
            InvokeOnMainThread(() =>
            {
                try
                {
                    VisibilidadePadrao(true);
                    emLigacao = false;
                    StubUtilidades.DesligaServico();
                    StubWebService.Servidor = null;
                    HabilitaBotaoFoto();
                    VisibilidadeBotoesTelefone(true);
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }

        private void AtualizaDadosGPS(CoordenadaDTO coordenada)
        {
            InvokeOnMainThread(async () =>
            {
                try
                {
                    var servidores = StubLocalizacao.Localizar(coordenada);
                    if (servidores != null && servidores.Count > 0)
                    {
                        if (EnderecoCriadoUsuario == null)
                        {
                            var endereco = await StubMapa.ReverterCoordenada(coordenada, null);
                            if (endereco != null)
                            {
                                lblEndereco.Text = string.IsNullOrEmpty(endereco.Nome) ? endereco.ToString() : endereco.Nome.ToUpper() + " - " + endereco.ToString();
                                EnderecoGPS = endereco;
                            }
                        }
                        VisibilidadeForaDaArea(false);
                    }
                    else
                    {
                        VisibilidadeForaDaArea(true);
                    }

                    if (emLigacao)
                    {
                        VisibilidadeBotoesTelefone(false);
                    }
                    aivGPSProgress.StopAnimating();
                }
                catch (ValidationException vex)
                {
                    Utils.Mensagem.Aviso(vex.Message);
                }
                catch (NSErrorException nsex)
                {
                    //dummy
                    //Utils.Mensagem.Aviso("Problemas ao localizar seu endereço atual."); //+ nsex.Domain + ":" + nsex.Code);
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }

        private void AtivaGPS()
        {
            if (StubGPS.StatusGPS())
            {
                lblEndereco.Text = "Aguardando Localização...";
                aivGPSProgress.StartAnimating();
                aivGPSProgress.Hidden = false;

                StubGPS.Recarrega(AtualizaDadosGPS, (o) => { });
            }
            else
            {
                if (!StubWalkThrough.GetInWalkThrough())
                {
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.GPS_Pedido_Permissao.Value);
                    InvokeOnMainThread(() =>
                    {
                        Utils.Mensagem.Alerta(
                            "O Aplicativo CHAMAR 192 necessita permissão para acessar o Serviço de Localização do Aparelho. Verifique.",
                            (o) =>
                            {
                                UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
                            });
                    });
                }
            }
        }

        private void BtnMenu_Clicked(object sender, EventArgs e)
        {
            try
            {
                (UIApplication.SharedApplication.Delegate as AppDelegate).RootViewController.MenuController.ShowForAcessibility();
                SidebarController.ToggleMenu();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CarregaCadastro(bool cadastrado)
        {
            lblNomePessoa.Text = CADASTRO.ToString();
            lblNumeroTelef.Text = cadastrado ? string.Format("({0}) {1}-{2}", CADASTRO.Telefones[0].Ddd, CADASTRO.Telefones[0].Numero.Substring(0, 5), CADASTRO.Telefones[0].Numero.Substring(5, 4)) : "(51) 99999-1234";
        }

        private void VisibilidadeForaDaArea(bool show)
        {
            btnChamarOutraPessoa.Hidden = btnChamarParaMim.Hidden = grpTroteCadastro.Hidden = grpQueixa.Hidden = grpOndeEstou.Hidden = grpIdentificacaoPessoa.Hidden = show;
            btnChamarSAMU.Hidden = grpForaCobertura.Hidden = grpTrote.Hidden = !show;
            if (show) StubAppCenter.AppAnalytic(Enums.AnalyticsType.ForaDeArea.Value);
        }

        private void VisibilidadePadrao(bool show)
        {
            btnChamarOutraPessoa.Hidden = btnChamarParaMim.Hidden = grpOndeEstou.Hidden = grpQueixa.Hidden = grpIdentificacaoPessoa.Hidden = grpTroteCadastro.Hidden = !show;
            btnChamarSAMU.Hidden = grpTrote.Hidden = show;
        }

        private void VisibilidadeTirarFoto(bool show)
        {
            btnTirarFoto.Hidden = !show;
            btnTirarFoto.AccessibilityElementsHidden = !show;
            btnTirarFoto.IsAccessibilityElement = !show;
            View.BringSubviewToFront(btnTirarFoto);
            lbl00.Text = (show ? "Tirar Foto" : "Chamar o SAMU 192");
            lbl00.AccessibilityElementsHidden = !show;

            //swtGPS.Enabled = swtFavorito.Enabled = !show;
            txtQueixa.Editable = !show;
            Utils.Interface.AlteraEstadoButton(btnEditarCad, !show);
            //Utils.Interface.AlteraCorButton(btnSelMapa, !show);
            //Utils.Interface.AlteraCorButton(btnSelFavoritos, !show);
            //Utils.Interface.AlteraCorButton(btnSelGPS, !show);
            if (show) StubAppCenter.AppAnalytic(Enums.AnalyticsType.Foto_ExibirBotao.Value);
        }

        private void VisibilidadeBotoesTelefone(bool show)
        {
            btnChamarSAMU.Hidden = btnChamarOutraPessoa.Hidden = btnChamarParaMim.Hidden = !show;
        }
    }
}