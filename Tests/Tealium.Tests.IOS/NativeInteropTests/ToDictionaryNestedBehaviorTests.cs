using System;
using System.Linq;
using NUnit.Framework;
using Tealium.iOS.NativeInterop.Extensions;
using Foundation;
using System.Collections.Generic;

namespace Tealium.Tests.iOS.NativeInteropTests
{
    /**
    *  Using toDictionary (as well as toList) is only supported for directly casting NSDictionaries and NSArrays of raw types or generic objects.
    *  Casting nested dictionary has to be made with the specific method for nested Dictionaries, Arrays and Sets.
    *  
    *  These tests are here just for understanding purposes and for future reference on the method behavior. 
    *  Should you have to change the behavior and break some test, feel free to change the behavior and change those tests too.
    *  
    *  Asserts are kept just to assert the behavior that, once again, is just here for understanding purposes. 
    *  Changing the behavior of this metod in relation to nested arrays/dicts is NOT prevented.
    *  
    *  Do NOT use these as a reference on how to use the ToDicitonary method. This is exactly how NOT to use it.
    */
    [TestFixture]
    public class ToDictionaryNestedBehaviorTests
    {

        
        [Test]
        public void NSDictionary_ToDict_Generic_DictOfStrings()
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

            Dictionary<string, Dictionary<string, string>> res = dict.ToDictionary<Dictionary<string, string>, NSDictionary<NSString, NSString>>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);
        }

        [Test]
        public void NSDictionary_ToDict_NonGeneric_DictOfStrings()
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
            NSDictionary<NSString, NSDictionary> dict = NSDictionary<NSString, NSDictionary>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, Dictionary<string, string>> res = dict.ToDictionary<Dictionary<string, string>, NSDictionary>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);
        }

        [Test]
        public void NSDictionary_ToDict_Generic_ArrayOfStrings()
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

            Dictionary<string, List<string>> res = dict.ToDictionary<List<string>, NSArray<NSString>>();
            Assert.AreEqual(first, res[firstKey][0]);
            Assert.AreEqual(second, res[firstKey][1]);
            Assert.AreEqual(third, res[secondKey][0]);
            Assert.AreEqual(fourth, res[secondKey][1]);
        }

        [Test]
        public void NSDictionary_ToDict_NonGeneric_ArrayOfStrings()
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

            NSDictionary<NSString, NSArray> dict = NSDictionary<NSString, NSArray>.FromObjectsAndKeys(arr, keyStrings, 2);

            Dictionary<string, List<string>> res = dict.ToDictionary<List<string>, NSArray>();
            Assert.AreEqual(first, res[firstKey][0]);
            Assert.AreEqual(second, res[firstKey][1]);
            Assert.AreEqual(third, res[secondKey][0]);
            Assert.AreEqual(fourth, res[secondKey][1]);
        }

        [Test]
        public void NSDictionary_ToDict_Generic_DictOfDecimals()
        {
            var first = 1;
            var second = 2;
            var firstKey = "key1";
            var secondKey = "key2";

            var third = 3;
            var fourth = 4;
            var thirdKey = "key3";
            var fourthKey = "key4";
            NSNumber[] strings12 = { new NSNumber(first), new NSNumber(second) };
            NSNumber[] strings34 = { new NSNumber(third), new NSNumber(fourth) };
            NSString[] keyStrings12 = { new NSString(firstKey), new NSString(secondKey) };
            NSString[] keyStrings34 = { new NSString(thirdKey), new NSString(fourthKey) };
            NSDictionary<NSString, NSNumber> dict12 = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings12, keyStrings12, 2);
            NSDictionary<NSString, NSNumber> dict34 = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings34, keyStrings34, 2);
            NSDictionary<NSString, NSNumber>[] dictArray = { dict12, dict34 };
            NSDictionary<NSString, NSDictionary<NSString, NSNumber>> dict = NSDictionary<NSString, NSDictionary<NSString, NSNumber>>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, Dictionary<string, object>> res = dict.ToDictionary<Dictionary<string, object>, NSDictionary<NSString, NSNumber>>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);


            // This Crashes! We can't get the inner target type to cast NSNumbers to Decimals!
            Assert.Throws<InvalidCastException>(() => dict.ToDictionary<Dictionary<string, decimal>, NSDictionary<NSString, NSNumber>>());
        }

        [Test]
        public void NSDictionary_ToDict_NonGeneric_DictOfDecimals()
        {
            var first = 1;
            var second = 2;
            var firstKey = "key1";
            var secondKey = "key2";

            var third = 3;
            var fourth = 4;
            var thirdKey = "key3";
            var fourthKey = "key4";
            NSNumber[] strings12 = { new NSNumber(first), new NSNumber(second) };
            NSNumber[] strings34 = { new NSNumber(third), new NSNumber(fourth) };
            NSString[] keyStrings12 = { new NSString(firstKey), new NSString(secondKey) };
            NSString[] keyStrings34 = { new NSString(thirdKey), new NSString(fourthKey) };
            NSDictionary dict12 = NSDictionary.FromObjectsAndKeys(strings12, keyStrings12, 2);
            NSDictionary dict34 = NSDictionary.FromObjectsAndKeys(strings34, keyStrings34, 2);
            NSDictionary[] dictArray = { dict12, dict34 };
            NSDictionary<NSString, NSDictionary> dict = NSDictionary<NSString, NSDictionary>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, Dictionary<string, object>> res = dict.ToDictionary<Dictionary<string, object>, NSDictionary>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);

            // This Crashes! We can't get the inner target type to cast NSNumbers to Decimals!
            Assert.Throws<InvalidCastException>(() => dict.ToDictionary<Dictionary<string, decimal>, NSDictionary>());
        }

        [Test]
        public void NSDictionary_ToDict_Generic_DictOfBools()
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
            NSDictionary<NSString, NSNumber> dict12 = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings12, keyStrings12, 2);
            NSDictionary<NSString, NSNumber> dict34 = NSDictionary<NSString, NSNumber>.FromObjectsAndKeys(strings34, keyStrings34, 2);
            NSDictionary<NSString, NSNumber>[] dictArray = { dict12, dict34 };
            NSDictionary<NSString, NSDictionary<NSString, NSNumber>> dict = NSDictionary<NSString, NSDictionary<NSString, NSNumber>>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, Dictionary<string, object>> res = dict.ToDictionary<Dictionary<string, object>, NSDictionary<NSString, NSNumber>>();
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);

            // This Crashes! We can't get the inner target type to cast NSNumbers to Bools!
            Assert.Throws<InvalidCastException>(() => dict.ToDictionary<Dictionary<string, bool>, NSDictionary<NSString, NSNumber>>());
        }

        [Test]
        public void NSDictionary_ToDict_NonGeneric_DictOfBools()
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
            NSDictionary dict12 = NSDictionary.FromObjectsAndKeys(strings12, keyStrings12, 2);
            NSDictionary dict34 = NSDictionary.FromObjectsAndKeys(strings34, keyStrings34, 2);
            NSDictionary[] dictArray = { dict12, dict34 };
            NSDictionary<NSString, NSDictionary> dict = NSDictionary<NSString, NSDictionary>.FromObjectsAndKeys(dictArray, keyStrings12, 2);

            Dictionary<string, Dictionary<string, object>> res = dict.ToDictionary<Dictionary<string, object>, NSDictionary>();
            Assert.IsTrue(res[firstKey][firstKey] is bool, Utils.TypeMismatchMessage(res[firstKey][firstKey], typeof(bool)));
            Assert.IsTrue(res[firstKey][secondKey] is bool, Utils.TypeMismatchMessage(res[firstKey][secondKey], typeof(bool)));
            Assert.IsTrue(res[secondKey][thirdKey] is bool, Utils.TypeMismatchMessage(res[secondKey][thirdKey], typeof(bool)));
            Assert.IsTrue(res[secondKey][fourthKey] is bool, Utils.TypeMismatchMessage(res[secondKey][fourthKey], typeof(bool)));
            Assert.AreEqual(first, res[firstKey][firstKey]);
            Assert.AreEqual(second, res[firstKey][secondKey]);
            Assert.AreEqual(third, res[secondKey][thirdKey]);
            Assert.AreEqual(fourth, res[secondKey][fourthKey]);

            // This Crashes! We can't get the inner target type to cast NSNumbers to Bool!
            Assert.Throws<InvalidCastException>(() => dict.ToDictionary<Dictionary<string, bool>, NSDictionary>());
        }
    }
}
