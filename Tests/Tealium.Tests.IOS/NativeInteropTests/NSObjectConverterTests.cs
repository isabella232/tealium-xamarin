using System;
using System.Collections.Generic;
using Foundation;
using NUnit.Framework;
using Tealium.iOS.NativeInterop;

namespace Tealium.Tests.iOS.NativeInteropTests
{
    [TestFixture]
    public class NSObjectConverterTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        private void TestConversionAsObject<T>(object testVal)
        {
            NSObject obj = NSObject.FromObject(testVal);
            object result = NSObjectConverter.Convert(obj);

            Assert.AreEqual(typeof(T), result.GetType());
            Assert.AreEqual(testVal, result);
        }

        private void TestConversionToTargetType<T>(object testVal, object targetResult = null)
        {
            NSObject obj = NSObject.FromObject(testVal);
            T result = NSObjectConverter.ConvertToTargetType<T>(obj);

            Assert.AreEqual(typeof(T), result.GetType());
            Assert.AreEqual(targetResult ?? testVal, result);
        }

        #region NSObject -> CLR conversion

        [Test]
        public void ThrowsIfAttemptToConvertNullNSObject()
        {
            Assert.Throws<NullReferenceException>(() => NSObjectConverter.Convert(null));
        }

        [Test]
        public void ConvertsNSStringToStringAsObject()
        {
            string testVal = "test";
            TestConversionAsObject<string>(testVal);
        }

        [Test]
        public void ConvertsNSStringToString()
        {
            string testVal = "test";
            TestConversionToTargetType<string>(testVal);
        }

        [Test]
        public void ConvertsIntNSNumberToLongAsObject()
        {
            int testVal = int.MaxValue;
            TestConversionAsObject<long>(testVal);
        }

        [Test]
        public void ConvertsNSNumberToInt()
        {
            int testVal = int.MaxValue;
            TestConversionToTargetType<int>(testVal);
        }

        [Test]
        public void ConvertsLongNSNumberToLongAsObject()
        {
            long testVal = long.MaxValue;
            TestConversionAsObject<long>(testVal);
        }

        [Test]
        public void ConvertsNSNumberToLong()
        {
            long testVal = long.MaxValue;
            TestConversionToTargetType<long>(testVal);
        }

        [Test]
        public void ConvertsFloatNSNumberToFloatAsObject()
        {
            float testVal = int.MaxValue >> 8;
            TestConversionAsObject<float>(testVal);
        }

        [Test]
        public void ConvertsNSNumberToFloat()
        {
            float testVal = int.MaxValue >> 1;
            TestConversionToTargetType<float>(testVal);
        }

        [Test]
        public void ConvertsDoubleNSNumberToDoubleAsObject()
        {
            double testVal = int.MaxValue;
            TestConversionAsObject<double>(testVal);
        }

        [Test]
        public void ConvertsNSNumberToDouble()
        {
            double testVal = int.MaxValue;
            TestConversionToTargetType<double>(testVal);
        }

        [Test]
        public void ConvertsBooleanNSNumberToBoleanAsObject()
        {
            bool testVal = true;
            TestConversionAsObject<bool>(testVal);
        }

        [Test]
        public void ConvertsNSNumberToBool()
        {
            bool testVal = true;
            TestConversionToTargetType<bool>(testVal);
        }

        #endregion NSObject -> CLR conversion

        #region NSObject -> CLR string conversion

        [Test]
        public void ConvertsNSNumberIntToString()
        {
            int testVal = int.MaxValue;
            TestConversionToTargetType<string>(testVal, testVal.ToString());
        }

        [Test]
        public void ConvertsNSNumberLongToString()
        {
            long testVal = long.MaxValue;
            TestConversionToTargetType<string>(testVal, testVal.ToString());
        }

        [Test]
        public void ConvertsNSNumberFloatToString()
        {
            float testVal = int.MaxValue >> 1;
            TestConversionToTargetType<string>(testVal, testVal.ToString());
        }

        [Test]
        public void ConvertsNSNumberDoubleToString()
        {
            double testVal = int.MaxValue;
            TestConversionToTargetType<string>(testVal, testVal.ToString());
        }

        [Test]
        public void ConvertsNSNumberBoolToString()
        {
            bool testVal = true;
            TestConversionToTargetType<string>(testVal, testVal.ToString());
        }

        #endregion NSObject -> CLR string conversion


        #region CLR -> NSObject conversion

        [Test]
        public void ThrowsIfAttemptToConvertClrNull()
        {
            Assert.Throws<InvalidOperationException>(() => NSObjectConverter.ConvertBack(null));
        }

        [Test]
        public void ConvertsStringToNSString()
        {
            string clrTestVal = "test";
            var testVal = new NSString(clrTestVal);

            NSObject result = NSObjectConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsIntToNSNumber()
        {
            int clrTestVal = 3;
            var testVal = NSObject.FromObject(clrTestVal);

            NSObject result = NSObjectConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(typeof(NSNumber)/*testVal.GetType()*/, result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsLongToNSNumber()
        {
            long clrTestVal = long.MaxValue;
            var testVal = NSObject.FromObject(clrTestVal);

            NSObject result = NSObjectConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(typeof(NSNumber)/*testVal.GetType()*/, result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsFloatToNSNumber()
        {
            float clrTestVal = 3.0f;
            var testVal = NSObject.FromObject(clrTestVal);

            NSObject result = NSObjectConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(typeof(NSNumber)/*testVal.GetType()*/, result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsDoubleToNSNumber()
        {
            double clrTestVal = 3.0;
            var testVal = NSObject.FromObject(clrTestVal);

            NSObject result = NSObjectConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(typeof(NSNumber)/*testVal.GetType()*/, result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsBoolToNSNumber()
        {
            bool clrTestVal = true;
            var testVal = NSObject.FromObject(clrTestVal);

            NSObject result = NSObjectConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(typeof(NSNumber)/*testVal.GetType()*/, result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        #endregion CLR -> NSObject conversion

        #region NSDictionary/NSArray - CLR list/dictionary conversion

        [Test]
        public void ConvertsNSArrayToICollectionOfObjects()
        {
            PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2);

            ////we make a list from the ICollection to access data using indexer
            var result = new List<object>(NSObjectConverter.ConvertToTargetType<ICollection<object>>(array));

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsNSArrayToIListOfObjects()
        {
            PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2);

            var result = NSObjectConverter.ConvertToTargetType<IList<object>>(array);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsNSArrayToListOfObjects()
        {
            PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2);

            var result = NSObjectConverter.ConvertToTargetType<List<object>>(array);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsNSArrayToListOfStrings()
        {
            PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2);

            var result = NSObjectConverter.ConvertFromNSArrayToTargetType<string>(array);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsNSArrayToArrayOfStrings()
        {
            PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2);

            var result = NSObjectConverter.ConvertToTargetType<string[]>(array);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsNSArrayToListOfStringsAsObject()
        {
            PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2);

            var result = NSObjectConverter.Convert(array);

            Assert.AreEqual(typeof(List<object>), result.GetType());
            Assert.AreEqual(testVal1, ((List<object>)result)[0]);
            Assert.AreEqual(testVal2, ((List<object>)result)[1]);
        }

        void PrepareNSArrayForTest(out NSArray array, out string testVal1, out string testVal2)
        {
            testVal1 = nameof(testVal1);
            testVal2 = nameof(testVal2);
            array = NSArray.FromObjects(new NSString(testVal1), new NSString(testVal2));
        }

        [Test]
        public void ConvertsNSDictionaryToIDictionaryOfObjects()
        {
            PrepareNSDictionaryForTest(out NSDictionary dict, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = NSObjectConverter.ConvertToTargetType<IDictionary<string, object>>(dict);

            Assert.AreEqual(testVal1, result[testKey1]);
            Assert.AreEqual(testVal2, result[testKey2]);
        }

        [Test]
        public void ConvertsNSDictionaryToGenericDictionaryOfObjects()
        {
            PrepareNSDictionaryForTest(out NSDictionary dict, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = NSObjectConverter.ConvertToTargetType<Dictionary<string, object>>(dict);

            Assert.AreEqual(testVal1, result[testKey1]);
            Assert.AreEqual(testVal2, result[testKey2]);
        }

        [Test]
        public void ConvertsNSDictionaryToDictionaryOfStrings()
        {
            PrepareNSDictionaryForTest(out NSDictionary dict, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = NSObjectConverter.ConvertFromNSDictionaryToTargetType<string>(dict);

            Assert.AreEqual(testVal1, result[testKey1]);
            Assert.AreEqual(testVal2, result[testKey2]);
        }

        [Test]
        public void ConvertsNSDictionaryToDictionaryOfObjectsAsObject()
        {
            PrepareNSDictionaryForTest(out NSDictionary dict, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = NSObjectConverter.Convert(dict);

            Assert.AreEqual(typeof(Dictionary<string, object>), result.GetType());
            Assert.AreEqual(testVal1, ((Dictionary<string, object>)result)[testKey1]);
            Assert.AreEqual(testVal2, ((Dictionary<string, object>)result)[testKey2]);
        }

        void PrepareNSDictionaryForTest(out NSDictionary dict, out string testKey1, out string testVal1, out string testVal2, out string testKey2)
        {
            testKey1 = nameof(testKey1);
            testKey2 = nameof(testKey2);
            testVal1 = nameof(testVal1);
            testVal2 = nameof(testVal2);
            dict = NSDictionary.FromObjectsAndKeys(
                new NSObject[] { new NSString(testVal1), new NSString(testVal2) },
                new NSObject[] { new NSString(testKey1), new NSString(testKey2) } );
        }

        #endregion NSDictionary/NSArray - CLR list/dictionary conversion

        #region CLR list/dictionary - NSDictionary/NSArray conversion

        [Test]
        public void ConvertsArrayOfStringsToNSArray()
        {
            string testVal1 = nameof(testVal1);
            string testVal2 = nameof(testVal2);
            var list = new string[2]
            {
                testVal1,
                testVal2
            };

            ConvertsIColectionOfStringsToNSArray(list, testVal1, testVal2);
        }

        [Test]
        public void ConvertsListOfStringsToNSArray()
        {
            string testVal1 = nameof(testVal1);
            string testVal2 = nameof(testVal2);
            List<string> list = new List<string>(2)
            {
                testVal1,
                testVal2
            };

            ConvertsIColectionOfStringsToNSArray(list, testVal1, testVal2);
        }

        void ConvertsIColectionOfStringsToNSArray(ICollection<string> list, string testVal1, string testVal2)
        {
            var result = NSObjectConverter.ConvertBack(list);

            Assert.AreEqual(typeof(NSArray), result.GetType());
            string resultStr1 = ((NSArray)result).GetItem<NSString>(0);
            string resultStr2 = ((NSArray)result).GetItem<NSString>(1);
            Assert.IsTrue(testVal1 == resultStr1, $"The result string {resultStr1} does not equal {testVal1}!");
            Assert.IsTrue(testVal2 == resultStr2, $"The result string {resultStr2} does not equal {testVal2}!");
            Assert.AreEqual(testVal1, resultStr1);
            Assert.AreEqual(testVal2, resultStr2);
        }

        [Test]
        public void ConvertsDictionaryOfStringsToNSDictionary()
        {
            string testKey1 = nameof(testKey1);
            string testKey2 = nameof(testKey2);
            string testVal1 = nameof(testVal1);
            string testVal2 = nameof(testVal2);
            Dictionary<string, string> dict = new Dictionary<string, string>(2)
            {
                { testKey1, testVal1 },
                { testKey2, testVal2 },
            };

            ////Dictionary<string, string> implements IDictionary<string, string>, so no need to write separate tests for this type as input
            var result = NSObjectConverter.ConvertBack(dict);

            Assert.IsTrue(result is NSDictionary, Utils.TypeMismatchMessage(result, typeof(NSDictionary)));
            string resultStr1 = ((NSDictionary)result)[testKey1].ToString();
            string resultStr2 = ((NSDictionary)result)[testKey2].ToString();
            Assert.IsTrue(testVal1 == resultStr1, $"The result string {resultStr1} does not equal {testVal1}!");
            Assert.IsTrue(testVal2 == resultStr2, $"The result string {resultStr2} does not equal {testVal2}!");
            Assert.AreEqual(testVal1, resultStr1);
            Assert.AreEqual(testVal2, resultStr2);
        }

        #endregion CLR list/dictionary - NSDictionary/NSArray conversion
    }
}
