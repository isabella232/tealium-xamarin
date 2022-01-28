using System;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.Tests.Common;
using TealiumNative = Com.Tealium;


namespace Tealium.Tests.Droid
{
    [TestFixture]
    public class DispatchValidatorTests
    {
        [Test]
        public void CallsShouldDropDispatch()
        {
            bool methodCalled = false;
            DelegateDispatchValidator validator = new DelegateDispatchValidator("MyValidator")
            {
                ShouldDropDispatchDelegate = (Dispatch arg1) =>
                {
                    methodCalled = true;
                    return false;
                }
            };
            TestTealiumDispatchValidator testValidator = new TestTealiumDispatchValidator(validator);

            _ = testValidator.CallShouldDropDispatch(new TealiumNative.Dispatcher.TealiumEvent("event"));

            Assert.IsTrue(methodCalled);
        }

        [Test]
        public void CallsShouldQueueDispatch()
        {
            bool methodCalled = false;
            DelegateDispatchValidator validator = new DelegateDispatchValidator("MyValidator")
            {
                ShouldQueueDispatchDelegate = (Dispatch arg1) =>
                {
                    methodCalled = true;
                    return false;
                }
            };
            TestTealiumDispatchValidator testValidator = new TestTealiumDispatchValidator(validator);

            _ = testValidator.CallShouldQueueDispatch(new TealiumNative.Dispatcher.TealiumEvent("event"));

            Assert.IsTrue(methodCalled);
        }

        /// <summary>
        /// A test class that enables to write behavioral tests for <see cref="NativeDispatchValidator"/>.
        /// </summary>
        class TestTealiumDispatchValidator : NativeDispatchValidator
        {
            public TestTealiumDispatchValidator(IDispatchValidator validator)
                : base(validator)
            { }

            public bool CallShouldDropDispatch(Com.Tealium.Dispatcher.IDispatch d)
            {
                return ((TealiumNative.Core.Validation.IDispatchValidator)this).ShouldDrop(d);
            }

            public bool CallShouldQueueDispatch(Com.Tealium.Dispatcher.IDispatch d)
            {
                return ((TealiumNative.Core.Validation.IDispatchValidator)this).ShouldQueue(d);
            }
        }
    }
}
