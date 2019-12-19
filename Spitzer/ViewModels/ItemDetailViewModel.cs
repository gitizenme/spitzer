using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms;
using Spitzer.Models;
using Spitzer.Models.ImageMetadata;
using Xamarin.Forms;

namespace Spitzer.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public IList<Uri> ImageUriList { get; }
        public IDictionary<string, StackLayout> ImageLayout { get; }
        public ImageMetaData Metadata { get; }
        public MediaItem Item { get; set; }
        public ItemDetailViewModel(MediaItem item = null)
        {
            ImageLayout = new Dictionary<string, StackLayout>();
            Title = item.Title;
            ImageUriList = item.Images;
            Metadata = item.MetaData;
            Item = item;
        }
    }

}
