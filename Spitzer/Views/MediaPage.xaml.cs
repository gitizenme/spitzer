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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Microsoft.Toolkit.Parsers.Rss;
using Spitzer.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spitzer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPage : ContentPage
    {
        readonly MediaPageViewModel viewModel;

        public MediaPage()
        {
            InitializeComponent();
            
            viewModel = new MediaPageViewModel();
            
            BindingContext = viewModel;
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadFeedCommand.Execute(null);
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs args)
        {
            var item = (args.CurrentSelection.FirstOrDefault() as RssSchema);
            if (item == null)
                return;

            var url = item.FeedUrl.Trim();
            Console.WriteLine($"feedUrl: {url}");
            var opened = await Launcher.TryOpenAsync(url);
            
            if (!opened)
            {
                // TODO duplicate code in ItemImagePreviewModel
                var backgroundColor = Color.White;
                var textColor = Color.Black;
                if (App.CurrentTheme == Theme.Dark)
                {
                    backgroundColor = Color.Black;
                    textColor = Color.White;
                }           
                
                UserDialogs.Instance.Toast(new ToastConfig("Unable to open the item content.")
                    .SetPosition(ToastPosition.Bottom)
                    .SetBackgroundColor(backgroundColor)
                    .SetMessageTextColor(textColor)
                );
            }
            
            // Manually deselect item.
            ItemsCollectionView.SelectedItem = null;
        }
        
        
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null && !string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                Console.WriteLine($"text: {e.NewTextValue}");
                viewModel?.FilterCommand.Execute(e.NewTextValue);
            }
            else
            {
                viewModel?.ResetItemsCommand.Execute(null);
            }
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SpitzerModelPage());
        }
    }
}