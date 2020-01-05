using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spitzer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
            // TODO Use Font Awesome for tab bar icons...
            // MediaPage.IconImageSource = 
        }
    }
}