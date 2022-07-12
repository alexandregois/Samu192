using System;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Content.PM;
using Android;
using SAMU192Droid.FacadeStub;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Droid.Interface.Activities;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace SAMU192Droid.Interface.Fragments
{
    public class MapaFragment : BaseFragment, IOnMapReadyCallback
    {
        public static string TAG = "Mapa";

        #region Campos de tela
        View view;

        Button btnAcao;
        Android.Support.V7.Widget.SearchView svEndereco = null;

        MapFragment oMapFragment;
        GoogleMap oMapa = null;
        ImageView pin;
        LinearLayout llLoadingMsg;
        EditText complemento_et;
        EditText searchEditText;

        TextView complemento_tv;
        ImageView imgComplemento;
        ProgressBar loader;
        #endregion

        CoordenadaDTO coordenadaCriadaUsuario = null, coordenadaAuxiliar = new CoordenadaDTO();
        EnderecoDTO enderecoCriadoUsuario = null;
        bool usuarioPressionouBuscar = false, usuarioEditouTextoPesquisa = false, usuarioEmPesquisa = false, gpsPermitido = true, 
            respondeuConnection = true, primeiroMoveAposPesquisa = false, pulaPesquisa = false, pararGPS = false;
        string enderecoDigitadoAnterior = string.Empty;
        string enderocoSearchView;
        internal EnderecoDTO EnderecoCriadoUsuario { get => enderecoCriadoUsuario; set => enderecoCriadoUsuario = value; }

        bool modoSelecao;
        internal bool ModoSelecao { get => modoSelecao; set => modoSelecao = value; }
        Action after_Select;
        internal Action After_Select { get => after_Select; set => after_Select = value; }
        Activity activityAux;

        Color LARANJA_ESCURO, LARANJA;

        public override void HideDescendantsForAccessibility()
        {
            view.Visibility = ViewStates.Gone;

            complemento_tv.ImportantForAccessibility =
            imgComplemento.ImportantForAccessibility =
            loader.ImportantForAccessibility =
            btnAcao.ImportantForAccessibility =
            svEndereco.ImportantForAccessibility =
            pin.ImportantForAccessibility =
            llLoadingMsg.ImportantForAccessibility = 
            complemento_et.ImportantForAccessibility = 
            searchEditText.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            view.Visibility = ViewStates.Visible;

            complemento_tv.ImportantForAccessibility =
            imgComplemento.ImportantForAccessibility =
            loader.ImportantForAccessibility =
            btnAcao.ImportantForAccessibility =
            svEndereco.ImportantForAccessibility =
            pin.ImportantForAccessibility =
            llLoadingMsg.ImportantForAccessibility =
            complemento_et.ImportantForAccessibility =
            searchEditText.ImportantForAccessibility = ImportantForAccessibility.Yes;
        }


        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                activityAux = this.Activity;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            try
            {
                e.Handled = false;
                Utils.Interface.FechaTeclado(activityAux);
                SvEndereco_QueryTextSubmit(null, null);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                view = inflater.Inflate(Resource.Layout.mapa, null);

                LoadStubFacades();
                LoadScreenControls();
                LoadControlValues();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        public override void OnResume()
        {
            try
            {
                base.OnResume();
                CarregaGPS();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CarregaGPS()
        {
            gpsPermitido = true;
            string[] PermissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
            foreach (string permission in PermissionsLocation)
            {
                if (Utils.VersaoAndroid.QualquerLollipop)
                    gpsPermitido &= Android.Support.V4.Content.ContextCompat.CheckSelfPermission(activityAux.ApplicationContext, permission) == Permission.Granted;
                else
                    gpsPermitido &= activityAux.CheckSelfPermission(permission) == Permission.Granted;
            }
            if (gpsPermitido)
                StubGPS.Carrega(activityAux, AtualizaDadosGPS, (b) => { });
        }

        private void AbreConfiguracao()
        {
            StartActivity(new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings, Android.Net.Uri.Parse("package:" + activityAux.PackageName)));

        }

        private void InicializaMapFragment()
        {
            try
            {
                oMapFragment = (MapFragment)ChildFragmentManager.FindFragmentById(Resource.Id.mapa_map);
                oMapFragment.GetMapAsync(this);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("Timed out") >= 0)
                {
                    Utils.Mensagem.Erro(new Exception("O servidor aguardou muito tempo para localizar o endereço!"));
                }
                else
                {
                    Utils.Mensagem.Erro(ex);
                }
            }
        }

        /// <summary>
        /// IOnMapReadyCallback: Callback da implementação
        /// </summary>
        /// <param name="googleMap"></param>
        public async void OnMapReady(GoogleMap googleMap)
        {
            try
            {
                HabilitaSelecao(false);
                oMapa = googleMap;
                StubMapa.Carrega(oMapa);
                pulaPesquisa = false;

                CoordenadaDTO coordenada = EnderecoCriadoUsuario != null ? EnderecoCriadoUsuario.Coordenada : null;
                if (coordenada == null)
                {
                    if (gpsPermitido)
                        coordenada = StubGPS.GetLastLocation();
                }
                else
                {
                    coordenadaCriadaUsuario = StubMapa.SetaLocalizacaoGPSNoMapa(coordenada, 17);
                    svEndereco.SetQuery(EnderecoCriadoUsuario.ToString(false), false);
                    complemento_et.Text = EnderecoCriadoUsuario.Complemento;
                    searchEditText.RequestFocus();
                    searchEditText.SetSelection(0);
                    usuarioEditouTextoPesquisa = false;
                    HabilitaSelecao(true);
                    pulaPesquisa = true;
                }

                if (gpsPermitido)
                    coordenadaCriadaUsuario = StubMapa.SetaLocalizacaoGPSNoMapa(coordenada, 17);
                if (coordenadaCriadaUsuario == null)
                    StubMapa.PosicionaMapa(StubMapa.CoordenadaDefault(), 17);

                var MapaPronto = StubMapa.ConfigurarMapa();
                oMapa.CameraChange += OMapa_CameraChange;
                oMapa.CameraMoveStarted += OMapa_CameraMoveStarted;

                if (MapaPronto != null)
                {
                    MapaPronto.Invoke(oMapa);
                    EnderecoDTO endereco;
                    var result = await StubMapa.ReverterCoordenada(coordenadaCriadaUsuario, activityAux);
                    if (result != null && !pulaPesquisa)
                    {
                        endereco = new EnderecoDTO(result.Logradouro, result.Numero, string.IsNullOrEmpty(complemento_et.Text) ? result.Complemento : complemento_et.Text,
                        result.Bairro, result.Cidade, result.Estado, coordenadaCriadaUsuario, result.Nome, result.Referencia);
                        svEndereco.SetQuery(endereco.ToString(false), false);
                        complemento_et.Text = endereco.Complemento;
                        searchEditText.RequestFocus();
                        searchEditText.SetSelection(0);
                        usuarioEditouTextoPesquisa = false;
                        EnderecoCriadoUsuario = endereco;
                    }
                    HabilitaSelecao(true, true);
                }
                else
                {
                    svEndereco.QueryHint = "Digite seu endereço";
                }
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Dialogs.AlertaSimples(activityAux, vex.Message);
                HabilitaSelecao(false);
                llLoadingMsg.Visibility = ViewStates.Gone;
            }
            catch (ConnectionException cex)
            {
                svEndereco.QueryHint = "Digite seu endereço";
                if (respondeuConnection)
                {
                    respondeuConnection = false;
                    Utils.Mensagem.Dialogs.AlertaSimples(activityAux, cex.Message, () => { respondeuConnection = true; });
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
                HabilitaSelecao(true);
            }
        }

        private void OMapa_CameraMoveStarted(object sender, GoogleMap.CameraMoveStartedEventArgs e)
        {
            try
            {
                if (usuarioEditouTextoPesquisa || usuarioEmPesquisa)
                    return;
                GoogleMap mapTemp = (GoogleMap)sender;
                CoordenadaDTO novaCoordenada = new CoordenadaDTO(mapTemp.CameraPosition.Target.Latitude, mapTemp.CameraPosition.Target.Longitude);
                if (!coordenadaAuxiliar.Equals(novaCoordenada))
                    HabilitaSelecao(false);
                coordenadaAuxiliar = novaCoordenada;
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
                StubMapa.Carrega(oMapa);
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
                LARANJA_ESCURO = Utils.Interface.Cores.GetByID(Resource.Color.laranja_escuro, this.Activity.ApplicationContext);
                LARANJA = Utils.Interface.Cores.GetByID(Resource.Color.laranja, this.Activity.ApplicationContext);

                llLoadingMsg = view.FindViewById<LinearLayout>(Resource.Id.loading_msg_ll);
                svEndereco = view.FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.mapa_sv);
                svEndereco.QueryTextChange += SvEndereco_QueryTextChange;
                svEndereco.FocusChange += SvEndereco_QueryTextFocusChange;
                svEndereco.QueryTextSubmit += SvEndereco_QueryTextSubmit;

                svEndereco.Click += SvEndereco_SearchClick;
                svEndereco.QueryHint = "Digite o endereço";
                svEndereco.SetIconifiedByDefault(false);

                complemento_tv = view.FindViewById<TextView>(Resource.Id.complemento_tv);
                imgComplemento = view.FindViewById<ImageView>(Resource.Id.imgComplemento);
                loader = view.FindViewById<ProgressBar>(Resource.Id.loader);

                //Captura do EditText contido no componente de Pesquisa
                var a = svEndereco.FindViewById(Resource.Id.search_src_text);
                searchEditText = (EditText)a;
                searchEditText.SetTextColor(Color.White);
                searchEditText.TextSize = 16;

                pin = view.FindViewById<ImageView>(Resource.Id.pin);

                btnAcao = view.FindViewById<Button>(Resource.Id.mapa_cmd);

                complemento_et = view.FindViewById<EditText>(Resource.Id.complemento_et);

                complemento_et.SetRawInputType(Android.Text.InputTypes.ClassText);
                complemento_et.SetImeActionLabel("Buscar", Android.Views.InputMethods.ImeAction.Done);
                complemento_et.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
                complemento_et.EditorAction += HandleEditorAction;

                if (ModoSelecao)
                {
                    btnAcao.Click += cmd_Click_Selecionar;
                }
                else
                {
                    btnAcao.Click += cmd_Click_Adicionar;
                }

                InicializaMapFragment();
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
                CoordenadaDTO coordenada = EnderecoCriadoUsuario != null ? EnderecoCriadoUsuario.Coordenada : null;
                coordenadaCriadaUsuario = StubMapa.SetaLocalizacaoGPSNoMapa(coordenada);
                usuarioPressionouBuscar = true;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SvEndereco_QueryTextChange(object sender, Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            try
            {
                usuarioEditouTextoPesquisa = true;
                btnAcao.Enabled = false;
                btnAcao.SetBackgroundColor(LARANJA_ESCURO);
                btnAcao.SetTextColor(Color.DarkGray);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SvEndereco_QueryTextFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            try
            {
                //usuarioEmPesquisa = e.HasFocus;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private async void SvEndereco_QueryTextSubmit(object sender, Android.Support.V7.Widget.SearchView.QueryTextSubmitEventArgs e)
        {
            try
            {
                EnderecoDTO result = new EnderecoDTO();
                HabilitaSelecao(false);
                usuarioEmPesquisa = true;
                usuarioEditouTextoPesquisa = false;
                result = await StubMapa.ReverterEndereco(svEndereco.Query, activityAux);

                StubGPS.StopLocationManager();
                if (result != null)
                {
                    EnderecoDTO endereco = new EnderecoDTO(result.Logradouro, result.Numero, result.Complemento,
                            result.Bairro, result.Cidade, result.Estado, result.Coordenada, result.Nome, result.Referencia);

                    primeiroMoveAposPesquisa = true;
                    coordenadaCriadaUsuario = StubMapa.PosicionaMapa(endereco.Coordenada, 17);
                    EnderecoCriadoUsuario = endereco;
                    svEndereco.SetQuery(endereco.ToString(), false);
                }
                else
                {
                    Utils.Mensagem.Aviso("Não foi possível encontrar o endereço digitado. Por favor, verifique o endereço digitado e faça novamente a pesquisa.");
                }
                usuarioEmPesquisa = false;
                HabilitaSelecao(true);
                Utils.Interface.FechaTeclado(activityAux);
            }
            catch (Java.Lang.Exception jex)
            {
                //dummy
                if (usuarioEditouTextoPesquisa)
                    Utils.Mensagem.Aviso("Não foi possível encontrar o endereço digitado. Por favor, verifique o endereço digitado e faça novamente a pesquisa.");
                HabilitaSelecao(true);
            }
            catch (ConnectionException cex)
            {
                if (respondeuConnection)
                {
                    respondeuConnection = false;
                    Utils.Mensagem.Dialogs.AlertaSimples(activityAux, cex.Message, () => { respondeuConnection = true; });
                    HabilitaSelecao(true);
                    usuarioEditouTextoPesquisa = false;
                }
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
                HabilitaSelecao(true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
                HabilitaSelecao(true);
            }
            finally
            {
                usuarioEditouTextoPesquisa = false;
                HabilitaSelecao(true);
            }
        }

        private void HabilitaSelecao(bool enable, bool loading = true)
        {
            try
            {
                llLoadingMsg.Visibility = enable && loading ? ViewStates.Gone : ViewStates.Visible;
                btnAcao.Enabled = enable;
                btnAcao.SetBackgroundColor(enable ? LARANJA : LARANJA_ESCURO);
                btnAcao.SetTextColor(enable ? Color.White : Color.DarkGray);

                Drawable img;
                if (enable)
                {
                    img = activityAux.ApplicationContext.Resources.GetDrawable(Resource.Drawable.ic_check);
                    img.SetTint(Color.White);
                    img.SetBounds(0, 0, 60, 60);
                }
                else
                {
                    img = activityAux.ApplicationContext.Resources.GetDrawable(Resource.Drawable.ic_check);
                    img.SetTint(Color.DarkGray);
                    img.SetBounds(0, 0, 60, 60);
                }

                btnAcao.SetCompoundDrawables(img, null, null, null);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void SvEndereco_SearchClick(object sender, EventArgs e)
        {
            try
            {
                usuarioEditouTextoPesquisa = true;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }        

        private void AtualizaDadosGPS(CoordenadaDTO coordenada)
        {
            try
            {
                if (pararGPS)
                    return;
                if (oMapa != null)
                    coordenadaCriadaUsuario = StubMapa.PosicionaMapa((coordenadaCriadaUsuario ?? coordenada), null);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private async void OMapa_CameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {            
            if (usuarioEditouTextoPesquisa || usuarioEmPesquisa)
                return;

            if (primeiroMoveAposPesquisa)
            {
                //Evita reversão de coordenada logo após uma reversão de texto
                //Evita discrepância do endereço encontrado na reversão "texto" VERSUS reversão de "coordenada"
                primeiroMoveAposPesquisa = false;
                return;
            }

            try
            {
                pararGPS = true;
                coordenadaCriadaUsuario = new CoordenadaDTO(oMapa.CameraPosition.Target.Latitude, oMapa.CameraPosition.Target.Longitude);
                if (coordenadaCriadaUsuario != null)
                {
                    HabilitaSelecao(false);
                    var result = await StubMapa.ReverterCoordenada(coordenadaCriadaUsuario, activityAux.ApplicationContext);
                    if (result != null)
                    {
                        EnderecoDTO endereco;
                        endereco = new EnderecoDTO(result.Logradouro, result.Numero, result.Complemento,
                        result.Bairro, result.Cidade, result.Estado, coordenadaCriadaUsuario, result.Nome, result.Referencia);

                        if (!usuarioEditouTextoPesquisa && !usuarioEmPesquisa && enderocoSearchView != endereco.ToString())
                        {
                            enderocoSearchView = endereco.ToString();
                            svEndereco.SetQuery(endereco.ToString(false), false);
                            complemento_et.Text = endereco.Complemento;
                            searchEditText.RequestFocus();
                            searchEditText.SetSelection(0);
                            usuarioEditouTextoPesquisa = false;
                        }
                        btnAcao.Enabled = true;
                        EnderecoCriadoUsuario = endereco;
                        HabilitaSelecao(true);
                    }
                    else
                    {
                        HabilitaSelecao(false, false);
                    }
                }
            }
            catch (Java.IO.IOException ioex)
            {
                //dummy
                //Utils.Log(ioex.ToString());
            }
            catch (ConnectionException cex)
            {
                if (respondeuConnection)
                {
                    respondeuConnection = false;
                    Utils.Mensagem.Dialogs.AlertaSimples(activityAux, cex.Message, () => { respondeuConnection = true; });
                    HabilitaSelecao(false);
                    llLoadingMsg.Visibility = ViewStates.Gone;
                }
            }
            catch (ValidationException vex)
            {
                if (!StubUtilidades.AppEmProducao())
                    Utils.Mensagem.Aviso(vex.Message);
                HabilitaSelecao(false);
                llLoadingMsg.Visibility = ViewStates.Gone;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
                HabilitaSelecao(false);
                llLoadingMsg.Visibility = ViewStates.Gone;
            }
        }

        private void MarcaEndereco(EnderecoDTO endereco)
        {
            svEndereco.SetQuery(endereco != null ? endereco.ToString(false) : string.Empty, false);
            complemento_et.Text = endereco.Complemento;
            searchEditText.RequestFocus();
            searchEditText.SetSelection(0);
            usuarioEditouTextoPesquisa = false;
            svEndereco.TextDirection = TextDirection.FirstStrong;
        }

        private void cmd_Click_Selecionar(object sender, EventArgs e)
        {
            try
            {
                if (StubLocalizacao.ValidarEndereco(EnderecoCriadoUsuario))
                {
                    EnderecoCriadoUsuario.Complemento = string.IsNullOrEmpty(complemento_et.Text) ? "" : complemento_et.Text;
                    VoltarParaFragmet(Resource.Id.tabFrameLayout1, new InicialFragment(), Resource.String.chamar_title, InicialFragment.TAG);
                    After_Select();
                }
                else
                {
                    Utils.Mensagem.Alerta("Endereço inválido ou fora da área de cobertura. Verifique o endereço pesquisado.");
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

        private void cmd_Click_Adicionar(object sender, EventArgs e)
        {
            try
            {
                if (StubLocalizacao.ValidarEndereco(EnderecoCriadoUsuario))
                {
                    EnderecoCriadoUsuario.Complemento = string.IsNullOrEmpty(complemento_et.Text) ? "" : complemento_et.Text;
                    var favoritoNomeFragment = new FavoritosNomeFragment();
                    favoritoNomeFragment.Endereco = EnderecoCriadoUsuario;
                    favoritoNomeFragment.FromWalkthrough = StubWalkThrough.GetWalkThrough();
                    this.HideDescendantsForAccessibility();
                    Utils.Interface.EmpilharFragment((MainActivity)activityAux, Resource.Id.tabFrameLayout1, favoritoNomeFragment, Resource.String.favoritos_nome_title, FavoritosNomeFragment.TAG);
                }
                else
                {
                    Utils.Mensagem.Alerta("Endereço inválido ou fora da área de cobertura. Verifique o endereço pesquisado.");
                    HabilitaSelecao(false);
                    llLoadingMsg.Visibility = ViewStates.Gone;
                    return;
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

        private void VoltarParaFragmet(int resourceId, Fragment fragment, int resourceIdTitle, string tag)
        {
            StubLocalizacao.ValidarEndereco(EnderecoCriadoUsuario);

            var mainActivity = ((MainActivity)activityAux);
            mainActivity.EnderecoCriadoUsuario = EnderecoCriadoUsuario;

            Utils.Interface.RetirarFragment(activityAux);
            Utils.Interface.RetirarFragment(activityAux);
            Utils.Interface.EmpilharFragment(mainActivity, resourceId, fragment, resourceIdTitle, tag);
        }
    }
}