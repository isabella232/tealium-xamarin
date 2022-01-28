using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class DispatchTestsBase
    {
        protected abstract Dispatch GetEmptyDispatch();
        protected abstract Dispatch GetDispatchWithData(string stringKey, string value);
        protected abstract Dispatch GetDispatchWithData(string arrayKey, string[] array);
        protected abstract Dispatch GetDispatchWithData(string dictionaryKey, IDictionary<string, object> dictionary);
        protected abstract Dispatch GetDispatchWithData(KeyValuePair<string, object> data);

        [Test]
        public void DoesNotContainAnyKeysWhenEmpty()
        {
            var dispatch = GetEmptyDispatch();

            bool contains = dispatch.DataLayer.ContainsKey("missingKey");

            Assert.False(contains, "Dispatch should not report it contains the missing key!");
        }

        [Test]
        public void DoesNotContainMissingKey()
        {
            var dispatch = GetDispatchWithData("otherKey", "otherValue");

            bool contains = dispatch.DataLayer.ContainsKey("missingKey");

            Assert.False(contains, "Dispatch should not report it contains the missing key!");
        }

        [Test]
        public void GetsString()
        {
            string key = nameof(key);
            string referenceValue = nameof(referenceValue);
            Dispatch dispatch = GetDispatchWithData(key, referenceValue);

            string testValue = dispatch.DataLayer[key] as string;

            Assert.AreEqual(referenceValue, testValue);
        }

        [Test]
        public void PutsAndGetsString()
        {
            string key = nameof(key);
            string referenceValue = nameof(referenceValue);
            Dispatch dispatch = GetEmptyDispatch();

            dispatch.DataLayer.Add(key, referenceValue);
            var testValue = dispatch.DataLayer[key];

            Assert.AreEqual(referenceValue, testValue);
        }

        [Test]
        public void GetsArrayOfStrings()
        {
            string key = nameof(key);
            string value1 = nameof(value1);
            string value2 = nameof(value2);
            string[] referenceArray = { value1, value2 };
            Dispatch dispatch = GetDispatchWithData(key, referenceArray);

            List<string> testArray = dispatch.DataLayer[key] as List<string>;

            Assert.AreEqual(referenceArray.Length, testArray.Count);
            Assert.AreEqual(referenceArray[0], testArray[0]);
            Assert.AreEqual(referenceArray[1], testArray[1]);
        }

        [Test]
        public void PutsAndGetsArrayOfStrings()
        {
            string key = nameof(key);
            string value1 = nameof(value1);
            string value2 = nameof(value2);
            string[] referenceArray = { value1, value2 };
            Dispatch dispatch = GetEmptyDispatch();

            dispatch.DataLayer.Add(key, referenceArray);
            string[] testArray = dispatch.DataLayer[key] as string[];

            Assert.AreEqual(referenceArray.Length, testArray.Length);
            Assert.AreEqual(referenceArray[0], testArray[0]);
            Assert.AreEqual(referenceArray[1], testArray[1]);
        }
    }
}
