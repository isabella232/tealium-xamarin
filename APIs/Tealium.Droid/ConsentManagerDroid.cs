using System;
using System.Linq;
using NativeConsent = Com.Tealium.Core.Consent;
using Tealium.Droid.NativeInterop.Extensions;

namespace Tealium.Droid
{
    public class ConsentManagerDroid : ConsentManager
    {
        readonly Com.Tealium.Core.Tealium nativeTealium;

        public ConsentManagerDroid(Com.Tealium.Core.Tealium tealium)
        {
            this.nativeTealium = tealium ?? throw new ArgumentNullException(nameof(tealium));
        }

        public override bool IsConsentManagerEnabled => nativeTealium.ConsentManager.Enabled;

        public override ConsentStatus UserConsentStatus
        {
            get => nativeTealium.ConsentManager?.UserConsentStatus?.ToStatus() ?? ConsentStatus.Unknown;
            set
            {
                if (IsConsentManagerEnabled && value != UserConsentStatus)
                {
                    this.nativeTealium.ConsentManager.UserConsentStatus = value.ToNativeStatus();
                }
            }
        }

        public override ConsentPolicy? Policy
        {
            get => nativeTealium.ConsentManager?.Policy.ToConsentPolicy();
        }

        public override ConsentCategory[] UserConsentCategories
        {
            get => nativeTealium.ConsentManager?.UserConsentCategories?
               .Select(nativeCat => nativeCat.ToCategory())
               .OfType<ConsentCategory>()
               .ToArray() ?? NoCategories;

            set => nativeTealium.ConsentManager.UserConsentCategories = value.Select(cat => cat.ToNativeCategory()).OfType<NativeConsent.ConsentCategory>().ToArray();
        }

        public override bool ConsentLoggingEnabled
        {
            get => nativeTealium.ConsentManager != null && nativeTealium.ConsentManager.ConsentLoggingEnabled;
        }

        public override void ResetUserConsentPreferences()
        {
            nativeTealium.ConsentManager?.Reset();
        }

        public override void UserConsentStatusWithCategories(ConsentStatus status, ConsentCategory[] categories)
        {
            nativeTealium.ConsentManager.UserConsentStatus = status.ToNativeStatus();
            nativeTealium.ConsentManager.UserConsentCategories = categories.Select(category => category.ToNativeCategory()).ToArray();
        }
    }
}
