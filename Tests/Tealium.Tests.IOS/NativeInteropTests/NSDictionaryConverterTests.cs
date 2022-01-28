using System;
using System.Collections.Generic;
using Foundation;
using NUnit.Framework;
using Tealium.iOS.NativeInterop;

namespace Tealium.Tests.iOS.NativeInteropTests
{
    [TestFixture]
    public class NSDictionaryConverterTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }


        [Test]
        public void ReturnsNullClrDictForNullNSDict()
        {
            NSDictionary nsDict = null;

            var resultDict = NSDictionaryConverter.Convert(nsDict);

            Assert.IsNull(resultDict);
        }

        [Test]
        public void ReturnsNullNSDictForNullClrDict()
        {
            Dictionary<string, object> clrDict = null;

            var resultDict = NSDictionaryConverter.ConvertBack<NSObject>(clrDict);

            Assert.IsNull(resultDict);
        }

        [Test]
        public void ReturnsEmptyNSDictForEmptyClrDict()
        {
            Dictionary<string, object> clrDict = new Dictionary<string, object>(0);

            var resultDict = NSDictionaryConverter.ConvertBack<NSObject>(clrDict);

            Assert.IsNotNull(resultDict);
            Assert.AreEqual(0, resultDict.Count);
        }

        [Test]
        public void ReturnsEmptyClrDictForEmptyJavaDict()
        {
            NSDictionary nsDict = new NSDictionary();

            var resultDict = NSDictionaryConverter.Convert(nsDict);

            Assert.IsNotNull(resultDict);
            Assert.AreEqual(0, resultDict.Count);
        }

        [Test]
        public void ConvertsClrDictToNSDictAndBack()
        {
            object[] testValues =
            {
                "test string",
                int.MaxValue,
                long.MaxValue,
                (double)int.MaxValue,
                (float)(int.MaxValue >> 8),
                true
            };
            Dictionary<string, object> testDict = new Dictionary<string, object>(testValues.Length);
            for (int i = 0; i < testValues.Length; i++)
            {
                testDict.Add((i + 1).ToString(), testValues[i]);
            }

            var nsDict = NSDictionaryConverter.ConvertBack<NSObject>(testDict);
            var resultDict = NSDictionaryConverter.Convert(nsDict);

            Assert.NotNull(resultDict);
            Assert.AreEqual(resultDict.Count, testDict.Count);
            for (int i = 0; i < testValues.Length; i++)
            {
                object testObj = testDict[(i + 1).ToString()];
                object resultObj = resultDict[(i + 1).ToString()];

                Assert.AreEqual(testObj, resultObj);
                var type = testObj.GetType();
                if (type == typeof(int))
                {
                    type = typeof(long);
                }
                Assert.AreEqual(type, resultObj.GetType());
            }
        }

        [Test]
        public void ConvertsNSDictToClrDictAndBack()
        {
            List<object> testClrValues = new List<object>()
            {
                "test string",
                int.MaxValue,
                long.MaxValue,
                double.MaxValue,
                float.MaxValue,
                true
            };
            List<NSObject> testNSValues = testClrValues.ConvertAll(val => NSObject.FromObject(val));
            NSMutableDictionary testDict = new NSMutableDictionary();
            for (int i = 0; i < testNSValues.Count; i++)
            {
                testDict.SetValueForKey(testNSValues[i], new NSString((i + 1).ToString()));
            }

            var clrDict = NSDictionaryConverter.Convert(testDict);
            var resultDict = NSDictionaryConverter.ConvertBack<NSObject>(clrDict);

            Assert.NotNull(resultDict);
            Assert.AreEqual(resultDict.Count, testDict.Count);
            for (int i = 0; i < testClrValues.Count; i++)
            {
                NSObject testObj = testDict[(i + 1).ToString()];
                NSObject resultObj = resultDict[(i + 1).ToString()];


                Assert.AreEqual(testObj, resultObj);
                var type = testObj.GetType();
                Assert.AreEqual(type, resultObj.GetType());
            }
        }
    }
}
