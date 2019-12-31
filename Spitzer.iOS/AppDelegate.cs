﻿using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace Spitzer.iOS
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
            AppCenter.Start("b35173bd-3ad1-453f-844e-761dbf03645b",
                typeof(Analytics), typeof(Crashes));
            
            Xamarin.Calabash.Start();

            CachedImageRenderer.Init();           

            global::Xamarin.Forms.Forms.Init();

            // CachedImageRenderer.InitImageSourceHandler();
                
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
