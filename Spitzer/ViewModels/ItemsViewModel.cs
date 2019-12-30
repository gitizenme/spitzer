using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Spitzer.Models;
using Spitzer.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Spitzer.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        readonly IList<MediaItem> source;

        public ObservableCollection<MediaItem> Items { get; private set; }
        public ICommand LoadItemsCommand { get; }
        public ICommand FilterCommand => new Command<string>(FilterItems);

        private bool isFirstLoad;

        public bool IsFirstLoad
        {
            get => isFirstLoad;
            private set => SetProperty(ref isFirstLoad, value);
        }

        public ItemsViewModel()
        {
            IsFirstLoad = true;
            Title = "Spitzer Gallery";
            source = new List<MediaItem>();
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
                    source.Add(item);
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
                if (IsFirstLoad)
                {
                    IsFirstLoad = false;
                }
            }
        }

        void FilterItems(string filter)
        {
            if (!String.IsNullOrEmpty(filter))
            {
                Debug.WriteLine($"filter = {filter}");
                var filteredItems = source.Where(item => item.Title.ToLower().Contains(filter.ToLower())).ToList();
                foreach (var item in source)
                {
                    if (!filteredItems.Contains(item))
                    {
                        Items.Remove(item);
                    }
                    else
                    {
                        if (!Items.Contains(item))
                        {
                            Items.Add(item);
                        }
                    }
                }
            }
            else
            {
                Items.Clear();
                foreach (var item in source)
                {
                    Items.Add(item);
                }
            }
        }
    }
}