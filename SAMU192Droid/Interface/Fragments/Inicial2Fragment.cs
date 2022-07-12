using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using SAMU192Core.DTO;
using SAMU192Droid.FacadeStub;
using SAMU192Core.Exceptions;
using SAMU192Droid.Interface.Activities;
using SAMU192Droid.Implementations;
using Android;
using Android.Content.PM;
using SAMU192Core.Utils;
using Microsoft.AppCenter.Analytics;

namespace SAMU192Droid.Interface.Fragments
{
    public class Inicial2Fragment : BaseFragment
    {
        public static string TAG = "Inicial";
        #region Campos de tela
        View view;

        ImageButton cmdChamar192_ligar;
        ImageButton cmdChat192_paramim;
        ImageButton cmdChat192_paraoutrapessoa;
        ImageButton cmdChamar192_enviarfoto;

        Button cmdMapa;
        Button cmdFavoritos;
        Button cmdGPS;
        Button cmdFora;
        Button main_seja_tv_2;
        LinearLayout main_fora_ll;
        LinearLayout main_chamado_ll;
        LinearLayout main_seja_ll;
        LinearLayout main_chamado_queixa;
        LinearLayout main_chamado_favoritos_ll;
        LinearLayout main_chamado_user_ll_ll;
        LinearLayout llTelefones;
        TextView main_chamado_favoritos_ondeestou_tv;
        EditText main_chamado_queixa_et;
        TextView main_chamado_user_nome_tv;
        TextView main_chamado_user_info_tv;
        ImageView iv_Seja;
        Button ivCadastroEditar;
        List<LinearLayout> lLlOndeEstouCell;
        ProgressBar progressBar;
        Button ultimoButtonSelecionado;

        Color LARANJA;
        Color LARANJA_ESCURO;
        #endregion

        CadastroDTO cadastro;
        SolicitarAtendimentoDTO PACOTE;
        string PLACE_HOLDER_QUEIXA = "Principal motivo que leva a acionar o SAMU.", QUEIXA = string.Empty;
        MainActivity activityAux;
        object phoneManager;
        public static bool emLigacao = false;
        bool paraOutraPessoa = false, gpsSelecionado = false, mapaSelecionado = false, favoritosSelecionado = false, podePedirAutorizacaoGPS = true, 
             pedidoAutorizacaoGPSRealizado = false, gpsPermitido = true;

        EnderecoDTO enderecoCriadoUsuario, enderecoGPS;
        internal EnderecoDTO EnderecoCriadoUsuario { get => enderecoCriadoUsuario; set => enderecoCriadoUsuario = value; }
        internal EnderecoDTO EnderecoGPS { get => enderecoGPS; set => enderecoGPS = value; }

        public override void HideDescendantsForAccessibility()
        {
            view.Visibility = ViewStates.Gone;
            view.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;


            cmdChamar192_ligar.ImportantForAccessibility =
            cmdChat192_paramim.ImportantForAccessibility =
            cmdChat192_paraoutrapessoa.ImportantForAccessibility =
            cmdChamar192_enviarfoto.ImportantForAccessibility =
            cmdMapa.ImportantForAccessibility =
            cmdFavoritos.ImportantForAccessibility =
            cmdGPS.ImportantForAccessibility =
            cmdFora.ImportantForAccessibility =
            main_seja_tv_2.ImportantForAccessibility =
            main_fora_ll.ImportantForAccessibility =
            //main_chamado_ll.ImportantForAccessibility =
            main_seja_ll.ImportantForAccessibility =
            main_chamado_queixa.ImportantForAccessibility =
            main_chamado_favoritos_ll.ImportantForAccessibility =
            //main_chamado_user_ll_ll.ImportantForAccessibility =
            llTelefones.ImportantForAccessibility =
            main_chamado_favoritos_ondeestou_tv.ImportantForAccessibility =
            main_chamado_queixa_et.ImportantForAccessibility =
            main_chamado_user_nome_tv.ImportantForAccessibility =
            main_chamado_user_info_tv.ImportantForAccessibility =
            iv_Seja.ImportantForAccessibility =
            ivCadastroEditar.ImportantForAccessibility =
            progressBar.ImportantForAccessibility = ImportantForAccessibility.No;
            //ultimoButtonSelecionado.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
            lLlOndeEstouCell.ForEach(o=> o.ImportantForAccessibility = ImportantForAccessibility.No);
        }

        public override void ShowDescendantsForAccessibility()
        {
            view.Visibility = ViewStates.Visible;
            view.ImportantForAccessibility = ImportantForAccessibility.Yes;

            cmdChamar192_ligar.ImportantForAccessibility =
            cmdChat192_paramim.ImportantForAccessibility =
            cmdChat192_paraoutrapessoa.ImportantForAccessibility =
            cmdChamar192_enviarfoto.ImportantForAccessibility =
            cmdMapa.ImportantForAccessibility =
            cmdFavoritos.ImportantForAccessibility =
            cmdGPS.ImportantForAccessibility =
            cmdFora.ImportantForAccessibility =
            main_seja_tv_2.ImportantForAccessibility =
            main_fora_ll.ImportantForAccessibility =
            //main_chamado_ll.ImportantForAccessibility =
            main_seja_ll.ImportantForAccessibility =
            main_chamado_queixa.ImportantForAccessibility =
            main_chamado_favoritos_ll.ImportantForAccessibility =
            //main_chamado_user_ll_ll.ImportantForAccessibility =
            llTelefones.ImportantForAccessibility =
            main_chamado_favoritos_ondeestou_tv.ImportantForAccessibility =
            main_chamado_queixa_et.ImportantForAccessibility =
            main_chamado_user_nome_tv.ImportantForAccessibility =
            main_chamado_user_info_tv.ImportantForAccessibility =
            iv_Seja.ImportantForAccessibility =
            ivCadastroEditar.ImportantForAccessibility =
            progressBar.ImportantForAccessibility = ImportantForAccessibility.Yes;
            //ultimoButtonSelecionado.ImportantForAccessibility = ImportantForAccessibility.Yes;
            lLlOndeEstouCell.ForEach(o => o.ImportantForAccessibility = ImportantForAccessibility.Yes);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                activityAux = (MainActivity)this.Activity;
                phoneManager = activityAux.GetSystemService("phone");
                Utils.Interface.FechaTeclado(Activity);
            }
            catch (ValidationException vex)
            {
                activityAux.RunOnUiThread(() => 
                {
                    Utils.Mensagem.Erro(vex);
                });
            }
            catch (Exception ex)
            {
                activityAux.RunOnUiThread(() =>
                {
                    Utils.Mensagem.Erro(ex);
                });
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                view = inflater.Inflate(Resource.Layout.inicial2, null);
                LoadScreenControls();
                LoadStubFacades();                

                cadastro = StubCadastro.RecuperaCadastro();
                if (!StubCadastro.ExisteCadastro(cadastro))
                {
                    LoadScreenControls();//força reload com os campos recuperados do cadastro
                    this.HideDescendantsForAccessibility();
                    Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, new CadastroFragment(), Resource.String.cadastro_title, CadastroFragment.TAG);
                }
                LoadControlValues();
                Utils.Interface.FechaTeclado(Activity);
            }
            catch (Exception ex)
            {
                activityAux.RunOnUiThread(() =>
                {
                    Utils.Mensagem.Erro(ex);
                });
            }
            return view;
        }

        public override void OnStop()
        {
            try
            {
                base.OnStop();
                StubGPS.Descarrega();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void OnPause()
        {
            try
            {
                base.OnPause();
                StubGPS.Descarrega();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void OnResume()
        {
            try
            {
                base.OnResume();
                StubTelefonia.Carrega(UpdatePhoneCallConnected, UpdatePhoneCallDisconnected, phoneManager);

                CarregaGPS();

                activityAux.RunOnUiThread(() =>
                {
                    HabilitaBotaoFoto();
                    if (emLigacao)
                        VisibilidadeBotoesTelefone(false);
                });
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
                    gpsPermitido &= Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this.Activity.ApplicationContext, permission) == Permission.Granted;
                else
                    gpsPermitido &= Activity.CheckSelfPermission(permission) == Permission.Granted;
            }

            var teste = gpsPermitido;
            if (!gpsPermitido && podePedirAutorizacaoGPS)
            {
                podePedirAutorizacaoGPS = false;
                pedidoAutorizacaoGPSRealizado = true;
                Activity.RequestPermissions(PermissionsLocation, (int)Enums.RequestPermissionCode.GPS);
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.GPS_Pedido_Permissao.Value, this.Tag);
            }

            if (gpsPermitido)  
            {
                StubGPS.Carrega(this.Activity, AtualizaDadosGPS, null);
                VisibilidadeForaDaArea(false);
                VisibilidadePadrao(true);
                VisibilidadeBotoesTelefone(true);

                //cmdChamar192_ligar.Visibility = ViewStates.Gone;
                cmdChamar192_ligar.Visibility = ViewStates.Visible;

                view.FindViewById<LinearLayout>(Resource.Id.main_chamado_ll).Visibility = ViewStates.Visible;
            }
            else
            {
                VisibilidadeForaDaArea(true);
                VisibilidadePadrao(false);
                VisibilidadeTirarFoto(false);
                VisibilidadeBotoesTelefone(false);
                cmdChamar192_ligar.Visibility = ViewStates.Visible;
                view.FindViewById<LinearLayout>(Resource.Id.main_chamado_ll).Visibility = ViewStates.Gone;
            }
        }

        internal void LoadStubFacades()
        {
            StubTelefonia.Carrega(UpdatePhoneCallConnected, UpdatePhoneCallDisconnected, phoneManager, true);
            StubLocalizacao.Carrega(activityAux.Assets);
            StubWebService.Carrega(null, LiberarBotaoFoto_Callback);
            StubCadastro.Carrega();
        }

        internal void LoadScreenControls()
        {
            try
            {
                LARANJA = Utils.Interface.Cores.GetByID(Resource.Color.laranja, this.Activity.ApplicationContext);
                LARANJA_ESCURO = Utils.Interface.Cores.GetByID(Resource.Color.laranja_escuro, this.Activity.ApplicationContext);

                
                cmdChamar192_ligar = view.FindViewById<ImageButton>(Resource.Id.chamar192_cmd_ligar);
                cmdChamar192_ligar.Visibility = ViewStates.Visible;
                cmdChamar192_ligar.Click += chamar192_cmd_ligar_Click;

                cmdChat192_paramim = view.FindViewById<ImageButton>(Resource.Id.chat192_cmd_paramim);
                cmdChat192_paramim.Click += chat192_cmd_paramim_Click;

                cmdChat192_paraoutrapessoa = view.FindViewById<ImageButton>(Resource.Id.chat192_cmd_paraoutrapessoa);
                cmdChat192_paraoutrapessoa.Click += chat192_cmd_paraoutrapessoa_Click;

                cmdChamar192_enviarfoto = view.FindViewById<ImageButton>(Resource.Id.chamar192_cmd_enviarfoto);
                cmdChamar192_enviarfoto.Click += chamar192_cmd_enviarfoto_Click;
                llTelefones = view.FindViewById<LinearLayout>(Resource.Id.main_chamado_user_telefones_ll);

                main_fora_ll = view.FindViewById<LinearLayout>(Resource.Id.main_fora_ll);
                main_chamado_ll = view.FindViewById<LinearLayout>(Resource.Id.main_chamado_ll);
                main_seja_ll = view.FindViewById<LinearLayout>(Resource.Id.main_seja_ll);
                main_chamado_queixa = view.FindViewById<LinearLayout>(Resource.Id.main_chamado_queixa);
                main_chamado_favoritos_ll = view.FindViewById<LinearLayout>(Resource.Id.main_chamado_favoritos_11);
                main_chamado_user_ll_ll = view.FindViewById<LinearLayout>(Resource.Id.main_chamado_user_ll_ll);
                main_seja_tv_2 = view.FindViewById<Button>(Resource.Id.main_seja_tv_2);

                main_chamado_user_nome_tv = view.FindViewById<TextView>(Resource.Id.main_chamado_user_nome_tv);
                main_chamado_user_info_tv = view.FindViewById<TextView>(Resource.Id.main_chamado_user_info_tv);

                //Onde Estou ? //////////
                lLlOndeEstouCell = new List<LinearLayout>();
                main_chamado_favoritos_ondeestou_tv = (TextView)main_chamado_favoritos_ll.FindViewById(Resource.Id.main_chamado_favoritos_ondeestou_tv);
                progressBar = main_chamado_favoritos_ll.FindViewById<ProgressBar>(Resource.Id.main_chamado_progressbar);
                
                cmdMapa = (Button)view.FindViewById(Resource.Id.main_mapa_cmd);
                cmdMapa.Click += CmdMapa_Click;

                cmdGPS = (Button)view.FindViewById(Resource.Id.main_gps_cmd);
                cmdGPS.Click += CmdGPS_Click;

                cmdFora = (Button)view.FindViewById(Resource.Id.main_fora_cmd);
                cmdFora.Click += CmdFora_Click;

                cmdFavoritos = (Button)view.FindViewById(Resource.Id.main_favoritos_cmd);
                cmdFavoritos.Click += CmdSelecionar_Click;
                              
                main_chamado_queixa_et = view.FindViewById<EditText>(Resource.Id.main_chamado_queixa_et);
                main_chamado_queixa_et.SetRawInputType(Android.Text.InputTypes.ClassText);
                main_chamado_queixa_et.SetImeActionLabel("Feito", Android.Views.InputMethods.ImeAction.Done);
                main_chamado_queixa_et.ImeOptions = Android.Views.InputMethods.ImeAction.Next;
                main_chamado_queixa_et.EditorAction += HandleEditorAction;

                ivCadastroEditar = view.FindViewById<Button>(Resource.Id.main_chamado_user_edit_iv);
                iv_Seja = view.FindViewById<ImageView>(Resource.Id.main_seja_iv);
                iv_Seja.Click += Iv_Seja_Click;
                iv_Seja = view.FindViewById<ImageView>(Resource.Id.main_seja_iv);
                ivCadastroEditar.Click += IvCadastroEditar_Click;
                main_seja_tv_2.Click += Main_seja_tv_2_Click;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CmdFora_Click(object sender, EventArgs e)
        {
            try
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                alert.SetTitle(Resource.String.fora_cobertura);
                alert.SetMessage(Activity.GetString(Resource.String.regiao_atendimento));
                alert.SetPositiveButton("Entendido", (senderAlert, args) => { });

                Dialog dialog = alert.Create();
                dialog.Show();
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
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void Main_seja_tv_2_Click(object sender, EventArgs e)
        {
            try
            {
                StubUtilidades.SetFromMenu(true);
                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, new QuandoFragment(), Resource.String.quando_title, QuandoFragment.TAG);
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
                VisibilidadeTirarFoto(false);
                CarregaCadastro(StubCadastro.ExisteCadastro(cadastro));
                CarregaEnderecos();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        internal void CarregaEnderecos()
        {
            StubCadastro.Carrega();
            var enderecos = StubCadastro.RecuperaEnderecos();
            if (enderecos != null && enderecos.Count > 0)
            {
                main_chamado_favoritos_ondeestou_tv.Text = enderecos[0].Nome.ToUpper() + " - " + enderecos[0].ToString();
                SelecionaButtonEdereco(cmdFavoritos);
                progressBar.Visibility = ViewStates.Gone;
                EnderecoCriadoUsuario = enderecos[0];
            }
            else
            {
                SelecionaButtonEdereco(cmdGPS);
                main_chamado_favoritos_ondeestou_tv.Text = "";
                progressBar.Visibility = ViewStates.Visible;
            }

            var mainActivity = ((MainActivity)activityAux);
            if (mainActivity.EnderecoCriadoUsuario != null)
            {
                EnderecoCriadoUsuario = mainActivity.EnderecoCriadoUsuario;
                string endereco;
                if (!string.IsNullOrEmpty(EnderecoCriadoUsuario.Nome))
                {
                    endereco = EnderecoCriadoUsuario.Nome.ToUpper() + " - " + EnderecoCriadoUsuario.ToString();
                    SelecionaButtonEdereco(cmdFavoritos);
                }
                else
                {
                    endereco = EnderecoCriadoUsuario.ToString();
                    SelecionaButtonEdereco(cmdMapa);
                }
                main_chamado_favoritos_ondeestou_tv.Text = endereco;
                progressBar.Visibility = ViewStates.Gone;
            }
        }

        private void CarregaCadastro(bool cadastrado)
        {
            main_chamado_user_nome_tv.Text = (cadastro == null ? "Nome da Pessoa" : cadastro.ToString());

            llTelefones = view.FindViewById<LinearLayout>(Resource.Id.main_chamado_user_telefones_ll);
            if (cadastro != null)
            {
                foreach (TelefoneDTO item in cadastro.Telefones)
                {
                    Utils.Interface.CreatePhoneTextView2(Activity, llTelefones, cadastrado ? string.Format("({0}) {1}-{2}", item.Ddd, item.Numero.Substring(0, 5), item.Numero.Substring(5, 4)) : "(51) 99999-1234");
                }
            }
        }

        private void CmdGPS_Click(object sender, EventArgs e)
        {
            try
            {
                StubGPS.Descarrega();
                SelecionaButtonEdereco(cmdGPS);
                CarregaGPS();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CmdMapa_Click(object sender, EventArgs e)
        {
            try
            {
                SelecionaButtonEdereco(cmdMapa);

                var mapaFragment = new MapaFragment();
                mapaFragment.EnderecoCriadoUsuario = EnderecoCriadoUsuario;
                mapaFragment.ModoSelecao = true;
                mapaFragment.After_Select = () => 
                {
                    try
                    {
                        CarregaEnderecos();
                        activityAux.SupportActionBar?.SetTitle(Utils.Interface.LastTitleResourceId());
                    }
                    catch (Exception ex)
                    {
                        Utils.Mensagem.Erro(ex);
                    }
                };
                StubUtilidades.SetFromMenu(true);
                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, mapaFragment, Resource.String.mapa_title, MapaFragment.TAG);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void CmdSelecionar_Click(object sender, EventArgs e)
        {
            try
            {
                SelecionaButtonEdereco(cmdFavoritos);
                var favoritosFragment = new FavoritosFragment();
                favoritosFragment.After_Select = () => 
                {
                    try
                    { 
                        CarregaEnderecos();
                        activityAux.SupportActionBar?.SetTitle(Utils.Interface.LastTitleResourceId());
                        this.ShowDescendantsForAccessibility();
                    }
                    catch (Exception ex)
                    {
                        Utils.Mensagem.Erro(ex);
                    }
                };
                StubUtilidades.SetFromMenu(true);
                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, favoritosFragment, Resource.String.favoritos_title, FavoritosFragment.TAG);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex); 
            }
        }

        private void SelecionaButtonEdereco(Button button)
        {
            if (button == cmdGPS)
            {
                gpsSelecionado = true;
                favoritosSelecionado = false;
                mapaSelecionado = false;
                progressBar.Visibility = ViewStates.Visible;
                cmdFavoritos.SetBackgroundColor(LARANJA);
                cmdMapa.SetBackgroundColor(LARANJA);
                EnderecoCriadoUsuario = null;
            }
            else
            if (button == cmdFavoritos)
            {
                gpsSelecionado = false;
                favoritosSelecionado = true;
                mapaSelecionado = false;
                progressBar.Visibility = ViewStates.Gone;
                cmdMapa.SetBackgroundColor(LARANJA);
                cmdGPS.SetBackgroundColor(LARANJA);
            }
            else
            if (button == cmdMapa)
            {
                mapaSelecionado = false;
                favoritosSelecionado = false;
                mapaSelecionado = true;
                progressBar.Visibility = ViewStates.Gone;
                cmdFavoritos.SetBackgroundColor(LARANJA);
                cmdGPS.SetBackgroundColor(LARANJA);
            }

            button.SetBackgroundColor(LARANJA_ESCURO);

            foreach (LinearLayout ll in lLlOndeEstouCell)
            {
                ll.Selected = false;
                ((ImageView)ll.FindViewById(Resource.Id.inicial_favoritos_cell_radio_iv)).SetBackgroundResource(Android.Resource.Drawable.RadiobuttonOffBackground);
            }

            button.Enabled = true;
            ultimoButtonSelecionado = button;
        }

        private void LigarEnviarPacote(bool outraPessoa)
        {
            try
            {
                //paraOutraPessoa = outraPessoa;
                //QUEIXA = (main_chamado_queixa_et.Text == PLACE_HOLDER_QUEIXA ? string.Empty : main_chamado_queixa_et.Text);

                //Dispara assíncrono o envio de pacote e efetua a ligação.
                BackgroundTask bgPacote = new BackgroundTask(activityAux, false, PreExecute_Pacote, RunInBackGround_Pacote, PostExecute_Pacote, OnCancel_Pacote, OnError_Pacote, OnValidationException_Pacote, "Enviando dados. Aguarde...", 30000, this.View);
                bgPacote.Execute();

                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, new ChatFragment(), Resource.String.cadastro_title, ChatFragment.TAG);

                //Efetua ligação
                //StubTelefonia.FazerLigacao(activityAux);

            }
            catch (ValidationException vex)
            {
                if (!StubUtilidades.AppEmProducao())
                    Utils.Mensagem.Alerta(vex.Message);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void RunInBackGround_Pacote(System.Threading.CancellationToken ct)
        {
            StubWebService.SolicitarAtendimento(PACOTE);
        }

        private void PreExecute_Pacote()
        {            
            PACOTE = StubWebService.MontaPacote(paraOutraPessoa, QUEIXA, cadastro, EnderecoCriadoUsuario ?? EnderecoGPS);
        }

        private void PostExecute_Pacote()
        { }

        private void OnCancel_Pacote()
        {
            //Não deve informar o usuário em caso de erro
        }

        private void OnError_Pacote(Exception ex)
        {
            Utils.Mensagem.Erro(ex);
        }

        private void OnValidationException_Pacote(ValidationException vex)
        {
            //Não deve informar o usuário em caso de erro
            StubAppCenter.AppAnalytic(Enums.AnalyticsType.EnvioPacote_Validacao.Value, this.Tag, vex.Message);
        }

        private void chamar192_cmd_ligar_Click(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Ligar.Value, this.Tag);
                LigarEnviarPacote(false);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void chat192_cmd_paramim_Click(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Chat_ParaMim.Value, this.Tag);
                LigarEnviarPacote(false);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void chat192_cmd_paraoutrapessoa_Click(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Chat_ParaOutraPessoa.Value, this.Tag);
                LigarEnviarPacote(true);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        internal void LiberarBotaoFoto_Callback(ServidorDTO servidor)
        {
            activityAux.RunOnUiThread(() => { 
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

        private void HabilitaBotaoFoto()
        {
            activityAux.RunOnUiThread(() =>
            {
                bool habilita = StubWebService.Servidor != null;
                VisibilidadeTirarFoto(habilita);
            });
        }

        internal void UpdatePhoneCallConnected()
        {
            activityAux.RunOnUiThread(() =>
            {
                try
                {
                    VisibilidadeBotoesTelefone(false);
                    VisibilidadePadrao(false);
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
            activityAux.RunOnUiThread(() =>
            {
                try
                {
                    VisibilidadePadrao(true);
                    emLigacao = false;
                    StubUtilidades.DesligaServico();
                    StubWebService.Servidor = null;
                    HabilitaBotaoFoto();
                    VisibilidadeBotoesTelefone(true);
                    Utils.Interface.FechaTeclado(activityAux);
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }

        private async void AtualizaDadosGPS(CoordenadaDTO coordenada)
        {
            {
                try
                {
                    var servidores = StubLocalizacao.Localizar(coordenada);
                    if (servidores != null && servidores.Count > 0)
                    {
                        if (EnderecoCriadoUsuario == null && gpsSelecionado)
                        {
                            var endereco = await StubMapa.ReverterCoordenada(coordenada, activityAux);
                            if (endereco != null)
                            {
                                main_chamado_favoritos_ondeestou_tv.Text = endereco.ToString();
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
                    progressBar.Visibility = ViewStates.Gone;
                }
                catch (ValidationException vex)
                {
                    if (!StubUtilidades.AppEmProducao())
                        Utils.Mensagem.Aviso(vex.Message);
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            }
            //});
        }

        private void Iv_Seja_Click(object sender, EventArgs e)
        {
            try
            {
                StubUtilidades.SetFromMenu(true);
                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, new QuandoFragment(), Resource.String.quando_title, QuandoFragment.TAG);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void chamar192_cmd_enviarfoto_Click(object sender, EventArgs e)
        {
            try
            {
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.Foto_Click.Value, this.Tag);
                StubUtilidades.SetFromMenu(true);
                Utils.Interface.IniciaNovaActivity((MainActivity)activityAux, typeof(CameraActivity), 0, (MainActivity) (Utils.VersaoAndroid.QualquerLollipop ? this.Activity : this.Context));
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void IvCadastroEditar_Click(object sender, EventArgs e)
        {
            try
            {
                StubUtilidades.SetFromMenu(true);
                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, new CadastroFragment(), Resource.String.cadastro_title, CadastroFragment.TAG);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void VisibilidadeForaDaArea(bool show)
        {
            //cmdChamar192_paramim.Visibility =
                cmdChat192_paraoutrapessoa.Visibility = main_chamado_queixa.Visibility = main_chamado_favoritos_ll.Visibility = main_chamado_user_ll_ll.Visibility = main_seja_ll.Visibility = !show ? ViewStates.Visible : ViewStates.Gone;

            //cmdChamar192_ligar.Visibility =
                main_fora_ll.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            llTelefones.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            main_chamado_user_info_tv.Visibility = show ? ViewStates.Gone : ViewStates.Visible;
            if (show) StubAppCenter.AppAnalytic(Enums.AnalyticsType.ForaDeArea.Value, this.Tag);
        }

        private void VisibilidadePadrao(bool show)
        {
            cmdChamar192_ligar.Visibility
                =
                cmdChat192_paramim.Visibility = cmdChat192_paraoutrapessoa.Visibility = main_chamado_favoritos_ll.Visibility = main_chamado_queixa.Visibility = main_chamado_user_ll_ll.Visibility = main_seja_ll.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            llTelefones.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
        }

        private void VisibilidadeTirarFoto(bool show)
        {
            cmdChamar192_enviarfoto.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
            cmdGPS.Enabled = cmdFavoritos.Enabled = !show;

            cmdMapa.Enabled = !show;
            if (show) StubAppCenter.AppAnalytic(Enums.AnalyticsType.Foto_ExibirBotao.Value, this.Tag);
        }

        private void VisibilidadeBotoesTelefone(bool show)
        {
            cmdChamar192_ligar.Visibility
                =
                cmdChat192_paramim.Visibility = cmdChat192_paraoutrapessoa.Visibility = show ? ViewStates.Visible : ViewStates.Gone;
         
        }
    }
}