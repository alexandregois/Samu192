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
    [Register ("MenuViewController")]
    partial class MenuViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCadastro { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnChamarSAMU { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnFavoritos { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnQuandoAcionar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSobre { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgLogo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCadastro != null) {
                btnCadastro.Dispose ();
                btnCadastro = null;
            }

            if (btnChamarSAMU != null) {
                btnChamarSAMU.Dispose ();
                btnChamarSAMU = null;
            }

            if (btnFavoritos != null) {
                btnFavoritos.Dispose ();
                btnFavoritos = null;
            }

            if (btnQuandoAcionar != null) {
                btnQuandoAcionar.Dispose ();
                btnQuandoAcionar = null;
            }

            if (btnSobre != null) {
                btnSobre.Dispose ();
                btnSobre = null;
            }

            if (imgLogo != null) {
                imgLogo.Dispose ();
                imgLogo = null;
            }
        }
    }
}