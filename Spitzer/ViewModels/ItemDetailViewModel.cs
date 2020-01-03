using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using FFImageLoading;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using FFImageLoading.Work;
using Spitzer.Models;

namespace Spitzer.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private ObservableCollection<object> detail;
        
        public ObservableCollection<object> Detail
        {
            get => detail;
            set => SetProperty(ref detail, value);
        }

        public MediaItem Item { get; }

        public ItemDetailViewModel(MediaItem item = null)
        {
            Item = item;
            Detail = new ObservableCollection<object>();

            Detail.Add(new ItemDetailHeader {Title = Item.Title, Description = Item.Description});
            AddImages();
            // AddMetaData();
        }
        
        private void AddMetaData()
        {
            var metadata = Item.MetaData;
            if (metadata != null)
            {
                foreach (var prop in metadata.GetType().GetProperties())
                {
                    if (prop.PropertyType.IsArray)
                    {
                        if (prop.GetValue(metadata, null) is string[] data)
                        {
                            var values = string.Join(", ", data.Select(item => item));
                            Debug.WriteLine($"Name: {prop.Name} => {values}");
                            Detail.Add(new ItemMetadata {Title = prop.Name, Description = values});
                        }
                    }
                    else
                    {
                        var description = $"{prop.GetValue(metadata, null)}";
                        Debug.WriteLine($"Name: {prop.Name} => {description}");
                        Detail.Add(new ItemMetadata {Title = prop.Name, Description = description});
                    }
                }
            }
        }

        private void AddImages()
        {
            foreach (var imageUri in Item.Images)
            {
                if (imageUri.ToString().EndsWith(".jpg", StringComparison.Ordinal))
                {
                    var labelText = "Default";
                    if (imageUri.PathAndQuery.Contains("~orig"))
                    {
                        labelText = $"Original";
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

                    try
                    {
                        var imagePreviewViewModel = new ItemImagePreviewViewModel
                        {
                            ImagePreview = imageUri,
                            ImageSize = labelText,
                            ImageTitle = Item.Title,
                            ImageDescription = Item.Description
                        };
                        Detail.Add(imagePreviewViewModel);

                        CachedImage.InvalidateCache(imageUri.ToString(), CacheType.All, true);
                        ImageService.Instance.LoadUrl(imageUri.AbsoluteUri).Success((imageInformation, loadingResult) =>
                        {

                            Console.WriteLine($"Success Load Image, FilePath: {imageInformation.FilePath}, Key = {imageInformation.CacheKey}, result: {loadingResult}");
                            if (!(loadingResult == LoadingResult.NotFound || loadingResult == LoadingResult.InvalidTarget ||
                                  loadingResult == LoadingResult.Canceled || loadingResult == LoadingResult.Failed))
                            {
                                ItemImagePreviewViewModel modelForImagePreview = (ItemImagePreviewViewModel)Detail.FirstOrDefault(foundModel =>
                                {
                                    if (foundModel is ItemImagePreviewViewModel m)
                                    {
                                        if (m.ImagePreview.Equals(imageInformation.Path))
                                        {
                                            return ((ItemImagePreviewViewModel)foundModel).ImagePreview.Equals(imageInformation.Path);
                                        }
                                    }
                                    return false;
                                });
                                if (imagePreviewViewModel != null)
                                {
                                    modelForImagePreview.ImageDimensions =
                                        $"({imageInformation.OriginalWidth}x{imageInformation.OriginalHeight})";
                                    modelForImagePreview.ImageInformation = imageInformation;
                                }
                            }
                        }).Error(exception =>
                        {
                            Debug.WriteLine($"Error Load Image: {exception}");
                        }).Finish(work =>
                        {
                            Debug.WriteLine($"Finish Load Image, completed: {work.IsCompleted}");
                            Debug.WriteLine($"Finish Load Image, cancelled: {work.IsCancelled}");
                        }).FileWriteFinished(info =>
                        {
                            Debug.WriteLine($"info.FilePath: {info.FilePath}");
                            Debug.WriteLine($"info.SourcePath: {info.SourcePath}");
                            ItemImagePreviewViewModel modelForImagePreview = (ItemImagePreviewViewModel)Detail.FirstOrDefault(model =>
                            {
                                if (model is ItemImagePreviewViewModel m)
                                {
                                    if (m.ImagePreview.Equals(info.SourcePath))
                                    {
                                        return ((ItemImagePreviewViewModel) model).ImagePreview.Equals(info.SourcePath);
                                    }
                                }
                                return false;
                            });
                            if(modelForImagePreview != null)
                            {
                                modelForImagePreview.FileWriteInfo = info;
                            }
                        }).Preload();

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}