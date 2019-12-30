using System.ComponentModel;
using Xamarin.Forms;
using Spitzer.Models;
using Spitzer.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Essentials;

namespace Spitzer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        readonly ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;

            BackgroundPreviewImage.Source = viewModel.Item.ImagePreview;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new MediaItem();

            item.Data = new List<Datum>(1);
            item.Data.Add(new Datum
            {
                Title = "No Images Found",
                Description = "Please try again later..."
            });

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs args)
        {
            var item = (args.CurrentSelection.FirstOrDefault() as ItemImagePreviewViewModel);
            if (item == null)
            {
                ItemDetailView.SelectedItem = null;
                return;
            }

            var action = await DisplayActionSheet("View, Save or Share?", "Cancel", null, "View", "Save", "Share");

            if (action == "View")
            {
                await Navigation.PushAsync(new ItemImagePage(new ItemImageViewModel(item)));
            }
            else if (action == "Save")
            {
            }
            else if (action == "Share")
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = item.ImagePreview.ToString(),
                    Title = item.ImageTitle
                });
            }

            // Manually deselect item.
            ItemDetailView.SelectedItem = null;
        }
    }
}