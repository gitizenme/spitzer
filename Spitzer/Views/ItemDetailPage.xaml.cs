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
                    Images.Children.Add(imageView);
                }
            //     else if (image.ToString().EndsWith(".json", StringComparison.Ordinal))
            //     {
            //         Images.Children.Add(divider);
            //         var metadata = viewModel.Metadata;
            //         if (metadata != null)
            //         {
            //             var kvp = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(metadata.ToJson());
            //             foreach(var item in kvp)
            //             {
            //                 Console.WriteLine($"Key: {item.Key} Value: {item.Value}");
            //                 Metadata.Children.Add(new Label {Text = $"{item.Key}: {item.Value}"});
            //             }
            //             // var formattedText = metadata.ToJson();
            //         }
            //     }
            }
        }
    }
}