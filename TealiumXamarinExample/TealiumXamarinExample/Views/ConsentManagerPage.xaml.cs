using System;
using System.Collections.Generic;
using Tealium;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TealiumXamarinExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsentManagerPage : ContentPage
    {
        readonly ITealium tealium;

        public ConsentManagerPage()
        {
            this.tealium = Teal.Helper.DefaultInstance;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            tealium.Track(new TealiumView("Consent Manager Page"));
        }
    }
}
