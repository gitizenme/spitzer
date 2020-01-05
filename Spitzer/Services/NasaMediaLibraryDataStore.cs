using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MonkeyCache.FileStore;
using RestSharp;
using Spitzer.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Spitzer.Services
{
    public class NasaMediaLibraryDataStore : IMediaLibrary<MediaItem>
    {
        NasaMediaLibrary library;

        public NasaMediaLibraryDataStore()
        {
            // https://images-api.nasa.gov/search?q=spitzer space telescope&keywords=spitzer space telescope&page=1
            var client = new RestClient("https://images-api.nasa.gov");

            var request = new RestRequest("search", Method.GET);
            request.AddParameter("q", "spitzer space telescope"); // adds to POST or URL querystring based on Method
            request.AddParameter("keywords", "spitzer space telescope"); // adds to POST or URL querystring based on Method

            var searchUri = client.BaseUrl.ToString() + request.Resource + "?" + string.Join("&", request.Parameters);
            
            var current = Connectivity.NetworkAccess;
            
            if (current != NetworkAccess.Internet)
            {
                library = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                if (library != null)
                {
                    GetPagesFromCache(client, request);
                    return;
                }
            }
            
            if(!Barrel.Current.IsExpired(key: request.Resource))
            {
                library = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                if (library != null)
                {
                    GetPagesFromCache(client, request);
                    return;
                }
            }
            else
            {
                library = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                if (library == null)
                {
                    // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
                    var libraryQueryResponse = client.Execute<NasaMediaLibrary>(request);
                    library = libraryQueryResponse.Data;
                    Barrel.Current.Add(key: searchUri, data: libraryQueryResponse.Content, expireIn: TimeSpan.FromDays(1));
                    GetPagesFromCache(client, request);
                }
            }

            if (library != null && library.Collection.Metadata.TotalHits > library.Collection.Items.Count)
            {
                Debug.WriteLine($"There is more data, TotalHits: {library.Collection.Metadata.TotalHits}, Items.Count: {library.Collection.Items.Count}");
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
                var searchUri = client.BaseUrl + request.Resource + "?" + string.Join("&", request.Parameters);
                var pageItems = Barrel.Current.Get<NasaMediaLibrary>(searchUri);
                library.Collection.Items.AddRange(pageItems.Collection.Items);
            }
        }

        private void GetPage(int page, long pages, RestRequest request, RestClient client)
        {
            Debug.WriteLine($"Loading page: {page} of {pages}");
            request.Parameters[2].Value = page;
            var searchUri = client.BaseUrl + request.Resource + "?" + string.Join("&", request.Parameters);
            var pageResponse = client.Execute<NasaMediaLibrary>(request);
            if (pageResponse.Data != null)
            {
                Debug.WriteLine($"Adding {pageResponse.Data.Collection.Items.Count} to library");
                library.Collection.Items.AddRange(pageResponse.Data.Collection.Items);
                Barrel.Current.Empty(searchUri);
                Barrel.Current.Add(key: searchUri, data: pageResponse.Content, expireIn: TimeSpan.FromDays(1));
            }
        }

        public async Task<MediaItem> GetItemAsync(string id)
        {

            foreach(MediaItem item in library.Collection.Items)
            {
                if(item.Data.FirstOrDefault(d => d.NasaId == id) != null)
                {
                    return await Task.FromResult(item);
                }
            }
            return await Task.FromResult<MediaItem>(null);
        }

        public async Task<IEnumerable<MediaItem>> GetItemsAsync(bool forceRefresh = false)
        {
            if (library.Collection.Items != null)
            {
                return await Task.FromResult(library.Collection.Items.Select((MediaItem arg) => arg));
            }
            return await Task.FromResult<IEnumerable<MediaItem>>(null);
        }
    }
}
