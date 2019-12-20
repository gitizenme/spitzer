using System;
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
                if (theme == Theme.Light)
                {
                    Current.Resources = new LightTheme();
                }
                else if (theme == Theme.Dark)
                {
                    Current.Resources = new DarkTheme(); // needs using DarkMode.Styles;
                }
            }
        }
    }
}
