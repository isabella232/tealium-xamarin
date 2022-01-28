using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tealium;
using TealiumXamarinExample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TealiumXamarinExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Teal.Helper.DefaultInstance.Track(new TealiumView("Login"));
        }
    }
}
