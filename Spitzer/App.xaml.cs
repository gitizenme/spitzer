using System;
using System.Collections.Generic;
using System.Reflection;
using Acr.UserDialogs;
using FFImageLoading;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Spitzer.Services;
using Spitzer.Views;
using Spitzer.Styles;

namespace Spitzer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            SetTheme(Theme.Light);

            ToastConfig.DefaultBackgroundColor = System.Drawing.Color.Gray;
            ToastConfig.DefaultMessageTextColor = System.Drawing.Color.Navy;
            ToastConfig.DefaultActionTextColor = System.Drawing.Color.DarkRed;
            ToastConfig.DefaultDuration = new TimeSpan(0, 0, 5);
            ToastConfig.DefaultPosition = ToastPosition.Bottom;
            
            DependencyService.Register<NasaMediaLibraryDataStore>();
            MainPage = new MainPage();
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        protected override async void OnStart()
        {
            base.OnStart();

            Theme theme = await DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
            Analytics.TrackEvent($"Called: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        protected override void OnSleep()
        {
            Analytics.TrackEvent("OnSleep");
            ImageService.Instance.SetExitTasksEarly(true);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            ImageService.Instance.SetExitTasksEarly(false);

            Theme theme = await DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
            Analytics.TrackEvent($"Called: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        void SetTheme(Theme theme)
        {
            if(MainPage != null)
            {
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();

                    switch (theme)
                    {
                        case Theme.Dark:
                            mergedDictionaries.Add(new DarkTheme());
                            break;
                        case Theme.Light:
                        default:
                            mergedDictionaries.Add(new LightTheme());
                            break;
                    }
                }
            }
            Analytics.TrackEvent($"Called: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }
    }
}
