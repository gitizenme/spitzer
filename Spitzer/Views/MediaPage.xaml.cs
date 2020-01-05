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
            
            viewModel.LoadFeedCommand.Execute(null);
            
            BindingContext = viewModel;
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");

        }
        
        private async void OnItemSelected(object sender, SelectionChangedEventArgs args)
        {
            var item = (args.CurrentSelection.FirstOrDefault() as RssSchema);
            if (item == null)
                return;

            var opened = await Launcher.TryOpenAsync(item.FeedUrl);
            
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
    }
}