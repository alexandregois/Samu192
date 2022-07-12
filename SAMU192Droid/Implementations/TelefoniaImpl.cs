using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Telephony;
using SAMU192Core.Interfaces;
using SAMU192Core.Utils;

namespace SAMU192Droid.Implementaitons
{
    internal class TelefoniaImpl : TelefoniaAbstract
    {
        StateListener listener;
        TelephonyManager telephonyManager;
        CallState previousState;

        public TelefoniaImpl(LigacaoConectada LigacaoConectada, LigacaoDesconectada LigacaoDesconectada, object phoneManager)
            : base(LigacaoConectada, LigacaoDesconectada, phoneManager)
        { }

        public override bool MakeCall(string number, object arg1 = null)
        {
            Activity activity = (Activity)arg1;
            if (CanDial(activity))
            {
                if (VerificaPermissao(activity))
                {
                    string dialIntent = Intent.ActionCall;
                    var telUri = Android.Net.Uri.Parse("tel:" + number);
                    var intent = new Intent(dialIntent, telUri);
                    activity.StartActivity(intent);
                    this.StartPhoneManager();
                    return true;
                }
            }
            return false;
        }

        public override bool VerificaPermissao(object act)
        {
            Activity activity = (Activity)act;
            const int RequestCallPhoneId = 1;
            string[] PermissionsPhoneCall = { Manifest.Permission.CallPhone, Manifest.Permission.CallPrivileged };

            bool ret = ActivityCompat.CheckSelfPermission(activity, PermissionsPhoneCall[0]) == Permission.Granted;
            if (!ret)
            {
                ActivityCompat.RequestPermissions(activity, PermissionsPhoneCall, RequestCallPhoneId);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CanDial(Activity activity)
        {
            Intent intent = new Intent(Intent.ActionCall);
            ComponentName componentName = intent.ResolveActivity(activity.PackageManager);
            if (componentName != null)
                return true;

            FeatureInfo[] v = activity.PackageManager.GetSystemAvailableFeatures();
            foreach (FeatureInfo fi in v)
            {
                if (fi != null)
                {
                    if ((fi.Name != null) && fi.Name.ToLower().Contains("telephon"))
                        return true;
                }
            }

            return false;
        }

        public override void StartPhoneManager()
        {
            telephonyManager = (TelephonyManager)PhoneManager;
            listener = new StateListener(this);
            telephonyManager.Listen(listener, PhoneStateListenerFlags.CallState);
        }

        public override void DisposePhoneManager()
        {
            listener.Dispose();
            listener = null;
            telephonyManager.Dispose();
            telephonyManager = null;
            ((TelephonyManager)base.PhoneManager).Dispose();
            base.DisposePhoneManager();
        }

        private class StateListener : PhoneStateListener
        {
            TelefoniaImpl phoneCallsImpl;

            public StateListener(TelefoniaImpl phoneCallsImpl)
            {
                this.phoneCallsImpl = phoneCallsImpl;
            }            

            public override void OnCallStateChanged(CallState state, string incomingNumber)
            {
                base.OnCallStateChanged(state, incomingNumber);

                switch (state)
                {
                    case CallState.Offhook:

                        if ((this.phoneCallsImpl.previousState == CallState.Idle))
                            this.phoneCallsImpl.OnCallStateChanged(Enums.PhoneCallState.Connected);

                        this.phoneCallsImpl.previousState = state;
                        break;

                    case CallState.Idle:
                        if ((this.phoneCallsImpl.previousState == CallState.Offhook))
                            this.phoneCallsImpl.OnCallStateChanged(Enums.PhoneCallState.Disconnected);

                        this.phoneCallsImpl.previousState = state;
                        break;
                }
            }
        }
    }
}