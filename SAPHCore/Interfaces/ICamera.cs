using System;

namespace SAMU192Core.Interfaces
{
    public interface ICamera
    {
        bool AbrirCamera(bool isVideo, object tela, object callback, object arg1, Action AfterGrant = null, object arg2 = null, int tipoCaptura = 1, int tempoLimite = 30);

        void CameraCallBack(object arg1, object arg2);

        void StartStopVideo(object _camera_Video_Callback);

        void FercharCamera();

        bool AbrirGaleria(object args, object callback);
    }
}