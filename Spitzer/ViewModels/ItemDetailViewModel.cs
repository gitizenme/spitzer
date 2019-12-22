using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FFImageLoading.Forms;
using Spitzer.Annotations;
using Spitzer.Models;
using Spitzer.Models.ImageMetadata;
using Xamarin.Essentials;
using Xamarin.Forms;

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

                    Detail.Add(new ItemImagePreview
                    {
                        ImagePreview = imageUri, Title = labelText, Description = "..."
                    });
                }
            }
        }
    }
}

public interface IItemDetailEntry
{
    string Title { get; set; }
    string Description { get; set; }
}

public class ItemDetailHeader : IItemDetailEntry
{
    public string Title { get; set; }
    public string Description { get; set; }
}

internal class ItemImagePreview : INotifyPropertyChanged
{
    private string description;
    public Uri ImagePreview { get; set; }
    public string Title { get; set; }

    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName]string propertyName = "",
        Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        var changed = PropertyChanged;
        if (changed == null)
            return;

        changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion    
}

internal class ItemMetadata  : IItemDetailEntry
{
    public string Title { get; set; }
    public string Description { get; set; }

}