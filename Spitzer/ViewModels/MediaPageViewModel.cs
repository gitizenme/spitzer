using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Parsers.Rss;
using Spitzer.Models;
using Xamarin.Forms;

namespace Spitzer.ViewModels
{
    public class MediaPageViewModel : BaseViewModel
    {
        private ObservableCollection<RssSchema> items;

        public MediaPageViewModel()
        {
            Items = new ObservableCollection<RssSchema>();
         
            LoadFeedCommand = new Command(async () => await ExecuteLoadFeedCommand());
            ResetItemsCommand = new Command(ExecuteResetItemsCommand);

        }

        private List<RssSchema> source { get; set; }

        public ICommand LoadFeedCommand { get; }
        public ICommand ResetItemsCommand { get; }
        public ICommand FilterCommand => new Command<string>(FilterItems);

        public ObservableCollection<RssSchema> Items
        {
            get => items;
            private set => SetProperty(ref items, value);
        }

        private void ExecuteResetItemsCommand()
        {
            Items = new ObservableCollection<RssSchema>(source.OrderByDescending(o=>o.PublishDate).ToList());
        }
        private async Task ExecuteLoadFeedCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                string feedContent;
                using (var client = new HttpClient())
                {
                    feedContent = await client.GetStringAsync("https://www.jpl.nasa.gov/multimedia/rss/news.xml");
                }

                if (feedContent == null)
                {
                    source = new List<RssSchema>();
                }

                var parser = new RssParser();
                var rssJPL = parser.Parse(feedContent);
                foreach (var schema in rssJPL)
                {
                    schema.Author = "JPL";
                }
                source = new List<RssSchema>(rssJPL);
                
                using (var client = new HttpClient())
                {
                    feedContent = await client.GetStringAsync("https://www.nasa.gov/rss/dyn/solar_system.rss");
                }

                if (feedContent != null)
                {
                    var nasaRss = parser.Parse(feedContent);
                    foreach (var schema in nasaRss)
                    {
                        schema.Author = "NASA";
                    }
                    source.AddRange(nasaRss);
                }

                Items = new ObservableCollection<RssSchema>(source.OrderByDescending(o=>o.PublishDate).ToList());
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
            
        }
        
        void FilterItems(string filter)
        {
            if (!String.IsNullOrEmpty(filter))
            {
                var filteredItems = source.Where(item => item.Title.ToLower().Contains(filter.ToLower())).ToList();
                Items = new ObservableCollection<RssSchema>(filteredItems.OrderByDescending(o=>o.PublishDate).ToList());
            }
            else
            {
                Items = new ObservableCollection<RssSchema>(source.OrderByDescending(o=>o.PublishDate).ToList());
            }
        }        
    }
}