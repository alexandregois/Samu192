using System;
using UIKit;
using SAMU192iOS.FacadeStub;
using SAMU192Core.DTO;
using SAMU192Core.Utils;

namespace SAMU192iOS.ViewControllers
{
    public partial class TermoViewController : BaseViewController
    {
        public TermoViewController(IntPtr handle) : base(handle)
        { }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Iniciado.Value);

                Utils.Interface.AlteraEstadoButton(btnAceitar, false);
                Utils.Interface.AlteraEstadoSwitchButton(swtEstouViente, lblEstouCiente, false);

                if (txtDescricao.ContentSize.Height < (nfloat)450)
                    txtDescricao.Scrolled += TxtDescricao_Scrolled;
                else
                    Utils.Interface.AlteraEstadoSwitchButton(swtEstouViente, lblEstouCiente, true);

                swtEstouViente.ValueChanged += SwtEstouCiente_ValueChanged;
                btnAceitar.TouchUpInside += BtnAceitar_TouchUpInside;
            }
            catch (Exception ex)
            {
               Utils.Mensagem.Erro(ex);
            }
        }

        private void SwtEstouCiente_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Utils.Interface.AlteraEstadoButton(btnAceitar, swtEstouViente.On);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void BtnAceitar_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                StubCadastro.SalvaAceiteTermo(new TermoDTO(true));
                StubAppCenter.AppAnalytic(Enums.AnalyticsType.WalkThrough_Termo_Aceite.Value);
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }

        private void TxtDescricao_Scrolled(object sender, EventArgs e)
        {
            try
            {
                if (txtDescricao.ContentOffset.Y > 260)
                {
                    Utils.Interface.AlteraEstadoSwitchButton(swtEstouViente, lblEstouCiente, true);
                }
            }
            catch (Exception ex)
            {
                Utils.Mensagem.Erro(ex);
            }
        }
    }
}