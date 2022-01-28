using System;
using System.Collections.Generic;
using TealiumXamarinExample.ViewModels;
using TealiumXamarinExample.Views;
using Xamarin.Forms;

namespace TealiumXamarinExample
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
