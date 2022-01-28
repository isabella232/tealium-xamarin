using System;
using System.Collections.Generic;
using System.Linq;
using static Tealium.ConsentManager;
using Foundation;
using Tealium.Platform.iOS;

#nullable enable
namespace Tealium.iOS.NativeInterop.Extensions
{

    /// <summary>
    /// Utility conversions for most Tealium CLR types into their iOS SDK
    /// native equivalent, and vice-versa.
    /// </summary>
    internal static class ConfigExtensions
    {

        internal static TealiumConfigWrapper ToNativeConfig(this TealiumConfig config)
        {


            List<CollectorType> collectors = config.Collectors.Select(c => c.ToNativeCollector())
                                                .ToList();
            var dispatchers = config.Dispatchers.Select(d => d.ToNativeDispatcher())
                                                .ToArray();
            if (config.VisitorServiceEnabled != null && config.VisitorServiceEnabled == true)
            {
                collectors.Add(CollectorType.VisitorService);
            }
            TealiumConfigWrapper nativeConfig = new TealiumConfigWrapper(config.Account, config.Profile, config.Environment.ToString().ToLower(), config.DataSource, null);

            nativeConfig.Collectors = collectors.Select(c => new NSNumber((long)c))
                                                .ToArray();
            nativeConfig.Dispatchers = dispatchers.Select(d => new NSNumber((long)d))
                                                .ToArray();


            // Config Overrides
            if (config.OverrideTagManagementURL != null)
            {
                nativeConfig.TagManagementOverrideURL = config.OverrideTagManagementURL;
            }

            if (config.OverrideCollectURL != null)
            {
                nativeConfig.OverrideCollectURL = config.OverrideCollectURL;
            }

            if (config.OverrideCollectBatchURL != null)
            {
                nativeConfig.OverrideCollectBatchURL = config.OverrideCollectBatchURL;
            }

            if (config.OverrideCollectDomain != null)
            {
                nativeConfig.OverrideCollectDomain = config.OverrideCollectDomain;
            }

            if (config.OverrideLibrarySettingsURL != null)
            {
                nativeConfig.PublishSettingsURL = config.OverrideLibrarySettingsURL;
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
                nativeConfig.ConsentLoggingEnabled = (bool)config.ConsentLoggingEnabled;
            }

            var policy = config.ConsentPolicy?.ToNativePolicy();
            if (policy != null)
            {
                nativeConfig.ConsentPolicy = policy ?? TealiumConsentPolicyWrapper.None;
            }

            var expiry = config.ConsentExpiry?.ToNativeConsentExpiry();
            if (expiry != null)
            {
                nativeConfig.ConsentExpiry = expiry;
            }

            if (config.UseRemoteLibrarySettings != null)
            {
                nativeConfig.ShouldUseRemotePublishSettings = (bool)config.UseRemoteLibrarySettings;
            }

            if (config.DispatchValidators.Count > 0)
            {
                nativeConfig.DispatchValidators = config.DispatchValidators.Select(dv =>
                {
                    if (dv is DispatchValidatorIOS dvios)
                    {
                        return dvios;
                    }
                    return new DispatchValidatorIOS(dv.Name, dv);
                }).ToArray();
            }


            if (config.LogLevel != null)
            {
                nativeConfig.LogLevel = config.LogLevel.Value.ToNativeLogLevel();
            }

            return nativeConfig;
        }

        /// <summary>
        /// Converts from a String representation of Consent Status to the iOS native enum
        /// </summary>
        /// <returns>The native status enum from string.</returns>
        /// <param name="statusName">Consent Status name.</param>
        internal static TealiumConsentStatusWrapper ToNativeStatus(this ConsentStatus statusName)
        {
            TealiumConsentStatusWrapper status = TealiumConsentStatusWrapper.Unknown;
            switch (statusName)
            {
                case ConsentStatus.Consented:
                    status = TealiumConsentStatusWrapper.Consented;
                    break;
                case ConsentStatus.NotConsented:
                    status = TealiumConsentStatusWrapper.NotConsented;
                    break;
            }
            return status;
        }

        /// <summary>
        /// Converts from the Native iOS enum to a String representation of the Consent Status
        /// </summary>
        /// <returns>The Consent Status string as defined in <see cref="TealiumConsentStatusWrapper"/></returns>
        /// <param name="status">Status enum</param>
        internal static ConsentStatus ToStatus(this TealiumConsentStatusWrapper status)
        {
            switch (status)
            {
                case TealiumConsentStatusWrapper.Consented:
                    return ConsentStatus.Consented;
                case TealiumConsentStatusWrapper.NotConsented:
                    return ConsentStatus.NotConsented;
                case TealiumConsentStatusWrapper.Unknown:
                    return ConsentStatus.Unknown;
                default:
                    return ConsentStatus.Unknown;
            }
        }

        internal static TealiumConsentCategoriesWrappers ToNativeCategory(this ConsentCategory category)
        {
            switch (category)
            {
                case ConsentCategory.Affiliates:
                    return TealiumConsentCategoriesWrappers.Affiliates;
                case ConsentCategory.Analytics:
                    return TealiumConsentCategoriesWrappers.Analytics;
                case ConsentCategory.BigData:
                    return TealiumConsentCategoriesWrappers.BigData;
                case ConsentCategory.CDP:
                    return TealiumConsentCategoriesWrappers.Cdp;
                case ConsentCategory.CookieMatch:
                    return TealiumConsentCategoriesWrappers.CookieMatch;
                case ConsentCategory.CRM:
                    return TealiumConsentCategoriesWrappers.Crm;
                case ConsentCategory.DisplayAds:
                    return TealiumConsentCategoriesWrappers.DisplayAds;
                case ConsentCategory.Email:
                    return TealiumConsentCategoriesWrappers.Email;
                case ConsentCategory.Engagement:
                    return TealiumConsentCategoriesWrappers.Engagement;
                case ConsentCategory.Mobile:
                    return TealiumConsentCategoriesWrappers.Mobile;
                case ConsentCategory.Monitoring:
                    return TealiumConsentCategoriesWrappers.Monitoring;
                case ConsentCategory.Personalization:
                    return TealiumConsentCategoriesWrappers.Personalization;
                case ConsentCategory.Search:
                    return TealiumConsentCategoriesWrappers.Search;
                case ConsentCategory.Social:
                    return TealiumConsentCategoriesWrappers.Social;
                case ConsentCategory.Misc:
                    return TealiumConsentCategoriesWrappers.Misc;
                default:
                    throw new ArgumentException("Can't find category", nameof(category));
            }
        }

        internal static ConsentCategory ToCategory(this NSNumber category)
        {
            long longValue = category.LongValue;
            switch (longValue)
            {
                case (long)TealiumConsentCategoriesWrappers.Affiliates:
                    return ConsentCategory.Affiliates;
                case (long)TealiumConsentCategoriesWrappers.Analytics:
                    return ConsentCategory.Analytics;
                case (long)TealiumConsentCategoriesWrappers.BigData:
                    return ConsentCategory.BigData;
                case (long)TealiumConsentCategoriesWrappers.Cdp:
                    return ConsentCategory.CDP;
                case (long)TealiumConsentCategoriesWrappers.CookieMatch:
                    return ConsentCategory.CookieMatch;
                case (long)TealiumConsentCategoriesWrappers.Crm:
                    return ConsentCategory.CRM;
                case (long)TealiumConsentCategoriesWrappers.DisplayAds:
                    return ConsentCategory.DisplayAds;
                case (long)TealiumConsentCategoriesWrappers.Email:
                    return ConsentCategory.Email;
                case (long)TealiumConsentCategoriesWrappers.Engagement:
                    return ConsentCategory.Engagement;
                case (long)TealiumConsentCategoriesWrappers.Mobile:
                    return ConsentCategory.Mobile;
                case (long)TealiumConsentCategoriesWrappers.Monitoring:
                    return ConsentCategory.Monitoring;
                case (long)TealiumConsentCategoriesWrappers.Personalization:
                    return ConsentCategory.Personalization;
                case (long)TealiumConsentCategoriesWrappers.Search:
                    return ConsentCategory.Search;
                case (long)TealiumConsentCategoriesWrappers.Social:
                    return ConsentCategory.Social;
                case (long)TealiumConsentCategoriesWrappers.Misc:
                    return ConsentCategory.Misc;
                default:
                    throw new ArgumentException("Can't find category", nameof(category));
            }
        }

        /// <summary>
        /// Converts an array of ConsentCategory into an array of native NSObjects.
        /// </summary>
        /// <returns>The NSObject array.</returns>
        /// <param name="strings">Array of ConsentCategory.</param>
        internal static NSNumber[] ToNativeStatuses(this ConsentCategory[] categories)
        {
            NSNumber[] objects = new NSNumber[categories.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                objects[i] = new NSNumber((long)categories[i].ToNativeCategory());
            }
            return objects;
        }

        /// <summary>
        /// Converts an array of native NSObjects into an array of ConsentCategory.
        /// </summary>
        /// <returns>The ConsentCategory array.</returns>
        /// <param name="objects">Array of NSObjects.</param>
        internal static ConsentCategory[] ToStatusArray(this NSNumber[] objects)
        {
            ConsentCategory[] categories = new ConsentCategory[objects.Length];
            for (int i = 0; i < objects.Length; i++)
            {
                categories[i] = objects[i].ToCategory();
            }
            return categories;
        }

        internal static ConsentPolicy? ToPolicy(this TealiumConsentPolicyWrapper policy)
        {
            switch (policy)
            {
                case TealiumConsentPolicyWrapper.Ccpa:
                    return ConsentPolicy.CCPA;
                case TealiumConsentPolicyWrapper.Gdpr:
                    return ConsentPolicy.GDPR;
                default:
                    return null;
            }
        }

        internal static ExpiryWrapper ToNativeExpiry(this Expiry expiry)
        {
            ExpiryWrapper expWrapper;
            switch (expiry)
            {
                case Expiry.Forever:
                    expWrapper = ExpiryWrapper.Forever;
                    break;
                case Expiry.Session:
                    expWrapper = ExpiryWrapper.Session;
                    break;
                case Expiry.UntilRestart:
                    expWrapper = ExpiryWrapper.UntilRestart;
                    break;
                default:
                    throw new ArgumentException("Value not supported", nameof(expiry));
            }
            return expWrapper;
        }

        /**
         * You should not use this method on Dictionary of other Dictionaries or Arrays, as it results in not well defined behavior.
         * 
         * Using this method with N = NSArray or NSDictionary comports a conversion with a best-effort conversion. 
         * Array of strings (for example) are successfully converted but array of NSNumbers are automatically converted to array of decimals.
         * Array with non specified generic type convert to array of objects. 
         * 
         * Casting to something that is not the specific array that we created (such as (List<objects>)List<string> or the other way around) results in a crash!!!
         * 
         * Use specific method for nested dictionaries:
         * - ToDictionaryOfLists
         * - ToDictionaryOfDictionaries
         * - ToDictionaryOfSets
         */
        internal static Dictionary<string, T>? ToDictionary<T, N>(this NSDictionary<NSString, N> nsDict) where N: NSObject
        {
            return NSObjectConverter.ConvertFromNSDictionaryToTargetType<T>(nsDict);
        }

        internal static List<T>? ToList<T, N>(this NSArray<N> nsArray) where N: NSObject
        {
            return NSObjectConverter.ConvertFromNSArrayToTargetType<T>(nsArray);
        }

        internal static Dictionary<string, IList<T>>? ToDictionaryOfLists<T, N>(this NSDictionary<NSString, NSArray<N>> nsDict) where N : NSObject
        {
            if (nsDict == null)
            {
                return null;
            }

            Dictionary<string, IList<T>> result = new Dictionary<string, IList<T>>((int)nsDict.Count);
            foreach (NSString key in nsDict.Keys)
            {
                List<T>? list = (nsDict[key])?.ToList<T,N>();
                if (list != null)
                {
                    result.Add(key, list);
                }
            }
            return result;
        }

        internal static Dictionary<string, ISet<T>>? ToDictionaryOfSets<T, N>(this NSDictionary<NSString, NSSet<N>> nsDict) where N : NSObject
        {
            if (nsDict == null)
            {
                return null;
            }

            Dictionary<string, ISet<T>> result = new Dictionary<string, ISet<T>>((int)nsDict.Count);
            foreach (NSString key in nsDict.Keys)
            {
                NSArray<N> arr = NSArray<N>.FromNSObjects(nsDict[key].ToArray<N>());
                List<T>? list = arr?.ToList<T, N>();
                if (list != null)
                {
                    result.Add(key, new HashSet<T>(list));
                }
            }
            return result;
        }

        internal static Dictionary<string, IDictionary<string, T>>? ToDictionaryOfDictionaries<T, N>(this NSDictionary<NSString, NSDictionary<NSString, N>> nsDict) where N : NSObject
        {
            if (nsDict == null)
            {
                return null;
            }

            Dictionary<string, IDictionary<string, T>> result = new Dictionary<string, IDictionary<string, T>>((int)nsDict.Count);
            foreach (NSString key in nsDict.Keys)
            {
                Console.WriteLine(nsDict[key].GetType());
                Dictionary<string, T>? dict = ((NSDictionary<NSString, N>)nsDict[key])?.ToDictionary<T, N>();
                if (dict != null)
                {
                    result.Add((NSString)key, dict);
                }
            }
            return result;
        }

        /// <summary>
        /// Converts the Tealium CLR Collector to its Native equivalent
        /// </summary>
        /// <param name="collector">Tealium Collector</param>
        /// <returns>Native Tealium Collector</returns>
        internal static CollectorType ToNativeCollector(this Collectors collector)
        {
            return collector switch
            {
                Collectors.AppData => CollectorType.AppData,
                Collectors.Connectivity => CollectorType.Connectivity,
                Collectors.DeviceData => CollectorType.DeviceData,
                Collectors.LifeCycle => CollectorType.Lifecycle,
                //Collectors.VisitorService => CollectorType.VisitorService, // TODO: missing Visitor Service collector?
                _ => throw new ArgumentException("Collector not recognized")
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Dispatcher to its Native equivalent
        /// </summary>
        /// <param name="dispatcher">Tealium Dispatcher</param>
        /// <returns>Native Tealium Dispatcher</returns>
        internal static DispatcherType ToNativeDispatcher(this Dispatchers dispatcher)
        {
            return dispatcher switch
            {
                Dispatchers.Collect => DispatcherType.Collect,
                Dispatchers.RemoteCommands => DispatcherType.RemoteCommands,
                Dispatchers.TagManagement => DispatcherType.TagManagement,
                _ => throw new ArgumentException("Dispatcher not recognized"),
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Log Level to its Native equivalent
        /// </summary>
        /// <param name="level">Tealium Log Level</param>
        /// <returns>Native Tealium Log Level</returns>
        internal static TealiumLogLevelWrapper ToNativeLogLevel(this LogLevel level)
        {
            return level switch
            {
                LogLevel.Dev => TealiumLogLevelWrapper.Info,
                LogLevel.Qa => TealiumLogLevelWrapper.Debug,
                LogLevel.Prod => TealiumLogLevelWrapper.Error,
                LogLevel.Silent => TealiumLogLevelWrapper.Silent,
                _ => TealiumLogLevelWrapper.Undefined
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Consent Policy to its Native equivalent
        /// </summary>
        /// <param name="policy">Tealium Consent Policy</param>
        /// <returns>Native Tealium Consent Policy</returns>
        internal static TealiumConsentPolicyWrapper ToNativePolicy(this ConsentPolicy policy)
        {
            return policy switch
            {
                ConsentPolicy.CCPA => TealiumConsentPolicyWrapper.Ccpa,
                ConsentPolicy.GDPR => TealiumConsentPolicyWrapper.Gdpr,
                _ => TealiumConsentPolicyWrapper.None
            };
        }

        /// <summary>
        /// Converts the Tealium CLR Consent Expiry to its Native equivalent
        /// </summary>
        /// <param name="expiry">Tealium Consent Expiry</param>
        /// <returns>Native Tealium Consent Expiry</returns>
        internal static ExpirationTime ToNativeConsentExpiry(this ConsentExpiry expiry)
        {
            return new ExpirationTime(expiry.Time, expiry.TimeUnit.ToNativeTimeUnit());
        }

        internal static TimeUnitWrapper ToNativeTimeUnit(this TimeUnit timeUnit)
        {
            return timeUnit switch
            {
                TimeUnit.Minutes => TimeUnitWrapper.Minutes,
                TimeUnit.Hours => TimeUnitWrapper.Hours,
                TimeUnit.Days => TimeUnitWrapper.Days,
                TimeUnit.Months => TimeUnitWrapper.Months,
                _ => throw new ArgumentException("Unexpected timeunit")
            };
        }

    }
}