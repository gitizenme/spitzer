using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spitzer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spitzer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemImagePage : ContentPage
    {
        private readonly ItemImageViewModel viewModel;

        public ItemImagePage(ItemImageViewModel itemImageViewModel)
        {
            InitializeComponent();
            
            BindingContext = viewModel = itemImageViewModel;
            
        }
    }
}