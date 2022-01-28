using NUnit.Framework;
using System;
namespace Tealium.Tests.Common
{
    [TestFixture()]
    public abstract class TealiumInstanceConsentManagerTestsBase
    {
        ITealiumInstanceManager instanceManager;
        static ITealium tealium;

        public TealiumInstanceConsentManagerTestsBase() {
            if (tealium == null)
            {
                instanceManager = new TealiumInstanceManager(GetInstanceFactory());
                tealium = instanceManager.CreateInstance(TealiumConfigHelper.GetTestConfigWithConsentManagerEnabled());
            }
        }

        protected abstract ITealiumInstanceFactory GetInstanceFactory();

        [TestFixtureSetUp]
        public void SetUp()
        {
            // Reset everything to start from scratch.
        }

        [TestFixtureTearDown]
        public void Cleanup()
        {
            tealium.ConsentManager?.ResetUserConsentPreferences();
        }

        [Test]
        public void ConsentManagerIsEnabled()
        {
            Wait();
            Assert.NotNull(tealium.ConsentManager);
        }

        [Test]
        public void GrantFullConsent()
        {
            Wait();
            tealium.ConsentManager.UserConsentStatus = ConsentManager.ConsentStatus.Consented;

            ConsentManager.ConsentCategory[] sortedArray = tealium.ConsentManager.UserConsentCategories;
            Array.Sort(sortedArray);
            Array.Sort(ConsentManager.AllCategories);

            // Setting to CONSENTED will automatically opt into All Categories, so we should test for both
            Assert.AreEqual(ConsentManager.ConsentStatus.Consented, tealium.ConsentManager.UserConsentStatus);
            Assert.AreEqual(ConsentManager.AllCategories, sortedArray);
        }

        [Test]
        public void GrantNoConsent()
        {
            Wait();
            tealium.ConsentManager.UserConsentStatus = ConsentManager.ConsentStatus.NotConsented;

            ConsentManager.ConsentCategory[] sortedArray = tealium.ConsentManager.UserConsentCategories;
            Array.Sort(sortedArray);

            // Setting to NOT_CONSENTED will automatically opt into No Categories, so we should test for both
            Assert.AreEqual(ConsentManager.ConsentStatus.NotConsented, tealium.ConsentManager.UserConsentStatus);
            Assert.AreEqual(ConsentManager.NoCategories, sortedArray);
        }

        [Test]
        public void GrantPartialConsent()
        {
            Wait();
            ConsentManager.ConsentCategory[] partialCategories = GetCategorySubset(3);
            tealium.ConsentManager.UserConsentStatusWithCategories(ConsentManager.ConsentStatus.Consented, partialCategories);

            ConsentManager.ConsentCategory[] sortedArray = tealium.ConsentManager.UserConsentCategories;
            Array.Sort(sortedArray);
            Array.Sort(partialCategories);

            Assert.AreEqual(ConsentManager.ConsentStatus.Consented, tealium.ConsentManager.UserConsentStatus);
            Assert.AreEqual(partialCategories, sortedArray);
        }

        [Test]
        public void ResetUserPreferences()
        {
            Wait();
            tealium.ConsentManager.ResetUserConsentPreferences();

            Assert.AreEqual(ConsentManager.ConsentStatus.Unknown, tealium.ConsentManager.UserConsentStatus);
            Assert.AreEqual(ConsentManager.NoCategories, tealium.ConsentManager.UserConsentCategories);
        }


        #region Helpers

        private ConsentManager.ConsentCategory[] GetCategorySubset(int noOfCategories = 3)
        {
            // Just in case someone stupidly supplies a number greater than the actual number of categories available.
            if (noOfCategories > ConsentManager.AllCategories.Length)
            {
                noOfCategories = ConsentManager.AllCategories.Length;
            }

            ConsentManager.ConsentCategory[] partialCategories = new ConsentManager.ConsentCategory[noOfCategories];
            Array.Copy(ConsentManager.AllCategories, partialCategories, noOfCategories);

            return partialCategories;
        }

        private static void Wait()
        {
            TaskWaitHelper.Wait(50);
        }
        #endregion Helpers
    }
}
