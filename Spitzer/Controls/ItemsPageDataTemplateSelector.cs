using System;
using System.Collections.ObjectModel;
using Spitzer.Models;
using Xamarin.Forms;

namespace Spitzer.Controls
{
    public class ItemsPageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextOnlyTemplate { get; set; }
        public DataTemplate ImageLeftTemplate { get; set; }
        public DataTemplate ImageRightTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var mediaItem = (MediaItem)item;
            var collectionView = (CollectionView)container;
            if(mediaItem.ImagePreview != null)
            {
                var items = (ObservableCollection<MediaItem>)collectionView.ItemsSource;
                if(items.IndexOf(mediaItem) % 2 == 0)
                {
                    return ImageRightTemplate;
                }
                return ImageLeftTemplate;
            }
            return TextOnlyTemplate;
        }
    }
}
