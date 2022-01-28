using System;
using System.Collections.Generic;
using NUnit.Framework;
using Org.Json;
using Tealium.Droid.NativeInterop;

namespace Tealium.Tests.Droid.NativeInteropTests
{
    [TestFixture]
    public class JavaToClrConverterTests
    {

        #region Java -> CLR conversion

        [Test]
        public void ConvertingJavaNullReturnsNull()
        {
            object result = JavaToClrConverter.Convert(null);

            Assert.Null(result);
        }

        [Test]
        public void ConvertsJavaStringToStringAsObject()
        {
            string testVal = "test";

            object result = JavaToClrConverter.Convert(new Java.Lang.String(testVal));

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.AreEqual(testVal, (string)result);
        }

        [Test]
        public void ConvertsJavaStringToString()
        {
            string testVal = "test";

            string result = JavaToClrConverter.Convert<string>(new Java.Lang.String(testVal));

            Assert.AreEqual(testVal, result);
        }

        [Test]
        public void ConvertsJavaIntegerToIntAsObject()
        {
            int testVal = 3;

            object result = JavaToClrConverter.Convert(new Java.Lang.Integer(testVal));

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.AreEqual(testVal, (int)result);
        }

        [Test]
        public void ConvertsJavaIntegerToInt()
        {
            int testVal = 3;

            int result = JavaToClrConverter.Convert<int>(new Java.Lang.Integer(testVal));

            Assert.AreEqual(testVal, result);
        }

        [Test]
        public void ConvertsJavaLongToLongAsObject()
        {
            long testVal = long.MaxValue;

            object result = JavaToClrConverter.Convert(new Java.Lang.Long(testVal));

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.AreEqual(testVal, (long)result);
        }

        [Test]
        public void ConvertsJavaLongToLong()
        {
            long testVal = long.MaxValue;

            long result = JavaToClrConverter.Convert<long>(new Java.Lang.Long(testVal));

            Assert.AreEqual(testVal, result);
        }

        [Test]
        public void ConvertsJavaFloatToFloatAsObject()
        {
            float testVal = 3.0f;

            object result = JavaToClrConverter.Convert(new Java.Lang.Float(testVal));

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.AreEqual(testVal, (float)result);
        }

        [Test]
        public void ConvertsJavaFloatToFloat()
        {
            float testVal = 3.0f;

            float result = JavaToClrConverter.Convert<float>(new Java.Lang.Float(testVal));

            Assert.AreEqual(testVal, result);
        }

        [Test]
        public void ConvertsJavaDoubleToDoubleAsObject()
        {
            double testVal = 3.0;

            object result = JavaToClrConverter.Convert(new Java.Lang.Double(testVal));

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.AreEqual(testVal, (double)result);
        }

        [Test]
        public void ConvertsJavaDoubleToDouble()
        {
            double testVal = 3.0;

            double result = JavaToClrConverter.Convert<double>(new Java.Lang.Double(testVal));

            Assert.AreEqual(testVal, result);
        }

        [Test]
        public void ConvertsJavaBooleanToBoolAsObject()
        {
            bool testVal = true;

            object result = JavaToClrConverter.Convert(new Java.Lang.Boolean(testVal));

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.AreEqual(testVal, (bool)result);
        }

        [Test]
        public void ConvertsJavaBooleanToBool()
        {
            bool testVal = true;

            bool result = JavaToClrConverter.Convert<bool>(new Java.Lang.Boolean(testVal));

            Assert.AreEqual(testVal, result);
        }

        #endregion Java -> CLR conversion

        #region Java -> CLR string conversion

        [Test]
        public void ConvertsJavaIntegerToString()
        {
            int testVal = 3;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Integer(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            Assert.AreEqual(testVal.ToString(), result);
        }

        [Test]
        public void ConvertsJavaLongToString()
        {
            long testVal = 3;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Long(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            Assert.AreEqual(testVal.ToString(), result);
        }

        [Test]
        public void ConvertsJavaFloatToString()
        {
            float testVal = 3.456f;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Float(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            Assert.AreEqual(testVal.ToString(), result);
        }

        [Test]
        public void ConvertsJavaDoubleToString()
        {
            double testVal = 2.123;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Double(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            Assert.AreEqual(testVal.ToString(), result);
        }

        [Test]
        public void ConvertsJavaFloatMaxToString()
        {
            float testVal = float.MaxValue;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Float(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            //due to precision issues we have to explicitly pass the expected string
            Assert.AreEqual("3.4028235E+38"/*testVal.ToString()*/, result);
        }

        [Test]
        public void ConvertsJavaDoubleMaxToString()
        {
            double testVal = double.MaxValue;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Double(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            //due to precision issues we have to explicitly pass the expected string
            Assert.AreEqual("1.7976931348623157E+308"/*testVal.ToString()*/, result);
        }

        [Test]
        public void ConvertsJavaBoolToString()
        {
            bool testVal = true;
            string result = null;

            Assert.DoesNotThrow(() => result = JavaToClrConverter.Convert<string>(new Java.Lang.Boolean(testVal)), "Conversion from any object to string should be possible (no exceptions thrown).");
            Assert.AreEqual(testVal.ToString(), result);
        }

        #endregion Java -> CLR string conversion

        #region CLR -> Java conversion

        [Test]
        public void ThrowsIfAttemptToConvertClrNull()
        {
            bool thrown = false;

            try
            {
                JavaToClrConverter.ConvertBack(null);
            }
            catch
            {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [Test]
        public void ConvertsStringToJavaString()
        {
            string clrTestVal = "test";
            var testVal = new Java.Lang.String(clrTestVal);

            Java.Lang.Object result = JavaToClrConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsIntToJavaInteger()
        {
            int clrTestVal = 3;
            var testVal = new Java.Lang.Integer(clrTestVal);

            Java.Lang.Object result = JavaToClrConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsLongToJavaLong()
        {
            long clrTestVal = long.MaxValue;
            var testVal = new Java.Lang.Long(clrTestVal);

            Java.Lang.Object result = JavaToClrConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsFloatToJavaFloat()
        {
            float clrTestVal = 3.0f;
            var testVal = new Java.Lang.Float(clrTestVal);

            Java.Lang.Object result = JavaToClrConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsDoubleToJavaDouble()
        {
            double clrTestVal = 3.0;
            var testVal = new Java.Lang.Double(clrTestVal);

            Java.Lang.Object result = JavaToClrConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        [Test]
        public void ConvertsBoolToJavaBoolean()
        {
            bool clrTestVal = true;
            var testVal = new Java.Lang.Boolean(clrTestVal);

            Java.Lang.Object result = JavaToClrConverter.ConvertBack(clrTestVal);

            Assert.AreEqual(testVal.GetType(), result.GetType());
            Assert.IsTrue(result.Equals(testVal), $"Resulting value {result} does not equal {testVal}!");
        }

        #endregion CLR -> Java conversion

        #region Java JSON - CLR list/dictionary conversion

        [Test]
        public void ConvertsJsonArrayToICollectionOfObjects()
        {
            PrepareJsonArrayForTest(out JSONArray json, out string testVal1, out string testVal2);

            //we make a list from the ICollection to access data using indexer
            var result = new List<object>(JavaToClrConverter.Convert<ICollection<object>>(json));

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsJsonArrayToIListOfObjects()
        {
            PrepareJsonArrayForTest(out JSONArray json, out string testVal1, out string testVal2);

            var result = JavaToClrConverter.Convert<IList<object>>(json);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsJsonArrayToListOfObjects()
        {
            PrepareJsonArrayForTest(out JSONArray json, out string testVal1, out string testVal2);

            var result = JavaToClrConverter.Convert<List<object>>(json);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsJsonArrayToArrayOfObjects()
        {
            PrepareJsonArrayForTest(out JSONArray json, out string testVal1, out string testVal2);

            var result = JavaToClrConverter.Convert<object[]>(json);

            Assert.AreEqual(testVal1, result[0]);
            Assert.AreEqual(testVal2, result[1]);
        }

        [Test]
        public void ConvertsJsonArrayToListOfStringsAsObject()
        {
            PrepareJsonArrayForTest(out JSONArray json, out string testVal1, out string testVal2);

            var result = JavaToClrConverter.Convert(json);

            Assert.AreEqual(typeof(List<object>), result.GetType());
            Assert.AreEqual(testVal1, ((List<object>)result)[0]);
            Assert.AreEqual(testVal2, ((List<object>)result)[1]);
        }

        void PrepareJsonArrayForTest(out JSONArray json, out string testVal1, out string testVal2)
        {
            testVal1 = nameof(testVal1);
            testVal2 = nameof(testVal2);
            json = new JSONArray();
            json.Put(testVal1);
            json.Put(testVal2);
        }

        [Test]
        public void ConvertsJsonObjectToIDictionaryOfObjects()
        {
            PrepareJsonObjectForTest(out JSONObject json, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = JavaToClrConverter.Convert<IDictionary<string, object>>(json);

            Assert.AreEqual(testVal1, result[testKey1]);
            Assert.AreEqual(testVal2, result[testKey2]);
        }

        [Test]
        public void ConvertsJsonObjectToDictionaryOfObjects()
        {
            PrepareJsonObjectForTest(out JSONObject json, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = JavaToClrConverter.Convert<Dictionary<string, object>>(json);

            Assert.AreEqual(testVal1, result[testKey1]);
            Assert.AreEqual(testVal2, result[testKey2]);
        }

        [Test]
        public void ConvertsJsonObjectToDictionaryOfStringsAsObject()
        {
            PrepareJsonObjectForTest(out JSONObject json, out string testKey1, out string testVal1, out string testVal2, out string testKey2);

            var result = JavaToClrConverter.Convert(json);

            Assert.AreEqual(typeof(Dictionary<string, object>), result.GetType());
            Assert.AreEqual(testVal1, ((Dictionary<string, object>)result)[testKey1]);
            Assert.AreEqual(testVal2, ((Dictionary<string, object>)result)[testKey2]);
        }

        void PrepareJsonObjectForTest(out JSONObject json, out string testKey1, out string testVal1, out string testVal2, out string testKey2)
        {
            testKey1 = nameof(testKey1);
            testKey2 = nameof(testKey2);
            testVal1 = nameof(testVal1);
            testVal2 = nameof(testVal2);
            json = new JSONObject();
            json.Put(testKey1, testVal1);
            json.Put(testKey2, testVal2);
        }

        #endregion Java JSON - CLR list/dictionary conversion

        #region CLR list/dictionary - Java JSON conversion

        [Test]
        public void ConvertsArrayOfStringsToJsonArray()
        {
            string testVal1 = nameof(testVal1);
            string testVal2 = nameof(testVal2);
            var list = new string[2]
            {
                testVal1,
                testVal2
            };

            ConvertsIColectionOfStringsToJSONArray(list, testVal1, testVal2);
        }

        [Test]
        public void ConvertsListOfStringsToJsonArray()
        {
            string testVal1 = nameof(testVal1);
            string testVal2 = nameof(testVal2);
            List<string> list = new List<string>(2)
            {
                testVal1,
                testVal2
            };

            //List<string> implements ICollection<string> and IList<string>, so no need to write separate tests for these types as inputs
            ConvertsIColectionOfStringsToJSONArray(list, testVal1, testVal2);
        }

        void ConvertsIColectionOfStringsToJSONArray(ICollection<string> list, string testVal1, string testVal2)
        {
            var result = JavaToClrConverter.ConvertBack(list);

            Assert.AreEqual(typeof(JSONArray), result.GetType());
            Assert.AreEqual(testVal1, ((JSONArray)result).GetString(0));
            Assert.AreEqual(testVal2, ((JSONArray)result).GetString(1));
        }

        [Test]
        public void ConvertsDictionaryOfStringsToJsonObject()
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

            //Dictionary<string, string> implements IDictionary<string, string>, so no need to write separate tests for this type as input
            var result = JavaToClrConverter.ConvertBack(dict);

            Assert.AreEqual(typeof(JSONObject), result.GetType());
            Assert.AreEqual(testVal1, ((JSONObject)result).GetString(testKey1));
            Assert.AreEqual(testVal2, ((JSONObject)result).GetString(testKey2));
        }

        #endregion CLR list/dictionary - Java JSON conversion
    }
}