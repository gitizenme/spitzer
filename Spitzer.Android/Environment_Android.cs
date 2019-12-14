using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Plugin.CurrentActivity;
using Xamarin.Forms;

using Spitzer.Droid;
using Android.OS;

[assembly: Dependency(typeof(Environment_Android))]

namespace Spitzer.Droid
{
    public class Environment_Android : IEnvironment
    {
        public Task<Theme> GetOperatingSystemTheme()
        {
            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var uiModeFlags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        return Task.FromResult(Theme.Dark);

                    case UiMode.NightNo:
                        return Task.FromResult(Theme.Light);

                    default:
                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
                }
            }
            else
            {
                return Task.FromResult(Theme.Light);
            }
        }
    }
}
