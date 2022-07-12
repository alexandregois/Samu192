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
    [Register ("FavoritosViewController")]
    partial class FavoritosViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnConfirmar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDescr { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollviewone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtNomeLocal { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtReferencia { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnConfirmar != null) {
                btnConfirmar.Dispose ();
                btnConfirmar = null;
            }

            if (lblDescr != null) {
                lblDescr.Dispose ();
                lblDescr = null;
            }

            if (scrollviewone != null) {
                scrollviewone.Dispose ();
                scrollviewone = null;
            }

            if (txtNomeLocal != null) {
                txtNomeLocal.Dispose ();
                txtNomeLocal = null;
            }

            if (txtReferencia != null) {
                txtReferencia.Dispose ();
                txtReferencia = null;
            }
        }
    }
}