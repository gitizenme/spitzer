using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Spitzer.Models;

namespace Spitzer.Services
{
    public class NasaMediaLibraryDataStore : IMediaLibrary<MediaItem>
    {
        NasaMediaLibrary library;

        public NasaMediaLibraryDataStore()
        {
            var client = new RestClient("https://images-api.nasa.gov");
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest("search", Method.GET);
            request.AddParameter("q", "spitzer space telescope"); // adds to POST or URL querystring based on Method
            request.AddParameter("keywords", "spitzer space telescope"); // adds to POST or URL querystring based on Method

            // return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            var response2 = client.Execute<NasaMediaLibrary>(request);
            library = response2.Data;
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
