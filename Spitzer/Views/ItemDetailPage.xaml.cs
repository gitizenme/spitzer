using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Spitzer.Models;
using Spitzer.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using FFImageLoading.Mock;
using FFImageLoading.Work;
using Spitzer.Models.ImageMetadata;
using Xamarin.Essentials;

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

            foreach (var imageUri in viewModel.ImageUriList)
            {
                if (imageUri.ToString().EndsWith(".jpg", StringComparison.Ordinal))
                {
                    var labelText = "Default";
                    if (imageUri.PathAndQuery.Contains("~orig"))
                    {
                        labelText = $"Original Resolution";
                    }
                    else if (imageUri.PathAndQuery.Contains("~large"))
                    {
                        labelText = "Large";
                    }
                    else if (imageUri.PathAndQuery.Contains("~medium"))
                    {
                        labelText = "Medium";
                    }
                    else if (imageUri.PathAndQuery.Contains("~small"))
                    {
                        labelText = "Small";
                    }
                    else if (imageUri.PathAndQuery.Contains("~thumb"))
                    {
                        labelText = "Thumbnail";
                    }

                    var imageLabel = new Label
                    {
                        Text = $"{labelText}", VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    var imageView = new CachedImage
                    {
                        Source = imageUri, HeightRequest = 50, WidthRequest = 50,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Start
                    };

                    var imageWithLabelLayout = new StackLayout {Orientation = StackOrientation.Horizontal};
                    imageView.Success += (sender, e) =>
                    {
                        var imageDimensionsLabel = new Label
                        {
                            Text = $"({e.ImageInformation.OriginalHeight}x{e.ImageInformation.OriginalHeight})",
                            VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start
                        };
                        if (viewModel.ImageLayout.TryGetValue(e.ImageInformation.Path, out var layout))
                        {
                            MainThread.BeginInvokeOnMainThread ( () => {
                                layout.Children.Add(imageDimensionsLabel);
                                layout.ForceLayout();
                            });                            
                        }
                    };

                    viewModel.ImageLayout[imageUri.ToString()] = imageWithLabelLayout;

                    Images.Children.Add(divider);
                    imageWithLabelLayout.Children.Add(imageView);
                    imageWithLabelLayout.Children.Add(imageLabel);
                    Images.Children.Add(imageWithLabelLayout);

                }
            }
            var metadata = viewModel.Metadata;
            if (metadata != null)
            {
                Images.Children.Add(divider);
                foreach (var prop in metadata.GetType().GetProperties())
                {
                    var metadataLine = $"{prop.Name}: {prop.GetValue(metadata, null)}";
                    Debug.WriteLine(metadataLine);
                    Metadata.Children.Add(new Label {Text = metadataLine});
                }
            }
        }
    }
}