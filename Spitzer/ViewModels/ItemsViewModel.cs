using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Spitzer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Microsoft.AppCenter.Crashes;

namespace Spitzer.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        readonly IList<MediaItem> source;

        private bool isFirstLoad;
        private ObservableCollection<MediaItem> items;
        private IOrderedEnumerable<MediaItem> sortedCollection;

        public ItemsViewModel()
        {
            IsFirstLoad = true;
            Title = "Gallery";
            source = new List<MediaItem>();
            Items = new ObservableCollection<MediaItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ResetItemsCommand = new Command(ExecuteResetItemsCommand);
        }

        public ObservableCollection<MediaItem> Items
        {
            get => items;
            private set => SetProperty(ref items, value);
        }

        public ICommand LoadItemsCommand { get; }
        public ICommand ResetItemsCommand { get; }
        public ICommand FilterCommand => new Command<string>(FilterItems);

        public bool IsFirstLoad
        {
            get => isFirstLoad;
            private set => SetProperty(ref isFirstLoad, value);
        }

        private void ExecuteResetItemsCommand()
        {
            Items = new ObservableCollection<MediaItem>(source);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await NasaMediaLibrary.GetItemsAsync(true);
                foreach (var item in items)
                {
                    source.Add(item);
                }

                sortedCollection = source.GroupBy(x => x.Title)
                    .Select(g => g.First()).OrderByDescending(o => o.DateSort);                
                
                Items = new ObservableCollection<MediaItem>(sortedCollection);

                Debug.WriteLine($"Items.Count: {Items.Count}");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
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
                var filteredItems = sortedCollection.Where(item => item.Title.ToLower().Contains(filter.ToLower())).ToList();
                Items = new ObservableCollection<MediaItem>(filteredItems);
            }
            else
            {
                Items = new ObservableCollection<MediaItem>(sortedCollection);
            }
        }
    }
}