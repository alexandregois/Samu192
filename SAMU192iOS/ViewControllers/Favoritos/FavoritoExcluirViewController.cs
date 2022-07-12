using SAMU192Core.DTO;
using SAMU192iOS.FacadeStub;
using System;
using System.Collections.Generic;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class FavoritoExcluirViewController : BaseViewController
    {
        EnderecoDTO enderecoRemover;
        List<EnderecoDTO> enderecos;
        public EnderecoDTO EnderecoRemover { get => enderecoRemover; set => enderecoRemover = value; }
        public List<EnderecoDTO> Enderecos { get => enderecos; set => enderecos = value; }

        public FavoritoExcluirViewController(IntPtr handle) : base(handle)
        {
            this.NavigationItem.Title = "Excluir";
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                btnExcluir.TouchUpInside += BtnExcluir_TouchUpInside;
                btnCancelar.TouchUpInside += BtnCancelar_TouchUpInside;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnCancelar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.VoltarViewController(false, true, this);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnExcluir_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Enderecos.Remove(EnderecoRemover);
                StubCadastro.SalvaEnderecos(Enderecos);

                Utils.Interface.VoltarViewController(false, true, this);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}