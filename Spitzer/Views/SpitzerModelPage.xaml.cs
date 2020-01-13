using Spitzer.Models;
using Urho;
using Urho.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spitzer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpitzerModelPage : ContentPage
    {
        private SpitzerModel urhoApp;

        public SpitzerModelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            // base.OnAppearing();
            urhoApp = await UrhoSurface.Show<SpitzerModel>(new ApplicationOptions(assetsFolder: "SpitzerModel")
                {Orientation = ApplicationOptions.OrientationType.LandscapeAndPortrait});
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }
    }
}