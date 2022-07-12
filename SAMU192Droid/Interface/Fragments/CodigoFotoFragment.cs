using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SAMU192Core.Exceptions;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Interface.Activities;

namespace SAMU192Droid.Interface.Fragments
{
    public class CodigoFotoFragment : BaseFragment
    {
        public static string TAG = "Código para Foto";

        View view;
        EditText cod01_et;
        EditText cod02_et;
        Button cmdAbrirFoto;
        Activity activityAux;

        public override void HideDescendantsForAccessibility()
        {
            cod01_et.ImportantForAccessibility =
            cod02_et.ImportantForAccessibility =
            cmdAbrirFoto.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            cod01_et.ImportantForAccessibility =
            cod02_et.ImportantForAccessibility =
            cmdAbrirFoto.ImportantForAccessibility = ImportantForAccessibility.Yes;
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                view = inflater.Inflate(Resource.Layout.foto_codigo, null);

                cmdAbrirFoto = view.FindViewById<Button>(Resource.Id.abrirfoto_cmd);
                cod01_et = view.FindViewById<EditText>(Resource.Id.cod01_et);
                cod02_et = view.FindViewById<EditText>(Resource.Id.cod02_et);

                string[] cod = StubUtilidades.Gerar(DateTime.Now).Split('-');
                cod01_et.Text = cod[0];
                cod02_et.Text = cod[1];

                cmdAbrirFoto.Click += CmdAbrirFoto_Click;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        private void CmdAbrirFoto_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = (cod01_et.Text + "-" + cod02_et.Text).ToUpper();
                StubUtilidades.ValidarCodigoPIN(DateTime.Now, codigo);
                Utils.Interface.RetirarFragment(activityAux);
                Utils.Interface.IniciaNovaActivity(activityAux, typeof(CameraActivity), 0, activityAux.ApplicationContext);
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
    }
}