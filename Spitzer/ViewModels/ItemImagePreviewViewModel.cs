using System;
using System.Windows.Input;
using Acr.UserDialogs;
using FFImageLoading;
using FFImageLoading.Work;
using Spitzer.Views;
using Xamarin.Forms;

namespace Spitzer.ViewModels
{
    public class ItemImagePreviewViewModel : BaseViewModel
    {
        private string imageDimensions = string.Empty;
        private ImageInformation imageInformation;
        private FileWriteInfo fileWriteInfo;
        public Uri ImagePreview { get; set; }
        public ItemImagePreviewViewModel()
        {
            this.ShowPinchZoomToast = new Command((args) =>
            {
                if (args is PreviewToastMessage toastMessage)
                {
                    var backgroundColor = Color.White;
                    var textColor = Color.Black;
                    if (App.CurrentTheme == Theme.Dark)
                    {
                        backgroundColor = Color.Black;
                        textColor = Color.White;
                    }
                    
                    UserDialogs.Instance.Toast(new ToastConfig(toastMessage.Message)
                        .SetDuration(TimeSpan.FromSeconds(toastMessage.Duration))
                        .SetPosition(ToastPosition.Bottom)
                        .SetBackgroundColor(backgroundColor)
                        .SetMessageTextColor(textColor)
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

        public ImageInformation ImageInformation
        {
            get => imageInformation;
            set => SetProperty(ref imageInformation, value);
        }

        public FileWriteInfo FileWriteInfo
        {
            get => fileWriteInfo;
            set => SetProperty(ref fileWriteInfo, value);
        }
    }
}