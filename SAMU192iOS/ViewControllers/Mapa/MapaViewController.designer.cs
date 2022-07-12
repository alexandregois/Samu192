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
    [Register ("MapaViewController")]
    partial class MapaViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAdicionar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnCurrentLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSelecionar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgComplemento { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblComplemento { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView oMapa { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView progressbarGeoCode { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView scrollviewone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchBar srcEndereco { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell tvcComplemento { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField txtComplemento { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAdicionar != null) {
                btnAdicionar.Dispose ();
                btnAdicionar = null;
            }

            if (btnCurrentLocation != null) {
                btnCurrentLocation.Dispose ();
                btnCurrentLocation = null;
            }

            if (btnSelecionar != null) {
                btnSelecionar.Dispose ();
                btnSelecionar = null;
            }

            if (imgComplemento != null) {
                imgComplemento.Dispose ();
                imgComplemento = null;
            }

            if (lblComplemento != null) {
                lblComplemento.Dispose ();
                lblComplemento = null;
            }

            if (oMapa != null) {
                oMapa.Dispose ();
                oMapa = null;
            }

            if (progressbarGeoCode != null) {
                progressbarGeoCode.Dispose ();
                progressbarGeoCode = null;
            }

            if (scrollviewone != null) {
                scrollviewone.Dispose ();
                scrollviewone = null;
            }

            if (srcEndereco != null) {
                srcEndereco.Dispose ();
                srcEndereco = null;
            }

            if (tvcComplemento != null) {
                tvcComplemento.Dispose ();
                tvcComplemento = null;
            }

            if (txtComplemento != null) {
                txtComplemento.Dispose ();
                txtComplemento = null;
            }
        }
    }
}