// MIT License
//
// Copyright (c) [2020] [Joe Chavez]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

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
                    // TODO duplicate code in MediaPage.xaml.cs

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