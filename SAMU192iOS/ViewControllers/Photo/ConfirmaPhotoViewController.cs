using SAMU192Core.Exceptions;
using SAMU192iOS.FacadeStub;
using System;

namespace SAMU192iOS.ViewControllers
{
    public partial class ConfirmaPhotoViewController : BaseViewController
    {
        public ConfirmaPhotoViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

#if debug
                string[] cod = StubUtilidades.Gerar(DateTime.Now).Split('-');
#endif

                Utils.Interface.AlteraEstadoButton(btnConfirma, false);
                txtCodigo1.EditingChanged += TxtCodigo_EditingChanged;
                txtCodigo2.EditingChanged += TxtCodigo_EditingChanged;
                Utils.Interface.ConfiguraTextField(txtCodigo1, scrollviewone, 200, 20);
                Utils.Interface.ConfiguraTextField(txtCodigo2, scrollviewone, 200, 20);
                btnConfirma.TouchUpInside += BtnConfirma_TouchUpInside;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnConfirma_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo1.Text + "-" + txtCodigo2.Text;
                string cod = StubUtilidades.Gerar(DateTime.Now);
                codigo = cod;
                StubUtilidades.ValidarCodigoPIN(DateTime.Now, codigo);
                Utils.Interface.VoltarViewController(false, false, this);
                Utils.Interface.EmpurrarViewController(this, "CapturarPhotoViewController");
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

        private void TxtCodigo_EditingChanged(object sender, EventArgs e)
        {
            try
            {
                bool informouAlgumCodigo = (!string.IsNullOrEmpty(txtCodigo1.Text)) && (!string.IsNullOrEmpty(txtCodigo2.Text));
                Utils.Interface.AlteraEstadoButton(btnConfirma, informouAlgumCodigo);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}