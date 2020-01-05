using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Spitzer.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Spitzer.Models;
using Spitzer.Views;
using Spitzer.ViewModels;

namespace Spitzer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        readonly ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            viewModel = new ItemsViewModel();
            
            if (viewModel.Items?.Count == 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }

            BindingContext = viewModel;
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs args)
        {
            var item = (args.CurrentSelection.FirstOrDefault() as MediaItem);
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

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