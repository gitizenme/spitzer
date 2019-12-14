using System;
using UIKit;
using Xamarin.Forms;
using Spitzer.iOS;
using System.Threading.Tasks;

[assembly: Dependency(typeof(Environment_iOS))]
namespace Spitzer.iOS
{
    public class Environment_iOS : IEnvironment
    {
        public async Task<Theme> GetOperatingSystemTheme()
        {
            //Ensure the current device is running 12.0 or higher, because `TraitCollection.UserInterfaceStyle` was introduced in iOS 12.0
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                var currentUIViewController = await GetVisibleViewController();

                var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

                switch (userInterfaceStyle)
                {
                    case UIUserInterfaceStyle.Light:
                        return Theme.Light;
                    case UIUserInterfaceStyle.Dark:
                        return Theme.Dark;
                    default:
                        throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported");
                }
            }
            else
            {
                return Theme.Light;
            }
        }

        static Task<UIViewController> GetVisibleViewController()
        {
			// UIApplication.SharedApplication can only be referenced on by Main Thread, so we'll use Device.InvokeOnMainThreadAsync which was introduced in Xamarin.Forms v4.2.0
			return Device.InvokeOnMainThreadAsync(() =>
			{
				var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

				switch (rootController.PresentedViewController)
				{
					case UINavigationController navigationController:
						return navigationController.TopViewController;

					case UITabBarController tabBarController:
						return tabBarController.SelectedViewController;

					case null:
						return rootController;

					default:
						return rootController.PresentedViewController;
				}
			});
        }
    }
}
