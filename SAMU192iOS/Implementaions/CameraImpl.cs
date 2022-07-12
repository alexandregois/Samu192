using System;
using AVFoundation;
using CoreGraphics;
using UIKit;
using Foundation;
using CoreMedia;
using SAMU192iOS.ViewControllers;
using SAMU192Core.Interfaces;
using SAMU192Core.Exceptions;

namespace SAMU192iOS.Implementaions
{
    public class CameraImpl : ICamera
    {
        static UIImagePickerController picker;
        static UIImageView staticImageView;
        Action<byte[], UIImage> callBack;
        bool weAreRecording;
        AVCaptureMovieFileOutput output;
        AVCaptureDevice device;
        AVCaptureDevice audioDevice;

        AVCaptureDeviceInput input;
        AVCaptureDeviceInput audioInput;
        AVCaptureSession session;

        AVCaptureVideoPreviewLayer previewlayer;
        VideoDelegate avDel;
        UIButton buttonToggle;
        UILabel lblTime;
        UILabel lblTimeLimite;
        UIView cameraView;
        System.Timers.Timer timer;
        const double TIMER_INTERVAL = 500;
        double elapsed = 0;
        int limitSeconds = 0;
        UIView view;

        public bool AbrirCamera(bool isVideo, object tela, object _callBack, object arg1, Action AfterGrant, object arg2, int tipoCaptura, int tempoLimite = 30)
        {
            AVAuthorizationStatus authStatusVideo = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authStatusVideo != AVAuthorizationStatus.Authorized)
            {
                // throw new ValidationException("Permissão para uso da Câmera não foi concedida.");
                AVCaptureDevice.RequestAccessForMediaType(AVAuthorizationMediaType.Video, new AVRequestAccessStatus(
                    (granted) =>
                    {
                        if (!granted)
                        {
                            new NSObject().InvokeOnMainThread(() =>
                            {
                                Utils.Mensagem.Alerta(
                                   "O Aplicativo CHAMAR 192 necessita permissão para acessar a câmera. Verifique.",
                                   (o) =>
                                   {
                                       UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
                                   });
                            });
                            return;
                        }
                        AfterGrant();
                    }));

                return false;
            }
            else if (isVideo)
            {
                limitSeconds = tempoLimite;


                AVAuthorizationStatus authStatusAudio = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Audio);
                if (authStatusAudio != AVAuthorizationStatus.Authorized)
                {

                    new NSObject().InvokeOnMainThread(() =>
                    {
                        Utils.Mensagem.Alerta(
                       "O Aplicativo CHAMAR 192 necessita permissão para acessar o microfone. Verifique.",
                       (o) =>
                       {
                           UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
                       });
                    });
                    return false;
                }

                weAreRecording = false;
                callBack = (Action<byte[], UIImage>)_callBack;

                view = (UIView)tela;

                nfloat CenterX = view.Bounds.Width / 2;
                nfloat Y = view.Bounds.Height;

                buttonToggle = new UIButton();
                buttonToggle.Frame = new CGRect(CenterX - 40, Y - 140, 80, 40);
                buttonToggle.BackgroundColor = UIColor.Red;

                buttonToggle.SetTitle("GRAVAR", UIControlState.Normal);
                buttonToggle.BackgroundColor = UIColor.Red;
                buttonToggle.TouchUpInside += StartStopPushed;

                session = new AVCaptureSession();
                device = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
                input = AVCaptureDeviceInput.FromDevice(device);
                session.AddInput(input);
                audioDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Audio);
                audioInput = AVCaptureDeviceInput.FromDevice(audioDevice);
                session.AddInput(audioInput);

                previewlayer = new AVCaptureVideoPreviewLayer(session);
                previewlayer.Frame = view.Bounds;

                cameraView = new UIView();
                cameraView.Layer.AddSublayer(previewlayer);
                view.AddSubview(cameraView);
                view.BringSubviewToFront(cameraView);

                lblTime = new UILabel();
                lblTime.Frame = new CGRect(CenterX - 30, Y - 170, 60, 20);
                lblTime.Text = "0 seg(s)";
                lblTime.TextColor = UIColor.White;
                lblTime.AdjustsFontSizeToFitWidth = true;
                lblTime.UserInteractionEnabled = true;
                view.AddSubview(lblTime);
                view.BringSubviewToFront(lblTime);

                lblTimeLimite = new UILabel();
                lblTimeLimite.Frame = new CGRect(CenterX - 40, Y - 80, 80, 20);
                lblTimeLimite.Text = "Limite: " + limitSeconds.ToString() + " seg(s)";
                lblTimeLimite.TextColor = UIColor.White;
                lblTimeLimite.AdjustsFontSizeToFitWidth = true;
                lblTimeLimite.UserInteractionEnabled = true;
                view.AddSubview(lblTimeLimite);
                view.BringSubviewToFront(lblTimeLimite);

                view.AddSubview(buttonToggle);
                view.BringSubviewToFront(buttonToggle);

                output = new AVCaptureMovieFileOutput();

                long totalSeconds = 10000;
                Int32 preferredTimeScale = 30;
                CMTime maxDuration = new CMTime(totalSeconds, preferredTimeScale);
                output.MinFreeDiskSpaceLimit = 1024 * 1024;
                output.MaxRecordedDuration = maxDuration;

                if (session.CanAddOutput(output))
                {
                    session.AddOutput(output);
                }

                session.SessionPreset = AVCaptureSession.PresetMedium;
                session.StartRunning();

                return true;
            }
            else
            {

                callBack = (Action<byte[], UIImage>)_callBack;
                var navigationController = (UINavigationController)arg1;
                staticImageView = (UIImageView)arg2;
                if (UIImagePickerController.IsSourceTypeAvailable((UIImagePickerControllerSourceType)tipoCaptura))
                {
                    picker = new UIImagePickerController();
                    picker.Delegate = new CameraDelegate(this);
                    picker.SourceType = (UIImagePickerControllerSourceType)tipoCaptura;
                    navigationController.PresentViewController(picker, true, null);

                    return true;
                }
                else
                {
                    throw new ValidationException("Câmera não disponivel ou não habilitada.");
                }
            }
        }

        public bool VerificaPermissao(Action afterGranted)
        {
            AVAuthorizationStatus authStatusVideo = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            return (authStatusVideo == AVAuthorizationStatus.Authorized);
        }

        public bool AbrirGaleria(object args, object callback)
        {
            throw new NotImplementedException();
        }

        public void CameraCallBack(object arg1, object arg2)
        {
            var picker = (UIImagePickerController)arg1;
            var info = (NSDictionary)arg2;

            picker.DismissModalViewController(true);
            var image2 = info.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
            staticImageView.Image = image2;

            byte[] myByteArray;

            using (NSData imageData = image2.AsJPEG((System.nfloat)0))
            {
                myByteArray = new System.Byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, System.Convert.ToInt32(imageData.Length));
            }
            callBack(myByteArray, image2);
        }

        public void FercharCamera()
        {
            session?.StopRunning();
            if (this.buttonToggle != null)
            {
                buttonToggle.TouchUpInside -= StartStopPushed;

                foreach (var view in view.Subviews)
                {
                    view.RemoveFromSuperview();
                }
            }
        }

        public void StartStopVideo(object _camera_Video_Callback)
        {
            Action<byte[], object> camera_Video_Callback = (Action<byte[], object>)_camera_Video_Callback;
            if (!weAreRecording)
            {
                Start(camera_Video_Callback);
            }
            else
            {
                Stop();
            }
        }

        private void Start(Action<byte[], object> camera_Video_Callback)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var library = System.IO.Path.Combine(documents, "..", "Library");
            var urlpath = System.IO.Path.Combine(library, "SAMU192MovieTemp.mov");

            NSUrl url = new NSUrl(urlpath, false);

            NSFileManager manager = new NSFileManager();
            NSError error = new NSError();

            if (manager.FileExists(urlpath))
            {
                manager.Remove(urlpath, out error);
            }

            avDel = new VideoDelegate(camera_Video_Callback, buttonToggle, lblTime, lblTimeLimite, cameraView);
            output.StartRecordingToOutputFile(url, avDel);
            weAreRecording = true;

            buttonToggle.SetTitle("PARAR", UIControlState.Normal);
            buttonToggle.BackgroundColor = UIColor.Blue;
            timer = new System.Timers.Timer(TIMER_INTERVAL);
            timer.Elapsed += Timer_Elapsed;
            elapsed = 0;
            timer.Start();
        }

        private void Stop()
        {
            timer.Stop();
            timer.Dispose();
            timer = null;
            output.StopRecording();
            weAreRecording = false;
            buttonToggle.SetTitle("GRAVAR", UIControlState.Normal);
            buttonToggle.BackgroundColor = UIColor.Red;
        }

        private void StartStopPushed(object sender, EventArgs ea)
        {
            try
            {
                StartStopVideo(callBack);
            }
            catch (Exception ex)
            {

            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                elapsed = elapsed + (TIMER_INTERVAL);
                int seconds = (int)(elapsed / 1000);

                //Auto Stop
                if (seconds == limitSeconds)
                    Stop();

                new NSObject().InvokeOnMainThread(() =>
                {
                    lblTime.Text = seconds.ToString() + " seg(s)";
                });
            }
            catch
            {
                //dummy 
            }
        }

        private class CameraDelegate : UIImagePickerControllerDelegate
        {
            CameraImpl camera;

            public CameraDelegate(CameraImpl _camera) : base()
            {
                camera = _camera;
            }

            public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
            {
                camera.CameraCallBack(picker, info);
            }
        }

        private class VideoDelegate : AVCaptureFileOutputRecordingDelegate
        {
            byte[] midia;
            Action<byte[], object> callbackFinishedRecord;
            UIButton buttonToggle;
            UILabel lblTime;
            UILabel lblTimeLimite;
            UIView cameraView;
            AVCaptureSession session;

            public VideoDelegate(Action<byte[], object> _callbackFinishedRecord, UIButton _buttonToggle, UILabel _lblTime, UILabel _lblTimeLimite, UIView _cameraView)
            {
                callbackFinishedRecord = _callbackFinishedRecord;
                buttonToggle = _buttonToggle;
                lblTime = _lblTime;
                lblTimeLimite = _lblTimeLimite;
                cameraView = _cameraView;
            }

            public byte[] GetMidiaFinalizada()
            {
                return midia;
            }

            public override void FinishedRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections, NSError error)
            {
                buttonToggle.RemoveFromSuperview();
                lblTime.RemoveFromSuperview();
                lblTimeLimite.RemoveFromSuperview();
                cameraView.RemoveFromSuperview();

                if (error != null)
                    throw new ValidationException(error.Description);

                MediaPlayer.MPMoviePlayerController movie = new MediaPlayer.MPMoviePlayerController(outputFileUrl);
                UIImage singleFrameImage = movie.ThumbnailImageAt(captureOutput.RecordedDuration.Seconds / 2, MediaPlayer.MPMovieTimeOption.NearestKeyFrame);

                midia = System.IO.File.ReadAllBytes(outputFileUrl.Path);
                callbackFinishedRecord(midia, singleFrameImage);
            }
        }
    }
}