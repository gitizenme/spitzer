﻿// MIT License
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
using System.ComponentModel;
using Xamarin.Forms;
using Spitzer.Models.NasaMedia;
using Spitzer.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using FFImageLoading.Forms;

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
                    if (item.FileWriteInfo.FilePath != null)
                    {
                        if (!Directory.Exists(destDirectory))
                        {
                            Directory.CreateDirectory(destDirectory);
                        }
                        File.Copy(item.FileWriteInfo.FilePath, destFile, true);
                        if (File.Exists(destFile))
                        {
                            await Share.RequestAsync(new ShareFileRequest()
                            {
                                File = new ShareFile(destFile),
                                Title = item.ImageTitle,
                                PresentationSourceBounds = DeviceInfo.Platform== DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                                    ? new System.Drawing.Rectangle(0, 20, 0, 0)
                                    : System.Drawing.Rectangle.Empty
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
                        Title = item.ImageTitle,
                        PresentationSourceBounds = DeviceInfo.Platform== DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                            ? new System.Drawing.Rectangle(0, 20, 0, 0)
                            : System.Drawing.Rectangle.Empty
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

        private void CachedImage_OnFileWriteFinished(object sender, CachedImageEvents.FileWriteFinishedEventArgs e)
        {
            Console.WriteLine($"CachedImage_OnFileWriteFinished : {e.FileWriteInfo.FilePath}");
            if (sender is CachedImage cachedImage)
            {
                if (cachedImage.BindingContext is ItemImagePreviewViewModel itemImagePreview)
                {
                    itemImagePreview.FileWriteInfo = e.FileWriteInfo;
                }
            }
            
        }
    }
}