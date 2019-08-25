using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spitzer.Services
{
    public interface IMediaLibrary<T>
    {
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
