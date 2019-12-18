using System;
using System.Collections.Generic;
using System.Linq;
using Spitzer.Models;
using Spitzer.Models.ImageMetadata;

namespace Spitzer.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public IList<Uri> Images { get; }
        public ImageMetaData Metadata { get; }
        public MediaItem Item { get; set; }
        public ItemDetailViewModel(MediaItem item = null)
        {
            Title = item.Title;
            Images = item.Images;
            Metadata = item.MetaData;
            Item = item;
        }
    }
}
