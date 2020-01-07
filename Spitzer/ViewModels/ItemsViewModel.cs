// MIT License
//
// Copyright (c) [2020] [Joe Chavez]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Spitzer.Models.NasaMedia;
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
                var filteredItems = sortedCollection.Where(item => item.Title.ToLower().Contains(filter.ToLower()) || item.Description.ToLower().Contains(filter.ToLower()) || item.DateCreated.ToLower().Contains(filter.ToLower())).ToList();
                Items = new ObservableCollection<MediaItem>(filteredItems);
            }
            else
            {
                Items = new ObservableCollection<MediaItem>(sortedCollection);
            }
        }
    }
}