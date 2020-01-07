// MIT License
//
// Copyright (c) [2020] [Joe Chavez]
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using Acr.UserDialogs;
using FFImageLoading;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MonkeyCache.FileStore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Spitzer.Interfaces;
using Spitzer.Views;
using Spitzer.Styles;

namespace Spitzer
{
    public partial class App : Application
    {
        public static Theme CurrentTheme;

        public App()
        {
            Barrel.ApplicationId = "app.spitzer.mobile";
            
            InitializeComponent();
            SetTheme(Theme.Light);
            
            ToastConfig.DefaultBackgroundColor = System.Drawing.Color.Gray;
            ToastConfig.DefaultMessageTextColor = System.Drawing.Color.Navy;
            ToastConfig.DefaultActionTextColor = System.Drawing.Color.DarkRed;
            ToastConfig.DefaultDuration = new TimeSpan(0, 0, 5);
            ToastConfig.DefaultPosition = ToastPosition.Bottom;
            
            DependencyService.Register<NasaMediaLibraryDataStore>();
            MainPage = new MainPage();
            Analytics.TrackEvent($"Opening: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        protected override async void OnStart()
        {
            base.OnStart();

            Theme theme = await DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(theme);
            Analytics.TrackEvent($"Called: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        protected override void OnSleep()
        {
            Analytics.TrackEvent("OnSleep");
            ImageService.Instance.SetExitTasksEarly(true);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            ImageService.Instance.SetExitTasksEarly(false);

            CurrentTheme = await DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();

            SetTheme(CurrentTheme);
            Analytics.TrackEvent($"Called: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }

        void SetTheme(Theme theme)
        {
            if(MainPage != null)
            {
                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();

                    switch (theme)
                    {
                        case Theme.Dark:
                            mergedDictionaries.Add(new DarkTheme());
                            break;
                        case Theme.Light:
                        default:
                            mergedDictionaries.Add(new LightTheme());
                            break;
                    }
                }
            }
            Analytics.TrackEvent($"Called: {MethodBase.GetCurrentMethod().ReflectedType?.Name}.{MethodBase.GetCurrentMethod().Name}");
        }
    }
}
