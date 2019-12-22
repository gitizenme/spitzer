using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
            // ItemsCollectionView.EmptyViewTemplate = Resources["BasicTemplate"] as DataTemplate;
        }

        async void OnItemSelected(object sender, SelectionChangedEventArgs args)
        {
            var item = (args.CurrentSelection.FirstOrDefault() as MediaItem);
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsCollectionView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
        }

        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.FilterCommand.Execute(e.NewTextValue);
            // if (viewModel.Items.Count == 0)
            // {
            //     ItemsCollectionView.EmptyViewTemplate = Resources["AdvancedTemplate"] as DataTemplate;
            // }
        }
    }
}