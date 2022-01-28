using System;
using NUnit.Framework;
using Tealium;


namespace Tealium.Tests.Common
{
    [TestFixture]
    public class KeyedCollectionTests
    {
        readonly KeyedCollection<string> stringCollection = new KeyedCollection<string>();

        [TestFixtureSetUp]
        public void SetUp()
        {
            stringCollection.Clear();
        }

        [Test]
        public void TestAddAndRemove()
        {
            var obj = "some";
            var key = stringCollection.Add(obj);
            var res = stringCollection.Get(key);
            Assert.AreEqual(obj, res);
            Assert.AreEqual(stringCollection.Keys.Count, 1);
            Assert.True(stringCollection.Contains(key));
            Assert.True(stringCollection.Remove(key));
            Assert.AreEqual(stringCollection.Keys.Count, 0);
            Assert.False(stringCollection.Contains(key));
        }

        [Test]
        public void TestAddTwice()
        {
            var obj = "some";
            var key1 = stringCollection.Add(obj);
            var key2 = stringCollection.Add(obj);
            var res1 = stringCollection.Get(key1);
            var res2 = stringCollection.Get(key2);

            Assert.AreEqual(res1, res2, obj);
            Assert.AreNotEqual(key1, key2);
        }

        [Test]
        public void TestOtherCollectionKey()
        {
            KeyedCollection<string> otherCollection = new KeyedCollection<string>();
            var obj = "some";
            var key = stringCollection.Add(obj);

            Assert.Throws(typeof(System.Collections.Generic.KeyNotFoundException), () => otherCollection.Get(key));
            Assert.False(otherCollection.Contains(key));
            Assert.False(otherCollection.Remove(key));
        }

        [Test]
        public void TestNonSpecificKey()
        {
            KeyedCollection<object> otherCollection = new KeyedCollection<object>();
            var obj = new object();
            var key = otherCollection.Add(obj);

            Assert.Throws(typeof(InvalidCastException), () => stringCollection.Get((CollectionSpecificKey<string>)(object)key));
            Assert.True(otherCollection.Contains(key));
            Assert.True(otherCollection.Remove(key));
        }
    }
}