using Tealium.iOS.NativeInterop.Extensions;
using Tealium.Platform.iOS;

namespace Tealium.iOS
{
    public class ConsentManagerIOS : ConsentManager
    {
        readonly ConsentManagerWrapper consentManager;

        public ConsentManagerIOS(ConsentManagerWrapper consentManager)
        {
            this.consentManager = consentManager;
        }

        public override bool IsConsentManagerEnabled => consentManager.ConsentManagerEnabled;

        public override ConsentPolicy? Policy
        {
            get => consentManager.Policy.ToPolicy();
        }

        public override ConsentStatus UserConsentStatus
        {
            get => consentManager.UserConsentStatus.ToStatus();
            set => consentManager.UserConsentStatus = value.ToNativeStatus();
        }

        public override ConsentCategory[] UserConsentCategories
        {
            get => consentManager.UserConsentCategories != null ? consentManager.UserConsentCategories.ToStatusArray() : NoCategories;
            set => consentManager.UserConsentCategories = value.ToNativeStatuses();
        }

        public override bool ConsentLoggingEnabled
        {
            get => consentManager.ConsentLoggingEnabled;
        }

        public override void ResetUserConsentPreferences()
        {
            consentManager.ResetUserConsentPreferences();
        }

        public override void UserConsentStatusWithCategories(ConsentStatus status, ConsentCategory[] categories)
        {
            consentManager.SetUserConsentStatus(status.ToNativeStatus(), categories.ToNativeStatuses());
        }

    }
}