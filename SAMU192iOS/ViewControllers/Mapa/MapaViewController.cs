using System;
using UIKit;
using MapKit;
using Foundation;
using SAMU192iOS.FacadeStub;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192iOS.Implementations;
using SAMU192iOS.Resources;

namespace SAMU192iOS.ViewControllers
{
    public partial class MapaViewController : BaseViewController
    {
        CoordenadaDTO coordenadaCriadaUsuario = null;
        EnderecoDTO enderecoCriadoUsuario = null;
        bool usuarioPressionouBuscar = false, usuarioEditouTextoPesquisa = false;
        string enderecoDigitadoAnterior = string.Empty;
        bool fromWalkThrough = false;
        public bool FromWalkThrough { get => fromWalkThrough; set => fromWalkThrough = value; }
        public EnderecoDTO EnderecoCriadoUsuario { get => enderecoCriadoUsuario; set => enderecoCriadoUsuario = value; }
        internal Action After_Aceitacao { get; set; }
        BackgroundTask btClearTextButton;
        bool enderecoValidado = false, loadOnce = false, despresa_RegionChanged = false, despresa_EmCache = false;
        CoordenadaDTO coordenada_anterior = new CoordenadaDTO(0, 0);

        public MapaViewController() : base()
        { }

        public MapaViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                loadOnce = false;
                srcEndereco.Placeholder = "Digite seu endereço";
                srcEndereco.Text = string.Empty;
                srcEndereco.CancelButtonClicked += SrcEndereco_CancelButtonClicked;
                Utils.Interface.ConfiguraTextField(txtComplemento, scrollviewone, 50, 20);
                VisibilidadeSelecionaAdiciona();
                if (btnSelecionar != null)
                    btnSelecionar.TouchUpInside += BtnSelecionar_TouchUpInside;
                if (btnAdicionar != null)
                    btnAdicionar.TouchUpInside += BtnAdicionar_TouchUpInside;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SrcEndereco_CancelButtonClicked(object sender, EventArgs e)
        {
            try
            {
                VisibilidadeSelecionaAdiciona();
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

                LoadStubFacades();
                LoadScreenControls();
                LoadControlValues();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            try
            { 
                base.ViewWillDisappear(animated);
                StubGPS.Descarrega();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        internal void LoadStubFacades()
        {
            try
            {
                StubGPS.Recarrega(AtualizaDadosGPS, (bool val) => { });
                StubMapa.Carrega(oMapa, false);
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
                if (!loadOnce)
                {
                    Utils.Interface.ConfiguraSearchBar(srcEndereco, SrcEndereco_SearchButtonClicked, SrcEndereco_TextChanged, SrcEndereco_OnEditingStarted, SrcEndereco_OnEditingStoped);
                    loadOnce = true;
                }
                btnCurrentLocation.TouchUpInside += ButtonCurrentLocation_TouchUpInside;
                oMapa = Utils.Interface.ConfiguraMKMapView(oMapa, View, OMapa_RegionChanged);
                //Utils.Interface.AlteraEstadoButton(btnSelecionar ?? btnAdicionar, false);
                progressbarGeoCode.StopAnimating();
                progressbarGeoCode.Hidden = true;
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
                if (EnderecoCriadoUsuario != null)
                {
                    despresa_RegionChanged = true;
                    despresa_EmCache = true;
                    srcEndereco.Text = EnderecoCriadoUsuario.ToString(false);
                    txtComplemento.Text = EnderecoCriadoUsuario.Complemento;
                    coordenadaCriadaUsuario = EnderecoCriadoUsuario.Coordenada;
                    StubMapa.PosicionaMapa(EnderecoCriadoUsuario.Coordenada);
                    VisibilidadeSelecionaAdiciona();
                    return;
                }

                if (StubGPS.StatusGPS())
                    coordenadaCriadaUsuario = null;

                StubMapa.SetaLocalizacaoGPSNoMapa(null);
                usuarioPressionouBuscar = true;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SrcEndereco_TextChanged(object sender, UISearchBarTextChangedEventArgs e)
        {
            try
            {
                AjustaClearTextButton((UIView)sender);

                if (enderecoDigitadoAnterior != srcEndereco.Text)
                {
                    usuarioEditouTextoPesquisa = string.IsNullOrEmpty(srcEndereco.Text) ? false : true;
                }

                usuarioPressionouBuscar = false;
                enderecoDigitadoAnterior = srcEndereco.Text;

                if (string.IsNullOrEmpty(srcEndereco.Text))
                    coordenadaCriadaUsuario = null;

                VisibilidadeSelecionaAdiciona();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void VisibilidadeSelecionaAdiciona()
        {
            if (string.IsNullOrEmpty(srcEndereco.Text))
            {
                Utils.Interface.AlteraEstadoButton(btnSelecionar ?? btnAdicionar, false);
            }
            else
            {
                Utils.Interface.AlteraEstadoButton(btnSelecionar ?? btnAdicionar, true);
            }
        }

        private void AjustaClearTextButton(UIView v)
        {
            if (btClearTextButton == null)
            {
                btClearTextButton = new BackgroundTask(this,
                    false,
                    () => { },
                    (c) =>
                    {
                        try
                        {
                            InvokeOnMainThread(() =>
                            {
                                try
                                {
                                    Utils.Interface.ConfiguraClearTextSearchBarForAccessibility(v, "Apagar Texto");
                                }
                                catch
                                { //dummy
                                }
                            });
                        }
                        catch
                        { //dummy
                        }
                    },
                    () => { },
                    () => { },
                    (m) => { },
                    (ex) => { });
            }
            btClearTextButton.Execute();
        }

        private void SrcEndereco_OnEditingStarted(object sender, EventArgs e)
        {
            try
            {
                usuarioEditouTextoPesquisa = true;
                AjustaClearTextButton(srcEndereco);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SrcEndereco_OnEditingStoped(object sender, EventArgs e)
        {
            try
            {
                usuarioEditouTextoPesquisa = false;
                if (string.IsNullOrEmpty(srcEndereco.Text))
                    coordenadaCriadaUsuario = null;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private async void SrcEndereco_SearchButtonClicked(object sender, EventArgs e)
        {
            try
            {
                progressbarGeoCode.StartAnimating();
                progressbarGeoCode.Hidden = false;
                var end = await StubMapa.ReverterEndereco(srcEndereco.Text, null);
                StubGPS.StopLocationManager();
                despresa_RegionChanged = true;
                despresa_EmCache = !end.EmCache;
                srcEndereco.Text = end.ToString(false);
                txtComplemento.Text = end.Complemento;
                coordenadaCriadaUsuario = end.Coordenada;
                EnderecoCriadoUsuario = end;
                StubMapa.PosicionaMapa(end.Coordenada);
                usuarioPressionouBuscar = true;
                srcEndereco.EndEditing(true);
                VisibilidadeSelecionaAdiciona();
            }
            catch (NSErrorException nsex)
            {
                //dummy
                if (usuarioEditouTextoPesquisa)
                    Utils.Mensagem.Aviso("Não foi possível encontrar o endereço digitado. Por favor, verifique o endereço digitado e faça novamente a pesquisa.");
            }
            catch (ConnectionException cex)
            {
                Utils.Mensagem.Aviso(cex.Message);
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            finally
            {
                usuarioEditouTextoPesquisa = false;
                progressbarGeoCode.StopAnimating();
                progressbarGeoCode.Hidden = true;
            }
        }

        private void BtnSelecionar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (StubLocalizacao.ValidarEndereco(EnderecoCriadoUsuario))
                {
                    EnderecoCriadoUsuario.Complemento = string.IsNullOrEmpty(txtComplemento.Text) ? "" : txtComplemento.Text;
                    var window = UIApplication.SharedApplication.KeyWindow;
                    var controller = ((RootViewController)window.RootViewController).NavController;
                    VoltarParaChamar(controller);
                }
                else
                {
                    ConfirmaEnderecoInvalido();
                }
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

        private void BtnAdicionar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (StubLocalizacao.ValidarEndereco(EnderecoCriadoUsuario))
                {
                    EnderecoCriadoUsuario.Complemento = string.IsNullOrEmpty(txtComplemento.Text) ? "" : txtComplemento.Text;
                    var window = UIApplication.SharedApplication.KeyWindow;
                    var controller = ((RootViewController)window.RootViewController).NavController;
                    VoltarParaFavorito(controller);
                }
                else
                {
                    ConfirmaEnderecoInvalidoFavorito();
                }
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

        /// <summary>
        /// Delegate para tratar chegada de coordenada GPS
        /// </summary>
        /// <param name="coordenada"></param>
        private void AtualizaDadosGPS(CoordenadaDTO coordenada)
        {
            try
            {
                if (coordenadaCriadaUsuario == null)
                    StubMapa.PosicionaMapa(coordenada);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        /// <summary>
        /// Tratamento evento Mapa alterado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OMapa_RegionChanged(object sender, MKMapViewChangeEventArgs e)
        {
            try
            {
                if (!ValidaReversao())
                    return;

                progressbarGeoCode.StartAnimating();
                progressbarGeoCode.Hidden = false;
                coordenadaCriadaUsuario = new CoordenadaDTO(oMapa.CenterCoordinate.Latitude, oMapa.CenterCoordinate.Longitude);
                EnderecoDTO endereco = await StubMapa.ReverterCoordenada(coordenadaCriadaUsuario, null);
                MarcaEndereco(endereco);
                //Utils.Interface.AlteraEstadoButtonRound(btnSelecionar ?? btnAdicionar, true);
                endereco.Coordenada = coordenadaCriadaUsuario;
                EnderecoCriadoUsuario = endereco;
                EnderecoCriadoUsuario.Complemento = string.IsNullOrEmpty(txtComplemento.Text) ? EnderecoCriadoUsuario.Complemento : txtComplemento.Text;
            }
            catch (ConnectionException cex)
            {
                Utils.Mensagem.Aviso(cex.Message);
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
            }
            catch (NSErrorException nsex)
            {
                //dummy
                //Utils.Mensagem.Aviso("Não foi possível modificar o endereço. Por favor tente novamente."); // + nsex.Domain + ":" + nsex.Code);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            finally
            {
                progressbarGeoCode.StopAnimating();
                progressbarGeoCode.Hidden = true;
            }
        }

        /// <summary>
        /// Tratamento do botão do localização atual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCurrentLocation_TouchUpInside(object sender, object e)
        {
            try
            {
                coordenadaCriadaUsuario = null;
                StubMapa.SetaLocalizacaoGPSNoMapa(null);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private bool ValidaReversao()
        {
            if (despresa_RegionChanged)
            {
                despresa_RegionChanged = false;
                //Utils.Interface.AlteraEstadoButtonRound(btnSelecionar ?? btnAdicionar, true);
                return false;
            }

            if (despresa_EmCache)
            {
                despresa_EmCache = false;
                //Utils.Interface.AlteraEstadoButtonRound(btnSelecionar ?? btnAdicionar, true);
                return false;
            }

            if (usuarioEditouTextoPesquisa)
            {
                return false;
            }

            if (coordenada_anterior.Latitude == 0 && coordenada_anterior.Longitude == 0)
            {
                coordenada_anterior = new CoordenadaDTO(oMapa.CenterCoordinate.Latitude, oMapa.CenterCoordinate.Longitude);
                return false;
            }

            if (coordenada_anterior.Latitude == oMapa.CenterCoordinate.Latitude && coordenada_anterior.Longitude == oMapa.CenterCoordinate.Longitude)
            {
                if (string.IsNullOrEmpty(srcEndereco.Text))
                {
                    return true;
                }
                else
                if (EnderecoCriadoUsuario == null || (EnderecoCriadoUsuario.Coordenada.Latitude == 0 && EnderecoCriadoUsuario.Coordenada.Longitude == 0) )
                {
                    return true;
                }
                return false;
            }

            if (oMapa.CenterCoordinate.Latitude.ToString() == StubMapa.CoordenadaDefault().Latitude.ToString() && oMapa.CenterCoordinate.Longitude.ToString() == StubMapa.CoordenadaDefault().Longitude.ToString())
            {
                return false;
            }

            coordenada_anterior = new CoordenadaDTO(oMapa.CenterCoordinate.Latitude, oMapa.CenterCoordinate.Longitude);

            return true;
        }

        private void MarcaEndereco(EnderecoDTO endereco)
        {
            if (!usuarioEditouTextoPesquisa && endereco != null)
            {
                srcEndereco.Text = endereco.ToString(false);
                txtComplemento.Text = endereco.Complemento;
                VisibilidadeSelecionaAdiciona();
            }
        }

        private async void ConfirmaEnderecoInvalido()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var controller = ((RootViewController)window.RootViewController).NavController;
            int button = await Utils.Mensagem.Questao("Confirmação", Strings.MSG_ValidaEnderecoForaDaArea, (o) => { }, NavigationController, "Cancelar", "Confirmar");
            if (button == 1)
            {
                VoltarParaChamar(controller);
            }
        }

        private async void ConfirmaEnderecoInvalidoFavorito()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var controller = ((RootViewController)window.RootViewController).NavController;
            int button = await Utils.Mensagem.Questao("Confirmação", Strings.MSG_ValidaEnderecoForaDaArea, (o) => { }, NavigationController, "Cancelar", "Confirmar");
            if (button == 1)
            {
                VoltarParaFavorito(controller);
            }
        }

        private void VoltarParaChamar(UINavigationController controller)
        {
            controller.PopToRootViewController(true);
            if (controller != null && controller.TopViewController is PrincipalViewController)
            {
                EnderecoCriadoUsuario.Nome = string.Empty;
                ((PrincipalViewController)controller.TopViewController).EnderecoCriadoUsuario = EnderecoCriadoUsuario;
            }
        }

        private void VoltarParaFavorito(UINavigationController controller)
        {
            FavoritosViewController favoritosVC = this.Storyboard.InstantiateViewController("FavoritosViewController") as FavoritosViewController;
            favoritosVC.Endereco = EnderecoCriadoUsuario;
            favoritosVC.FromWalkThrough = FromWalkThrough;
            controller.PushViewController(favoritosVC, true);
        }
    }
}