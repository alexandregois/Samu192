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
    [Register ("ConfirmaPhotoViewController")]
    partial class ConfirmaPhotoViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnConfirma { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollviewone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCodigo1 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtCodigo2 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnConfirma != null) {
                btnConfirma.Dispose ();
                btnConfirma = null;
            }

            if (scrollviewone != null) {
                scrollviewone.Dispose ();
                scrollviewone = null;
            }

            if (txtCodigo1 != null) {
                txtCodigo1.Dispose ();
                txtCodigo1 = null;
            }

            if (txtCodigo2 != null) {
                txtCodigo2.Dispose ();
                txtCodigo2 = null;
            }
        }
    }
}