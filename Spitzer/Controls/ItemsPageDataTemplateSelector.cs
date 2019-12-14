using System;
using Xamarin.Forms;

namespace Spitzer.Controls
{
    public class ItemsPageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextOnlyTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return TextOnlyTemplate;
        }
    }
}
