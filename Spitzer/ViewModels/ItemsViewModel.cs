using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Spitzer.Models;
using Spitzer.Views;
using System.Collections.Generic;
using System.Linq;

namespace Spitzer.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<MediaItem> Items { get; private set; }
        public Command LoadItemsCommand { get; }

        public ItemsViewModel()
        {
            Title = "Spitzer Gallery";
            Items = new ObservableCollection<MediaItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await NasaMediaLibrary.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                Debug.WriteLine($"Items.Count: {Items.Count}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}