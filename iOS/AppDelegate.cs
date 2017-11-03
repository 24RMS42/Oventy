using System;
using System.Collections.Generic;
using System.Linq;
using WindowsAzure.Messaging;
using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;

namespace oventy.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // Register for push notifications.
            var settings = UIUserNotificationSettings.GetSettingsForTypes(
            	UIUserNotificationType.Alert
            	| UIUserNotificationType.Badge
            	| UIUserNotificationType.Sound,
            	new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            global::Xamarin.Forms.Forms.Init();

            App.DeviceType = ClientConstants.Platform_iOS;
            app.StatusBarHidden = true;

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Console.WriteLine("nsdata device token:" + deviceToken);
            Settings.DeviceToken = deviceToken.Description.Trim('<', '>').Replace(" ", "");
            //Hub = new SBNotificationHub(ClientConstants.ConnectionString, ClientConstants.NotificationHubPath);

            //Hub.UnregisterAllAsync (deviceToken, (error) => {
            //     if (error != null)
            //     {
            //         Console.WriteLine("Error calling Unregister: {0}", error.ToString());
            //         return;
            //     }

            //     NSSet tags = null; // create tags if you want
            //     Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) => {
            //         Console.WriteLine("register success:" + errorCallback);
            //         if (errorCallback != null)
            //             Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
            //         else
            //         {
            //             Settings.DeviceToken = deviceToken.Description.Trim('<', '>').Replace(" ", "");
            //         }
            //     });
            //});
        }

        public override void ReceivedRemoteNotification(UIApplication app, NSDictionary userInfo)
        {
            // Process a notification received while the app was already open
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
        	// Check to see if the dictionary has the aps key.  This is the notification payload you would have sent
        	if (null != options && options.ContainsKey(new NSString("aps")))
        	{
        		//Get the aps dictionary
        		NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

        		string alert = string.Empty;

        		if (aps.ContainsKey(new NSString("alert")))
        			alert = (aps[new NSString("alert")] as NSString).ToString();

                //If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    //Manually show an alert
                    if (!string.IsNullOrEmpty(alert))
                    {
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();
                    }
                }
             }
        }
    }
}
