using System.ComponentModel;
using Xamarin.Forms;
using Spitzer.Models;
using Spitzer.ViewModels;
using System.Collections.Generic;

namespace Spitzer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

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

        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (BindingContext is ItemDetailViewModel viewModel)
            {
                var itemImagePreview = e.CurrentSelection[0] as ItemImagePreview;
                // TODO load the image ... 
            }
        }
    }
}