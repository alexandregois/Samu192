using Foundation;
using SAMU192Core.Utils;
using SAMU192iOS.FacadeStub;
using System;
using UIKit;

namespace SAMU192iOS.ViewControllers
{
    public partial class WalkThroughViewController : BaseViewController
    {

        bool notificouSobrePermissao = false;
        public WalkThroughViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                if (btnComeceUsar != null)
                    btnComeceUsar.TouchUpInside += BtnComecarUsar_TouchUpInside;
                if (btnFechar1 != null)
                    btnFechar1.TouchUpInside += BtnFechar_TouchUpInside;
                if (btnFechar2 != null)
                    btnFechar2.TouchUpInside += BtnFechar_TouchUpInside;
                if (btnFechar3 != null)
                    btnFechar3.TouchUpInside += BtnFechar_TouchUpInside;
                if (btnFechar4 != null)
                    btnFechar4.TouchUpInside += BtnFechar_TouchUpInside;
                if (btnFechar5 != null)
                    btnFechar5.TouchUpInside += BtnFechar_TouchUpInside;

                if (pgControl0 != null)
                    pgControl0.ValueChanged += PgControl0_ValueChanged;
                if (pgControl1 != null)
                    pgControl1.ValueChanged += PgControl0_ValueChanged;
                if (pgControl2 != null)
                    pgControl2.ValueChanged += PgControl0_ValueChanged;
                if (pgControl3 != null)
                    pgControl3.ValueChanged += PgControl0_ValueChanged;
                if (pgControl4 != null)
                    pgControl4.ValueChanged += PgControl0_ValueChanged;


                if (camadaFundo0 != null)
                {
                    camadaFundo0.UserInteractionEnabled = false;
                    camadaFundo0.AccessibilityElementsHidden = true;
                    camadaFundo0.IsAccessibilityElement = false;
                }

                if (camadaFundo1 != null)
                {
                    camadaFundo1.UserInteractionEnabled = false;
                    camadaFundo1.AccessibilityElementsHidden = true;
                    camadaFundo1.IsAccessibilityElement = false;
                }

                if (camadaFundo2 != null)
                {
                    camadaFundo2.UserInteractionEnabled = false;
                    camadaFundo2.AccessibilityElementsHidden = true;
                    camadaFundo2.IsAccessibilityElement = false;
                }

                if (camadaFundo3 != null)
                {
                    camadaFundo3.UserInteractionEnabled = false;
                    camadaFundo3.AccessibilityElementsHidden = true;
                    camadaFundo3.IsAccessibilityElement = false;
                }

                if (camadaFundo4 != null)
                {
                    camadaFundo4.UserInteractionEnabled = false;
                    camadaFundo4.AccessibilityElementsHidden = true;
                    camadaFundo4.IsAccessibilityElement = false;
                }

                if (camadaFundo5 != null)
                {
                    camadaFundo5.UserInteractionEnabled = false;
                    camadaFundo5.AccessibilityElementsHidden = true;
                    camadaFundo5.IsAccessibilityElement = false;
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }


        private void PgControl0_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                base.PerformSegue("segueCompleteCadastro", new NSObject());
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnFechar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.FecharViewControllerAtual(true);
                Utils.Interface.EmpurrarViewController(this, "NumeroTelefoneViewController");
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Fechado.Value);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnComecarUsar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.FecharViewControllerAtual(true);
                Utils.Interface.EmpurrarViewController(this, "NumeroTelefoneViewController");
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Completado.Value);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            try
            {
                if (segueIdentifier == "segueCompleteCadastro")
                {
                    notificouSobrePermissao = StubGPS.NotificaAtivacao(
                        (bool val) =>
                        {
                            if (val && !notificouSobrePermissao)
                            {
                                base.PerformSegue(segueIdentifier, sender);
                                notificouSobrePermissao = true;
                            }
                        });
                    return false;
                }
                return base.ShouldPerformSegue(segueIdentifier, sender);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
                return false;
            }
        }
    }
}