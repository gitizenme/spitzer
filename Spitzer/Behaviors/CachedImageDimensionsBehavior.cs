using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Spitzer.Behaviors
{
    public class CachedImageDimensionsBehavior : Behavior<CachedImage>
    {
        protected override void OnAttachedTo (CachedImage image)
        {
            image.Success += ImageOnSuccess;
            base.OnAttachedTo (image);
            // Perform setup
        }

        protected override void OnDetachingFrom (CachedImage image)
        {
            image.Success -= ImageOnSuccess;
            base.OnDetachingFrom (image);
            // Perform clean up
        }

        private void ImageOnSuccess(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            if (sender is CachedImage cachedImage)
            {
                if (cachedImage.BindingContext is ItemImagePreview itemImagePreview)
                {
                    itemImagePreview.Description = $"({e.ImageInformation.OriginalHeight}x{e.ImageInformation.OriginalHeight})";
                }
            }
        }
    }
}