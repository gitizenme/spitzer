using System.Collections.ObjectModel;
using Spitzer.Models;
using Spitzer.ViewModels;
using Xamarin.Forms;

namespace Spitzer.Controls
{
    public class ItemPageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemHeadingTemplate { get; set; }
        public DataTemplate ImageLeftTitleResolutionTemplate { get; set; }
        public DataTemplate ImageMetadataItemTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ItemDetailHeader)
            {
                return ItemHeadingTemplate;
            }
            else if (item is ItemMetadata)
            {
                return ImageMetadataItemTemplate;
            }
            else if (item is ItemImagePreviewViewModel)
            {
                return ImageLeftTitleResolutionTemplate;
            }
            
            return ItemHeadingTemplate;
        }
    }
}