using System;
using System.Linq;

namespace Spitzer.Models
{
    public partial class MediaItem
    {
        public string Title => Data.Select(d => d.Title).FirstOrDefault() ?? string.Empty;
        public string DateCreated => Data.Select(d => d.DateCreated).FirstOrDefault().ToLocalTime().ToString() ?? string.Empty;
        public string Description => Data.Select(d => d.Description).FirstOrDefault() ?? string.Empty;
        public string Description508 => Data.Select(d => d.Description508).FirstOrDefault() ?? string.Empty;
    }
}
