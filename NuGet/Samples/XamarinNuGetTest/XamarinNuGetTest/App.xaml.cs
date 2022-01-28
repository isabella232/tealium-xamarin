using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using System.Collections.Generic;
using Tealium;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinNuGetTest
{
    public partial class App : Application
    {
        AppConfig config;

        public App(AppConfig conf)
        {
            this.config = conf;
            InitializeComponent();

            MainPage = new MainPage(conf.instanceManager);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
