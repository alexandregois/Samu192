using System;
using Android.App;
using SAMU192Droid.Implementations;
using SAMU192Core.Facades;
using Android.Widget;
using Android.Content.PM;
using Android.Content;
using System.Collections.Generic;
using Android.Provider;
using Android.Graphics.Drawables;
using System.IO;
using Android.Graphics;

namespace SAMU192Droid.FacadeStub
{
    public static class StubMidia
    {
        static CameraImpl camera;
        static Action<byte[], object> imagemCallback;
        static Action OnError;

        public static void Carrega(Activity activity, Action<byte[], object> _imagemCallback)
        {
            camera = new CameraImpl(activity);
            imagemCallback = _imagemCallback;
            FacadeMidia.Carrega(camera, _imagemCallback);
        }

        public static bool AbrirCamera(bool isVideo, Activity activity, Action onError)
        {
            OnError = onError;
            return FacadeMidia.AbrirCamera(isVideo, new object(), imagemCallback, null, activity, null, 1);
        }

        public static bool AbrirGaleria(Activity activity)
        {
            return FacadeMidia.AbrirGaleria(activity, imagemCallback);
        }

        public static void CameraCallBack(object data)
        {
            FacadeMidia.CameraCallBack(data, null);
        }

        public static bool PodeTirarFotos(PackageManager packageManager)
        {
            return IsThereAnAppToTakePictures(packageManager);
        }

        private static bool IsThereAnAppToTakePictures(PackageManager packageManager)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = packageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        public static byte[] ExtraiFotoFromImageView(ImageView image)
        {
            byte[] foto = null;
            BitmapDrawable bitmapDrawable = ((BitmapDrawable)image.Drawable);
            Bitmap bitmap = bitmapDrawable.Bitmap;
            using (var stream22 = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream22);
                foto = stream22.ToArray();
            }
            return foto;
        }
    }
}