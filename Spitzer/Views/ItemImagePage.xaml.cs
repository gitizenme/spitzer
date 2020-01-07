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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
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

            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
            
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