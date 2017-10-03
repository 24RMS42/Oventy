using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Newtonsoft.Json.Linq;
using UIKit;

namespace oventy.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
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
        	const string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

        	JObject templates = new JObject();
        	templates["genericMessage"] = new JObject
    		{
    		   {"body", templateBodyAPNS}
    		};

        	// Register for push with your mobile app
        	//Push push = TodoItemManager.DefaultManager.CurrentClient.GetPush();
        	//push.RegisterAsync(deviceToken, templates);
        }

        public override void DidReceiveRemoteNotification(UIApplication application,
                                                          NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
        	NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

        	string alert = string.Empty;
        	if (aps.ContainsKey(new NSString("alert")))
        		alert = (aps[new NSString("alert")] as NSString).ToString();

        	//show alert
        	if (!string.IsNullOrEmpty(alert))
        	{
        		UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
        		avAlert.Show();
            }
        }
    }
}
