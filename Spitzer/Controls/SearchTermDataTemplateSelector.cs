using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Spitzer.Models;
using Spitzer.ViewModels;
using Xamarin.Forms;

namespace Spitzer.Controls
{
    public class SearchTermDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BasicTemplate { get; set; }
        public DataTemplate AdvancedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewModel = (ItemsViewModel)container.BindingContext;
            if (viewModel.IsFirstLoad)
            {
                return BasicTemplate;
            }
            Debug.WriteLine($"viewModel.Items.Count: {viewModel.Items.Count}");
            return viewModel.Items.Count == 0 ? AdvancedTemplate : BasicTemplate;
        }
    }
}
