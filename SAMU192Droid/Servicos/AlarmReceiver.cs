using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMU192Droid.Servicos
{
    //[BroadcastReceiver(Enabled = true, Exported = true)]
    [Service(
        Enabled = true
        , Exported = true
        , IsolatedProcess = true
        , Name = "com.trueinformationtechnology.samu192.AlarmReceiver"
    )]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted,
        "android.intent.action.QUICKBOOT_POWERON",
        "android.intent.action.BOOT_COMPLETED",
        "android.intent.action.LOCKED_BOOT_COMPLETED"
    }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var backgroundServiceIntent = new Intent(context, typeof(LerMensagensService));
            context.StartService(backgroundServiceIntent);
        }
    }
}