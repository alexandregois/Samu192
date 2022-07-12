using System;
using SAMU192Core.DTO;
using SAMU192Core.Utils;
using SAMU192iOS.FacadeStub;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class FavoritosViewController : BaseViewController
    {
        EnderecoDTO endereco;
        public EnderecoDTO Endereco { get => endereco; set => endereco = value; }
        bool fromWalkThrough = false;
        public bool FromWalkThrough { get => fromWalkThrough; set => fromWalkThrough = value; }

        bool EstadoBotao { get { return (Endereco != null) && (!string.IsNullOrEmpty(txtNomeLocal.Text) && txtNomeLocal.Text.Length > 0); } }

        public FavoritosViewController(IntPtr handle) : base(handle)
        {
            this.NavigationItem.Title = "Nome do Favorito";
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                Utils.Interface.ConfiguraTextField(txtNomeLocal, scrollviewone, 50, 20);
                Utils.Interface.ConfiguraTextField(txtReferencia, scrollviewone, 200, 20);
                if (btnConfirmar != null)
                    btnConfirmar.TouchUpInside += BtnConfirmar_TouchUpInside;
                txtNomeLocal.EditingChanged += TxtNomeLocal_EditingChanged;
                Utils.Interface.AlteraEstadoButton(btnConfirmar, EstadoBotao);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void TxtNomeLocal_EditingChanged(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.AlteraEstadoButton(btnConfirmar, EstadoBotao);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnConfirmar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Endereco = new EnderecoDTO(
                    Endereco.Logradouro,
                    Endereco.Numero,
                    Endereco.Complemento,
                    Endereco.Bairro,
                    Endereco.Cidade,
                    Endereco.Estado,
                    Endereco.Coordenada,
                    txtNomeLocal.Text,
                    txtReferencia.Text);

                if (!StubCadastro.ExisteEnderecos())
                    StubAppCenter.AppAnalytic(Enums.AnalyticsType.Favorito_Cadastrado.Value);

                StubCadastro.AdicionarEndereco(Endereco);

                if (FromWalkThrough)
                {
                    Utils.Interface.VoltarViewController(true, true, this);
                }
                else
                {
                    Utils.Interface.VoltarViewController(false, true, this);
                    Utils.Interface.VoltarViewController(false, true, this);
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}