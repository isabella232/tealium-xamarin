using System;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class RemoteCommandPayloadTestsBase
    {
        protected abstract IRemoteCommandPayload GetPayload(TestPayloadData data);

        [Test]
        public void GetsString()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            string result = payload.GetValueForKey<string>(payloadData.KeyForString);

            Assert.True(payload.ContainsKey(payloadData.KeyForString), "Payload object should confirm that the data with the given key is present.");
            Assert.AreEqual(payloadData.ValueForString, result);
        }

        [Test]
        public void GetsInt()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            int result = payload.GetValueForKey<int>(payloadData.KeyForInt);

            Assert.True(payload.ContainsKey(payloadData.KeyForInt), "Payload object should confirm that the data with the given key is present.");
            Assert.AreEqual(payloadData.ValueForInt, result);
        }

        [Test]
        public void GetsLong()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            long result = payload.GetValueForKey<long>(payloadData.KeyForLong);

            Assert.True(payload.ContainsKey(payloadData.KeyForLong), "Payload object should confirm that the data with the given key is present.");
            Assert.AreEqual(payloadData.ValueForLong, result);
        }

        [Test]
        public void GetsFloat()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            float result = payload.GetValueForKey<float>(payloadData.KeyForFloat);

            Assert.True(payload.ContainsKey(payloadData.KeyForFloat), "Payload object should confirm that the data with the given key is present.");
            Assert.AreEqual(payloadData.ValueForFloat, result);
        }

        [Test]
        public void GetsDouble()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            double result = payload.GetValueForKey<double>(payloadData.KeyForDouble);

            Assert.True(payload.ContainsKey(payloadData.KeyForDouble), "Payload object should confirm that the data with the given key is present.");
            Assert.AreEqual(payloadData.ValueForDouble, result);
        }

        [Test]
        public void GetsBool()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            bool result = payload.GetValueForKey<bool>(payloadData.KeyForBool);

            Assert.AreEqual(payloadData.ValueForBool, result);
        }

        [Test]
        public void ThrowsForMissingKey()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            Assert.Throws(typeof(NullReferenceException), () => payload.GetValueForKey<string>(payloadData.MissingKey));
        }

        [Test]
        public void TellsThatDoesNotContainTheMissingKey()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            Assert.False(payload.ContainsKey(payloadData.MissingKey), "Payload object should inform that data with the given key is not present.");
        }

        [Test]
        public void ThrowsWhenBadCastStringToInt()
        {
            ThrowsWhenBadCastStringToType<int>();
        }

        [Test]
        public void ThrowsWhenBadCastStringToLong()
        {
            ThrowsWhenBadCastStringToType<long>();
        }

        [Test]
        public void ThrowsWhenBadCastStringToFloat()
        {
            ThrowsWhenBadCastStringToType<float>();
        }

        [Test]
        public void ThrowsWhenBadCastStringToDouble()
        {
            ThrowsWhenBadCastStringToType<double>();
        }

        [Test]
        public void ThrowsWhenBadCastStringToBool()
        {
            ThrowsWhenBadCastStringToType<bool>();
        }

        [Test]
        public void DoesNotThrowWhenCastingIntToFloat()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            payload.GetValueForKey<float>(payloadData.KeyForInt);
        }

        [Test]
        public void DoesNotThrowWhenCastingLongToFloat()
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            payload.GetValueForKey<float>(payloadData.KeyForLong);
        }

        [Test]
        public void ThrowsWhenBadCastLongToInt()
        {
#if __IOS__
            // down cast of long to int always succeds:
            // On 32 bit devices we get no overflow error (even if we would've overflowed)
            // on 64 bit devices we get the long anyway
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            payload.GetValueForKey<int>(payloadData.KeyForLong);
            return;
#else
            ThrowsWhenBadCastLongToType<int>();
#endif
        }

#region Helper methods

        //TODO: make more tests like this one - even more combinations of cross-casting
        void ThrowsWhenBadCastStringToType<T>()
        {
            ThrowsWhenBadCastToType<T>(payload => payload.KeyForString);
        }

        void ThrowsWhenBadCastLongToType<T>()
        {
            ThrowsWhenBadCastToType<T>(payload => payload.KeyForLong);
        }

        void ThrowsWhenBadCastToType<T>(Func<TestPayloadData, string> sourceTypeKeyFunc)
        {
            var payloadData = new TestPayloadData();
            IRemoteCommandPayload payload = GetPayload(payloadData);

            string errMsg = "Exception should be thrown when trying to retrieve data and cast it to improper type.";
#if __IOS__
            errMsg += " On iOS no exceptions will be thrown, as NSNumber can be converted to any CLR simple type, including bool.";
#endif
            Assert.Throws(typeof(InvalidCastException),
                          () => payload.GetValueForKey<T>(sourceTypeKeyFunc(payloadData)));
        }

#endregion Helper methods
    }

    public class TestPayloadData
    {
        public string KeyForString { get; set; } = "keyString";
        public string ValueForString { get; set; } = "string value";

        public string KeyForInt { get; set; } = "keyInt";
        public int ValueForInt { get; set; } = int.MaxValue;

        public string KeyForLong { get; set; } = "keyLong";
        public long ValueForLong { get; set; } = long.MaxValue;

        public string KeyForFloat { get; set; } = "keyFloat";
        public float ValueForFloat { get; set; } = float.MaxValue;

        public string KeyForDouble { get; set; } = "keyDouble";
        public double ValueForDouble { get; set; } = double.MaxValue;

        public string KeyForBool { get; set; } = "keyBool";
        public bool ValueForBool { get; set; } = true;

        public string MissingKey { get; set; } = "missingKey";
    }
}
