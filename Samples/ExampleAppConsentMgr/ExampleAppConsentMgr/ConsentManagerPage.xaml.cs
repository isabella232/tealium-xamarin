using System;
using System.Collections.Generic;

using Tealium;

using Xamarin.Forms;

namespace ExampleAppConsentMgr
{
    public partial class ConsentManagerPage : ContentPage
    {
        ConsentManagerModel consent;

        public ConsentManagerPage()
        {
            InitializeComponent();

            consent = TealiumConsts.ConsentManagerModel;

            this.BindingContext = consent;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            TealiumConsts.DefaultInstance.TrackView(nameof(ConsentManagerPage));
        }

        void ToggleConsent_Clicked(object sender, System.EventArgs e)
        {
            switch(consent.SelectedStatus){
                case ConsentManager.ConsentStatus.NotConsented:
                case ConsentManager.ConsentStatus.Unknown:
                    consent.SelectedStatus = ConsentManager.ConsentStatus.Consented;
                    break;
                case ConsentManager.ConsentStatus.Consented:
                    consent.SelectedStatus = ConsentManager.ConsentStatus.NotConsented;
                    break;
                default:
                    break;
            }
        }

        void ResetConsent_Clicked(object sender, System.EventArgs e)
        {
            consent.Reset();
        }

        void SaveConsent_Clicked(object sender, System.EventArgs e)
        {
            consent.Save();
        }
    }
}
