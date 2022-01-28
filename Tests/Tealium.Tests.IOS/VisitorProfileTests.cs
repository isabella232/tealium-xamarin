using System;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Platform.iOS;
using Foundation;

namespace Tealium.Tests.iOS
{
    [TestFixture]
    public class VisitorProfileTests
    {
        [Test]
        public void CanCreateFullProfileFromJSON()
        {
            TealiumVisitorProfileWrapper profile = GetVisitorProfileWrapper();
            Assert.NotNull(profile);
            Assert.NotNull(profile.Numbers);
            Assert.NotNull(profile.Strings);
            Assert.NotNull(profile.Booleans);
            Assert.NotNull(profile.ArraysOfNumbers);
            Assert.NotNull(profile.ArraysOfStrings);
            Assert.NotNull(profile.ArraysOfBooleans);
            Assert.NotNull(profile.SetsOfStrings);
            Assert.NotNull(profile.Dates);
            Assert.NotNull(profile.Tallies);
            Assert.NotNull(profile.Audiences);
            Assert.NotNull(profile.Badges);
            Assert.NotNull(profile.CurrentVisit);
            Assert.NotNull(profile.CurrentVisit.Numbers);
            Assert.NotNull(profile.CurrentVisit.Strings);
            Assert.NotNull(profile.CurrentVisit.Booleans);
            Assert.NotNull(profile.CurrentVisit.ArraysOfNumbers);
            Assert.NotNull(profile.CurrentVisit.ArraysOfStrings);
            Assert.NotNull(profile.CurrentVisit.ArraysOfBooleans);
            Assert.NotNull(profile.CurrentVisit.SetsOfStrings);
            Assert.NotNull(profile.CurrentVisit.Dates);
            Assert.NotNull(profile.CurrentVisit.Tallies);
        }

        [Test]
        public void ExpectedDataFromJSON()
        {
            TealiumVisitorProfileWrapper visitor = GetVisitorProfileWrapper();
            Assert.NotNull(visitor.Audiences?["services-christina_advance_110"]);
            Assert.Null(visitor.Audiences?["blah"]);
            Assert.AreEqual(visitor.Badges?["8535"], new NSNumber(true));
            Assert.AreEqual(visitor.Badges?["6301"], new NSNumber(true));
            Assert.Null(visitor.Badges?["9999"]);
            Assert.Null(visitor.Tallies?["9999"]);
            Assert.NotNull(visitor.Tallies?["8481"]);
            NSDictionary tally = (NSDictionary)visitor.Tallies?["8481"];
            Assert.NotNull(tally["category 5"]);
            Assert.Null(tally["category 99"]);
            Assert.NotNull(visitor.CurrentVisit.Strings?["44"]);
            Assert.NotNull(visitor.CurrentVisit.Strings?["44"]);
            Assert.NotNull(visitor.CurrentVisit.Strings?["44"]);
            Assert.Null(visitor.CurrentVisit.Strings?["999"]);
            Assert.NotNull(visitor.CurrentVisit.Dates?["11"]);
        }

        [Test]
        public void TestBadges()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.AreEqual(visitor.Badges?["8535"], true);
            Assert.AreEqual(visitor.Badges?["6301"], true);
            Assert.False(visitor.Badges.ContainsKey("9999"));
        }

        [Test]
        public void TestNumbers()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.AreEqual(visitor.Numbers?["22"], 25.0);
            Assert.AreEqual(visitor.Numbers?["6113"], 0.0);
            Assert.False(visitor.Numbers.ContainsKey("100"));
        }

        [Test]
        public void TestStrings()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.AreEqual(visitor.Strings?["8480"], "category 5");
            Assert.AreEqual(visitor.Strings?["60"], "browser");
            Assert.False(visitor.Strings.ContainsKey("5866"));
        }

        [Test]
        public void TestBooleans()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.AreEqual(visitor.Booleans?["27"], true);
        }

        [Test]
        public void TestAudiences()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.Audiences?["services-christina_advance_110"]);
            Assert.False(visitor.Audiences.ContainsKey("blah"));
        }

        [Test]
        public void TestTallies()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.False(visitor.Tallies.ContainsKey("9999"));
            Assert.NotNull(visitor.Tallies?["8481"]);
            var tally = visitor.Tallies?["8481"];
            Assert.NotNull(tally["category 5"]);
            Assert.False(tally.ContainsKey("category 99"));
        }

        [Test]
        public void TestArrayOfNumbers()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.False(visitor.ArraysOfNumbers.ContainsKey("9999"));
            Assert.NotNull(visitor.ArraysOfNumbers?["8487"]);
            var arr = visitor.ArraysOfNumbers?["8487"];
            Assert.AreEqual(arr[0], 3.0);
        }

        [Test]
        public void TestArrayOfBooleans()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.False(visitor.ArraysOfBooleans.ContainsKey("9999"));
            Assert.NotNull(visitor.ArraysOfBooleans?["8479"]);
            var arr = visitor.ArraysOfBooleans?["8479"];
            Assert.AreEqual(arr[0], true);
        }

        [Test]
        public void TestArrayOfStrings()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.False(visitor.ArraysOfStrings.ContainsKey("9999"));
            Assert.NotNull(visitor.ArraysOfStrings?["8483"]);
            var arr = visitor.ArraysOfStrings?["8483"];
            Assert.AreEqual(arr[0], "category 4");
        }

        [Test]
        public void TestSetsOfStrings()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.False(visitor.SetsOfStrings.ContainsKey("9999"));
            Assert.NotNull(visitor.SetsOfStrings?["49"]);
            var set = visitor.SetsOfStrings?["49"];
            Assert.True(set.Contains("Chrome"));
        }

        [Test]
        public void TestCurrentVisit()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            var currentVisit = visitor.CurrentVisit;
            Assert.NotNull(currentVisit.Strings?["44"]);
            Assert.NotNull(currentVisit.Strings?["44"]);
            Assert.NotNull(currentVisit.Strings?["44"]);
            Assert.False(currentVisit.Strings.ContainsKey("999"));
            Assert.NotNull(currentVisit.Dates?["11"]);
        }

        [Test]
        public void TestCurrentVisitNumbers()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.AreEqual(currentVisit.Numbers?["12"], 0.2);
            Assert.AreEqual(currentVisit.Numbers?["6101"], -1.0);
            Assert.False(currentVisit.Numbers.ContainsKey("800"));
        }

        [Test]
        public void TestCurrentVisitStrings()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.AreEqual(currentVisit.Strings?["44"], "Chrome");
            Assert.AreEqual(currentVisit.Strings?["46"], "Mac desktop");
            Assert.False(currentVisit.Strings.ContainsKey("479"));
        }

        [Test]
        public void TestCurrentVisitBooleans()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.AreEqual(currentVisit.Booleans?["8475"], true);
        }

        [Test]
        public void TestCurrentVisitTallies()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.False(currentVisit.Tallies.ContainsKey("9999"));
            Assert.NotNull(currentVisit.Tallies?["8481"]);
            var tally = currentVisit.Tallies?["8481"];
            Assert.NotNull(tally["category 5"]);
            Assert.False(tally.ContainsKey("category 99"));
        }

        [Test]
        public void TestCurrentVisitArrayOfNumbers()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.False(currentVisit.ArraysOfNumbers.ContainsKey("9999"));
            Assert.NotNull(currentVisit.ArraysOfNumbers?["8487"]);
            var arr = currentVisit.ArraysOfNumbers?["8487"];
            Assert.AreEqual(arr[0], 3.0);
        }

        [Test]
        public void TestCurrentVisitArrayOfBooleans()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.False(currentVisit.ArraysOfBooleans.ContainsKey("9999"));
            Assert.NotNull(currentVisit.ArraysOfBooleans?["8479"]);
            var arr = currentVisit.ArraysOfBooleans?["8479"];
            Assert.AreEqual(arr[0], true);
        }

        [Test]
        public void TestCurrentVisitArrayOfStrings()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.False(currentVisit.ArraysOfStrings.ContainsKey("9999"));
            Assert.NotNull(currentVisit.ArraysOfStrings?["8483"]);
            var arr = currentVisit.ArraysOfStrings?["8483"];
            Assert.AreEqual(arr[0], "category 4");
        }

        [Test]
        public void TestCurrentVisitSetsOfStrings()
        {
            VisitorProfileIOS visitor = GetVisitorProfileIOS();
            Assert.NotNull(visitor);
            Assert.NotNull(visitor.CurrentVisit);
            var currentVisit = visitor.CurrentVisit;
            Assert.False(currentVisit.SetsOfStrings.ContainsKey("9999"));
            Assert.NotNull(currentVisit.SetsOfStrings?["49"]);
            var set = currentVisit.SetsOfStrings?["49"];
            Assert.True(set.Contains("Chrome"));
        }

        TealiumVisitorProfileWrapper visitorProfile;
        TealiumVisitorProfileWrapper GetVisitorProfileWrapper()
        {
            if (visitorProfile != null)
            {
                return visitorProfile;
            }
            NSUrl path = NSBundle.MainBundle.GetUrlForResource("Assets/visitor", "json");
            NSData data = NSData.FromUrl(path);
            NSError err;
            TealiumVisitorProfileWrapper profile = new TealiumVisitorProfileWrapper(data, out err);
            if (err != null)
            {
                throw new Exception();
            }
            visitorProfile = profile;
            return profile;
        }

        VisitorProfileIOS GetVisitorProfileIOS()
        {
            TealiumVisitorProfileWrapper profile = GetVisitorProfileWrapper();
            VisitorProfileIOS visitor = new VisitorProfileIOS(profile);
            return visitor;
        }
    }
}
