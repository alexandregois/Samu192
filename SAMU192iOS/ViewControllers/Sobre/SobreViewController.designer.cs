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
    [Register ("SobreViewController")]
    partial class SobreViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnregiaoURL { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnURL { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollviewone { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnregiaoURL != null) {
                btnregiaoURL.Dispose ();
                btnregiaoURL = null;
            }

            if (btnURL != null) {
                btnURL.Dispose ();
                btnURL = null;
            }

            if (scrollviewone != null) {
                scrollviewone.Dispose ();
                scrollviewone = null;
            }
        }
    }
}