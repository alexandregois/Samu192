using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using SAMU192Droid.Interface.Activities;
using SAMU192Droid.FacadeStub;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Core.Utils;
using Android.Text;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Preferences;

namespace SAMU192Droid.Interface.Fragments
{
    public class Cadastro2Fragment : BaseFragment
    {
        public static string TAG = "Cadastro2";
        #region Campos de tela
        View view;
        EditText etCadastro2Nome;
        TextView cadastro2_datanasc_tv;
        Spinner spnCadastro2Sexo;
        Button cmdCadastro2Salvar;
        DatePicker datanasc_dtp;
        TextView tvDescricao;
        TextView tvTitulo;
        LinearLayout main_content;
        Switch deficiencia_switch;
        EditText cadastro2_cpf_et;

        #endregion

        Activity activityAux;

        CadastroDTO cadastro;
        internal CadastroDTO Cadastro
        {
            get => cadastro;
            set => cadastro = value;
        }

        public override void HideDescendantsForAccessibility()
        {
            view.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            view.ImportantForAccessibility = ImportantForAccessibility.Yes;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                activityAux = this.Activity;
                Utils.Interface.FechaTeclado(this.Activity);
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
                view = inflater.Inflate(Resource.Layout.cadastro2, null);

                LoadScreenControls();

                StubUtilidades.SetLockBackButton(false);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Handler h = new Handler();
            System.Action myAction = () =>
            {
                if (tvTitulo != null)
                {
                    tvTitulo.Focusable = true;
                    tvTitulo.RequestFocus();
                    tvTitulo.SendAccessibilityEvent(Android.Views.Accessibility.EventTypes.ViewAccessibilityFocused);
                }
            };

            h.PostDelayed(myAction, 1000);
        }

        private void LoadScreenControls()
        {

            main_content = view.FindViewById<LinearLayout>(Resource.Id.main_content);
            etCadastro2Nome = view.FindViewById<EditText>(Resource.Id.cadastro2_nome_et);
            cadastro2_datanasc_tv = view.FindViewById<TextView>(Resource.Id.cadastro2_datanasc_tv);
            spnCadastro2Sexo = view.FindViewById<Spinner>(Resource.Id.cadastro2_sexo_spn);
            cmdCadastro2Salvar = view.FindViewById<Button>(Resource.Id.cadastro2_salvar_cmd);
            datanasc_dtp = view.FindViewById<DatePicker>(Resource.Id.datanasc_dtp);
            cmdCadastro2Salvar.Click += cmdCadastro2Salvar_Click;
            tvTitulo = view.FindViewById<TextView>(Resource.Id.tvTitulo);
            tvDescricao = view.FindViewById<TextView>(Resource.Id.tvDescricao);

            deficiencia_switch = view.FindViewById<Switch>(Resource.Id.deficiencia_switch);

            var pref = Application.Context.GetSharedPreferences("PacoteDados", FileCreationMode.Private);
            string pneSelecionado = pref.GetString("pneSelecionado", "");

            if ("pneSelecionado" == "1")
                deficiencia_switch.Checked = true;
            else
                deficiencia_switch.Checked = false;


            cadastro2_cpf_et = view.FindViewById<EditText>(Resource.Id.cadastro2_cpf_et);
            cadastro2_cpf_et.TextChanged += Cadastro2_cpf_et_TextChanged;

            deficiencia_switch.CheckedChange += Deficiencia_switch_CheckedChange;

            spnCadastro2Sexo.Adapter = GeraAdapterSexo(true);
            spnCadastro2Sexo.ItemSelected += SpnCadastro2Sexo_ItemSelected;

            if (Cadastro != null)
            {
                etCadastro2Nome.Text = Cadastro.Nome;
                if (Cadastro.DtNasc.HasValue)
                    datanasc_dtp.DateTime = Cadastro.DtNasc.Value;
                if (Cadastro.Sexo.HasValue)
                    CarregaSpinnerSexo();
            }
            etCadastro2Nome.SetRawInputType(Android.Text.InputTypes.ClassText);
            etCadastro2Nome.SetImeActionLabel("Feito", Android.Views.InputMethods.ImeAction.Done);
            etCadastro2Nome.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
            etCadastro2Nome.EditorAction += HandleEditorAction;
        }

        private void Cadastro2_cpf_et_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {

            //var ev = e as TextChangedEventArgs;
            //if (ev. != ev.OldTextValue)
            //{
            //    var entry = (EditText)sender;
            //    string text = Regex.Replace(ev.Text, @"[^0-9]", "");

            //    text = text.PadRight(11);

            //    if (text.Length <= 11)
            //        text = text.Insert(3, ".").Insert(7, ".").Insert(11, "-").TrimEnd(new char[] { ' ', '.', '-' });
            //    else if (text.Length > 11)
            //    {
            //        text = text.PadRight(14);
            //        text = text.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-").TrimEnd(new char[] { ' ', '.', '-' });
            //        if (entry.Text != text)
            //            entry.Text = text;
            //    }

            //    if (entry.Text != text)
            //        entry.Text = text;
            //}

        }

        private void Deficiencia_switch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (deficiencia_switch.Checked == false)
                cadastro2_cpf_et.Visibility = ViewStates.Gone;
            else
            {

                ISharedPreferences pref = Application.Context.GetSharedPreferences("pne", FileCreationMode.Private);
                ISharedPreferencesEditor edit = pref.Edit();
                edit.PutString("pneSelecionado", "1");

                edit.Commit();
                edit.Apply();

                cadastro2_cpf_et.Visibility = ViewStates.Visible;
                Intent intent = new Intent(this.Activity, typeof(InformaActivity));
                StartActivity(intent);
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

        private ArrayAdapter<string> GeraAdapterSexo(bool ini)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleSpinnerItem, new string[] { "", "Feminino", "Masculino" });
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            return adapter;
        }

        private void SpnCadastro2Sexo_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            try
            {
                Spinner s = (Spinner)sender;
                TextView t = (TextView)s.GetChildAt(0);
                if (s.SelectedItemPosition == 0)
                {
                    t.Text = "Selecione";
                    t.SetTextColor(Utils.Interface.Cores.GetByID(Resource.Color.cinza_escuro, Activity.ApplicationContext));
                }
                else
                {
                    t.SetTextColor(Utils.Interface.Cores.GetByID(Resource.Color.preto, Activity.ApplicationContext));
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void cmdCadastro2Salvar_Click(object sender, EventArgs e)
        {
            try
            {
                char? sexo = ResolveSexo();
                DateTime? dtNasc = dtNasc = datanasc_dtp.DateTime;

                CadastroDTO temp = new CadastroDTO(etCadastro2Nome.Text, dtNasc, sexo, string.Empty, Cadastro.Telefones);
                StubCadastro.ValidaCadastro(temp);
                if (!StubCadastro.ExisteCadastro())
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.Cadastro_Efetuado.Value, this.Tag);
                StubCadastro.SalvaCadastro(temp);

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.Context);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutInt("chat", 0);


                //Grava o CPF internamento no Device
                editor.PutString("cpf", cadastro2_cpf_et.Text);
                editor.Apply();
                editor.Commit();


                //Abrir Favorito??
                if (!StubCadastro.ExisteEnderecos())
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(Activity);
                    alert.SetTitle("Abrir Favorito?");
                    alert.SetMessage(Activity.GetString(Resource.String.abrir_favorito));
                    alert.SetNegativeButton("Mais tarde", (senderAlert, args) =>
                    {
                        if (deficiencia_switch.Checked == false)
                            FluxoSaidaQuestao(activityAux, new InicialFragment(), Resource.String.chamar_title, InicialFragment.TAG);
                        else
                        {
                            FluxoSaidaQuestao(activityAux, new Inicial2Fragment(), Resource.String.chamar_title, Inicial2Fragment.TAG);

                            editor.PutInt("chat", 1);
                            editor.Apply();
                            editor.Commit();

                        }

                        StubWalkThrough.SetStartInWalkThrough(false);
                        StubWalkThrough.EncerraFluxoWalkthrough((Activities.MainActivity)activityAux);
                        activityAux.Recreate();
                    });
                    alert.SetPositiveButton("Agora", (senderAlert, args) =>
                    {
                        FluxoSaidaQuestao(this.Activity, new FavoritosFragment(), Resource.String.favoritos_title, FavoritosFragment.TAG);
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    if (deficiencia_switch.Checked == false)
                        FluxoSaidaQuestao(this.Activity, new InicialFragment(), Resource.String.chamar_title, InicialFragment.TAG);
                    else
                    {
                        FluxoSaidaQuestao(this.Activity, new Inicial2Fragment(), Resource.String.chamar_title, Inicial2Fragment.TAG);

                        editor.PutInt("chat", 1);
                        editor.Apply();
                        editor.Commit();

                    }
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

        private void FluxoSaidaQuestao(Activity act, Fragment frag, int resourceId, string tag)
        {
            if (StubUtilidades.GetFromMenu())
            {
                Utils.Interface.RetirarFragment(act);
                Utils.Interface.RetirarFragment(act);
                this.HideDescendantsForAccessibility();
            }
            else
            {
                Utils.Interface.LimparPilhaFragments3(act);
                StubUtilidades.SetLockBackButton(false);
            }

            Utils.Interface.EmpilharFragment((MainActivity)act, Resource.Id.tabFrameLayout1, frag, resourceId, tag);
        }

        private char? ResolveSexo()
        {
            switch (spnCadastro2Sexo.SelectedItemPosition)
            {
                case 1:
                    return 'F';
                case 2:
                    return 'M';
                default:
                    return null;
            }
        }

        private void CarregaSpinnerSexo()
        {
            switch (Cadastro.Sexo.Value.ToString().ToUpper())
            {
                case "F":
                    spnCadastro2Sexo.SetSelection(1);
                    break;
                case "M":
                    spnCadastro2Sexo.SetSelection(2);
                    break;
                default:
                    spnCadastro2Sexo.SetSelection(0);
                    break;
            }
        }
    }
}
