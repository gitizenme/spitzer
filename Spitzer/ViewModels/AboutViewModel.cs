using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Spitzer.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenSpitzerAppWebsiteCommand = new Command(() => Launcher.TryOpenAsync("https://spitzer.app"));
            SendEMailCommand = new Command(o => SendEmail());
        }

        private async void SendEmail()
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Spitzer Gallery Mobile App Feedback",
                    Body = "All comments, questions and/or suggestions are welcome!",
                    To = new List<string>
                    {
                        "info@spitzer.app"
                    }
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {    
                Crashes.TrackError(fbsEx);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }            
        }

        public ICommand OpenSpitzerAppWebsiteCommand { get; }
        public ICommand SendEMailCommand { get; }

    }
}