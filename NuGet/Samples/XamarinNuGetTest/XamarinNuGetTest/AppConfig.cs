using System;
using Tealium;

namespace XamarinNuGetTest
{
    public class AppConfig
    {
        public ITealiumInstanceManager instanceManager;

        public AppConfig(ITealiumInstanceManager instanceManager)
        {
            this.instanceManager = instanceManager;
        }
    }
}
