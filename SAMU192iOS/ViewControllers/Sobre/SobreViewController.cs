using System;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class SobreViewController : BaseViewController
    {
        public SobreViewController(IntPtr handle) : base(handle)
        {
            this.NavigationItem.Title = "Sobre";
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                btnURL.TouchUpInside += BtnURL_TouchUpInside;
                btnregiaoURL.TouchUpInside += BtnregiaoURL_TouchUpInside;
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnURL_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                var url = new Foundation.NSUrl(@"http://www.true.com.br");
                if (UIApplication.SharedApplication.CanOpenUrl(url))
                    UIApplication.SharedApplication.OpenUrl(url);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnregiaoURL_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                var url = new Foundation.NSUrl(@"http://www.true.com.br/aplicativo-samu-192/");
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