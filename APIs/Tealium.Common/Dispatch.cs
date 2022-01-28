using System;
using System.Collections.Generic;

namespace Tealium
{
    /// <summary>
    /// Abstract representation of a Tealium event.
    /// </summary>
    public abstract class Dispatch
    {
        public IDictionary<string, object> DataLayer { get; protected set; }
        public string Type { get; protected set; }
    }

    /// <summary>
    /// Dispatch implementation representing a screen view.
    /// </summary>
    public sealed class TealiumView : Dispatch
    {
        public string ViewName { get; private set; }

        public TealiumView(string name) : this(name, new Dictionary<string, object>()) { }

        public TealiumView(string name, Dictionary<string, object> dataLayer)
        {
            Type = "view";
            ViewName = name;
            DataLayer = dataLayer;
        }
    }

    /// <summary>
    /// Dispatch implementation representing an interaction.
    /// </summary>
    public sealed class TealiumEvent : Dispatch
    {
        public string EventName { get; private set; }

        public TealiumEvent(string name) : this(name, new Dictionary<string, object>()) { }

        public TealiumEvent(string name, Dictionary<string, object> dataLayer)
        {
            Type = "event";
            EventName = name;
            DataLayer = dataLayer;
        }
    }
}
