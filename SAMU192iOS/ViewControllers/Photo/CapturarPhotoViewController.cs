using System;
using System.Threading;
using SAMU192Core.Exceptions;
using SAMU192Core.Utils;
using SAMU192iOS.FacadeStub;
using SAMU192iOS.Implementations;

namespace SAMU192iOS.ViewControllers
{
    public partial class CapturarPhotoViewController : BaseViewController
    {
        bool ehVideo = false;
        string Capturar_Foto = "Capturar Mídia", Enviar_Foto = "Enviar Mídia", nomeArquivo = string.Empty;
        BackgroundTask bt;
        byte[] midia;

        public CapturarPhotoViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                StubMidia.Carrega(false, ImagemCallback);
                aivStatus.StartAnimating();
                aivStatus.Hidden = false;
                btnPhoto01.TouchUpInside += BtnPhoto_TouchUpInside;
                BotaoCapturarFoto();
                StatusInicial();
                AbrirCamera();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void After_Grant()
        {
            InvokeOnMainThread(() =>
            {
                try
                {
                    AbrirCamera();
                }
                catch (Exception ex)
                {
                    Utils.Mensagem.Erro(ex);
                }
            });
        }

        private void BtnPhoto_TouchUpInside(object sender, EventArgs eventArgs)
        {
            try
            {
                if (btnPhoto01.Title(UIKit.UIControlState.Normal) == Capturar_Foto)
                    AbrirCamera();
                else
                    ReenviarFoto();
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BotaoReenviarFoto()
        {
            StatusErro();
            btnPhoto01.SetTitle("Sim", UIKit.UIControlState.Normal);
        }

        private void BotaoCapturarFoto()
        {
            StatusOK();
            btnPhoto01.SetTitle(Capturar_Foto, UIKit.UIControlState.Normal);
        }

        private void StatusOK()
        {
            btnPhoto01.Enabled = true;
            aivStatus.Hidden = true;
            aivStatus.StopAnimating();
            lblStatusEnvio.Text = string.Empty;
            lblStatusEnvio.Hidden = true;
            btnPhoto01.SetTitle(Capturar_Foto, UIKit.UIControlState.Normal);
        }

        private void StatusInicial()
        {
            btnPhoto01.Enabled = true;
            aivStatus.Hidden = false;
            aivStatus.StartAnimating();
            lblStatusEnvio.Text = "Aguardando câmera...";
            lblStatusEnvio.Hidden = false;
            btnPhoto01.SetTitle(Enviar_Foto, UIKit.UIControlState.Normal);
        }

        private void StatusEnviandoFoto()
        {
            btnPhoto01.Enabled = false;
            aivStatus.Hidden = false;
            aivStatus.StartAnimating();
            lblStatusEnvio.Text = "Enviando foto para a central...";
            lblStatusEnvio.Hidden = false;
        }

        private void StatusErro()
        {
            btnPhoto01.Enabled = true;
            aivStatus.Hidden = true;
            aivStatus.StopAnimating();
            lblStatusEnvio.Text = "Problemas ao enviar foto. Deseja reenviá-la?";
            lblStatusEnvio.Hidden = false;
        }
        private void StatusMidiaProntaParaEnvio()
        {
            btnPhoto01.Enabled = true;
            aivStatus.Hidden = true;
            aivStatus.StopAnimating();
            lblStatusEnvio.Text = "Deseja enviar esta mídia?";
            lblStatusEnvio.Hidden = false;
            btnPhoto01.SetTitle("Sim", UIKit.UIControlState.Normal);
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

            aivStatus.Hidden = false;
            aivStatus.StartAnimating();

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
            InvokeOnMainThread(() =>
            {
                aivStatus.Hidden = false;
                aivStatus.StartAnimating();
                StatusEnviandoFoto();
            });
        }

        private void PostExecute()
        {
            InvokeOnMainThread(() =>
            {
                Envio_Callback();
                aivStatus.Hidden = true;
                aivStatus.StopAnimating();
                Utils.Interface.VoltarViewController(true, true, this.NavigationController);
            });
        }

        private void OnCancel()
        {
            InvokeOnMainThread(() =>
            {
                aivStatus.Hidden = true;
                aivStatus.StopAnimating();
                
                    Utils.Mensagem.Aviso("Operação interrompida!");
            });
        }
        private void OnError(Exception ex)
        {
            InvokeOnMainThread(() =>
            {
                StatusErro();
                Utils.Mensagem.Erro(ex);
            });
        }
        private void OnValidationException(ValidationException vex)
        {
            InvokeOnMainThread(() =>
            {
                StatusErro();
                Utils.Mensagem.Aviso(vex.Message);
            });
        }

        private void Envio_Callback()
        {
            try
            {
                aivStatus.Hidden = true;
                aivStatus.StopAnimating();
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
                imgPhoto01.Hidden = false;
                aivStatus.Hidden = true;
                aivStatus.StopAnimating();
                lblStatusEnvio.Text = string.Empty;
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
            StubAppCenter.AppAnalytic(Enums.AnalyticsType.Foto_Click.Value);
            StubMidia.AbrirCamera(false, View, this.NavigationController, imgPhoto01, StatusErro, After_Grant);
        }

        private void FotoEnviada_CallBack()
        {
            InvokeOnMainThread(() =>
            {
                try
                {
                    if (btnPhoto01 != null)
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