using System;
using System.Linq;
using NUnit.Framework;
using Tealium.iOS.NativeInterop.Extensions;
using Tealium.Platform.iOS;
using Foundation;
using System.Collections.Generic;

namespace Tealium.Tests.iOS.NativeInteropTests
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
                    "overrideConsentCategoriesKey");

            TealiumConfigWrapper nativeConfig = config.ToNativeConfig();

            // Account info
            Assert.AreEqual(config.Account, nativeConfig.Account);
            Assert.AreEqual(config.Profile, nativeConfig.Profile);
            Assert.AreEqual("dev", nativeConfig.Environment);

            // Dispatchers
            Assert.True(nativeConfig.Dispatchers.Contains(new NSNumber(((int)DispatcherType.Collect))));
            Assert.True(nativeConfig.Dispatchers.Contains(new NSNumber(((int)DispatcherType.TagManagement))));
            Assert.True(nativeConfig.Dispatchers.Contains(new NSNumber(((int)DispatcherType.RemoteCommands))));

            // Collectors
            Assert.True(nativeConfig.Collectors.Contains(new NSNumber(((int)CollectorType.AppData))));
            Assert.True(nativeConfig.Collectors.Contains(new NSNumber(((int)CollectorType.DeviceData))));
            Assert.True(nativeConfig.Collectors.Contains(new NSNumber(((int)CollectorType.Connectivity))));
            Assert.True(nativeConfig.Collectors.Contains(new NSNumber(((int)CollectorType.Lifecycle))));
            Assert.True(nativeConfig.Collectors.Contains(new NSNumber(((int)CollectorType.VisitorService)))); // Due to visitorServiceEnabled

            // Misc
            Assert.AreEqual("dataSource", nativeConfig.DataSourceKey);
            Assert.True(nativeConfig.DeepLinkTrackingEnabled);
            Assert.True(nativeConfig.QrTraceEnabled);
            Assert.AreEqual("overrideLibrarySettingsUrl", nativeConfig.PublishSettingsURL);
            Assert.True(nativeConfig.ShouldUseRemotePublishSettings);
            Assert.AreEqual("visitorIdentityKey", nativeConfig.VisitorIdentityKey);
            

            // Lifecycle
            Assert.True((bool)nativeConfig.LifecycleAutoTrackingEnabled);

            // Collect
            Assert.AreEqual("overrideCollectUrl", nativeConfig.OverrideCollectURL);
            Assert.AreEqual("overrideBatchCollectUrl", nativeConfig.OverrideCollectBatchURL);
            Assert.AreEqual("overrideCollectDomain", nativeConfig.OverrideCollectDomain);

            // TagManagement
            Assert.AreEqual("overrideTagManagementUrl", nativeConfig.TagManagementOverrideURL);

            // Consent
            Assert.True(nativeConfig.ConsentLoggingEnabled);
            Assert.AreEqual(TealiumConsentPolicyWrapper.Gdpr, nativeConfig.ConsentPolicy);
            var consentExpiry = nativeConfig.ConsentExpiry;
            Assert.AreEqual(10, consentExpiry.Time);
            Assert.AreEqual(TimeUnitWrapper.Days, consentExpiry.Unit);
            Assert.AreEqual("overrideConsentCategoriesKey", nativeConfig.OverrideConsentCategoriesKey);
        }

        [Test]
        public void ConsentStatus_ToNative()
        {
            var consented = ConsentManager.ConsentStatus.Consented.ToNativeStatus();
            var notConsented = ConsentManager.ConsentStatus.NotConsented.ToNativeStatus();
            var unknown = ConsentManager.ConsentStatus.Unknown.ToNativeStatus();

            Assert.AreEqual(TealiumConsentStatusWrapper.Consented, consented);
            Assert.AreEqual(TealiumConsentStatusWrapper.NotConsented, notConsented);
            Assert.AreEqual(TealiumConsentStatusWrapper.Unknown, unknown);
        }

        [Test]
        public void ConsentStatus_FromNative()
        {
            var consented = TealiumConsentStatusWrapper.Consented.ToStatus();
            var notConsented = TealiumConsentStatusWrapper.NotConsented.ToStatus();
            var unknown = TealiumConsentStatusWrapper.Unknown.ToStatus();

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

            Assert.AreEqual(TealiumConsentCategoriesWrappers.Affiliates, affiliates);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Analytics, analytics);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Crm, crm);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.CookieMatch, cookieMatch);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Cdp, cdp);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.DisplayAds, displayAds);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Email, email);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Engagement, engagement);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Misc, misc);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Mobile, mobile);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Monitoring, monitoring);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Personalization, personalization);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Search, search);
            Assert.AreEqual(TealiumConsentCategoriesWrappers.Social, social);
        }

        [Test]
        public void ConsentCategory_FromNative()
        {
            var affiliates = new NSNumber((long)TealiumConsentCategoriesWrappers.Affiliates).ToCategory();
            var analytics = new NSNumber((long)TealiumConsentCategoriesWrappers.Analytics).ToCategory();
            var crm = new NSNumber((long)TealiumConsentCategoriesWrappers.Crm).ToCategory();
            var cookieMatch = new NSNumber((long)TealiumConsentCategoriesWrappers.CookieMatch).ToCategory();
            var cdp = new NSNumber((long)TealiumConsentCategoriesWrappers.Cdp).ToCategory();
            var displayAds = new NSNumber((long)TealiumConsentCategoriesWrappers.DisplayAds).ToCategory();
            var email = new NSNumber((long)TealiumConsentCategoriesWrappers.Email).ToCategory();
            var engagement = new NSNumber((long)TealiumConsentCategoriesWrappers.Engagement).ToCategory();
            var misc = new NSNumber((long)TealiumConsentCategoriesWrappers.Misc).ToCategory();
            var mobile = new NSNumber((long)TealiumConsentCategoriesWrappers.Mobile).ToCategory();
            var monitoring = new NSNumber((long)TealiumConsentCategoriesWrappers.Monitoring).ToCategory();
            var personalization = new NSNumber((long)TealiumConsentCategoriesWrappers.Personalization).ToCategory();
            var search = new NSNumber((long)TealiumConsentCategoriesWrappers.Search).ToCategory();
            var social = new NSNumber((long)TealiumConsentCategoriesWrappers.Social).ToCategory();

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
            Assert.AreEqual(TimeUnitWrapper.Minutes, oneMinutes.Unit);
            Assert.AreEqual(2, twoHours.Time);
            Assert.AreEqual(TimeUnitWrapper.Hours, twoHours.Unit);
            Assert.AreEqual(3, threeDays.Time);
            Assert.AreEqual(TimeUnitWrapper.Days, threeDays.Unit);
            Assert.AreEqual(3, threeMonths.Time);
            Assert.AreEqual(TimeUnitWrapper.Months, threeMonths.Unit);
        }

        [Test]
        public void ConsentPolicy_ToNative()
        {
            var gdpr = ConsentManager.ConsentPolicy.GDPR.ToNativePolicy();
            var ccpa = ConsentManager.ConsentPolicy.CCPA.ToNativePolicy();

            Assert.AreEqual(TealiumConsentPolicyWrapper.Gdpr, gdpr);
            Assert.AreEqual(TealiumConsentPolicyWrapper.Ccpa, ccpa);
        }

        [Test]
        public void ConsentPolicy_FromNative()
        {
            var gdpr = TealiumConsentPolicyWrapper.Gdpr.ToPolicy();
            var ccpa = TealiumConsentPolicyWrapper.Ccpa.ToPolicy();

            Assert.AreEqual(ConsentManager.ConsentPolicy.GDPR, gdpr);
            Assert.AreEqual(ConsentManager.ConsentPolicy.CCPA, ccpa);
        }

        [Test]
        public void Expiry_ToNative()
        {
            var session = Expiry.Session.ToNativeExpiry();
            var forever = Expiry.Forever.ToNativeExpiry();
            var untilRestart = Expiry.UntilRestart.ToNativeExpiry();

            Assert.AreEqual(ExpiryWrapper.Session, session);
            Assert.AreEqual(ExpiryWrapper.Forever, forever);
            Assert.AreEqual(ExpiryWrapper.UntilRestart, untilRestart);
        }

        [Test]
        public void Collectors_ToNative()
        {
            var appData = Collectors.AppData.ToNativeCollector();
            var deviceData = Collectors.DeviceData.ToNativeCollector();
            var connectivityData = Collectors.Connectivity.ToNativeCollector();
            var lifecycle = Collectors.LifeCycle.ToNativeCollector();

            Assert.AreEqual(CollectorType.AppData, appData);
            Assert.AreEqual(CollectorType.DeviceData, deviceData);
            Assert.AreEqual(CollectorType.Connectivity, connectivityData);
            Assert.AreEqual(CollectorType.Lifecycle, lifecycle);
        }

        [Test]
        public void Dispatchers_ToNative()
        {
            var collect = Dispatchers.Collect.ToNativeDispatcher();
            var tagMgmt = Dispatchers.TagManagement.ToNativeDispatcher();
            var remoteCommands = Dispatchers.RemoteCommands.ToNativeDispatcher();

            Assert.AreEqual(DispatcherType.Collect, collect);
            Assert.AreEqual(DispatcherType.TagManagement, tagMgmt);
            Assert.AreEqual(DispatcherType.RemoteCommands, remoteCommands);
        }

        [Test]
        public void LogLevel_ToNative()
        {
            var dev = LogLevel.Dev.ToNativeLogLevel();
            var qa = LogLevel.Qa.ToNativeLogLevel();
            var prod = LogLevel.Prod.ToNativeLogLevel();
            var silent = LogLevel.Silent.ToNativeLogLevel();

            Assert.AreEqual(TealiumLogLevelWrapper.Info, dev);
            Assert.AreEqual(TealiumLogLevelWrapper.Debug, qa);
            Assert.AreEqual(TealiumLogLevelWrapper.Error, prod);
            Assert.AreEqual(TealiumLogLevelWrapper.Silent, silent);
        }


        /// List
        [Test]
        public void NSArray_ToList_Strings()
        {
            var first = "1";
            var second = "2";
            NSString[] strings = { new NSString(first), new NSString(second) };
            NSArray<NSString> arr = NSArray<NSString>.FromNSObjects(strings);

            List<string> res = arr.ToList<string, NSString>();
            Assert.AreEqual(first, res[0]);
            Assert.AreEqual(second, res[1]);
        }

        [Test]
        public void NSArray_ToList_Dates()
        {
            var first = new NSDate();
            var second = first.AddSeconds(10);
            NSDate[] strings = { first, second };
            NSArray<NSDate> arr = NSArray<NSDate>.FromNSObjects(strings);

            List<DateTime> res = arr.ToList<DateTime, NSDate>();
            var startTime = new DateTime(1970, 1, 1);
            Assert.AreEqual(Math.Round(first.SecondsSince1970), Math.Round(res[0].Subtract(startTime).TotalSeconds));
            Assert.AreEqual(Math.Round(second.SecondsSince1970), Math.Round(res[1].Subtract(startTime).TotalSeconds));
        }

        [Test]
        public void NSArray_ToList_Bools()
        {
            var first = true;
            var second = false;
            NSNumber[] strings = { new NSNumber(first), new NSNumber(second) };
            NSArray<NSNumber> arr = NSArray<NSNumber>.FromNSObjects(strings);

            List<bool> res = arr.ToList<bool, NSNumber>();
            Assert.AreEqual(first, res[0]);
            Assert.AreEqual(second, res[1]);
        }

        [Test]
        public void NSArray_ToList_Decimals()
        {
            var first = (decimal)1;
            var second = (decimal)2;
            NSDecimalNumber[] strings = { new NSDecimalNumber(first), new NSDecimalNumber(second) };
            NSArray<NSDecimalNumber> arr = NSArray<NSDecimalNumber>.FromNSObjects(strings);

            List<decimal> res = arr.ToList<decimal, NSDecimalNumber>();
            Assert.AreEqual(first, res[0]);
            Assert.AreEqual(second, res[1]);
        }


        /// Dict
        [Test]
        public void NSDictionary_ToDict_Strings()
        {
            var first = "1";
            var second = "2";
            var firstKey = "key1";
            var secondKey = "key2";
            NSString[] strings = { new NSString(first), new NSString(second) };
            NSString[] keyStrings = { new NSString(firstKey), new NSString(secondKey) };
            NSDictionary<NSString, NSString> dict = NSDictionary<NSString, NSString>.FromObjectsAndKeys(strings, keyStrings, 2);

            Dictionary<string, string> res = dict.ToDictionary<string, NSString>();
            Assert.AreEqual(first, res[firstKey]);
            Assert.AreEqual(second, res[secondKey]);
        }

        [Test]
        public void NSDictionary_ToDict_Bools()
        {
            var first = true;
            var second = false;
            var firstKey = "key1";
            var secondKey = "key2";
            NSNumber[] strings = { new NSNumber(first), new NSNumber(second) };
            NSString[] keyStrings = { new NSString(firstKey), new NSString(secondKey) };
            NSDictionary<NSString, NSNumber> dict = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings, keyStrings, 2);

            Dictionary<string, bool> res = dict.ToDictionary<bool, NSNumber>();
            Assert.AreEqual(first, res[firstKey]);
            Assert.AreEqual(second, res[secondKey]);
        }

        [Test]
        public void NSDictionary_ToDict_Mixed()
        {
            var first = "1";
            var second = 2;
            var third = new NSDate();
            var fourth = true;
            var firstKey = "key1";
            var secondKey = "key2";
            var thirdKey = "key3";
            var fourthKey = "key4";
            NSObject[] objects = { new NSString(first), new NSNumber(second), third, new NSNumber(fourth) };
            NSString[] keyStrings = { new NSString(firstKey), new NSString(secondKey), new NSString(thirdKey), new NSString(fourthKey) };
            NSDictionary<NSString, NSObject> dict = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(objects, keyStrings, 4);

            Dictionary<string, object> res = dict.ToDictionary<object, NSObject>();
            Assert.AreEqual(first, res[firstKey]);
            Assert.AreEqual(second, res[secondKey]);
            var startTime = new DateTime(1970, 1, 1);
            Assert.AreEqual(Math.Round(third.SecondsSince1970), Math.Round(((DateTime)res[thirdKey]).Subtract(startTime).TotalSeconds));
            Assert.AreEqual(fourth, res[fourthKey]);
        }

        [Test]
        public void NSDictionary_ToDict_Mixed_Nested()
        {
            var first = "1";
            var second = 2;
            var third = new NSDate();
            var aBool = true;
            var aDouble = double.MaxValue;
            var firstKey = "key1";
            var secondKey = "key2";
            var thirdKey = "key3";
            var fourthKey = "key4";
            var fifthKey = "key5";
            var sixthKey = "key6";
            var seventhKey = "key7";
            var eighthKey = "key8";
            NSString[] strings = { new NSString(first) };
            NSNumber[] numbers = { new NSNumber(second), new NSNumber(aBool), new NSNumber(aDouble) };
            NSObject[] mixed = { new NSString(first), new NSNumber(second), third };
            NSDate[] dates = { third };
            NSObject[] objects = {
                new NSString(first),
                new NSNumber(second),
                third,
                NSArray<NSString>.FromNSObjects(strings),
                NSArray<NSNumber>.FromNSObjects(numbers),
                NSArray<NSObject>.FromNSObjects(mixed),
                NSArray.FromObjects(mixed),
                NSArray<NSDate>.FromNSObjects(dates)
            };
            NSString[] keyStrings = {
                new NSString(firstKey),
                new NSString(secondKey),
                new NSString(thirdKey),
                new NSString(fourthKey),
                new NSString(fifthKey),
                new NSString(sixthKey),
                new NSString(seventhKey),
                new NSString(eighthKey)
            };

            NSDictionary<NSString, NSObject> dict = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(objects, keyStrings, 8);

            Dictionary<string, object> res = dict.ToDictionary<object, NSObject>();
            Assert.AreEqual(first, res[firstKey]);
            Assert.AreEqual(second, res[secondKey]);
            var startTime = new DateTime(1970, 1, 1);
            Assert.AreEqual(Math.Round(third.SecondsSince1970), Math.Round(((DateTime)res[thirdKey]).Subtract(startTime).TotalSeconds));

            // TODO: use a better assert by upgrading NUnit?
            Assert.IsTrue(res[fourthKey] is List<string>, Utils.TypeMismatchMessage(res[fourthKey], typeof(List<string>)));
            Assert.IsTrue(res[fifthKey] is List<object>, Utils.TypeMismatchMessage(res[fifthKey], typeof(List<object>)));

            Assert.AreEqual(((List<object>)res[fifthKey])[0], second);
            Assert.AreEqual(((List<object>)res[fifthKey])[1], aBool);
            Assert.AreEqual(((List<object>)res[fifthKey])[2], aDouble);

            Assert.IsTrue(res[sixthKey] is List<object>, Utils.TypeMismatchMessage(res[sixthKey], typeof(List<object>)));
            Assert.IsTrue(res[seventhKey] is List<object>, Utils.TypeMismatchMessage(res[seventhKey], typeof(List<object>)));
            Assert.IsTrue(res[eighthKey] is List<DateTime>, Utils.TypeMismatchMessage(res[eighthKey], typeof(List<DateTime>)));
        }

        /// Dict of Dict
        [Test]
        public void NSDictionary_ToDictOfDict_GenericDictOfStrings()
        {
            var first = "1";
            var second = "2";
            var firstKey = "key1";
            var secondKey = "key2";

            var third = "3";
            var fourth = "4";
            var thirdKey = "key3";
            var fourthKey = "key4";
            NSString[] strings12 = { new NSString(first), new NSString(second) };
            NSString[] strings34 = { new NSString(third), new NSString(fourth) };
            NSString[] keyStrings12 = { new NSString(firstKey), new NSString(secondKey) };
            NSString[] keyStrings34 = { new NSString(thirdKey), new NSString(fourthKey) };
            NSDictionary<NSString, NSString> dict12 = NSDictionary<NSString, NSString>.FromObjectsAndKeys(strings12, keyStrings12, 2);
            NSDictionary<NSString, NSString> dict34 = NSDictionary<NSString, NSString>.FromObjectsAndKeys(strings34, keyStrings34, 2);
            NSDictionary<NSString, NSString>[] dictArray = { dict12, dict34 };
            NSDictionary<NSString, NSDictionary<NSString, NSString>> dict = NSDictionary<NSString, NSDictionary<NSString, NSString>>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, IDictionary<string, string>> res = dict.ToDictionaryOfDictionaries<string, NSString>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);
        }

        [Test]
        public void NSDictionary_ToDictOfDict_GenericDictOfBools()
        {
            var first = true;
            var second = false;
            var firstKey = "key1";
            var secondKey = "key2";

            var third = false;
            var fourth = true;
            var thirdKey = "key3";
            var fourthKey = "key4";
            NSNumber[] strings12 = { new NSNumber(first), new NSNumber(second) };
            NSNumber[] strings34 = { new NSNumber(third), new NSNumber(fourth) };
            NSString[] keyStrings12 = { new NSString(firstKey), new NSString(secondKey) };
            NSString[] keyStrings34 = { new NSString(thirdKey), new NSString(fourthKey) };
            NSDictionary dict12 = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings12, keyStrings12, 2);
            NSDictionary dict34 = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings34, keyStrings34, 2);
            NSDictionary[] dictArray = { dict12, dict34 };
            NSDictionary<NSString, NSDictionary<NSString, NSNumber>> dict = NSDictionary<NSString, NSDictionary<NSString, NSNumber>>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, IDictionary<string, bool>> res = dict.ToDictionaryOfDictionaries<bool, NSNumber>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);
        }


        /// Dict of List
        [Test]
        public void NSDictionary_ToDictOfList_GenericArrayOfStrings()
        {
            var first = "1";
            var second = "2";
            var firstKey = "key1";
            var secondKey = "key2";

            var third = "3";
            var fourth = "4";
            NSString[] strings12 = { new NSString(first), new NSString(second) };
            NSString[] strings34 = { new NSString(third), new NSString(fourth) };
            NSArray<NSString> arr1 = NSArray<NSString>.FromNSObjects(strings12);
            NSArray<NSString> arr2 = NSArray<NSString>.FromNSObjects(strings34);

            NSString[] keyStrings = { new NSString(firstKey), new NSString(secondKey) };
            NSArray<NSString>[] arr = { arr1, arr2 };

            NSDictionary<NSString, NSArray<NSString>> dict = NSDictionary<NSString, NSArray<NSString>>.FromObjectsAndKeys(arr, keyStrings, 2);

            Dictionary<string, IList<string>> res = dict.ToDictionaryOfLists<string, NSString>();
            Assert.AreEqual(first, res[firstKey][0]);
            Assert.AreEqual(second, res[firstKey][1]);
            Assert.AreEqual(third, res[secondKey][0]);
            Assert.AreEqual(fourth, res[secondKey][1]);
        }

    }
}
