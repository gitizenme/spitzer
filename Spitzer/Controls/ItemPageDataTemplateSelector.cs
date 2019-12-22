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
            else if (item is ItemImagePreview)
            {
                return ImageLeftTitleResolutionTemplate;
            }
            
            return ItemHeadingTemplate;
        }

        private string UpdateImageDescription(object item)
        {
                                
            // imageView.Success += (sender, e) =>
            // {
            //     imageView.Success += (sender, e) =>
            //     {
            //         var imageDimensionsLabel = new Label
            //         {
            //             Text = $"({e.ImageInformation.OriginalHeight}x{e.ImageInformation.OriginalHeight})",
            //             VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start,
            //             Style = App.Current.Resources["SmallImageOverlayLabelStyle"] as Style
            //         };
            //         if (ImageLayout.TryGetValue(e.ImageInformation.Path, out var layout))
            //         {
            //             MainThread.BeginInvokeOnMainThread(() =>
            //             {
            //                 layout.Children.Add(imageDimensionsLabel);
            //                 layout.ForceLayout();
            //             });
            //         }
            //     };                    
            // };
            return "";
        }
    }
}