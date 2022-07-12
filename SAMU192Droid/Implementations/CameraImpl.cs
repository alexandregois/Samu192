using System;
using System.IO;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Android.Media;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;
using Android.Support.V4.Content;
using SAMU192Core.Exceptions;
using SAMU192Droid.Interface;
using static SAMU192Core.Utils.Constantes;
using Uri = Android.Net.Uri;
using Android.Content.PM;

namespace SAMU192Droid.Implementations
{
    public static class Diretorio
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;
        public static string file;
        public static String mCurrentPhotoPath;
    }

    internal class CameraImpl : ICamera
    {
        Action<byte[], Bitmap> callBack;
        Activity context;

        bool isVideo;
        bool isGaleria;
        static String mCurrentPhotoPath;
        static String mCurrentVideoPath;

        public CameraImpl(Activity activity)
        {
            context = activity;
        }

        public bool AbrirCamera(bool ehVideo, object tela, object _callBack, object arg1, Action AfterCGrant = null, object arg2 = null, int tipoCaptura = 1, int tempoLimite = 30)
        {
            tempoLimite = 10;
            var activity = (Activity)arg1;
            callBack = (Action<byte[], Bitmap>)_callBack;
            isVideo = ehVideo;
            Intent intent;

            if (!VerificaPermissao(activity))
                return false;

            if (isVideo)
            {
                intent = new Intent(MediaStore.ActionVideoCapture);
                Java.IO.File videoFile = null;
                isGaleria = false;
                try
                {
                    if (VerificaPermissaoStorage(activity))
                        videoFile = CreateImageFile("SAMUVideo", ".mp4");
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    throw new ValidationException("Falha ao abrir a câmera de video");
                }

                var midiaURI = FileProvider.GetUriForFile(this.context, activity.ApplicationContext.PackageName + ".provider", videoFile);
                intent.PutExtra(MediaStore.ExtraOutput, midiaURI);
                intent.PutExtra(MediaStore.ExtraDurationLimit, tempoLimite);
                intent.PutExtra(MediaStore.ExtraVideoQuality, 0);
                intent.PutExtra(MediaStore.ExtraSizeLimit, (long)(DROID_MAX_VIDEO_SIZE_KB * 1024));
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                intent.AddFlags(ActivityFlags.GrantWriteUriPermission);
                activity.StartActivityForResult(intent, (int)Enums.ActivityResult.Video);
            }
            else
            {
                intent = new Intent(MediaStore.ActionImageCapture);
                Java.IO.File photoFile = null;
                isGaleria = false;
                try
                {
                    if (VerificaPermissaoStorage(activity))
                        photoFile = CreateImageFile("SAMUFoto", ".jpg");
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    throw new ValidationException("Falha ao abrir a câmera");
                }
                Uri photoUri = FileProvider.GetUriForFile(this.context, activity.ApplicationContext.PackageName + ".provider", photoFile);
                intent.PutExtra(MediaStore.ExtraOutput, photoUri);
                activity.StartActivityForResult(intent, (int)Enums.ActivityResult.Foto);

            }
            return true;
        }

        private bool VerificaPermissao(Activity activity)
        {
            string[] Permissions = { Manifest.Permission.Camera };

            bool permissionCheck;

            if (Utils.VersaoAndroid.QualquerLollipop)
                permissionCheck = ContextCompat.CheckSelfPermission(activity.ApplicationContext, Manifest.Permission.Camera) == Permission.Granted;
            else
                permissionCheck = activity.CheckSelfPermission(Manifest.Permission.Camera) == Permission.Granted;

            if (!permissionCheck)
                activity.RequestPermissions(Permissions, (int)Enums.RequestPermissionCode.Camera);

            return permissionCheck;
        }

        private bool VerificaPermissaoStorage(Activity activity)
        {
            var permissionCheck = ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage);
            if (permissionCheck == Permission.Granted)
            {
                return true;
            }
            else
            {
                string[] PermissionsLocation = { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                Android.Support.V4.App.ActivityCompat.RequestPermissions(activity, PermissionsLocation, (int)Enums.RequestPermissionCode.Storage);
                return false;
            }
        }

        private Java.IO.File CreateImageFile(string imageFileName, string extensao)
        {
            Java.IO.File storageDir = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File image = Java.IO.File.CreateTempFile(imageFileName, extensao, storageDir);
            mCurrentPhotoPath = image.AbsolutePath;
            return image;
        }

        public bool AbrirGaleria(object args, object _callBack)
        {
            try
            {
                callBack = (Action<byte[], Bitmap>)_callBack;
                isVideo = false;
                var activity = (Activity)args;
                Intent intent = new Intent();
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);
                activity.StartActivityForResult(Intent.CreateChooser(intent, "Selecione uma foto"), (int)Enums.ActivityResult.Galeria);
                isGaleria = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new ValidationException("Falha ao abrir a galeria");
            }
        }

        public byte[] ConverteParaArray(Bitmap bitmap)
        {
            using (var stream22 = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 30, stream22);
                return stream22.ToArray();
            }
        }

        public void CameraCallBack(object data, object arg2)
        {
            if (isVideo)
            {
                try
                {
                    Bitmap thumbnail = ThumbnailUtils.CreateVideoThumbnail(mCurrentVideoPath, ThumbnailKind.MiniKind);
                    Java.IO.File file = new Java.IO.File(mCurrentVideoPath);
                    byte[] video = new byte[(int)file.Length()];
                    Java.IO.FileInputStream fileInputStream = new Java.IO.FileInputStream(file);
                    fileInputStream.Read(video);
                    if (video.Length > 0)
                        callBack(video, thumbnail);
                }
                catch (Exception ex)
                {
                    throw new ValidationException("Não foi possível carregar a vídeo.");
                }
            }
            else
            {
                Bitmap bitmap = null;
                if (isGaleria)
                {
                    try
                    {
                        Android.Net.Uri uri = (Android.Net.Uri)data;
                        bitmap = MediaStore.Images.Media.GetBitmap(context.ContentResolver, uri);
                        bitmap = Utils.Ferramentas.ResizeBitmap(bitmap, DROID_IMG_MAX_DIMENSION);
                    }
                    catch (Exception ex)
                    {
                        throw new ValidationException("Não foi possível carregar a imagem.");
                    }
                }
                else
                {
                    try
                    {
                        bitmap = BitmapFactory.DecodeFile(mCurrentPhotoPath);
                        if (bitmap != null)
                            bitmap = Utils.Ferramentas.ResizeBitmap(bitmap, DROID_IMG_MAX_DIMENSION);
                    }
                    catch (InvalidCastException)
                    {
                        throw new ValidationException("Não foi possível carregar a imagem.");
                    }
                }

                if (bitmap == null)
                    throw new ValidationException("Não foi possível carregar a imagem.");

                callBack(ConverteParaArray(bitmap), bitmap);
            }
        }

        public Bitmap LoadAndResizeBitmap(string fileName, int width, int height)
        {
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                    ? outHeight / height
                                    : outWidth / width;
            }

            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }

        public void StartStopVideo(object _camera_Video_Callback)
        {
            throw new NotImplementedException();
        }

        public void FercharCamera()
        {
            throw new NotImplementedException();
        }
    }
}