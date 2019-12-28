namespace Spitzer.ViewModels
{
    public class ItemImageViewModel : BaseViewModel
    {
        public ItemImagePreview Item { get; }

        public ItemImageViewModel(ItemImagePreview item)
        {
            Item = item;
        }
    }
}