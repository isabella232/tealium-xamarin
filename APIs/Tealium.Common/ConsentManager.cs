using System;
namespace Tealium
{
    public abstract class ConsentManager
    {
        #region Abstracts

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Tealium.ConsentManager"/> is consent manager enabled.
        /// </summary>
        /// <value><c>true</c> if is consent manager enabled; otherwise, <c>false</c>.</value>
        public abstract bool IsConsentManagerEnabled { get; }

        /// <summary>
        /// Gets or sets the policy - default is GDPR and will be sent on every event.
        /// </summary>
        /// <value>The policy.</value>
        public abstract ConsentPolicy? Policy { get; }

        /// <summary>
        /// Gets or sets the user consent status - <see cref="ConsentStatus"/> 
        /// </summary>
        /// <value>The user consent status.</value>
        public abstract ConsentStatus UserConsentStatus { get; set; }

        /// <summary>
        /// Gets or sets the user consent categories - <see cref="ConsentCategory"/>
        /// </summary>
        /// <value>The user consent categories.</value>
        public abstract ConsentCategory[] UserConsentCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Tealium.ConsentManager"/> consent logging enabled.
        /// </summary>
        /// <value><c>true</c> if consent logging enabled; otherwise, <c>false</c>.</value>
        public abstract bool ConsentLoggingEnabled { get; }


        //Methods
        /// <summary>
        /// Sets the User's the consent status with a selection of categories as well.
        /// </summary>
        /// <param name="status">Status.</param>
        /// <param name="categories">Categories.</param>
        public abstract void UserConsentStatusWithCategories(ConsentStatus status, ConsentCategory[] categories);

        /// <summary>
        /// Resets the user consent preferences back to an Unknown status.
        /// </summary>
        public abstract void ResetUserConsentPreferences();

        #endregion Abstracts

        #region Public Constants

        /// <summary>
        /// Value to represent the current consent status
        /// </summary>
        public enum ConsentStatus
        {
            Unknown,
            Consented,
            NotConsented,
        }

        /// <summary>
        /// Value to represent a consent category 
        /// </summary>
        public enum ConsentCategory
        {
            Affiliates,
            Analytics,
            BigData,
            CDP,
            CookieMatch,
            CRM,
            DisplayAds,
            Email,
            Engagement,
            Mobile,
            Monitoring,
            Personalization,
            Search,
            Social,
            Misc
        }

        /// <summary>
        /// Helper array containing all available consent categories
        /// </summary>
        public static readonly ConsentCategory[] AllCategories = new ConsentCategory[] {
            ConsentCategory.Affiliates,
            ConsentCategory.Analytics,
            ConsentCategory.BigData,
            ConsentCategory.CDP,
            ConsentCategory.CookieMatch,
            ConsentCategory.CRM,
            ConsentCategory.DisplayAds,
            ConsentCategory.Email,
            ConsentCategory.Engagement,
            ConsentCategory.Misc,
            ConsentCategory.Mobile,
            ConsentCategory.Monitoring,
            ConsentCategory.Personalization,
            ConsentCategory.Search,
            ConsentCategory.Social
        };

        /// <summary>
        /// Helper array containing zero consent categories.
        /// </summary>
        public static readonly ConsentCategory[] NoCategories = new ConsentCategory[0];

        /// <summary>
        /// Value representing the consent policy
        /// </summary>
        public enum ConsentPolicy
        {
            GDPR, CCPA
        }

        /// <summary>
        /// Describes the expiration time to use for Consent, in the form of
        /// a value and its unit of time.
        /// </summary>
        public sealed class ConsentExpiry
        {
            public readonly int Time;
            public readonly TimeUnit TimeUnit;
            public ConsentExpiry(int time, TimeUnit unit)
            {
                Time = time;
                TimeUnit = unit;
            }
        }

        #endregion Public Constants
    }
}
