using System;
using NUnit.Framework;
using Tealium.Droid.NativeInterop.Extensions;
using TealiumNative = Com.Tealium.Core;

namespace Tealium.Tests.Droid.NativeInteropTests
{
    [TestFixture]
    public class ExtensionsTests
    {

        [Test]
        public void ToNativeConfig()
        {
            TealiumConfig config = new TealiumConfig("account",
                    "profile",
                    Environment.Dev,
                    new System.Collections.Generic.List<Dispatchers>() {
                        Dispatchers.Collect, Dispatchers.TagManagement, Dispatchers.RemoteCommands
                    },
                    new System.Collections.Generic.List<Collectors>() {
                        Collectors.AppData, Collectors.Connectivity, Collectors.DeviceData, Collectors.LifeCycle
                    },
                    "dataSource",
                    "customVisitorId",
                    true,
                    "overrideCollectUrl",
                    "overrideBatchCollectUrl",
                    "overrideCollectDomain",
                    "overrideLibrarySettingsUrl",
                    "overrideTagManagementUrl",
                    true,
                    true,
                    LogLevel.Qa,
                    true,
                    ConsentManager.ConsentPolicy.GDPR,
                    new ConsentManager.ConsentExpiry(10, TimeUnit.Days),
                    true,
                    true,
                    true,
                    null,
                    "visitorIdentityKey",
                    "overrideConsentCategoriesKey"
                );

            TealiumNative.TealiumConfig nativeConfig = config.ToNativeConfig(MainActivity.CurrentApplication);

            // Account info
            Assert.AreEqual(config.Account, nativeConfig.AccountName);
            Assert.AreEqual(config.Profile, nativeConfig.ProfileName);
            Assert.AreEqual(TealiumNative.Environment.Dev, nativeConfig.Environment);
            Assert.AreEqual(config.CustomVisitorId, nativeConfig.ExistingVisitorId);

            // Dispatchers
            Assert.True(nativeConfig.Dispatchers.Contains(Com.Tealium.Collectdispatcher.CollectDispatcher.CompanionInstance));
            Assert.True(nativeConfig.Dispatchers.Contains(Com.Tealium.Tagmanagementdispatcher.TagManagementDispatcher.CompanionInstance));
            Assert.True(nativeConfig.Dispatchers.Contains(Com.Tealium.Remotecommanddispatcher.RemoteCommandDispatcher.CompanionInstance));

            // Collectors
            Assert.True(nativeConfig.Collectors.Contains(Com.Tealium.Core.Collection.AppCollector.CompanionInstance));
            Assert.True(nativeConfig.Collectors.Contains(Com.Tealium.Core.Collection.DeviceCollector.CompanionInstance));
            Assert.True(nativeConfig.Collectors.Contains(Com.Tealium.Core.Collection.ConnectivityCollector.CompanionInstance));

            // Modules
            Assert.True(nativeConfig.Modules.Contains(Com.Tealium.Lifecycle.Lifecycle.CompanionInstance));
            Assert.True(nativeConfig.Modules.Contains(Com.Tealium.Visitorservice.VisitorService.CompanionInstance));

            // Misc
            Assert.AreEqual("dataSource", nativeConfig.DataSourceId);
            Assert.True(nativeConfig.DeepLinkTrackingEnabled);
            Assert.True(nativeConfig.QrTraceEnabled);
            Assert.AreEqual("overrideLibrarySettingsUrl", nativeConfig.OverrideLibrarySettingsUrl);
            Assert.True(nativeConfig.UseRemoteLibrarySettings);
            Assert.AreEqual("visitorIdentityKey", nativeConfig.VisitorIdentityKey);

            // Lifecycle
            Assert.True((bool)Com.Tealium.Lifecycle.TealiumConfigLifecycleKt.IsAutoTrackingEnabled(nativeConfig));

            // Collect
            Assert.AreEqual("overrideCollectUrl", Com.Tealium.Collectdispatcher.TealiumConfigCollectDispatcherKt.GetOverrideCollectUrl(nativeConfig));
            Assert.AreEqual("overrideBatchCollectUrl", Com.Tealium.Collectdispatcher.TealiumConfigCollectDispatcherKt.GetOverrideCollectBatchUrl(nativeConfig));
            Assert.AreEqual("overrideCollectDomain", Com.Tealium.Collectdispatcher.TealiumConfigCollectDispatcherKt.GetOverrideCollectDomain(nativeConfig));

            // TagManagement
            Assert.AreEqual("overrideTagManagementUrl", Com.Tealium.Tagmanagementdispatcher.TealiumConfigTagManagementDispatcherKt.GetOverrideTagManagementUrl(nativeConfig));

            // Consent
            Assert.True((bool)TealiumNative.Consent.TealiumConfigConsentManagerKt.GetConsentManagerLoggingEnabled(nativeConfig));
            Assert.AreEqual(TealiumNative.Consent.ConsentPolicy.Gdpr, TealiumNative.Consent.TealiumConfigConsentManagerKt.GetConsentManagerPolicy(nativeConfig));
            var consentExpiry = TealiumNative.Consent.TealiumConfigConsentManagerKt.GetConsentExpiry(nativeConfig);
            Assert.AreEqual(10, consentExpiry.Time);
            Assert.AreEqual(Java.Util.Concurrent.TimeUnit.Days, consentExpiry.Unit);
            Assert.AreEqual("overrideConsentCategoriesKey", TealiumNative.Consent.TealiumConfigConsentManagerKt.GetOverrideConsentCategoriesKey(nativeConfig));
        }

        [Test]
        public void ConsentStatus_ToNative()
        {
            var consented = ConsentManager.ConsentStatus.Consented.ToNativeStatus();
            var notConsented = ConsentManager.ConsentStatus.NotConsented.ToNativeStatus();
            var unknown = ConsentManager.ConsentStatus.Unknown.ToNativeStatus();

            Assert.AreEqual(TealiumNative.Consent.ConsentStatus.Consented, consented);
            Assert.AreEqual(TealiumNative.Consent.ConsentStatus.NotConsented, notConsented);
            Assert.AreEqual(TealiumNative.Consent.ConsentStatus.Unknown, unknown);
        }

        [Test]
        public void ConsentStatus_FromNative()
        {
            var consented = TealiumNative.Consent.ConsentStatus.Consented.ToStatus();
            var notConsented = TealiumNative.Consent.ConsentStatus.NotConsented.ToStatus();
            var unknown = TealiumNative.Consent.ConsentStatus.Unknown.ToStatus();

            Assert.AreEqual(ConsentManager.ConsentStatus.Consented, consented);
            Assert.AreEqual(ConsentManager.ConsentStatus.NotConsented, notConsented);
            Assert.AreEqual(ConsentManager.ConsentStatus.Unknown, unknown);
        }

        [Test]
        public void ConsentCategory_ToNative()
        {
            var affiliates = ConsentManager.ConsentCategory.Affiliates.ToNativeCategory();
            var analytics = ConsentManager.ConsentCategory.Analytics.ToNativeCategory();
            var crm = ConsentManager.ConsentCategory.CRM.ToNativeCategory();
            var cookieMatch = ConsentManager.ConsentCategory.CookieMatch.ToNativeCategory();
            var cdp = ConsentManager.ConsentCategory.CDP.ToNativeCategory();
            var displayAds = ConsentManager.ConsentCategory.DisplayAds.ToNativeCategory();
            var email = ConsentManager.ConsentCategory.Email.ToNativeCategory();
            var engagement = ConsentManager.ConsentCategory.Engagement.ToNativeCategory();
            var misc = ConsentManager.ConsentCategory.Misc.ToNativeCategory();
            var mobile = ConsentManager.ConsentCategory.Mobile.ToNativeCategory();
            var monitoring = ConsentManager.ConsentCategory.Monitoring.ToNativeCategory();
            var personalization = ConsentManager.ConsentCategory.Personalization.ToNativeCategory();
            var search = ConsentManager.ConsentCategory.Search.ToNativeCategory();
            var social = ConsentManager.ConsentCategory.Social.ToNativeCategory();

            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Affiliates, affiliates);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Analytics, analytics);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Crm, crm);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Cookiematch, cookieMatch);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Cdp, cdp);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.DisplayAds, displayAds);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Email, email);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Engagement, engagement);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Misc, misc);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Mobile, mobile);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Monitoring, monitoring);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Personalization, personalization);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Search, search);
            Assert.AreEqual(TealiumNative.Consent.ConsentCategory.Social, social);
        }

        [Test]
        public void ConsentCategory_FromNative()
        {
            var affiliates = TealiumNative.Consent.ConsentCategory.Affiliates.ToCategory();
            var analytics = TealiumNative.Consent.ConsentCategory.Analytics.ToCategory();
            var crm = TealiumNative.Consent.ConsentCategory.Crm.ToCategory();
            var cookieMatch = TealiumNative.Consent.ConsentCategory.Cookiematch.ToCategory();
            var cdp = TealiumNative.Consent.ConsentCategory.Cdp.ToCategory();
            var displayAds = TealiumNative.Consent.ConsentCategory.DisplayAds.ToCategory();
            var email = TealiumNative.Consent.ConsentCategory.Email.ToCategory();
            var engagement = TealiumNative.Consent.ConsentCategory.Engagement.ToCategory();
            var misc = TealiumNative.Consent.ConsentCategory.Misc.ToCategory();
            var mobile = TealiumNative.Consent.ConsentCategory.Mobile.ToCategory();
            var monitoring = TealiumNative.Consent.ConsentCategory.Monitoring.ToCategory();
            var personalization = TealiumNative.Consent.ConsentCategory.Personalization.ToCategory();
            var search = TealiumNative.Consent.ConsentCategory.Search.ToCategory();
            var social = TealiumNative.Consent.ConsentCategory.Social.ToCategory();

            Assert.AreEqual(ConsentManager.ConsentCategory.Affiliates, affiliates);
            Assert.AreEqual(ConsentManager.ConsentCategory.Analytics, analytics);
            Assert.AreEqual(ConsentManager.ConsentCategory.CRM, crm);
            Assert.AreEqual(ConsentManager.ConsentCategory.CookieMatch, cookieMatch);
            Assert.AreEqual(ConsentManager.ConsentCategory.CDP, cdp);
            Assert.AreEqual(ConsentManager.ConsentCategory.DisplayAds, displayAds);
            Assert.AreEqual(ConsentManager.ConsentCategory.Email, email);
            Assert.AreEqual(ConsentManager.ConsentCategory.Engagement, engagement);
            Assert.AreEqual(ConsentManager.ConsentCategory.Misc, misc);
            Assert.AreEqual(ConsentManager.ConsentCategory.Mobile, mobile);
            Assert.AreEqual(ConsentManager.ConsentCategory.Monitoring, monitoring);
            Assert.AreEqual(ConsentManager.ConsentCategory.Personalization, personalization);
            Assert.AreEqual(ConsentManager.ConsentCategory.Search, search);
            Assert.AreEqual(ConsentManager.ConsentCategory.Social, social);
        }

        [Test]
        public void ConsentExpiry_ToNative()
        {
            var oneMinutes = new ConsentManager.ConsentExpiry(1, TimeUnit.Minutes).ToNativeConsentExpiry();
            var twoHours = new ConsentManager.ConsentExpiry(2, TimeUnit.Hours).ToNativeConsentExpiry();
            var threeDays = new ConsentManager.ConsentExpiry(3, TimeUnit.Days).ToNativeConsentExpiry();
            var threeMonths = new ConsentManager.ConsentExpiry(3, TimeUnit.Months).ToNativeConsentExpiry();

            Assert.AreEqual(1, oneMinutes.Time);
            Assert.AreEqual(Java.Util.Concurrent.TimeUnit.Minutes, oneMinutes.Unit);
            Assert.AreEqual(2, twoHours.Time);
            Assert.AreEqual(Java.Util.Concurrent.TimeUnit.Hours, twoHours.Unit);
            Assert.AreEqual(3, threeDays.Time);
            Assert.AreEqual(Java.Util.Concurrent.TimeUnit.Days, threeDays.Unit);
            Assert.True(threeMonths.Time >= 28 + 31 + 30); // Feb + Mar + Apr
            Assert.True(threeMonths.Time <= 31 + 31 + 30); // Jul + Aug + Sep
            Assert.AreEqual(Java.Util.Concurrent.TimeUnit.Days, threeMonths.Unit); // No TimeUnit.Months
        }

        [Test]
        public void ConsentPolicy_ToNative()
        {
            var gdpr = ConsentManager.ConsentPolicy.GDPR.ToNativePolicy();
            var ccpa = ConsentManager.ConsentPolicy.CCPA.ToNativePolicy();

            Assert.AreEqual(TealiumNative.Consent.ConsentPolicy.Gdpr, gdpr);
            Assert.AreEqual(TealiumNative.Consent.ConsentPolicy.Ccpa, ccpa);
        }

        [Test]
        public void ConsentPolicy_FromNative()
        {
            var gdpr = TealiumNative.Consent.ConsentPolicy.Gdpr.ToConsentPolicy();
            var ccpa = TealiumNative.Consent.ConsentPolicy.Ccpa.ToConsentPolicy();

            Assert.AreEqual(ConsentManager.ConsentPolicy.GDPR, gdpr);
            Assert.AreEqual(ConsentManager.ConsentPolicy.CCPA, ccpa);
        }

        [Test]
        public void Environment_ToNative()
        {
            var dev = Environment.Dev.ToNativeEnvironment();
            var qa = Environment.Qa.ToNativeEnvironment();
            var prod = Environment.Prod.ToNativeEnvironment();

            Assert.AreEqual(TealiumNative.Environment.Dev, dev);
            Assert.AreEqual(TealiumNative.Environment.Qa, qa);
            Assert.AreEqual(TealiumNative.Environment.Prod, prod);
        }

        [Test]
        public void Environment_FromNative()
        {
            var dev = TealiumNative.Environment.Dev.ToEnvironment();
            var qa = TealiumNative.Environment.Qa.ToEnvironment();
            var prod = TealiumNative.Environment.Prod.ToEnvironment();

            Assert.AreEqual(Environment.Dev, dev);
            Assert.AreEqual(Environment.Qa, qa);
            Assert.AreEqual(Environment.Prod, prod);
        }

        [Test]
        public void Expiry_ToNative()
        {
            var session = Expiry.Session.ToNativeExpiry();
            var forever = Expiry.Forever.ToNativeExpiry();
            var untilRestart = Expiry.UntilRestart.ToNativeExpiry();

            Assert.AreEqual(TealiumNative.Persistence.Expiry.Session, session);
            Assert.AreEqual(TealiumNative.Persistence.Expiry.Forever, forever);
            Assert.AreEqual(TealiumNative.Persistence.Expiry.UntilRestart, untilRestart);
        }

        [Test]
        public void Collectors_ToNative()
        {
            var appData = Collectors.AppData.ToNativeCollector();
            var deviceData = Collectors.DeviceData.ToNativeCollector();
            var connectivityData = Collectors.Connectivity.ToNativeCollector();

            Assert.AreEqual(TealiumNative.Collection.AppCollector.CompanionInstance, appData);
            Assert.AreEqual(TealiumNative.Collection.DeviceCollector.CompanionInstance, deviceData);
            Assert.AreEqual(TealiumNative.Collection.ConnectivityCollector.CompanionInstance, connectivityData);
        }

        [Test]
        public void Dispatchers_ToNative()
        {
            var collect = Dispatchers.Collect.ToNativeDispatcher();
            var tagMgmt = Dispatchers.TagManagement.ToNativeDispatcher();
            var remoteCommands = Dispatchers.RemoteCommands.ToNativeDispatcher();

            Assert.AreEqual(Com.Tealium.Collectdispatcher.CollectDispatcher.CompanionInstance, collect);
            Assert.AreEqual(Com.Tealium.Tagmanagementdispatcher.TagManagementDispatcher.CompanionInstance, tagMgmt);
            Assert.AreEqual(Com.Tealium.Remotecommanddispatcher.RemoteCommandDispatcher.CompanionInstance, remoteCommands);
        }

        [Test]
        public void LogLevel_ToNative()
        {
            var dev = LogLevel.Dev.ToNativeLogLevel();
            var qa = LogLevel.Qa.ToNativeLogLevel();
            var prod = LogLevel.Prod.ToNativeLogLevel();
            var silent = LogLevel.Silent.ToNativeLogLevel();

            Assert.AreEqual(TealiumNative.LogLevel.Dev, dev);
            Assert.AreEqual(TealiumNative.LogLevel.Qa, qa);
            Assert.AreEqual(TealiumNative.LogLevel.Prod, prod);
            Assert.AreEqual(TealiumNative.LogLevel.Silent, silent);
        }
    }
}
