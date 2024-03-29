﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SAMU192Droid.Interface.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAMU192Droid
{

    [BroadcastReceiver(Enabled = true, DirectBootAware = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class BootBroadcast : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			Intent i = new Intent(context, typeof(MainActivity));
			i.AddFlags(ActivityFlags.NewTask);
			context.StartActivity(i);

			MainActivity.SetAlarmForBackgroundServices(context);
		}
	}
}