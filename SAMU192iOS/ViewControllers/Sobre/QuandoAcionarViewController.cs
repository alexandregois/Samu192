using System;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class QuandoAcionarViewController : BaseViewController
    {
        public QuandoAcionarViewController(IntPtr handle) : base(handle)
        {
            this.NavigationItem.Title = "Quando Acionar";
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                btnfonteURL.TouchUpInside += BtnfonteURl_TouchUpInside;

            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnfonteURl_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                var url = new Foundation.NSUrl(@"http://bvsms.saude.gov.br/bvs/saudelegis/gm/2002/prt2048_05_11_2002.html");
                if (UIApplication.SharedApplication.CanOpenUrl(url))
                    UIApplication.SharedApplication.OpenUrl(url);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}