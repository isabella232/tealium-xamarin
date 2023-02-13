using System;
using System.Collections.Generic;

namespace Tealium
{
    public class TealiumInstanceManager : ITealiumInstanceManager
    {
        const int DEFAULT_INSTANCE_CNT = 2;

        readonly ITealiumInstanceFactory instanceFactory;

        Dictionary<string, ITealium> tealiumInstances;
        object sync = new object();

        public TealiumInstanceManager(ITealiumInstanceFactory instanceFactory)
        {
            this.instanceFactory = instanceFactory ?? throw new ArgumentNullException(nameof(instanceFactory));
            tealiumInstances = new Dictionary<string, ITealium>(DEFAULT_INSTANCE_CNT);
        }

        public ITealium CreateInstance(TealiumConfig config)
        {
            return CreateInstance(config, null);
        }

        public ITealium CreateInstance(TealiumConfig config, Action<ITealium> ready)
        {
            lock (sync)
            {
                if (tealiumInstances.ContainsKey(config.InstanceId))
                {
                    throw new InvalidOperationException($"Tried to duplicate instance {config.InstanceId}");
                }
                else
                {
                    ITealium newInstance = instanceFactory.CreateInstance(config, (teal) =>
                    {
                        if (teal != null)
                        {
                            var pluginData = new Dictionary<string, object>{
                            { Constants.DataLayerKeys.pluginName, Constants.Values.pluginName },
                            { Constants.DataLayerKeys.pluginVersion, Constants.Values.pluginVersion }
                            };
                            teal.AddToDataLayer(pluginData, Expiry.Forever);
                        }
                        if (ready != null)
                        {
                            ready(teal);
                        }
                    });
                    tealiumInstances.Add(config.InstanceId, newInstance);
                    return newInstance;
                }
            }
        }

        public bool DisposeInstace(string instanceId)
        {
            lock (sync)
            {
                var disposed = DisposeInstanceInternal(instanceId);
                if (disposed != null)
                {
                    tealiumInstances.Remove(instanceId);
                }
                return disposed != null;
            }
        }

        public bool DisposeAllInstances()
        {
            bool result = true;
            lock (sync)
            {
                foreach (string id in tealiumInstances.Keys)
                {
                    var disposed = DisposeInstanceInternal(id);
                    result = result && disposed != null;
                }
                tealiumInstances = new Dictionary<string, ITealium>(DEFAULT_INSTANCE_CNT);
            }
            return result;
        }

        ITealium DisposeInstanceInternal(string instanceId)
        {
            if (instanceId != null && tealiumInstances.ContainsKey(instanceId))
            {
                ITealium tealium = tealiumInstances[instanceId];
                try
                {
                    tealium.Dispose();
                }
                catch (Exception e)
                {
                    //TODO: Better log?
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                return tealium;
            }
            else
            {
                return null;
            }
        }

        public ITealium GetExistingInstance(string instanceId)
        {
            lock (sync)
            {
                if (instanceId != null && tealiumInstances.ContainsKey(instanceId))
                {
                    ITealium tealium = tealiumInstances[instanceId];
                    return tealium;
                }
                else
                {
                    return null;
                }
            }
        }

        public ITealium GetExistingInstance(string account, string profile, Environment environment)
        {
            return GetExistingInstance(GetInstanceId(account, profile, environment));
        }

        public Dictionary<string, ITealium> GetAllInstances()
        {
            lock (sync)
            {
                return new Dictionary<string, ITealium>(tealiumInstances);
            }
        }

        public static string GetInstanceId(string account, string profile, Environment environment)
        {
            return account + "." + profile + "." + environment.ToString().ToLower();
        }
    }
}
