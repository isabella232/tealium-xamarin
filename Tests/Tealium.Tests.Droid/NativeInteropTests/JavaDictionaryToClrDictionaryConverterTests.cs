using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tealium.Droid.NativeInterop;

namespace Tealium.Tests.Droid.NativeInteropTests
{
    [TestFixture]
    public class JavaDictionaryToClrDictionaryConverterTests
    {

        [Test]
        public void ReturnsNullClrDictForNullJavaDict()
        {
            Dictionary<string, Java.Lang.Object> javaDict = null;

            var resultDict = JavaDictionaryToClrDictionaryConverter.Convert(javaDict);

            Assert.IsNull(resultDict);
        }

        [Test]
        public void ReturnsNullJavaDictForNullClrDict()
        {
            Dictionary<string, object> clrDict = null;

            var resultDict = JavaDictionaryToClrDictionaryConverter.ConvertBack(clrDict);

            Assert.IsNull(resultDict);
        }

        [Test]
        public void ReturnsEmptyJavaDictForEmptyClrDict()
        {
            Dictionary<string, object> clrDict = new Dictionary<string, object>(0);

            var resultDict = JavaDictionaryToClrDictionaryConverter.ConvertBack(clrDict);

            Assert.IsNotNull(resultDict);
            Assert.AreEqual(0, resultDict.Count);
        }

        [Test]
        public void ReturnsEmptyClrDictForEmptyJavaDict()
        {
            Dictionary<string, Java.Lang.Object> javaDict = new Dictionary<string, Java.Lang.Object>(0);

            var resultDict = JavaDictionaryToClrDictionaryConverter.Convert(javaDict);

            Assert.IsNotNull(resultDict);
            Assert.AreEqual(0, resultDict.Count);
        }

        [Test]
        public void ConvertsClrDictToJavaDictAndBack()
        {
            object[] testValues =
            {
                "test string", int.MaxValue, long.MaxValue, double.MaxValue, float.MaxValue, true
            };
            Dictionary<string, object> testDict = new Dictionary<string, object>(testValues.Length);
            for (int i = 0; i < testValues.Length; i++)
            {
                testDict.Add((i + 1).ToString(), testValues[i]);
            }

            var javaDict = JavaDictionaryToClrDictionaryConverter.ConvertBack(testDict);
            var resultDict = JavaDictionaryToClrDictionaryConverter.Convert(javaDict);

            Assert.NotNull(resultDict);
            Assert.AreEqual(resultDict.Count, testDict.Count);
            for (int i = 0; i < testValues.Length; i++)
            {
                object testObj = testDict[(i + 1).ToString()];
                object resultObj = resultDict[(i + 1).ToString()];
                Assert.AreEqual(testObj.GetType(), resultObj.GetType());
                Assert.AreEqual(testObj, resultObj);
            }
        }

        [Test]
        public void ConvertsJavaDictToClrDictAndBack()
        {
            Java.Lang.Object[] testValues =
            {
                new Java.Lang.String("test string"),
                new Java.Lang.Integer(int.MaxValue),
                new Java.Lang.Long(long.MaxValue),
                new Java.Lang.Double(double.MaxValue),
                new Java.Lang.Float(float.MaxValue),
                new Java.Lang.Boolean(true)
            };
            Dictionary<string, Java.Lang.Object> testDict = new Dictionary<string, Java.Lang.Object>(testValues.Length);
            for (int i = 0; i < testValues.Length; i++)
            {
                testDict.Add((i + 1).ToString(), testValues[i]);
            }

            var clrDict = JavaDictionaryToClrDictionaryConverter.Convert(testDict);
            var resultDict = JavaDictionaryToClrDictionaryConverter.ConvertBack(clrDict);

            Assert.NotNull(resultDict);
            Assert.AreEqual(resultDict.Count, testDict.Count);
            for (int i = 0; i < testValues.Length; i++)
            {
                Java.Lang.Object testObj = testDict[(i + 1).ToString()];
                Java.Lang.Object resultObj = resultDict[(i + 1).ToString()];
                Assert.AreEqual(testObj.GetType(), resultObj.GetType());
                Assert.IsTrue(testObj.Equals(resultObj), $"Resulting value {resultObj} does not equal {testObj}!");
            }
        }
    }
}
