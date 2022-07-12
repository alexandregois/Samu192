using System;
using System.Threading;
using Android.OS;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using Android.Views;
using SAMU192Droid.FacadeStub;
using SAMU192Droid.Implementations;
using SAMU192Core.Utils;
using SAMU192Core.Exceptions;

namespace SAMU192Droid.Interface.Activities
{
    [Activity(Label = "CameraActivity")]
    internal class CameraActivity : Activity
    {
        ImageView foto_iv;
        Button capturar_cmd;
        ProgressBar envio_progressbar;
        TextView reenviar_tv;
        bool podeTirarFoto = false, ehVideo = false;
        string Capturar_Foto = "Capturar Mídia", Enviar_Foto = "Enviar Mídia", nomeArquivo = string.Empty;
        BackgroundTask bt;
        byte[] midia;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.camera);

                StubMidia.Carrega(this, ImagemCallback);
                podeTirarFoto = StubMidia.PodeTirarFotos(this.PackageManager);
                if (podeTirarFoto)
                {
                    capturar_cmd = FindViewById<Button>(Resource.Id.capturar_cmd);
                    foto_iv = FindViewById<ImageView>(Resource.Id.foto_iv);
                    reenviar_tv = FindViewById<TextView>(Resource.Id.reenviar_tv);
                    envio_progressbar = FindViewById<ProgressBar>(Resource.Id.envio_progressbar);
                    envio_progressbar.Animate();
                    envio_progressbar.Visibility = Android.Views.ViewStates.Gone;
                    capturar_cmd.Click += Capturar_cmd_Click;
                    BotaoCapturarFoto();
                    StatusInicial();
                    AbrirCamera();
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void Capturar_cmd_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                if (capturar_cmd.Text == Capturar_Foto)
                    AbrirCamera();
                else
                    ReenviarFoto();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                switch ((Enums.ActivityResult)requestCode)
                {
                    case Enums.ActivityResult.Foto:
                        StubMidia.CameraCallBack(new object());
                        nomeArquivo = "Foto_" + Guid.NewGuid().ToString("N") + ".jpg";
                        StatusMidiaProntaParaEnvio();
                        break;
                    case Enums.ActivityResult.Video:
                        StubMidia.CameraCallBack(new object());
                        nomeArquivo = "Video_" + Guid.NewGuid().ToString("N") + ".mp4";
                        StatusMidiaProntaParaEnvio();
                        break;
                    case Enums.ActivityResult.Galeria:
                        if (data != null)
                        {
                            StubMidia.CameraCallBack(data.Data);
                            nomeArquivo = "FotoGaleria_" + Guid.NewGuid().ToString("N") + ".jpg";
                            StatusMidiaProntaParaEnvio();
                            break;
                        }
                        else
                            break;
                    default:
                        break;
                }
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
                StatusErro();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                switch (requestCode)
                {
                    case 2: //Storage / para foto/video
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                            AbrirCamera();
                        break;
                    case 3: //Camera Foto Video
                        if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                            AbrirCamera();
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BotaoReenviarFoto()
        {
            StatusErro();
            capturar_cmd.Text = "Sim";
        }

        private void BotaoCapturarFoto()
        {
            StatusOK();
            capturar_cmd.Text = Capturar_Foto;
        }

        private void StatusOK()
        {
            capturar_cmd.Enabled = true;
            envio_progressbar.Visibility = Android.Views.ViewStates.Gone;
            reenviar_tv.Text = "";
            reenviar_tv.Visibility = Android.Views.ViewStates.Gone;
            capturar_cmd.Text = Capturar_Foto;
        }

        private void StatusInicial()
        {
            capturar_cmd.Enabled = true;
            envio_progressbar.Visibility = Android.Views.ViewStates.Visible;
            reenviar_tv.Text = "Aguardando câmera...";
            reenviar_tv.Visibility = Android.Views.ViewStates.Visible;
            capturar_cmd.Text = Enviar_Foto;
        }

        private void StatusEnviandoFoto()
        {
            capturar_cmd.Enabled = false;
            envio_progressbar.Visibility = Android.Views.ViewStates.Visible;
            reenviar_tv.Text = "Enviando foto para a central...";
            reenviar_tv.Visibility = Android.Views.ViewStates.Visible;
        }

        private void StatusErro()
        {
            capturar_cmd.Enabled = true;
            envio_progressbar.Visibility = Android.Views.ViewStates.Gone;
            reenviar_tv.Text = "Problemas ao enviar foto. Deseja reenviá-la?";
            reenviar_tv.Visibility = Android.Views.ViewStates.Visible;
        }
        private void StatusMidiaProntaParaEnvio()
        {
            capturar_cmd.Enabled = true;
            envio_progressbar.Visibility = Android.Views.ViewStates.Gone;
            reenviar_tv.Text = "Deseja enviar esta mídia?";
            capturar_cmd.Text = "Sim";
            reenviar_tv.Visibility = Android.Views.ViewStates.Visible;
        }

        private void ReenviarFoto()
        {
            EnviarMidia();
        }

        private void EnviarMidia()
        {
            if (midia == null)
            {
                Utils.Mensagem.Aviso("Selecione uma mídia primeiro.");
                return;
            }

            envio_progressbar.Visibility = ViewStates.Visible;

                bt = new BackgroundTask(this, false,
                    PreExecute,
                    OnExecute,
                    PostExecute,
                    OnCancel,
                    OnError,
                    OnValidationException);
                bt.Execute();
        }

        private void OnExecute(CancellationToken ct)
        {
            StubWebService.EnviaMidia(StubWebService.Servidor, ehVideo ? SAMU192InterfaceService.Utils.Enums.TipoMidia.Video : SAMU192InterfaceService.Utils.Enums.TipoMidia.Foto, midia, FotoEnviada_CallBack);
        }

        private void PreExecute()
        {
            RunOnUiThread(() =>
            {
                envio_progressbar.Visibility = ViewStates.Visible;
                StatusEnviandoFoto();
            });
        }

        private void PostExecute()
        {
            RunOnUiThread(() =>
            {
                Envio_Callback();
                envio_progressbar.Visibility = ViewStates.Gone;
                Finish();
            });
        }

        private void OnCancel()
        {
            RunOnUiThread(() =>
            {
                envio_progressbar.Visibility = ViewStates.Gone;
                if (!StubUtilidades.AppEmProducao())
                    Utils.Mensagem.Aviso("Operação interrompida!");
            });
        }
        private void OnError(Exception ex)
        {
            RunOnUiThread(() =>
            {
                StatusErro();
                Utils.Mensagem.Erro(ex);
            });
        }
        private void OnValidationException(ValidationException vex)
        {
            RunOnUiThread(() =>
            {
                StatusErro();
                Utils.Mensagem.Dialogs.AlertaSimples(this, vex.Message);
            });
        }

        private void Envio_Callback()
        {
            try
            {
                envio_progressbar.Visibility = ViewStates.Gone;
                Utils.Mensagem.Aviso("Mídia enviada com sucesso!");
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void ImagemCallback(byte[] _midia, object thumbnail)
        {
            try
            {
                midia = _midia;
                Bitmap thumb = (Bitmap)thumbnail;
                foto_iv.Visibility = ViewStates.Visible;

                const double screenLimit = 500.0;
                foto_iv.SetImageBitmap(Utils.Ferramentas.ResizeBitmap(thumb, (int)screenLimit));
            }
            catch (ValidationException vex)
            {
                Utils.Mensagem.Aviso(vex.Message);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void AbrirCamera()
        {
            StubMidia.AbrirCamera(false, this, StatusErro);
        }

        private void FotoEnviada_CallBack()
        {
            RunOnUiThread(() =>
            {
                try
                {
                    if (capturar_cmd != null)
                    {
                        BotaoCapturarFoto();
                    }
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }
    }
}