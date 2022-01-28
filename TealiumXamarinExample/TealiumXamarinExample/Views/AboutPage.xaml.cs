using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Tealium;

namespace TealiumXamarinExample.Views
{
    public partial class AboutPage : ContentPage
    {
        ITealium Tealium;
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Teal.Helper.DefaultInstance.Track(new TealiumView("About"));
        }
    }
}
