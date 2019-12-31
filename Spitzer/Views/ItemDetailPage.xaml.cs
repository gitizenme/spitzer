using System;
using System.ComponentModel;
using Xamarin.Forms;
using Spitzer.Models;
using Spitzer.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
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

            var viewButton = "View";
            var shareImage = "Share - image";
            var shareWebLink = "Share - Web link";
            var action = await DisplayActionSheet("View or Share?", "Cancel", null, viewButton, shareImage, shareWebLink);

            Analytics.TrackEvent($"{action} for image: {item.ImageTitle}");

            if (action == viewButton)
            {
                await Navigation.PushAsync(new ItemImagePage(new ItemImageViewModel(item)));
            }
            else if (action == shareImage)
            {
                try
                {
                    var destDirectory = FileSystem.CacheDirectory + Path.DirectorySeparatorChar + "images";
                    var destFile = FileSystem.CacheDirectory + Path.DirectorySeparatorChar + "images" +
                                   Path.DirectorySeparatorChar + item.ImageTitle + "." +
                                   item.ImageInformation.Type.ToString().ToLower();
                    if (item.ImageInformation.FilePath != null)
                    {
                        if (!Directory.Exists(destDirectory))
                        {
                            Directory.CreateDirectory(destDirectory);
                        }
                        File.Copy(item.ImageInformation.FilePath, destFile, true);
                        if (File.Exists(destFile))
                        {
                            await Share.RequestAsync(new ShareFileRequest()
                            {
                                File = new ShareFile(destFile),
                                Title = item.ImageTitle
                            });                    
                        }
                        else
                        {
                            await UserDialogs.Instance.AlertAsync("There was a problem sharing the image, please try again later.", "Unable to Share", "Okay");
                        }
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync("There was a problem sharing the image, please try again later.", "Unable to Share", "Okay");
                    }
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    await UserDialogs.Instance.AlertAsync("There was a problem sharing the image, please try again later.", "Unable to Share", "Okay");
                }
            }
            else if (action == shareWebLink)
            {
                try
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Uri = item.ImagePreview.ToString(),
                        Title = item.ImageTitle
                    });
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    await UserDialogs.Instance.AlertAsync("There was a problem sharing the link, please try again later.", "Unable to Share", "Okay");
                }
            }

            // Manually deselect item.
            ItemDetailView.SelectedItem = null;
        }
    }
}