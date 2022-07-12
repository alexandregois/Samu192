using System;
using UIKit;
using Foundation;
using CoreTelephony;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;

namespace SAMU192iOS.Implementaitons
{
    internal class TelefoniaImpl : TelefoniaAbstract
    {
        CTCallCenter telephonyManager;

        public TelefoniaImpl(TelefoniaAbstract.LigacaoConectada UpdatePhoneCallConnected, TelefoniaAbstract.LigacaoDesconectada UpdatePhoneCallDisconnected) 
            : base(UpdatePhoneCallConnected, UpdatePhoneCallDisconnected, new CTCallCenter())
        {
            //base constructor
        }

        public override bool MakeCall(string number, object arg1 = null)
        {
            var callURL = new NSUrl("tel:" + number);
            if (UIApplication.SharedApplication.CanOpenUrl(callURL))
            {
                if (UIApplication.SharedApplication.OpenUrl(callURL))
                {
                    this.StartPhoneManager();
                    return true;
                }
            }
            return false;
        }

        public override void StartPhoneManager()
        {
            if (telephonyManager == null)
            {
                telephonyManager = (CTCallCenter)PhoneManager;
                telephonyManager.CallEventHandler += CallEventHandler;
            }
        }

        public override void DisposePhoneManager()
        {
            telephonyManager.Dispose();
            telephonyManager = null;
            ((CTCallCenter)base.PhoneManager).Dispose();
            base.DisposePhoneManager();
        }

        private void CallEventHandler(CTCall call)
        {
            //A garantia para controlar apenas ligações feitas pelo App SAMU192 é existir 
            //uma instância deste objeto "CTCallCenter", logo este EventHandler será apenas do 192

            string state = call.CallState;

            if (state == call.StateDisconnected)
            {
                this.OnCallStateChanged(Enums.PhoneCallState.Disconnected);
            }
            else
            if (state == call.StateDialing)
            {
                //Compatinilizar comportamento com Android
                this.OnCallStateChanged(Enums.PhoneCallState.Connected);
            }
            else
            if (state == call.StateConnected)
            {
                //dummy
            }
            else
            if (state == call.StateIncoming)
            {
                //dummy
            }
        }

        public override bool VerificaPermissao(object activity)
        {
            var callURL = new NSUrl("tel:" + SAMU192Core.Utils.Constantes.NUMERO_TELEFONE);
            return UIApplication.SharedApplication.CanOpenUrl(callURL);
        }
    }
}