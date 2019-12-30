namespace Spitzer.ViewModels
{
    public class ItemImageViewModel : BaseViewModel
    {
        public ItemImagePreviewViewModel Item { get; }

        public ItemImageViewModel(ItemImagePreviewViewModel item)
        {
            Item = item;
        }
    }
}