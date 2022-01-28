using System;
using NUnit.Framework;
using Tealium.Droid;
using Native = Com.Tealium.Dispatcher;

namespace Tealium.Tests.Droid
{
    [TestFixture]
    public class EventListenersTests
    {
        [Test]
        public void CallsDispatchDropped()
        {
            bool methodCalled = false;
            DispatchDroppedDelegateEventListener sendingListener = new DispatchDroppedDelegateEventListener()
            {
                DispatchDropped = (dispatch) =>
                {
                    methodCalled = true;
                }
            };
            TestNativeDispatchDroppedListener nativeListener = new TestNativeDispatchDroppedListener(sendingListener);

            nativeListener.CallOnDispatchSend(new Native.TealiumEvent("test"));

            Assert.IsTrue(methodCalled);
        }

        [Test]
        public void CallsDispatchQueued()
        {
            bool methodCalled = false;
            DispatchQueuedDelegateEventListener queuingListener = new DispatchQueuedDelegateEventListener()
            {
                DispatchQueued = (dispatch) =>
                {
                    methodCalled = true;
                }
            };
            TestNativeDispatchQueuedListener nativeListener = new TestNativeDispatchQueuedListener(queuingListener);

            nativeListener.CallOnDispatchQueued(new Native.TealiumEvent("test"));

            Assert.IsTrue(methodCalled);
        }

        [Test]
        public void CallsSettingsPublished()
        {
            bool methodCalled = false;
            SettingsPublishedDelegateEventListener settingsListener = new SettingsPublishedDelegateEventListener()
            {
                SettingsPublished = () =>
                {
                    methodCalled = true;
                }
            };
            TestNativePublishSettingsUpdateListener nativeListener = new TestNativePublishSettingsUpdateListener(settingsListener);

            nativeListener.CallOnPublishSettingsUpdate();

            Assert.IsTrue(methodCalled);
        }

        #region Helper classes

        /// <summary>
        /// A test class that enables to write behavioral tests for <see cref="NativeDispatchDroppedListener"/>.
        /// </summary>
        class TestNativeDispatchDroppedListener : NativeEventListener
        {
            public TestNativeDispatchDroppedListener(IDispatchDroppedEventListener listener)
                : base(listener)
            { }

            public void CallOnDispatchSend(Native.IDispatch d)
            {
                OnDispatchDropped(d);
            }
        }

        /// <summary>
        /// A test class that enables to write behavioral tests for <see cref="NativeDispatchQueuedListener"/>.
        /// </summary>
        class TestNativeDispatchQueuedListener : NativeEventListener
        {
            public TestNativeDispatchQueuedListener(IDispatchQueuedEventListener listener)
                : base(listener)
            { }

            public void CallOnDispatchQueued(Native.IDispatch d)
            {
                OnDispatchQueued(d);
            }
        }

        /// <summary>
        /// A test class that enables to write behavioral tests for <see cref="NativePublishSettingsUpdateListener"/>.
        /// </summary>
        class TestNativePublishSettingsUpdateListener : NativeEventListener
        {
            public TestNativePublishSettingsUpdateListener(ISettingsPublishedEventListener listener)
                : base(listener)
            { }

            public void CallOnPublishSettingsUpdate()
            {
                OnLibrarySettingsUpdated(null);
            }
        }

        #endregion Helper classes
    }
}
