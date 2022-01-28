using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TealiumXamarinExample.Models;
using TealiumXamarinExample.ViewModels;

using Tealium;

namespace TealiumXamarinExample.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Teal.Helper.DefaultInstance.Track(new TealiumView("NewItem"));
        }
    }
}
