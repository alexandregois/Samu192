using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Droid.Interface.Activities;
using SAMU192Droid.FacadeStub;
using Android.Runtime;
using Android.Views.InputMethods;

namespace SAMU192Droid.Interface.Fragments
{
    public class CadastroFragment : BaseFragment
    {
        public static string TAG = "Cadastro1";

        View view;
        CadastroDTO cadastro = null;
        LinearLayout cadastro_ll;
        List<TelefoneView> lEtCadastroCelular = new List<TelefoneView>();
        Button cmdCadastroAvancar;
        Activity activityAux;
        TextView tvTitulo;
        TextView tvDescricao;
        ImageView ivImagem;

        class TelefoneView
        {
            EditText dddEt;
            EditText numeroEt;

            public TelefoneView(EditText dddEt, EditText numeroEt)
            {
                this.dddEt = dddEt;
                this.numeroEt = numeroEt;
            }

            public void SetVisibility(ViewStates vs)
            {
                dddEt.Visibility = vs;
                numeroEt.Visibility = vs;
            }

            public void SetImportantForAccessibility(ImportantForAccessibility ifa)
            {
                dddEt.ImportantForAccessibility = ifa;
                numeroEt.ImportantForAccessibility = ifa;
            }

            public void SetText(string s1, string s2)
            {
                dddEt.Text = s1;
                numeroEt.Text = s2;
            }

            public string DDD
            {
                get
                {
                    return dddEt.Text;
                }
            }

            public string Numero
            {
                get
                {
                    return numeroEt.Text;
                }
            }

            public bool Valida()
            {
                return !string.IsNullOrEmpty(numeroEt.Text) && !string.IsNullOrEmpty(dddEt.Text);
            }

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
                view = inflater.Inflate(Resource.Layout.cadastro, null);

                LoadScreenControls();

                if (StubUtilidades.GetFromMenu())
                    StubUtilidades.SetLockBackButton(false);
                else
                    StubUtilidades.SetLockBackButton(true);
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
            cadastro_ll = view.FindViewById<LinearLayout>(Resource.Id.cadastro_ll);
            tvDescricao = view.FindViewById<TextView>(Resource.Id.tvDescricao);
            tvTitulo = view.FindViewById<TextView>(Resource.Id.tvTitulo);
            ivImagem = view.FindViewById<ImageView>(Resource.Id.ivImagem);

            //StubTelefonia.CreatePhoneField(Activity, Context.TelephonyService);
            TelephonyManager manager = (TelephonyManager)Activity.GetSystemService(Context.TelephonyService);
            int phoneCount = manager.PhoneCount;

            lEtCadastroCelular.Add(new TelefoneView(view.FindViewById<EditText>(Resource.Id.cadastro_ddd_1_et), view.FindViewById<EditText>(Resource.Id.cadastro_celular_1_et)));
            lEtCadastroCelular.Add(new TelefoneView(view.FindViewById<EditText>(Resource.Id.cadastro_ddd_2_et), view.FindViewById<EditText>(Resource.Id.cadastro_celular_2_et)));
            lEtCadastroCelular.Add(new TelefoneView(view.FindViewById<EditText>(Resource.Id.cadastro_ddd_3_et), view.FindViewById<EditText>(Resource.Id.cadastro_celular_3_et)));
            lEtCadastroCelular.Add(new TelefoneView(view.FindViewById<EditText>(Resource.Id.cadastro_ddd_4_et), view.FindViewById<EditText>(Resource.Id.cadastro_celular_4_et)));

            foreach (TelefoneView tv in lEtCadastroCelular)
                tv.SetVisibility(ViewStates.Gone);

            for (int i = 0; i < phoneCount; i++)
                lEtCadastroCelular[i].SetVisibility(ViewStates.Visible);

            cadastro = StubCadastro.RecuperaCadastro();
            if (cadastro != null)
                foreach (TelefoneDTO item in cadastro.Telefones)                
                    lEtCadastroCelular[item.SlotId].SetText(item.Ddd, item.Numero);                
        
            cmdCadastroAvancar = view.FindViewById<Button>(Resource.Id.cadastro_avancar_cmd);
            cmdCadastroAvancar.Click += cmdCadastroAvancar_Click;

            EditText final_et = null;
            if (phoneCount == 1)
                final_et = view.FindViewById<EditText>(Resource.Id.cadastro_celular_1_et);
            else
            if (phoneCount == 2)
                final_et = view.FindViewById<EditText>(Resource.Id.cadastro_celular_2_et);
            else
            if (phoneCount == 4)
                final_et = view.FindViewById<EditText>(Resource.Id.cadastro_celular_4_et);

            final_et.EditorAction += HandleEditorAction;
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

        private void cmdCadastroAvancar_Click(object sender, EventArgs e)
        {
            try
            {
                List<TelefoneDTO> telefones = new List<TelefoneDTO>();
                int count = 0;
                foreach (TelefoneView tv in lEtCadastroCelular)
                {
                    if (tv.Valida())
                        telefones.Add(new TelefoneDTO(tv.DDD, tv.Numero, count));
                    count++;
                }
                
                CadastroDTO cadastroSalva = new CadastroDTO(cadastro.Nome, cadastro.DtNasc, cadastro.Sexo, cadastro.Historico, telefones.ToArray());
                StubCadastro.ValidaTelefone(cadastroSalva);

                Cadastro2Fragment cadastroFragment = new Cadastro2Fragment();
                cadastroFragment.Cadastro = cadastroSalva;

                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)this.Activity, Resource.Id.tabFrameLayout1, cadastroFragment, Resource.String.cadastro_title, Cadastro2Fragment.TAG);
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

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            return false;
        }

        public void Dispose()
        { }
    }
}