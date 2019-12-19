using System;
using System.Collections.ObjectModel;
using Spitzer.Models;
using Xamarin.Forms;

namespace Spitzer.Controls
{
    public class SearchTermDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasicTemplate { get; set; }
        public DataTemplate AdvancedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ObservableCollection<MediaItem> query = (ObservableCollection<MediaItem>)item;
            return query.Count == 0 ? AdvancedTemplate : BasicTemplate;
        }
    }
}
