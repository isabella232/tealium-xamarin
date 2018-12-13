using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Tealium;

namespace ExampleApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        void TrackView(object sender, EventArgs e)
        {
            TealiumConsts.DefaultInstance.TrackView("Main Page");
        }

        void TrackEvent(object sender, EventArgs e)
        {
            TealiumConsts.DefaultInstance.TrackEvent("Button Clicked", new Dictionary<String, object>(3)
            {
                { "event_category", "Main Page" },
                { "event_action", "Button Click" },
                { "event_label", "Track Event" }
            });
        }

    }
}
