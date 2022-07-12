using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SAMU192Core.DTO;
using SAMU192Core.Exceptions;
using SAMU192Core.Utils;
using SAMU192Droid.FacadeStub;

namespace SAMU192Droid.Interface.Fragments
{
    public class FavoritosNomeFragment : BaseFragment
    {
        public static string TAG = "Nome do Favorito";
        View view;
        EditText nomefavorito_et;
        EditText referencia_et;
        Button cmdGrava;
        Activity activityAux;
        bool fromWalkthrough;
        internal bool FromWalkthrough { get => fromWalkthrough; set => fromWalkthrough = value; }
        EnderecoDTO endereco;
        internal EnderecoDTO Endereco { get => endereco; set => endereco = value; }

        public override void HideDescendantsForAccessibility()
        {
            nomefavorito_et.ImportantForAccessibility =
            referencia_et.ImportantForAccessibility =
            cmdGrava.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            nomefavorito_et.ImportantForAccessibility =
            referencia_et.ImportantForAccessibility =
            cmdGrava.ImportantForAccessibility = ImportantForAccessibility.Yes;
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
                view = inflater.Inflate(Resource.Layout.favoritos_nome, null);

                cmdGrava = view.FindViewById<Button>(Resource.Id.salvar_cmd);
                nomefavorito_et = view.FindViewById<EditText>(Resource.Id.nomefavorito_et);
                referencia_et = view.FindViewById<EditText>(Resource.Id.referencia_et);

                referencia_et.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
                referencia_et.EditorAction += HandleEditorAction;

                cmdGrava.Click += CmdGrava_Click;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
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

        private void CmdGrava_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = nomefavorito_et.Text;
                string referencia = referencia_et.Text;

                StubCadastro.ValidaFavorito(nome, referencia);

                Endereco.Nome = nomefavorito_et.Text;
                Endereco.Referencia = referencia_et.Text;

                var enderecos = StubCadastro.RecuperaEnderecos();

                if (enderecos.Count == 0)
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.Favorito_Cadastrado.Value, this.Tag);

                enderecos.Add(Endereco);

                StubCadastro.SalvaEnderecos(enderecos);
                Utils.Interface.RetirarFragment(activityAux);
                Utils.Interface.RetirarFragment(activityAux);

                Utils.Interface.FechaTeclado(activityAux);

                if (StubWalkThrough.GetStartInWalkThrough())
                {
                    Utils.Interface.RetirarFragment(activityAux);
                    StubWalkThrough.SetStartInWalkThrough(false);
                    StubWalkThrough.EncerraFluxoWalkthrough((Activities.MainActivity)activityAux);
                    activityAux.Recreate();
                }
                else
                {
                    var favoritosFrag = (FavoritosFragment)Utils.Interface.FragmentTopo(activityAux);
                    Utils.Interface.RetirarFragment(activityAux);
                    var novoFavortiosfrag = new FavoritosFragment();
                    novoFavortiosfrag.After_Select = favoritosFrag.After_Select;
                    this.HideDescendantsForAccessibility();
                    Utils.Interface.EmpilharFragment((Activities.MainActivity)activityAux, Resource.Id.tabFrameLayout1, novoFavortiosfrag, Resource.String.favoritos_title, FavoritosFragment.TAG);
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
    }
}