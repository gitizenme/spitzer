using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Spitzer.Services;
using Spitzer.Views;

namespace Spitzer
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
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
            // TODO handle light & dark theme
        }
    }
}
