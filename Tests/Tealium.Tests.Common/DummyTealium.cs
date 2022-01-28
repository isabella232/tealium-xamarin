using System;
using System.Collections.Generic;

namespace Tealium.Tests.Common
{
    /// <summary>
    /// Dummy Tealium - for tests requiring ITealium instance.
    /// </summary>
    public class DummyTealium : ITealium
    {
        public string InstanceId => "DummyTealium";

        public ConsentManager ConsentManager => throw new NotImplementedException();

        public void AddRemoteCommand(IRemoteCommand remoteCommand)
        {
            throw new NotImplementedException();
        }

        public void AddToDataLayer(IDictionary<string, object> data, Expiry expiry)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<ConsentManager.ConsentCategory> GetConsentCategories()
        {
            throw new NotImplementedException();
        }

        public ConsentManager.ConsentStatus GetConsentStatus()
        {
            throw new NotImplementedException();
        }

        public object GetFromDataLayer(string id)
        {
            throw new NotImplementedException();
        }

        public string GetVisitorId()
        {
            throw new NotImplementedException();
        }

        public void JoinTrace(string id)
        {
            throw new NotImplementedException();
        }

        public void LeaveTrace()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromDataLayer(ICollection<string> keys)
        {
            throw new NotImplementedException();
        }

        public void RemoveRemoteCommand(string id)
        {
            throw new NotImplementedException();
        }

        public void SetConsentCategories(List<ConsentManager.ConsentCategory> categories)
        {
            throw new NotImplementedException();
        }

        public AnyCollectionKey AddConsentExpiryListener(Action callback)
        {
            throw new NotImplementedException();
        }

        public void RemoveConsentExpiryListener(AnyCollectionKey key)
        {
            throw new NotImplementedException();
        }

        public void SetConsentStatus(ConsentManager.ConsentStatus status)
        {
            throw new NotImplementedException();
        }

        public AnyCollectionKey AddVisitorServiceListener(Action<IVisitorProfile> callback)
        {
            throw new NotImplementedException();
        }

        public void RemoveVisitorServiceListener(AnyCollectionKey key)
        {
            throw new NotImplementedException();
        }

        public void Track(Dispatch dispatch)
        {
            throw new NotImplementedException();
        }
    }
}
