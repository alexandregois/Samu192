using System;
using SAMU192Core.Interfaces;

namespace SAMU192Core.Facades
{
    public static class FacadeMidia
    {
        static ICamera camera;
        static Action<byte[], object> cameraCallBack;

        public static void Carrega(ICamera _camera, Action<byte[], object> _cameraCallBack)
        {
            camera = _camera;
            cameraCallBack = _cameraCallBack;
        }

        public static bool AbrirCamera(bool isVideo, object tela, object callback, Action AfterGrant, object arg1, object arg2, int tipoCaptura)
        {
            return camera.AbrirCamera(isVideo, tela, callback, arg1, AfterGrant, arg2, tipoCaptura);
        }

        public static void CameraCallBack(object data, object _thumbnail)
        {
            camera.CameraCallBack(data, _thumbnail);
        }

        public static void StartStopVideo()
        {
            camera.StartStopVideo(cameraCallBack);
        }

        public static void FecharCamera()
        {
            camera?.FercharCamera();
        }

        public static bool AbrirGaleria(object args, object callback)
        {
            return camera.AbrirGaleria(args, callback);
        }
    }
}