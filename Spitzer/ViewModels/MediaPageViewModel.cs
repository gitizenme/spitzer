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
            Title = "Media";

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
                var rssContent = await GetContentForFeed("https://www.jpl.nasa.gov/multimedia/rss/news.xml", "JPL");
                source = rssContent == null ? new List<RssSchema>() : new List<RssSchema>(rssContent);

                rssContent = await GetContentForFeed("https://www.nasa.gov/rss/dyn/solar_system.rss", "NASA");
                source.AddRange(rssContent);

                rssContent = await GetContentForFeed("http://www.spitzer.caltech.edu/news_category/12-Home-Page-Features?format=xml", "Spitzer");
                source.AddRange(rssContent);

                var uniqueContent = source.GroupBy(x => x.Title)
                    .Select(g => g.First()).OrderByDescending(o=>o.PublishDate);                
                
                Items = new ObservableCollection<RssSchema>(uniqueContent.ToList());
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

        private async Task<IEnumerable<RssSchema>> GetContentForFeed(string uri, string author)
        {
            var parser = new RssParser();
            string feedContent;
            IEnumerable<RssSchema> rssSchemata = null;

            using (var client = new HttpClient())
            {
                feedContent = await client.GetStringAsync(uri);
            }

            if (feedContent != null)
            {
                rssSchemata = parser.Parse(feedContent);
                foreach (var schema in rssSchemata)
                {
                    schema.Author = author;
                }
            }

            return rssSchemata;
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