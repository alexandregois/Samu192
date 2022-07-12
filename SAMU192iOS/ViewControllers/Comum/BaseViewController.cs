using System;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class BaseViewController : UIViewController
    {
        public BaseViewController(IntPtr handle) : base(handle)
        { }

        public BaseViewController() : base(null, null)
        { }

        public override void DidReceiveMemoryWarning()
        {
            try
            {
                base.DidReceiveMemoryWarning();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        //public override bool ShouldAutorotate()
        //{
        //    return false;
        //}

        //public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation interfaceOrientation)
        //{
        //    return interfaceOrientation == UIInterfaceOrientation.Portrait || interfaceOrientation == UIInterfaceOrientation.PortraitUpsideDown;
        //}

        //public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        //{
        //    return UIInterfaceOrientationMask.Portrait;
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    UIApplication.SharedApplication.SetStatusBarOrientation(UIInterfaceOrientation.Portrait, true);
        //    base.ViewWillAppear(animated);
        //}

        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            try
            {
                base.ViewWillDisappear(animated);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}