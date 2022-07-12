using System;
using UIKit;
using SAMU192Core.Facades;
using SAMU192iOS.Implementaions;

namespace SAMU192iOS.FacadeStub
{
    public static class StubMidia
    {
        static CameraImpl camera;
        static Action OnError;
        static Action<byte[], object> cameraCallback;

        public static void Carrega(bool isVideo, Action<byte[], object> _cameraCallback)
        {
            camera = new CameraImpl();
            cameraCallback = _cameraCallback;
            FacadeMidia.Carrega(camera, cameraCallback);
        }

        public static bool AbrirCamera(bool isVideo, UIView view, UINavigationController navigationController, UIImageView imageView, Action onError, Action AfterGrant)
        {
            OnError = onError;
            return FacadeMidia.AbrirCamera(isVideo, view, cameraCallback, AfterGrant, navigationController, imageView, 1);
        }

        public static byte[] ExtraiFotoFromImageView(UIImage image)
        {
            byte[] foto = null;

            using (Foundation.NSData imageData = image.AsJPEG((System.nfloat)0))
            {
                foto = new System.Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, foto, 0, System.Convert.ToInt32(imageData.Length));
            }
            return foto;
        }

        public static bool AbrirGaleria(UINavigationController navigationController, UIImageView imageView, Action onError, Action AfterGrant)
        {
            OnError = onError;
            return FacadeMidia.AbrirCamera(false, null, cameraCallback, AfterGrant, navigationController, imageView, 0);
        }

        internal static void FecharCamera()
        {
            FacadeMidia.FecharCamera();
        }

        internal static bool PodeTirarFotos(Action afterGrant)
        {
            return camera.VerificaPermissao(afterGrant);
        }
    }
}