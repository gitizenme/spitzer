using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Spitzer.Models;
using Spitzer.ViewModels;
using System.Collections.Generic;
using Spitzer.Models.ImageMetadata;

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

            AddImagesAndMetatdata();
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

        private void AddImagesAndMetatdata()
        {
            var divider = new BoxView {BackgroundColor = Color.Black, HeightRequest = 3};

            foreach (var image in viewModel.Images)
            {
                if (image.ToString().EndsWith(".jpg", StringComparison.Ordinal))
                {
                    Images.Children.Add(divider);
                    var imageView = new Image
                        {Source = image, HeightRequest = 50, WidthRequest = 50, HorizontalOptions = LayoutOptions.Start};

                    var imageWithLabelLayout = new StackLayout {Orientation = StackOrientation.Horizontal};
                    var labelText = "Default";
                    if (image.PathAndQuery.Contains("~orig"))
                    {
                        labelText = $"Original Resolution";
                    }
                    else if (image.PathAndQuery.Contains("~large"))
                    {
                        labelText = "Large";
                    }
                    else if (image.PathAndQuery.Contains("~medium"))
                    {
                        labelText = "Medium";
                    }
                    else if (image.PathAndQuery.Contains("~small"))
                    {
                        labelText = "Small";
                    }
                    else if (image.PathAndQuery.Contains("~thumb"))
                    {
                        labelText = "Thumbnail";
                    }

                    var imageLabel = new Label {Text = $"{labelText} ({imageView.Width}x{imageView.Height})"};
                    imageWithLabelLayout.Children.Add(imageView);
                    imageWithLabelLayout.Children.Add(imageLabel);
                    
                    Images.Children.Add(imageWithLabelLayout);
                }
            }
            var metadata = viewModel.Metadata;
            if (metadata != null)
            {
                Images.Children.Add(divider);
                foreach(var prop in metadata.GetType().GetProperties()) {
                    var metadataLine = $"{prop.Name}: {prop.GetValue(metadata, null)}";
                    Console.WriteLine(metadataLine);
                    Metadata.Children.Add(new Label {Text = metadataLine});
                }
            }
        }
    }
}