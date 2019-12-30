using System;
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
        public ICommand HelpToast { get; }

        public ItemImagePage(ItemImageViewModel itemImageViewModel)
        {
            InitializeComponent();

            BindingContext = itemImageViewModel;
            
            Task.Delay(new TimeSpan(0, 0, 5)).ContinueWith(_ => { itemImageViewModel.Item.ShowPinchZoomToast.Execute(new PreviewToastMessage
            {
                Message = "Pinch to zoom in/out",
                Duration = 3
            }); });

            Task.Delay(new TimeSpan(0, 0, 10)).ContinueWith(_ => { itemImageViewModel.Item.ShowPinchZoomToast.Execute(new PreviewToastMessage
            {
                Message = "Double-tap for min/max zoom",
                Duration = 3
            }); });
            
            Task.Delay(new TimeSpan(0, 0, 15)).ContinueWith(_ => { itemImageViewModel.Item.ShowPinchZoomToast.Execute(new PreviewToastMessage
            {
                Message = "One-finger to pan",
                Duration = 3
            }); });
        }
    }

    public class PreviewToastMessage
    {
        public string Message { get; set; }
        public int Duration { get; set; }
    }
}