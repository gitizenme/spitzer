using System;
using System.Collections.Generic;
using System.Linq;
using Spitzer.Models;

namespace Spitzer.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public IList<object> Detail { get; }
        public MediaItem Item { get; }

        public ItemDetailViewModel(MediaItem item = null)
        {
            Item = item;
            Detail = new List<object>();

            Detail.Add(new ItemDetailHeader {Title = Item.Title, Description = Item.Description});
            AddImages();
            AddMetaData();
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
                            Detail.Add(new ItemMetadata {Title = prop.Name, Description = values});
                        }
                    }
                    else
                    {
                        var description = $"{prop.GetValue(metadata, null)}";
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

                    Detail.Add(new ItemImagePreviewViewModel
                    {
                        ImagePreview = imageUri, ImageDimensions = "(calculating dimensions)", ImageSize = labelText,
                        ImageTitle = Item.Title, ImageDescription = Item.Description
                    });
                }
            }
        }
    }
}