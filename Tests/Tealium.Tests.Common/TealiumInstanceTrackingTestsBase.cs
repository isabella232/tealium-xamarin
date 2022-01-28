using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tealium.Tests.Common
{
    public abstract class TealiumInstanceTrackingTestsBase
    {
        ITealiumInstanceManager instanceManager;
        ITealium tealium;

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
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
        public void DoesNotThrowWhenTracksViewWithNoParameters()
        {
            tealium.Track(new TealiumView("TestView"));
        }

        [Test]
        public void DoesNotThrowWhenTracksViewWithNullParameters()
        {
            tealium.Track(new TealiumView("TestView", null));
        }

        [Test]
        public void DoesNotThrowWhenTracksViewWith2Params()
        {
            tealium.Track(new TealiumView("TestView", new Dictionary<string, object>()
            {
                {"p1", "Parameter 1 value"},
                {"p2", "Parameter 2 value"},
            }));
        }

        [Test]
        public void DoesNotThrowWhenTracksEventWithNoParameters()
        {
            tealium.Track(new TealiumEvent("TestEvent"));
        }

        [Test]
        public void DoesNotThrowWhenTracksEventWithNullParameters()
        {
            tealium.Track(new TealiumEvent("TestEvent", null));
        }

        [Test]
        public void DoesNotThrowWhenTracksEventWith2Params()
        {
            tealium.Track(new TealiumEvent("TestEvent", new Dictionary<string, object>()
            {
                {"p1", "Parameter 1 value"},
                {"p2", "Parameter 2 value"},
            }));
        }

        [Test]
        public void DoesNotThrowWhenTracksEventWithNestedDictionary()
        {
            tealium.Track(new TealiumEvent("TestEvent", new Dictionary<string, object>
            {
                { "string", new Dictionary<string, object>
                    {
                        { "string1", "string2" },
                        { "int", new List<int> { 10, 11, 12 } },
                        { "double", new List<double> { 10.1d, 11.2d, 12.3 } },
                    }
                },
                { "long", new List<long> { 100L, 200L, 300L } },
                { "bool", new List<bool> { true, false, true } },
                { "mixed", new List<object> { "string", 10, 10.1d, 100L, true } }
            }));
        }
    }
}
