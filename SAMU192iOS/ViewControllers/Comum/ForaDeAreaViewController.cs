using System;

namespace SAMU192iOS.ViewControllers.Comum
{
    public partial class ForaDeAreaViewController : BaseViewController
    {
        public ForaDeAreaViewController(IntPtr handle) : base(handle)
        {
            this.NavigationItem.Title = "Fora de Área";
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                btnEntendido.TouchUpInside += BtnEntendido_TouchUpInside;
            }
            catch(Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnEntendido_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.VoltarViewController(true, true, this);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}