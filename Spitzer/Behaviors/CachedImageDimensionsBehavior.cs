using System;
using System.Diagnostics;
using FFImageLoading.Forms;
using Spitzer.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Spitzer.Behaviors
{
    public class CachedImageDimensionsBehavior : Behavior<CachedImage>
    {
        protected override void OnAttachedTo (CachedImage image)
        {
            // image.Success += ImageOnSuccess;
            image.FileWriteFinished += ImageFileWriteFinished;
            base.OnAttachedTo (image);
        }

        protected override void OnDetachingFrom (CachedImage image)
        {
            // image.Success -= ImageOnSuccess;
            image.FileWriteFinished -= ImageFileWriteFinished;
            base.OnDetachingFrom (image);
        }

        private void ImageOnSuccess(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            if (sender is CachedImage cachedImage)
            {
                if (cachedImage.BindingContext is ItemImagePreviewViewModel itemImagePreview)
                {
                    // Console.WriteLine($"ImageDimensions : ({itemImagePreview.ImageInformation.OriginalHeight}x{itemImagePreview.ImageInformation.OriginalHeight})");
                    // itemImagePreview.ImageDimensions = $"({itemImagePreview.ImageInformation.OriginalHeight}x{itemImagePreview.ImageInformation.OriginalHeight})";
                }
            }
        }
        
        private void ImageFileWriteFinished(object sender, CachedImageEvents.FileWriteFinishedEventArgs e)
        {
            Console.WriteLine($"ImageFileWriteFinished : {e.FileWriteInfo.FilePath}");
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