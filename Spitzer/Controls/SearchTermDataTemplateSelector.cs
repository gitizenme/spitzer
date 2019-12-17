using System;
using Xamarin.Forms;

namespace Spitzer.Controls
{
    public class SearchTermDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasicTemplate { get; set; }
        public DataTemplate AdvancedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            string query = (string)item;
            return query.ToLower().Equals("spitzer") ? AdvancedTemplate : BasicTemplate;
        }
    }
}
