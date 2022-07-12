using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Interface.Activities;

namespace SAMU192Droid.Interface.Fragments
{
    public class FavoritosFragment : BaseFragment
    {
        public static string TAG = "Favoritos";

        View view;
        ListView lvFavoritos;
        TextView favoitos_vazio_tv;
        Button fab;
        Action after_Select;
        internal Action After_Select { get => after_Select; set => after_Select = value; }

        public override void HideDescendantsForAccessibility()
        {
            favoitos_vazio_tv.ImportantForAccessibility =
            fab.ImportantForAccessibility =
            lvFavoritos.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
        }

        public override void ShowDescendantsForAccessibility()
        {
            favoitos_vazio_tv.ImportantForAccessibility =
            fab.ImportantForAccessibility =
            lvFavoritos.ImportantForAccessibility = ImportantForAccessibility.Yes;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            { 
                base.OnCreate(savedInstanceState);
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
                view = inflater.Inflate(Resource.Layout.favoritos, null);

                lvFavoritos = view.FindViewById<ListView>(Resource.Id.favoritos_lv);
                var enderecos = StubCadastro.RecuperaEnderecos();

                lvFavoritos.Adapter = new FavoritosAdapter((MainActivity)Activity, enderecos, After_Select);

                favoitos_vazio_tv = view.FindViewById<TextView>(Resource.Id.favoritos_vazia_tvw);
                favoitos_vazio_tv.Visibility = (enderecos.Count == 0 ? ViewStates.Visible : ViewStates.Gone);
                favoitos_vazio_tv.ImportantForAccessibility = (enderecos.Count == 0 ? ImportantForAccessibility.Yes : ImportantForAccessibility.NoHideDescendants);

                fab = view.FindViewById<Button>(Resource.Id.favoritos_adicionar_cmd);
                fab.Click += Fab_Click;

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            try
            {
                var mapaFragment = new MapaFragment();
                mapaFragment.EnderecoCriadoUsuario = null;
                mapaFragment.ModoSelecao = false;
                this.HideDescendantsForAccessibility();
                Utils.Interface.EmpilharFragment((MainActivity)Activity, Resource.Id.tabFrameLayout1, mapaFragment, Resource.String.mapa_title, MapaFragment.TAG);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}