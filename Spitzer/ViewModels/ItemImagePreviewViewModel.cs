using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Spitzer.Views;
using Xamarin.Forms;

namespace Spitzer.ViewModels
{
    public class ItemImagePreviewViewModel : BaseViewModel
    {
        private string imageDimensions = string.Empty;
        public Uri ImagePreview { get; set; }
        public ItemImagePreviewViewModel()
        {
            
            this.ShowPinchZoomToast = new Command((args) =>
            {
                if (args is PreviewToastMessage toastMessage)
                {
                    UserDialogs.Instance.Toast(new ToastConfig(toastMessage.Message)
                        .SetDuration(TimeSpan.FromSeconds(toastMessage.Duration))
                        .SetPosition(ToastPosition.Bottom)
                    );
                }
            });
        }

        public string ImageDimensions
        {
            get => imageDimensions;
            set => SetProperty(ref imageDimensions, value);
        }

        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public string ImageSize { get; set; }
        public ICommand ShowPinchZoomToast { get; }
    }
}