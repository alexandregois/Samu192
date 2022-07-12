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
    [Register ("TermoViewController")]
    partial class TermoViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAceitar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblEstouCiente { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTitulo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollviewone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch swtEstouViente { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txtDescricao { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAceitar != null) {
                btnAceitar.Dispose ();
                btnAceitar = null;
            }

            if (lblEstouCiente != null) {
                lblEstouCiente.Dispose ();
                lblEstouCiente = null;
            }

            if (lblTitulo != null) {
                lblTitulo.Dispose ();
                lblTitulo = null;
            }

            if (scrollviewone != null) {
                scrollviewone.Dispose ();
                scrollviewone = null;
            }

            if (swtEstouViente != null) {
                swtEstouViente.Dispose ();
                swtEstouViente = null;
            }

            if (txtDescricao != null) {
                txtDescricao.Dispose ();
                txtDescricao = null;
            }
        }
    }
}