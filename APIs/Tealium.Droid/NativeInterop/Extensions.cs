using System;
using System.Linq;
using Android.App;
using static Tealium.ConsentManager;
using NativeConsent = Com.Tealium.Core.Consent;

#nullable enable
namespace Tealium.Droid.NativeInterop.Extensions
{
    /// <summary>
    /// Utility conversions for most Tealium CLR types into their Android SDK
    /// native equivalent, and vice-versa.
    /// </summary>
    public static class ConfigExtensions
    {
        /// <summary>
        /// Transforms a <see cref="TealiumConfig"/> into it's equivalent
        /// <see cref="Com.Tealium.Core.TealiumConfig"/> object based on the
        /// input config.
        /// </summary>
        /// <param name="config">The CLR configuration</param>
        /// <param name="application">The current Application</param>
        /// <returns>The native Tealium Config</returns>
        public static Com.Tealium.Core.TealiumConfig ToNativeConfig(this TealiumConfig config, Application application)
        {
            var collectors = config.Collectors.Select(c => c.ToNativeCollector())
                                                .Where(c => c != null)
                                                .Append(Com.Tealium.Core.Collection.TimeCollectorKt.GetTime(Com.Tealium.Core.Collectors.Instance))
                                                .ToList();
            var dispatchers = config.Dispatchers.Select(d => d.ToNativeDispatcher())
                                                .Where(c => c != null).ToList();
            Com.Tealium.Core.TealiumConfig nativeConfig = new Com.Tealium.Core.TealiumConfig(application,
                config.Account,
                config.Profile,
                config.Environment.ToNativeEnvironment(),
                config.DataSource,
                collectors,
                dispatchers
                );

            if (config.Collectors.Contains(Collectors.LifeCycle))
            {
                nativeConfig.Modules.Add(Com.Tealium.Lifecycle.LifecycleKt.GetLifecycle(Com.Tealium.Core.Modules.Instance));
                var autotracking = config.LifecycleAutotrackingEnabled;
                if (autotracking != null)
                {
                    Com.Tealium.Lifecycle.TealiumConfigLifecycleKt.SetAutoTrackingEnabled(nativeConfig, new Java.Lang.Boolean((bool)autotracking));
                }
            }

            if (config.VisitorServiceEnabled != null && config.VisitorServiceEnabled == true)
            {
                Com.Tealium.Core.IModuleFactory visitorService = Com.Tealium.Visitorservice.VisitorServiceKt.GetVisitorService(Com.Tealium.Core.Modules.Instance);
                nativeConfig.Modules.Add(visitorService);
            }

            if (config.CustomVisitorId != null)
            {
                nativeConfig.ExistingVisitorId = config.CustomVisitorId;
            }

            // Config Overrides
            if (config.OverrideTagManagementURL != null)
            {
                Com.Tealium.Tagmanagementdispatcher.TealiumConfigTagManagementDispatcherKt.SetOverrideTagManagementUrl(nativeConfig, config.OverrideTagManagementURL);
            }

            if (config.OverrideCollectURL != null)
            {
                Com.Tealium.Collectdispatcher.TealiumConfigCollectDispatcherKt.SetOverrideCollectUrl(nativeConfig, config.OverrideCollectURL);
            }

            if (config.OverrideCollectBatchURL != null)
            {
                Com.Tealium.Collectdispatcher.TealiumConfigCollectDispatcherKt.SetOverrideCollectBatchUrl(nativeConfig, config.OverrideCollectBatchURL);
            }

            if (config.OverrideCollectDomain != null)
            {
                Com.Tealium.Collectdispatcher.TealiumConfigCollectDispatcherKt.SetOverrideCollectDomain(nativeConfig, config.OverrideCollectDomain);
            }

            if (config.OverrideLibrarySettingsURL != null)
            {
                nativeConfig.OverrideLibrarySettingsUrl = config.OverrideLibrarySettingsURL;
            }

            if (config.DeepLinkTrackingEnabled != null)
            {
                nativeConfig.DeepLinkTrackingEnabled = (bool)config.DeepLinkTrackingEnabled;
            }

            if (config.QrTraceEnabled != null)
            {
                nativeConfig.QrTraceEnabled = (bool)config.QrTraceEnabled;
            }

            if (config.ConsentLoggingEnabled != null)
            {
                NativeConsent.TealiumConfigConsentManagerKt.SetConsentManagerLoggingEnabled(nativeConfig, new Java.Lang.Boolean((bool)config.ConsentLoggingEnabled));
            }

            if (config.OverrideConsentCategoriesKey != null)
            {
                NativeConsent.TealiumConfigConsentManagerKt.SetOverrideConsentCategoriesKey(nativeConfig, config.OverrideConsentCategoriesKey);
            }

            if (config.VisitorIdentityKey != null)
            {
                nativeConfig.VisitorIdentityKey = config.VisitorIdentityKey;
            }

            var policy = config.ConsentPolicy?.ToNativePolicy();
            if (policy != null)
            {
                NativeConsent.TealiumConfigConsentManagerKt.SetConsentManagerPolicy(nativeConfig, policy);
            }

            var expiry = config.ConsentExpiry?.ToNativeConsentExpiry();
            if (expiry != null)
            {
                NativeConsent.TealiumConfigConsentManagerKt.SetConsentExpiry(nativeConfig, expiry);
            }

            if (config.UseRemoteLibrarySettings != null)
            {
                nativeConfig.UseRemoteLibrarySettings = (bool)config.UseRemoteLibrarySettings;
            }

            if (config.DispatchValidators.Count > 0)
            {
                var nativeValidators = config.DispatchValidators.Select(dv => dv.ToNativeValidator()).ToList();
                nativeValidators.ForEach((validator) =>
                {
                    nativeConfig.Validators.Add(validator);
                });
            }

            return nativeConfig;
        }

        /// <summary>
        /// Converts the Tealium CLR Consent Status to its Native equivalent
        /// </summary>
        /// <param name="status">Tealium Consent Status</param>
        /// <returns>Native Tealium Consent Status</returns>
        public static NativeConsent.ConsentStatus ToNativeStatus(this ConsentStatus status)
        {
            return status switch
            {
                ConsentStatus.Consented => NativeConsent.ConsentStatus.Consented,
                ConsentStatus.NotConsented => NativeConsent.ConsentStatus.NotConsented,
                _ => NativeConsent.ConsentStatus.Unknown,
            };
        }

        /// <summary>
        /// Converts the Tealium Native Consent Status to its CLR equivalent
        /// </summary>
        /// <param name="status">Tealium Consent Status</param>
        /// <returns>Tealium CLR Consent Status</returns>
        public static ConsentStatus ToStatus(this NativeConsent.ConsentStatus status)
        {
            if (status == NativeConsent.ConsentStatus.Consented)
            {
                return ConsentStatus.Consented;
            }
            else if (status == NativeConsent.ConsentStatus.NotConsented)
            {
                return ConsentStatus.NotConsented;
            }
            else
            {
                return ConsentStatus.Unknown;
            }
        }

        /// <summary>
        /// Converts the Tealium CLR Consent Category to its Native equivalent
        /// </summary>
        /// <param name="category">Tealium Consent Category</param>
        /// <returns>Native Tealium Consent Category</returns>
        public static NativeConsent.ConsentCategory? ToNativeCategory(this ConsentCategory category)
        {
            return category switch
            {
                ConsentCategory.Affiliates => NativeConsent.ConsentCategory.Affiliates,
                ConsentCategory.Analytics => NativeConsent.ConsentCategory.Analytics,
                ConsentCategory.BigData => NativeConsent.ConsentCategory.BigData,
                ConsentCategory.CDP => NativeConsent.ConsentCategory.Cdp,
                ConsentCategory.CookieMatch => NativeConsent.ConsentCategory.Cookiematch,
                ConsentCategory.CRM => NativeConsent.ConsentCategory.Crm,
                ConsentCategory.DisplayAds => NativeConsent.ConsentCategory.DisplayAds,
                ConsentCategory.Email => NativeConsent.ConsentCategory.Email,
                ConsentCategory.Engagement => NativeConsent.ConsentCategory.Engagement,
                ConsentCategory.Mobile => NativeConsent.ConsentCategory.Mobile,
                ConsentCategory.Monitoring => NativeConsent.ConsentCategory.Monitoring,
                ConsentCategory.Personalization => NativeConsent.ConsentCategory.Personalization,
                ConsentCategory.Search => NativeConsent.ConsentCategory.Search,
                ConsentCategory.Social => NativeConsent.ConsentCategory.Social,
                ConsentCategory.Misc => NativeConsent.ConsentCategory.Misc,
                _ => null,
            };
        }

        /// <summary>
        /// Converts the Tealium Native Consent Category to its CLR equivalent
        /// </summary>
        /// <param name="category">Tealium Consent Category</param>
        /// <returns>Tealium CLR Consent Category</returns>
        public static ConsentCategory? ToCategory(this NativeConsent.ConsentCategory category)
        {
            if (category == NativeConsent.ConsentCategory.Affiliates)
            {
                return ConsentCategory.Affiliates;
            }
            else if (category == NativeConsent.ConsentCategory.Analytics)
            {
                return ConsentCategory.Analytics;
            }
            else if (category == NativeConsent.ConsentCategory.BigData)
            {
                return ConsentCategory.BigData;
            }
            else if (category == NativeConsent.ConsentCategory.Crm)
            {
                return ConsentCategory.CRM;
            }
            else if (category == NativeConsent.ConsentCategory.Cookiematch)
            {
                return ConsentCategory.CookieMatch;
            }
            else if (category == NativeConsent.ConsentCategory.Cdp)
            {
                return ConsentCategory.CDP;
            }
            else if (category == NativeConsent.ConsentCategory.DisplayAds)
            {
                return ConsentCategory.DisplayAds;
            }
            else if (category == NativeConsent.ConsentCategory.Email)
            {
                return ConsentCategory.Email;
            }
            else if (category == NativeConsent.ConsentCategory.Engagement)
            {
                return ConsentCategory.Engagement;
            }
            else if (category == NativeConsent.ConsentCategory.Misc)
            {
                return ConsentCategory.Misc;
            }
            else if (category == NativeConsent.ConsentCategory.Mobile)
            {
                return ConsentCategory.Mobile;
            }
            else if (category == NativeConsent.ConsentCategory.Monitoring)
            {
                return ConsentCategory.Monitoring;
            }
            else if (category == NativeConsent.ConsentCategory.Personalization)
            {
                return ConsentCategory.Personalization;
            }
            else if (category == NativeConsent.ConsentCategory.Search)
            {
                return ConsentCategory.Search;
            }
            else if (category == NativeConsent.ConsentCategory.Social)
            {
                return ConsentCategory.Social;
            }

            return null;
        }

        /// <summary>
        /// Converts the Tealium CLR Consent Expiry to its Native equivalent
        /// </summary>
        /// <param name="expiry">Tealium Consent Expiry</param>
        /// <returns>Native Tealium Consent Expiry</returns>
        public static NativeConsent.ConsentExpiry? ToNativeConsentExpiry(this ConsentExpiry expiry)
        {
            if (expiry.TimeUnit == TimeUnit.Months)
            {
                // No TimeUnit.MONTHS in Java, so needs conversion to days.
                var cal = Java.Util.Calendar.Instance;
                var today = cal.TimeInMillis;
                cal.Add(Java.Util.CalendarField.Month, expiry.Time);
                var days = (cal.TimeInMillis - today) / (1000 * 60 * 60 * 24);
                return new NativeConsent.ConsentExpiry(days, expiry.TimeUnit.ToNativeTimeUnit());
            }
            else
            {
                return new NativeConsent.ConsentExpiry(expiry.Time, expiry.TimeUnit.ToNativeTimeUnit());
            }
        }

        /// <summary>
        /// Converts the Tealium CLR Time Unit to its Native equivalent
        /// </summary>
        /// <param name="unit">Tealium Time Unit</param>
        /// <returns>Native Tealium Time Unit</returns>
        public static Java.Util.Concurrent.TimeUnit ToNativeTimeUnit(this TimeUnit unit)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return unit switch
            {
                TimeUnit.Months => Java.Util.Concurrent.TimeUnit.Days,
                TimeUnit.Days => Java.Util.Concurrent.TimeUnit.Days,
                TimeUnit.Hours => Java.Util.Concurrent.TimeUnit.Hours,
                TimeUnit.Minutes => Java.Util.Concurrent.TimeUnit.Minutes,
                _ => Java.Util.Concurrent.TimeUnit.Days
            };
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Converts the Tealium CLR Consent Policy to its Native equivalent
        /// </summary>
        /// <param name="policy">Tealium Consent Policy</param>
        /// <returns>Native Tealium Consent Policy</returns>
        public static NativeConsent.ConsentPolicy? ToNativePolicy(this ConsentPolicy policy)
        {
            return policy switch
            {
                ConsentPolicy.CCPA => NativeConsent.ConsentPolicy.Ccpa,
                ConsentPolicy.GDPR => NativeConsent.ConsentPolicy.Gdpr,
                _ => null
            };
        }

        /// <summary>
        /// Converts the Tealium Native Consent Policy to its CLR equivalent
        /// </summary>
        /// <param name="policy">Tealium Consent Policy</param>
        /// <returns>Tealium CLR Consent Policy</returns>
        public static ConsentPolicy? ToConsentPolicy(this NativeConsent.ConsentPolicy policy)
        {
            if (policy == NativeConsent.ConsentPolicy.Ccpa)
            {
                return ConsentPolicy.CCPA;
            }
            else if (policy == NativeConsent.ConsentPolicy.Gdpr)
            {
                return ConsentPolicy.GDPR;
            }

            return null;
        }

        /// <summary>
        /// Converts the Tealium CLR Environment to its Native equivalent
        /// </summary>
        /// <param name="environment">Tealium Environment</param>
        /// <returns>Native Tealium Environment</returns>
        public static Com.Tealium.Core.Environment ToNativeEnvironment(this Environment environment)
        {

            return environment switch
            {
                Environment.Dev => Com.Tealium.Core.Environment.Dev,
                Environment.Qa => Com.Tealium.Core.Environment.Qa,
                Environment.Prod => Com.Tealium.Core.Environment.Prod,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Converts the Tealium Native Environment to its CLRequivalent
        /// </summary>
        /// <param name="environment">Tealium Environment</param>
        /// <returns>Tealium CLR Environment</returns>
        public static Environment ToEnvironment(this Com.Tealium.Core.Environment environment)
        {
            if (environment == Com.Tealium.Core.Environment.Dev)
            {
                return Environment.Dev;
            }
            else if (environment == Com.Tealium.Core.Environment.Qa)
            {
                return Environment.Qa;
            }

            return Environment.Prod;
        }

        /// <summary>
        /// Converts the Tealium CLR Expiry to its Native equivalent
        /// </summary>
        /// <param name="expiry">Tealium Expiry</param>
        /// <returns>Native Tealium Expiry</returns>
        public static Com.Tealium.Core.Persistence.Expiry ToNativeExpiry(this Expiry expiry)
        {
            return expiry switch
            {
                Expiry.Session => Com.Tealium.Core.Persistence.Expiry.Session,
                Expiry.Forever => Com.Tealium.Core.Persistence.Expiry.Forever,
                Expiry.UntilRestart => Com.Tealium.Core.Persistence.Expiry.UntilRestart,
                _ => Com.Tealium.Core.Persistence.Expiry.Session
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Collector to its Native equivalent
        /// </summary>
        /// <param name="collector">Tealium Collector</param>
        /// <returns>Native Tealium Collector</returns>
        public static Com.Tealium.Core.ICollectorFactory? ToNativeCollector(this Collectors collector)
        {
            var collectors = Com.Tealium.Core.Collectors.Instance;
            return collector switch
            {
                Collectors.AppData => Com.Tealium.Core.Collection.AppCollectorKt.GetApp(collectors),
                Collectors.Connectivity => Com.Tealium.Core.Collection.ConnectivityCollectorKt.GetConnectivity(collectors),
                Collectors.DeviceData => Com.Tealium.Core.Collection.DeviceCollectorKt.GetDevice(collectors),
                _ => null,
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Dispatcher to its Native equivalent
        /// </summary>
        /// <param name="dispatcher">Tealium Dispatcher</param>
        /// <returns>Native Tealium Dispatcher</returns>
        public static Com.Tealium.Core.IDispatcherFactory? ToNativeDispatcher(this Dispatchers dispatcher)
        {
            var dispatchers = Com.Tealium.Core.Dispatchers.Instance;
            return dispatcher switch
            {
                Dispatchers.Collect => Com.Tealium.Collectdispatcher.CollectDispatcherKt.GetCollect(dispatchers),
                Dispatchers.RemoteCommands => Com.Tealium.Remotecommanddispatcher.RemoteCommandDispatcherKt.GetRemoteCommands(dispatchers),
                Dispatchers.TagManagement => Com.Tealium.Tagmanagementdispatcher.TagManagementDispatcherKt.GetTagManagement(dispatchers),
                _ => null,
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Log Level to its Native equivalent
        /// </summary>
        /// <param name="level">Tealium Log Level</param>
        /// <returns>Native Tealium Log Level</returns>
        public static Com.Tealium.Core.LogLevel ToNativeLogLevel(this LogLevel level)
        {
            return level switch
            {
                LogLevel.Dev => Com.Tealium.Core.LogLevel.Dev,
                LogLevel.Qa => Com.Tealium.Core.LogLevel.Qa,
                LogLevel.Prod => Com.Tealium.Core.LogLevel.Prod,
                LogLevel.Silent => Com.Tealium.Core.LogLevel.Silent,
                _ => Com.Tealium.Core.LogLevel.Prod
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Event Listener to its Native equivalent
        /// </summary>
        /// <param name="eventListener">Tealium Event Listener</param>
        /// <returns>Native Tealium Listener</returns>
        public static Com.Tealium.Core.Messaging.IListener ToNativeEventListener(this ITealiumEventListener eventListener)
        {
            return new NativeEventListener(eventListener);
        }

        /// <summary>
        /// Converts the Tealium CLR Dispatch Validator to its Native equivalent
        /// </summary>
        /// <param name="dispatchValidator">Dispatch Validator</param>
        /// <returns>Native Tealium Dispatch Validator</returns>
        public static Com.Tealium.Core.Validation.IDispatchValidator ToNativeValidator(this IDispatchValidator dispatchValidator)
        {
            return new NativeDispatchValidator(dispatchValidator);
        }
    }
}
