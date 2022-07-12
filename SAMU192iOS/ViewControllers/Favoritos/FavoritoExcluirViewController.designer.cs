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
    [Register ("FavoritoExcluirViewController")]
    partial class FavoritoExcluirViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCancelar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnExcluir { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblPergunta { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitulo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnCancelar != null) {
                btnCancelar.Dispose ();
                btnCancelar = null;
            }

            if (btnExcluir != null) {
                btnExcluir.Dispose ();
                btnExcluir = null;
            }

            if (lblPergunta != null) {
                lblPergunta.Dispose ();
                lblPergunta = null;
            }

            if (lblTitulo != null) {
                lblTitulo.Dispose ();
                lblTitulo = null;
            }
        }
    }
}