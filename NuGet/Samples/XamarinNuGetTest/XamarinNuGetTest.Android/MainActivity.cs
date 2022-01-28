using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;



namespace XamarinNuGetTest.Droid
{
    [Activity(Label = "XamarinNuGetTest", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            TealiumHelper.GetTealium(this.Application).TrackView("App Launched");

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AppConfig(TealiumHelper.instanceManager)));
        }
    }
}

