// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SAMU192iOS.ViewControllers
{
    [Register ("CapturarPhotoViewController")]
    partial class CapturarPhotoViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView aivStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnPhoto01 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgPhoto01 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblStatusEnvio { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (aivStatus != null) {
                aivStatus.Dispose ();
                aivStatus = null;
            }

            if (btnPhoto01 != null) {
                btnPhoto01.Dispose ();
                btnPhoto01 = null;
            }

            if (imgPhoto01 != null) {
                imgPhoto01.Dispose ();
                imgPhoto01 = null;
            }

            if (lblStatusEnvio != null) {
                lblStatusEnvio.Dispose ();
                lblStatusEnvio = null;
            }
        }
    }
}