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
    [Register ("CadastroPessoaViewController")]
    partial class CadastroPessoaViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSalvar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker dtpDtNasc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl00 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl01 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl02 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblDtNasc { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollviewone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl segGenero { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtNome { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnSalvar != null) {
                btnSalvar.Dispose ();
                btnSalvar = null;
            }

            if (dtpDtNasc != null) {
                dtpDtNasc.Dispose ();
                dtpDtNasc = null;
            }

            if (lbl00 != null) {
                lbl00.Dispose ();
                lbl00 = null;
            }

            if (lbl01 != null) {
                lbl01.Dispose ();
                lbl01 = null;
            }

            if (lbl02 != null) {
                lbl02.Dispose ();
                lbl02 = null;
            }

            if (lblDtNasc != null) {
                lblDtNasc.Dispose ();
                lblDtNasc = null;
            }

            if (scrollviewone != null) {
                scrollviewone.Dispose ();
                scrollviewone = null;
            }

            if (segGenero != null) {
                segGenero.Dispose ();
                segGenero = null;
            }

            if (txtNome != null) {
                txtNome.Dispose ();
                txtNome = null;
            }
        }
    }
}