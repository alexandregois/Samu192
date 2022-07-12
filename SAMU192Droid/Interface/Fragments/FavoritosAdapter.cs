using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using SAMU192Core.DTO;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Interface.Activities;

namespace SAMU192Droid.Interface.Fragments
{
    class FavoritosAdapter : BaseAdapter<EnderecoDTO>
    {
        List<EnderecoDTO> items;
        Activity context;
        EnderecoDTO endereco;
        Button ivFavoritosCellExcluir;
        TextView favoritos_cell_referencia_lbl_tv;
        Action after_Select;
        internal Action After_Select { get => after_Select; set => after_Select = value; }
        int oldPosition = -1, oldPositionRemover = -1;

        public FavoritosAdapter(Activity context, List<EnderecoDTO> items, Action After_Select) : base()
        {
            this.context = context;
            this.items = items;
            this.After_Select = After_Select;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override EnderecoDTO this[int position]
        {
            get { return items[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = null;
            endereco = null;
            try
            {
                view = convertView;
                if (view == null)
                    view = context.LayoutInflater.Inflate(Resource.Layout.favoritos_cell, null);

                endereco = items[position];
                view.FindViewById<TextView>(Resource.Id.favoritos_cell_nome_tv).Text = endereco.Nome;
                view.FindViewById<TextView>(Resource.Id.favoritos_cell_referencia_tv).Text = endereco.Referencia;
                favoritos_cell_referencia_lbl_tv = view.FindViewById<TextView>(Resource.Id.favoritos_cell_referencia_lbl_tv);
                view.FindViewById<TextView>(Resource.Id.favoritos_cell_endereco_tv).Text = endereco.ToString();

                favoritos_cell_referencia_lbl_tv.Visibility = string.IsNullOrEmpty(endereco.Referencia) ? ViewStates.Gone : ViewStates.Visible;
                favoritos_cell_referencia_lbl_tv.ImportantForAccessibility = string.IsNullOrEmpty(endereco.Referencia) ? ImportantForAccessibility.NoHideDescendants : ImportantForAccessibility.Yes;

                view.Clickable = true;
                view.Click += (sender, e) =>
                {
                    if (oldPosition != position)
                        ClickSelecionar(items[position]);
                    oldPosition = position;
                };

                ivFavoritosCellExcluir = view.FindViewById<Button>(Resource.Id.favoritos_cell_excluir_iv);
                ivFavoritosCellExcluir.ImportantForAccessibility = ImportantForAccessibility.Yes;
                ivFavoritosCellExcluir.Clickable = true;
                ivFavoritosCellExcluir.Click += (sender, e) =>
                {
                    if (oldPositionRemover != position || oldPositionRemover == -1)
                        ClickBtnRemover(items[position]);
                    oldPositionRemover = position;
                };
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
            return view;
        }

        private void ClickSelecionar(EnderecoDTO item)
        {
            try
            {
                StubLocalizacao.ValidarEndereco(item);
                if (After_Select != null)
                {
                    var mainActivity = ((MainActivity)this.context);
                    mainActivity.EnderecoCriadoUsuario = item;

                    Utils.Interface.RetirarFragment(mainActivity);
                    After_Select();
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private sealed class OnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
        {
            private readonly Action action;

            public OnDismissListener(Action action)
            {
                this.action = action;
            }

            public void OnDismiss(IDialogInterface dialog)
            {
                this.action();
            }
        }

        private void ClickBtnRemover(EnderecoDTO item)
        {
            try
            {
                //TODO: metodo generico UTIL para perguntas!!!
                AlertDialog.Builder alert = new AlertDialog.Builder(this.context);
                alert.SetTitle("Excluir");
                alert.SetMessage("Deseja excluir este endereço?");
                alert.SetOnDismissListener(new OnDismissListener(() =>
                {
                    oldPositionRemover = -1;
                    return;
                }));
                alert.SetPositiveButton("Sim", (senderAlert, args) =>
                {
                    items.Remove(item);
                    StubCadastro.SalvaEnderecos(items);
                    var fragFavorito = (FavoritosFragment)Utils.Interface.FragmentTopo(this.context);
                    Utils.Interface.RetirarFragment(this.context);
                    var novoFragFavorito = new FavoritosFragment();
                    novoFragFavorito.After_Select = fragFavorito.After_Select;
                    Utils.Interface.EmpilharFragment((Activities.MainActivity)this.context, Resource.Id.tabFrameLayout1, novoFragFavorito, Resource.String.favoritos_title, FavoritosFragment.TAG);
                });
                alert.SetNegativeButton("Não", (senderAlert, args) =>
                {
                    oldPositionRemover = -1;
                    return;
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }
    }
}