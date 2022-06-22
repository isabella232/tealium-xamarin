using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Tealium;
using Tealium.iOS;
using Tealium.Platform.iOS;
using Tealium.RemoteCommands.Firebase.iOS;

namespace TealiumXamarinExample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // We create Tealium instance here so that application lifecycle can start right away
            Teal.Helper.SetInstanceManager(new TealiumInstanceManager(new TealiumInstanceFactoryIOS()));
            var command = new FirebaseRemoteCommandIOS(new RemoteCommandTypeWrapper("firebase", NSBundle.MainBundle));
            Console.WriteLine(command.Name);
            Teal.Helper.RemoteCommands.Add(command);
            Teal.Helper.Init();
            //var config = new TealiumConfigWrapper("", "", "", "", null);
            //var teal = new TealiumWrapper(config, null);

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
