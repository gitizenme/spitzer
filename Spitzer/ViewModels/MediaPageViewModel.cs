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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Parsers.Rss;
using Spitzer.Models.NasaMedia;
using Xamarin.Forms;

namespace Spitzer.ViewModels
{
    public class MediaPageViewModel : BaseViewModel
    {
        private ObservableCollection<RssSchema> items;
        private IOrderedEnumerable<RssSchema> uniqueContent;

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
            Items = new ObservableCollection<RssSchema>(source.OrderByDescending(o => o.PublishDate).ToList());
        }

        private async Task ExecuteLoadFeedCommand()
        {
            try
            {
                var rssContent = await GetContentForFeed("https://www.jpl.nasa.gov/multimedia/rss/news.xml", "JPL");
                source = rssContent == null ? new List<RssSchema>() : new List<RssSchema>(rssContent);

                rssContent = await GetContentForFeed("https://www.nasa.gov/rss/dyn/solar_system.rss", "NASA");
                source.AddRange(rssContent);

                rssContent =
                    await GetContentForFeed(
                        "http://www.spitzer.caltech.edu/news_category/12-Home-Page-Features?format=xml",
                        "Spitzer Home Page");
                source.AddRange(rssContent);

                rssContent =
                    await GetContentForFeed("http://www.spitzer.caltech.edu/resource_list/2-Featured-Images?format=xml",
                        "Spitzer Featured Images");
                source.AddRange(rssContent);

                rssContent =
                    await GetContentForFeed("http://www.spitzer.caltech.edu/news_category/NewsCategory?format=xml",
                        "Spitzer Recent News");
                source.AddRange(rssContent);

                rssContent = await GetContentForFeed("http://www.spitzer.caltech.edu/feeds/video_showcase.xml",
                    "Spitzer Video Showcase");
                source.AddRange(rssContent);

                uniqueContent = source.GroupBy(x => x.Title)
                    .Select(g => g.First()).OrderByDescending(o => o.PublishDate);

                Items = new ObservableCollection<RssSchema>(uniqueContent);
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
            IEnumerable<RssSchema> rssSchemata = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var parser = new RssParser();
                    var feedContent = await client.GetStringAsync(uri);
                    if (feedContent != null)
                    {
                        rssSchemata = parser.Parse(feedContent);
                        foreach (var schema in rssSchemata)
                        {
                            schema.Author = author;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string>
                {
                    { "URI" , uri },
                    { "Author", author }
                });
            }
            
            return rssSchemata;
        }

        void FilterItems(string filter)
        {
            if (!String.IsNullOrEmpty(filter))
            {
                var filteredItems = uniqueContent.Where(item =>
                    item.Title.ToLower().Contains(filter.ToLower()) ||
                    item.Author.ToLower().Contains(filter.ToLower()) ||
                    item.Content.ToLower().Contains(filter.ToLower()) || item.PublishDate
                        .ToString(CultureInfo.CurrentCulture).ToLower().Contains(filter.ToLower())).ToList();
                Items = new ObservableCollection<RssSchema>(filteredItems);
            }
            else
            {
                Items = new ObservableCollection<RssSchema>(uniqueContent);
            }
        }
    }
}