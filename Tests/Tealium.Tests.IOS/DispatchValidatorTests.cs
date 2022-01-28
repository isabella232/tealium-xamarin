using System;
using NUnit.Framework;
using Tealium.iOS;


namespace Tealium.Tests.iOS
{
    [TestFixture]
    public class DispatchValidatorTests
    {
        [Test]
        public void CallsShouldDropDispatch()
        {
            bool methodCalled = false;
            TestTealiumDispatchValidator testValidator = new TestTealiumDispatchValidator("MyValidator", (req) =>
            {
                methodCalled = true;
                return false;
            }, null);

            testValidator.ShouldDropWithRequest(null);

            Assert.IsTrue(methodCalled);
        }

        [Test]
        public void CallsShouldQueueDispatch()
        {
            bool methodCalled = false;
            TestTealiumDispatchValidator testValidator = new TestTealiumDispatchValidator("MyValidator", null, (req) =>
            {
                methodCalled = true;
                return false;
            });

            testValidator.ShouldQueueWithRequest(null);

            Assert.IsTrue(methodCalled);
        }

        /// <summary>
        /// A test class that enables to write behavioral tests for <see cref="NativeDispatchValidator"/>.
        /// </summary>
        class TestTealiumDispatchValidator : DispatchValidatorIOS
        {
            private readonly Func<Dispatch, bool> shouldDrop;
            private readonly Func<Dispatch, bool> shouldQueue;
            public TestTealiumDispatchValidator(string id, Func<Dispatch, bool> shouldDrop, Func<Dispatch, bool> shouldQueue)
                : base(id, null)
            {
                this.shouldDrop = shouldDrop;
                this.shouldQueue = shouldQueue;
            }

            public override bool ShouldDrop(Dispatch dispatch)
            {
                if (shouldDrop != null)
                {
                    return shouldDrop(dispatch);
                }
                return base.ShouldDrop(dispatch);
            }

            public override bool ShouldQueue(Dispatch dispatch)
            {
                if (shouldQueue != null)
                {
                    return shouldQueue(dispatch);
                }
                return base.ShouldQueue(dispatch);
            }
        }
    }
}
