using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class TealiumInstancePersistentDataSourcesTestsBase
    {
        ITealiumInstanceManager instanceManager;
        ITealium tealium;

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void Setup()
        {
            instanceManager = new TealiumInstanceManager(GetInstanceFactory());
            tealium = instanceManager.CreateInstance(TealiumConfigHelper.GetSimpleTestConfig());
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            instanceManager.DisposeAllInstances();
        }

        [Test]
        public void PluginDataPresent()
        {
            Wait(500);
            var pluginName = tealium.GetFromDataLayer("plugin_name");
            Assert.NotNull(pluginName);
            Assert.NotNull(tealium.GetFromDataLayer("plugin_version"));
            Assert.AreEqual(pluginName, "Tealium-Xamarin");
        }

        [Test]
        public void HandlesString()
        {
            string testValue = "TestString";
            string testKey = "PersistentString";

            MakeTest<string>(testKey, testValue);
        }

        [Test]
        public void OverridesValueWithTheSameKey()
        {
            string testValue1 = "TestString1";
            string testValue2 = "TestString2";
            string testKey = "PersistentString";
            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { testKey, testValue1 }
            }, Expiry.Session);
            Wait();

            MakeTest<string>(testKey, testValue2);
        }

        [Test]
        public void DoesNotAcceptNullString()
        {
            string testValue = null;
            string testKey = "PersistentNullString";

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { testKey, testValue }
            }, Expiry.Session);
            Wait();
            object result = tealium.GetFromDataLayer(testKey);

            Assert.Null(result, "Resulting persistent data sources should not contain the null string.");
        }

        [Test]
        public void HandlesInt()
        {
            int testValue = 10;
            string testKey = "PersistentInt";

#if __IOS__
            MakeTest<long>(testKey, testValue);
#else
            MakeTest<int>(testKey, testValue);
#endif

        }

        [Test]
        public void HandlesLong()
        {
            long testValue = long.MaxValue;
            string testKey = "PersistentLong";

            MakeTest<long>(testKey, testValue);
        }

        [Test]
        public void HandlesDoublesLessOrEqualToFloatMax()
        {
            double testValue = float.MaxValue;
            string testKey = "PersistentSmallPositiveDouble";

            MakeTest<double>(testKey, testValue);
        }

        [Test]
        public void HandlesDoublesGreaterOrEqualToFloatMin()
        {
            double testValue = float.MinValue;
            string testKey = "PersistentSmallNegativeDouble";

            MakeTest<double>(testKey, testValue);
        }

        [Test]
        public void DoesNotThrowForDoublesGreaterThanDoubleMax()
        {
            double testValue = double.MaxValue;
            string testKey = "PersistentBigDouble";

            tealium.AddToDataLayer(new Dictionary<string, object>
                {
                    { testKey, testValue }
                }, Expiry.Session);
        }

        [Test]
        public void HandlesFloat()
        {
            float testValue = float.MaxValue;
            string testKey = "PersistentFloat";

            MakeTest<double>(testKey, testValue);
        }

        [Test]
        public void HandlesBool()
        {
            bool testValue = true;
            string testKey = "PersistentBool";

            MakeTest<bool>(testKey, testValue);
        }

        [Test]
        public void RemovesGivenKeys()
        {
            string testValue1 = "TestString";
            string testKey1 = "PersistentString";
            int testValue2 = 590;
            string testKey2 = "PersistentInt";
            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { testKey1, testValue1 },
                { testKey2, testValue2 }
            }, Expiry.Session);
            Wait();
            var data1 = tealium.GetFromDataLayer(testKey1);
            var data2 = tealium.GetFromDataLayer(testKey2);
            if (data1 == null || data2 == null)
            {
                Assert.Inconclusive("Unable to add required persistent data to perform the test!");
            }

            tealium.RemoveFromDataLayer(new string[] { testKey1, testKey2 });
            Wait();
            data1 = tealium.GetFromDataLayer(testKey1);
            data2 = tealium.GetFromDataLayer(testKey2);

            Assert.Null(data1, "Test value 1 was not removed from persistent data sources.");
            Assert.Null(data2, "Test value 2 was not removed from persistent data sources.");
        }

        [Test]
        public void KeepsNotRemovedKeys()
        {
            string testValue1 = "TestString";
            string testKey1 = "PersistentString";
            int testValue2 = 590;
            string testKey2 = "PersistentInt";
            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { testKey1, testValue1 },
                { testKey2, testValue2 }
            }, Expiry.Session);
            Wait();
            var data1 = tealium.GetFromDataLayer(testKey1);
            var data2 = tealium.GetFromDataLayer(testKey2);
            if (data1 == null || data2 == null)
            {
                Assert.Inconclusive("Unable to add required persistent data to perform the test!");
            }

            tealium.RemoveFromDataLayer(new string[] { testKey1 });
            Wait();
            data1 = tealium.GetFromDataLayer(testKey1);
            data2 = tealium.GetFromDataLayer(testKey2);

            Assert.Null(data1, "Test value 1 was not removed from persistent data sources.");
#if __IOS__
            Assert.AreEqual(testValue2, (long)data2, "Test value 2 was removed from persistent data sources.");
#else
            Assert.AreEqual(testValue2, (int)data2, "Test value 2 was removed from persistent data sources.");
#endif

        }

        [Test]
        public void HandlesListOfStrings()
        {
            string key = "PersistentListOfStrings";
            var referenceList = new List<string> { "listValue1", "listValue2" };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as IList<object>;
#else
            var result = tealium.GetFromDataLayer(key) as IList<string>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Count, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesArrayOfStrings()
        {
            string key = "PersistentArrayOfStrings";
            var referenceList = new string[] { "arrayValue1", "arrayValue2" };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as IList<object>;
#else
            var result = tealium.GetFromDataLayer(key) as IList<string>;
#endif

            Assert.AreEqual(referenceList.Length, result.Count, "Resulting array should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesListOfInts()
        {
            string key = "PersistentListOfInts";
            var referenceList = new List<int> { 1, 2, 3 };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<int>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Count, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesArrayOfInts()
        {
            string key = "PersistentArrayOfInts";
            var referenceList = new int[] { 1, 2, 3 };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<int>;
#endif

            Assert.AreEqual(referenceList.Length, result.Count, "Resulting array should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesListOfBools()
        {
            string key = "PersistentListOfBools";
            var referenceList = new List<bool> { true, false, true };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<bool>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Count, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesArrayOfBool()
        {
            string key = "PersistentArrayOfBools";
            var referenceList = new bool[] { true, false, true };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();

#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<bool>;
#endif
            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Length, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesListOfDouble()
        {
            string key = "PersistentListOfDouble";
            var referenceList = new List<double> { 11.1d, 22.2d, 33.3d };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<double>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Count, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesArrayOfDouble()
        {
            string key = "PersistentArrayOfDouble";
            var referenceList = new double[] { 11.1d, 22.2d, 33.3d };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<double>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Length, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesListOfLong()
        {
            string key = "PersistentListOfLong";
            var referenceList = new List<long> { 100, 200, 300 };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<long>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Count, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesArrayOfLong()
        {
            string key = "PersistentArrayOfLong";
            var referenceList = new long[] { 100L, 200L, 300L };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceList }
            }, Expiry.Session);

            Wait();
#if __IOS__
            // On iOS we have no way to translate NSArrays of NSNumbers to List<bool>
            var result = tealium.GetFromDataLayer(key) as List<object>;
#else
            var result = tealium.GetFromDataLayer(key) as List<long>;
#endif

            Assert.NotNull(result);
            Assert.AreEqual(referenceList.Length, result.Count, "Resulting list should have the same object count as the reference!");
            Assert.AreEqual(referenceList[0], result[0]);
            Assert.AreEqual(referenceList[1], result[1]);
        }

        [Test]
        public void HandlesDictionaries()
        {
            string key = "PersistentDictionary";
            var referenceDict = new Dictionary<string, object>
            {
                { "string", "string" },
                { "int", 10 },
                { "double", 10.1d },
                { "long", 100L },
                { "bool", true }
            };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceDict }
            }, Expiry.Session);

            Wait();

            var result = tealium.GetFromDataLayer(key) as Dictionary<string, object>;

            Assert.NotNull(result);
            Assert.AreEqual(referenceDict.Count, result.Count, "Resulting dictionary should have the same object count as the reference!");
            Assert.AreEqual(referenceDict, result);
        }

        [Test]
        public void HandlesDictionariesOfArrays()
        {
            string key = "PersistentDictionaryOfArrays";
            var referenceDict = new Dictionary<string, object>
            {
                { "string", new string[] { "string1", "string2" } },
                { "int", new int[] { 10, 11, 12 } },
                { "double", new double[] { 10.1d, 11.2d, 12.3 } },
                { "long", new long[] { 100L, 200L, 300L } },
                { "bool", new bool[] { true, false, true } },
                { "mixed", new object[] { "string", 10, 10.1d, 100L, true } }
            };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceDict }
            }, Expiry.Session);

            Wait();

            var result = tealium.GetFromDataLayer(key) as Dictionary<string, object>;

            Assert.NotNull(result);
            Assert.AreEqual(referenceDict.Count, result.Count, "Resulting list should have the same object count as the reference!");
            foreach (var entry in referenceDict)
            {
                List<object> data = result[entry.Key] as List<object>;
                Assert.AreEqual(entry.Value, data.ToArray());
            }
        }

        [Test]
        public void HandlesDictionariesOfLists()
        {
            string key = "PersistentDictionaryOfLists";
            var referenceDict = new Dictionary<string, object>
            {
                { "string", new List<string> { "string1", "string2" } },
                { "int", new List<int> { 10, 11, 12 } },
                { "double", new List<double> { 10.1d, 11.2d, 12.3 } },
                { "long", new List<long> { 100L, 200L, 300L } },
                { "bool", new List<bool> { true, false, true } },
                { "mixed", new List<object> { "string", 10, 10.1d, 100L, true } }
            };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceDict }
            }, Expiry.Session);

            Wait();

            var result = tealium.GetFromDataLayer(key) as Dictionary<string, object>;

            Assert.NotNull(result);
            Assert.AreEqual(referenceDict.Count, result.Count, "Resulting dictionary should have the same object count as the reference!");
            foreach (var entry in referenceDict)
            {
                List<object> data = result[entry.Key] as List<object>;
                Assert.AreEqual(entry.Value, data);
            }
        }

        [Test]
        public void HandlesDictionariesOfDictionaries()
        {
            string key = "PersistentDictionaryOfLists";
            var referenceDict = new Dictionary<string, object>
            {
                { "string", new Dictionary<string, object>
                    {
                        { "string1", "string2" },
                        { "int", new List<int> { 10, 11, 12 } },
                        { "double", new List<double> { 10.1d, 11.2d, 12.3 } },
                        { "long", new List<long> { 100L, 200L, 300L } },
                        { "bool", new List<bool> { true, false, true } },
                        { "mixed", new List<object> { "string", 10, 10.1d, 100L, true } }
                    }
                }
            };

            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { key, referenceDict }
            }, Expiry.Session);

            Wait();

            var result = tealium.GetFromDataLayer(key) as Dictionary<string, object>;

            Assert.NotNull(result);
            Assert.AreEqual(referenceDict.Count, result.Count, "Resulting dictionary should have the same object count as the reference!");
            foreach (var entry in referenceDict)
            {
                Dictionary<string, object> referenceData = entry.Value as Dictionary<string, object>;
                Dictionary<string, object> resultData = result[entry.Key] as Dictionary<string, object>;
                foreach (var subEntry in referenceData)
                {
                    Assert.AreEqual(subEntry.Value, resultData[subEntry.Key]);
                }
            }
        }

#region helper methods

        void MakeTest<T>(string testKey, object testValue)
        {
            tealium.AddToDataLayer(new Dictionary<string, object>
            {
                { testKey, testValue }
            }, Expiry.Session);

            Wait();

            T result = (T)tealium.GetFromDataLayer(testKey);

            Assert.AreEqual(result.GetType(), typeof(T));
            if (result is double dResult && testValue is double dValue)
            {
                Assert.AreEqual(dValue, dResult, Math.Abs(dResult/10E10));
            }
            else
            {
                Assert.AreEqual(testValue, result);
            }
            
        }

        private static void Wait(int delayMillis = 5)
        {
            TaskWaitHelper.Wait(delayMillis);
        }

#endregion helper methods
    }
}
