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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using RestSharp;
using Spitzer.Models.NasaMedia;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Spitzer.Interfaces
{
    public class NasaMediaLibraryDataStore : IMediaLibrary<MediaItem>
    {
        NasaMediaLibrary library;

        public NasaMediaLibraryDataStore()
        {
        }

        public void LoadDataStore(bool forceRefresh = false)
        {
            Debug.WriteLine($"forceRefresh: {forceRefresh}");

            // https://images-api.nasa.gov/search?q=spitzer space telescope&keywords=spitzer space telescope&page=1
            var client = new RestClient("https://images-api.nasa.gov");

            var request = new RestRequest("search", Method.GET);
            request.AddParameter("q", "spitzer space telescope"); // adds to POST or URL querystring based on Method
            request.AddParameter("keywords",
                "spitzer space telescope"); // adds to POST or URL querystring based on Method

            var searchUri = client.BaseUrl + request.Resource + "?" + string.Join("&", request.Parameters);

            var networkAccess = Connectivity.NetworkAccess;

            if (networkAccess != NetworkAccess.Internet)
            {
                Debug.WriteLine("Offline: loading pages from cache");
                library = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                if (library != null)
                {
                    GetPagesFromCache(client, request);
                    return;
                }
            }

            if (!forceRefresh && !Barrel.Current.IsExpired(key: searchUri))
            {
                Debug.WriteLine("Loading pages from cache");
                library = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                if (library != null)
                {
                    request.AddParameter("page", 0);
                    GetPagesFromCache(client, request);
                }
            }
            else
            {
                Debug.WriteLine("Loading pages from API");
                var libraryQueryResponse = client.Execute<NasaMediaLibrary>(request);
                library = libraryQueryResponse.Data;
                Barrel.Current.Add(key: searchUri, data: libraryQueryResponse.Content, expireIn: TimeSpan.FromDays(1));
                Debug.WriteLine(
                    $"There is more data, TotalHits: {library.Collection.Metadata.TotalHits}, Items.Count: {library.Collection.Items.Count}");
                var pages = library.Collection.Metadata.TotalHits / library.Collection.Items.Count;
                Debug.WriteLine($"pages: {pages}");
                request.AddParameter("page", 0);
                for (int page = 2; page <= pages; page++)
                {
                    GetPage(page, pages, request, client);
                }
            }

        }

        private void GetPagesFromCache(RestClient client, RestRequest request)
        {
            var pages = library.Collection.Metadata.TotalHits / library.Collection.Items.Count;
            for (int page = 2; page <= pages; page++)
            {
                Debug.WriteLine($"GetPagesFromCache Loading page: {page} of {pages}");
                request.Parameters[2].Value = page;
                var searchUri = client.BaseUrl + request.Resource + "?" + string.Join("&", request.Parameters);
                var pageItems = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                Debug.WriteLine($"GetPagesFromCache Adding {pageItems.Collection.Items.Count} to library, from URI: {searchUri}");
                library.Collection.Items.AddRange(pageItems.Collection.Items);
            }
        }

        private void GetPage(int page, long pages, RestRequest request, RestClient client)
        {
            Debug.WriteLine($"GetPage Loading page: {page} of {pages}");
            request.Parameters[2].Value = page;
            var searchUri = client.BaseUrl + request.Resource + "?" + string.Join("&", request.Parameters);
            var pageResponse = client.Execute<NasaMediaLibrary>(request);
            if (pageResponse.Data != null)
            {
                Debug.WriteLine($"GetPage Adding {pageResponse.Data.Collection.Items.Count} to library, from URI: {searchUri}");
                library.Collection.Items.AddRange(pageResponse.Data.Collection.Items);
                Barrel.Current.Empty(searchUri);
                Barrel.Current.Add(key: searchUri, data: pageResponse.Content, expireIn: TimeSpan.FromDays(1));
            }
        }

        public async Task<MediaItem> GetItemAsync(string id)
        {
            foreach (MediaItem item in library.Collection.Items)
            {
                if (item.Data.FirstOrDefault(d => d.NasaId == id) != null)
                {
                    return await Task.FromResult(item);
                }
            }

            return await Task.FromResult<MediaItem>(null);
        }

        public async Task<IEnumerable<MediaItem>> GetItemsAsync(bool forceRefresh = false)
        {
            if(forceRefresh)
            {
                LoadDataStore(forceRefresh);
            }

            if (library.Collection.Items != null)
            {
                return await Task.FromResult(library.Collection.Items.Select((MediaItem arg) => arg));
            }

            return await Task.FromResult<IEnumerable<MediaItem>>(null);
        }
    }
}