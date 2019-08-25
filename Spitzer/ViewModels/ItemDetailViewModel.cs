using System;
using System.Linq;
using Spitzer.Models;

namespace Spitzer.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public MediaItem Item { get; set; }
        public ItemDetailViewModel(MediaItem item = null)
        {
            Title = item.Title;
            Item = item;
        }
    }
}
