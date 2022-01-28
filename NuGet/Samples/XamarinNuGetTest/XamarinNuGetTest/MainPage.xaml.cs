using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//using Tealium.Droid;
//using Tealium.iOS;

using Tealium;

namespace XamarinNuGetTest
{
    public partial class MainPage : ContentPage
    {
        ITealiumInstanceManager instanceManager;


        public MainPage(ITealiumInstanceManager instanceManager)
        {
            this.instanceManager = instanceManager;
            InitializeComponent();
        }


        void TrackView(object sender, EventArgs e)
        {
            instanceManager.GetExistingInstance(TealiumConsts.INSTANCE_ID).TrackView("Main Page", new Dictionary<String, object>(3){
                {"event_category","Main Page"},
                {"event_action", "Button Click"},
                {"event_label", "Track View"}
            });
        }

        void TrackEvent(object sender, EventArgs e)
        {

            instanceManager.GetExistingInstance(TealiumConsts.INSTANCE_ID).TrackEvent("Button Clicked", new Dictionary<String, object>(3){
                {"event_category","Main Page"},
                {"event_action", "Button Click"},
                {"event_label", "Track Event"}
            });
        }

    }
}
