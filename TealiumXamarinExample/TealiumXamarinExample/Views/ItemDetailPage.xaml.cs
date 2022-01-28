using System.ComponentModel;
using System.Collections.Generic;
using Xamarin.Forms;
using TealiumXamarinExample.ViewModels;
using Tealium;

namespace TealiumXamarinExample.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var itemDetailViewModel = (ItemDetailViewModel)BindingContext;
            var itemId = itemDetailViewModel.ItemId;
            var text = itemDetailViewModel.Text;
            var description = itemDetailViewModel.Description;

            Teal.Helper.DefaultInstance.Track(new TealiumView("ItemDetails", new Dictionary<string, object>
            {
                { "item_id", itemId },
                { "item_text", text },
                { "item_description", description },
                { "map", new Dictionary<string, object> {
                    { "string", "data" },
                    { "int_array", new int[] { 1, 2, 3 } }
                } }
            }));

            Teal.Helper.DefaultInstance.AddToDataLayer(new Dictionary<string, object>
            {
                { "last_item_viewed_id", itemId },
                { "last_item_viewed_text", text },
                { "last_item_viewed_description", description }
            }, Expiry.Session);
        }
    }
}
