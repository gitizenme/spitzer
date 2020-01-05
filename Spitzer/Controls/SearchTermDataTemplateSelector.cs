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
            if (container.BindingContext is ItemsViewModel viewModel && viewModel.IsFirstLoad)
            {
                return BasicTemplate;
            }
            return AdvancedTemplate;
        }
    }
}
