using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Spitzer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spitzer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemImagePage : ContentPage
    {
        private Task zoomToast;
        private Task doubleTapToast;
        private Task panToast;
        private readonly ItemImageViewModel viewModel;
        private readonly CancellationTokenSource zoomToastCancelTokenSource;
        private readonly CancellationTokenSource doubleTapToastCancelTokenSource;
        private readonly CancellationTokenSource panToastCancelTokenSource;
        public ICommand HelpToast { get; }

        public ItemImagePage(ItemImageViewModel itemImageViewModel)
        {
            InitializeComponent();
            zoomToastCancelTokenSource = new CancellationTokenSource();
            doubleTapToastCancelTokenSource = new CancellationTokenSource();
            panToastCancelTokenSource = new CancellationTokenSource();
            BindingContext = viewModel = itemImageViewModel;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            zoomToast = Task.Delay(new TimeSpan(0, 0, 5), zoomToastCancelTokenSource.Token).ContinueWith(_ => { viewModel.Item.ShowPinchZoomToast.Execute(new PreviewToastMessage
            {
                Message = "Use two fingers to zoom in/out",
                Duration = 3
            }); }, zoomToastCancelTokenSource.Token);

            doubleTapToast = Task.Delay(new TimeSpan(0, 0, 10), doubleTapToastCancelTokenSource.Token).ContinueWith(_ => { viewModel.Item.ShowPinchZoomToast.Execute(new PreviewToastMessage
            {
                Message = "Double-tap for min/max zoom",
                Duration = 3
            }); }, doubleTapToastCancelTokenSource.Token);
            
            panToast = Task.Delay(new TimeSpan(0, 0, 15), panToastCancelTokenSource.Token).ContinueWith(_ => { viewModel.Item.ShowPinchZoomToast.Execute(new PreviewToastMessage
            {
                Message = "One-finger to pan",
                Duration = 3
            }); }, panToastCancelTokenSource.Token);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            zoomToastCancelTokenSource.Cancel();
            doubleTapToastCancelTokenSource.Cancel();
            panToastCancelTokenSource.Cancel();
        }
    }

    public class PreviewToastMessage
    {
        public string Message { get; set; }
        public int Duration { get; set; }
    }
}