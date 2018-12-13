using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExampleAppConsentMgr
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = TealiumConsts.ConsentManagerModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TealiumConsts.DefaultInstance.TrackView(nameof(MainPage));
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            TealiumConsts.DefaultInstance.TrackEvent("Button Clicked");
            Navigation.PushAsync(new ConsentManagerPage());
        }
    }
}
