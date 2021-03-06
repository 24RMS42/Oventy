﻿using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;
using oventy;
using oventy.Droid;
using WindowsAzure.Messaging;


[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is needed only for Android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace GetStartedXamarinAndroid
{

	[BroadcastReceiver(Permission=Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
	[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE },
		Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK },
		Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY },
		Categories = new string[] { "@PACKAGE_NAME@" })]
	public class MyBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
	{
		public static string[] SENDER_IDS = new string[] { ClientConstants.GoogleSenderId };

		public const string TAG = "MyBroadcastReceiver-GCM";
	}


	[Service] // Must use the service tag
	public class PushHandlerService : GcmServiceBase
	{
		public static string RegistrationID { get; private set; }
		private NotificationHub Hub { get; set; }
        private Context _context;

		public PushHandlerService() : base(ClientConstants.GoogleSenderId)
		{
			Log.Info(MyBroadcastReceiver.TAG, "PushHandlerService() constructor");
		}

		protected override void OnRegistered(Context context, string registrationId)
		{
			Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            Settings.DeviceToken = registrationId;
			/*RegistrationID = registrationId;
            _context = context;

			//createNotification("PushHandlerService-GCM Registered...", "The device has been Registered!");

            Hub = new NotificationHub(ClientConstants.NotificationHubPath, ClientConstants.ConnectionString, context);
			try
			{
				Hub.UnregisterAll(registrationId);
			}
			catch (Exception ex)
			{
				Log.Error(MyBroadcastReceiver.TAG, ex.Message);
			}

			//var tags = new List<string>() { "falcons" }; // create tags if you want
			var tags = new List<string>() {};

			try
			{
				var hubRegistration = Hub.Register(registrationId, tags.ToArray());
                Settings.DeviceToken = registrationId;
			}
			catch (Exception ex)
			{
				Log.Error(MyBroadcastReceiver.TAG, ex.Message);
			}*/
		}

		protected override void OnMessage(Context context, Intent intent)
		{
			Log.Info(MyBroadcastReceiver.TAG, "GCM Message Received!");

			var msg = new StringBuilder();

			if (intent != null && intent.Extras != null)
			{
				foreach (var key in intent.Extras.KeySet())
					msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
			}

			string messageText = intent.Extras.GetString("message");

            //var intent = new Intent(context, typeof(MainActivity));
            //intent.PutExtra(MainActivity.GoToAction, action);
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            var pushId = DateTime.Now.TimeOfDay.Milliseconds;
            var pendingIntent = PendingIntent.GetActivity(context, pushId, intent, PendingIntentFlags.OneShot);

            // Set custom push notification sound.
            //var pathToPushSound = "android.resource://" + context.ApplicationContext.PackageName + "/raw/pushalert";
            //var soundUri = Android.Net.Uri.Parse(pathToPushSound);

            var notificationBuilder = new Android.App.Notification.Builder(context)
                .SetDefaults(NotificationDefaults.All)
                //.SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
                .SetSmallIcon(oventy.Droid.Resource.Drawable.icon)
                .SetContentTitle("Oventy")
                .SetContentText(messageText)
                .SetAutoCancel(true)
                .SetStyle(new Android.App.Notification.BigTextStyle().BigText(messageText))
                .SetVibrate(new long[] { 100, 1000, 100 })
                .SetLights(Android.Resource.Color.HoloBlueLight, 1, 1)
                .SetContentIntent(pendingIntent);

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(pushId, notificationBuilder.Build());

            //if (!string.IsNullOrEmpty (messageText))
            //{
            //	createNotification ("New hub message!", messageText);
            //}
            //else
            //{
            //	createNotification ("Unknown message details", msg.ToString ());
            //}
        }


		void createNotification(string title, string desc)
		{
			//Create notification
			var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

			//Create an intent to show UI
			var uiIntent = new Intent(this, typeof(MainActivity));

			//Create the notification
			var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

			//Auto-cancel will remove the notification once the user touches it
			notification.Flags = NotificationFlags.AutoCancel;

			//Set the notification info
			//we use the pending intent, passing our ui intent over, which will get called
			//when the notification is tapped.
			notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

			//Show the notification
			notificationManager.Notify(1, notification);
			//dialogNotify (title, desc);
		}

		protected void dialogNotify(String title, String message)
		{

			//_context.RunOnUiThread(() => {
				AlertDialog.Builder dlg = new AlertDialog.Builder(_context);
				AlertDialog alert = dlg.Create();
				alert.SetTitle(title);
				alert.SetButton("Ok", delegate {
					alert.Dismiss();
				});
				alert.SetMessage(message);
				alert.Show();
			//});
		}


		protected override void OnUnRegistered(Context context, string registrationId)
		{
			Log.Verbose(MyBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);

			createNotification("GCM Unregistered...", "The device has been unregistered!");
		}

		protected override bool OnRecoverableError(Context context, string errorId)
		{
			Log.Warn(MyBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

			return base.OnRecoverableError (context, errorId);
		}

		protected override void OnError(Context context, string errorId)
		{
			Log.Error(MyBroadcastReceiver.TAG, "GCM Error: " + errorId);
		}

	}

}

