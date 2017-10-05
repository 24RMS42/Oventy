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

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Hub = new SBNotificationHub(ClientConstants.ConnectionString, ClientConstants.NotificationHubPath);

            Hub.UnregisterAllAsync (deviceToken, (error) => {
                     if (error != null)
                     {
                         Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                         return;
                     }

                     NSSet tags = null; // create tags if you want
                     Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) => {
                     Console.WriteLine("register success:" + errorCallback);
                     if (errorCallback != null)
                         Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                     });
            });
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

        		//Extract the alert text
        		// NOTE: If you're using the simple alert by just specifying
        		// "  aps:{alert:"alert msg here"}  ", this will work fine.
        		// But if you're using a complex alert with Localization keys, etc.,
        		// your "alert" object from the aps dictionary will be another NSDictionary.
        		// Basically the JSON gets dumped right into a NSDictionary,
        		// so keep that in mind.
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
