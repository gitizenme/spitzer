using System;
using System.Collections.Generic;
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
            DependencyService.Register<NasaMediaLibraryDataStore>();
            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            Theme theme = await DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnResume()
        {
            base.OnResume();

            Theme theme = await DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
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
        }
    }
}
