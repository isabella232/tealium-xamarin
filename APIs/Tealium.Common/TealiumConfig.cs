using System;
using System.Collections.Generic;
using static Tealium.ConsentManager;

#nullable enable
namespace Tealium
{
    /// <summary>
    /// Holds all Tealium configuration data.
    /// Used to create new <see cref="ITealium"/> instances by <see cref="ITealiumInstanceFactory"/>
    /// and <see cref="ITealiumInstanceManager"/>.
    /// </summary>
    public sealed class TealiumConfig
    {
        /// <summary>
        /// Instance name identifying this instance of tealium.
        /// </summary>
        public string InstanceId { get => TealiumInstanceManager.GetInstanceId(Account, Profile, Environment); }

        /// <summary>
        /// The Tealium Account name
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// The Tealium Profile name
        /// </summary>
        public string Profile { get; private set; }

        /// <summary>
        /// The Tealium environment.
        /// </summary>
        public Environment Environment { get; private set; }

        /// <summary>
        /// A list of the data Collectors to enable for this instance.
        /// </summary>
        public IList<Collectors> Collectors { get; private set; }

        /// <summary>
        /// A list of the Dispatchers to enable for this instance.
        /// </summary>
        public IList<Dispatchers> Dispatchers { get; private set; }

        /// <summary>
        /// The Data Source id that identifies this application.
        /// </summary>
        public string? DataSource { get; set; }

        /// <summary>
        /// Sets a known custom visitor id to use as the tealium_visitor_id
        /// </summary>
        public string? CustomVisitorId { get; set; }

        /// <summary>
        /// (iOS only) Sets whether or not to include memory usage data
        /// </summary>
        public bool? MemoryReportingEnabled { get; set; }

        /// <summary>
        /// Overrides the Collect URL that individual event data is sent to from the
        /// Collect Dispatcher
        /// </summary>
        public string? OverrideCollectURL { get; set; }

        /// <summary>
        /// Overrides the Collect URL that batched event data is sent to from
        /// the Collect Dispatcher
        /// </summary>
        public string? OverrideCollectBatchURL { get; set; }

        /// <summary>
        /// Overrides just the domain used by Collect URL; used for keeping
        /// data within a specific Tealium region.
        /// </summary>
        public string? OverrideCollectDomain { get; set; }

        /// <summary>
        /// Overrides the URL used when using remote library settings.
        /// </summary>
        public string? OverrideLibrarySettingsURL { get; set; }

        /// <summary>
        /// Overrides the URL used by the TagManagement Dispatcher.
        /// </summary>
        public string? OverrideTagManagementURL { get; set; }

        /// <summary>
        /// Enables or disables whether or not to track deeplinks automatically
        /// </summary>
        public bool? DeepLinkTrackingEnabled { get; set; }

        /// <summary>
        /// Enabled or disables the QR Trace feature.
        /// </summary>
        public bool? QrTraceEnabled { get; set; }

        /// <summary>
        /// Overrides the LogLevel in use for Tealium logs, otherwise the
        /// LogLevel will be take from the Environment used.
        /// </summary>
        public LogLevel? LogLevel { get; set; }

        /// <summary>
        /// Enables or disables whether consent changes should be logged.
        /// </summary>
        public bool? ConsentLoggingEnabled { get; set; }

        /// <summary>
        /// Sets the Consent Policy that should be enforced.
        /// </summary>
        public ConsentPolicy? ConsentPolicy { get; set; }

        /// <summary>
        /// Sets the length of time that provided consent should remain current.
        /// See <see cref="ITealium.SetConsentExpiryListener(Action)"/>.
        /// </summary>
        public ConsentExpiry? ConsentExpiry { get; set; }

        /// <summary>
        /// Allows the Consent Categories key to be overridden
        /// </summary>
        public string? OverrideConsentCategoriesKey { get; set; }

        /// <summary>
        /// Enables or disables automated tracking of application lifecycle
        /// events (launch, sleep, wake)
        /// </summary>
        public bool? LifecycleAutotrackingEnabled { get; set; }

        /// <summary>
        /// Enables or disables whether or not to use library settings from a
        /// remote source.
        /// </summary>
        public bool? UseRemoteLibrarySettings { get; set; }

        /// <summary>
        /// Enables or disables the Visitor Service module. See
        /// <see cref="ITealium.SetVisitorServiceListener(Action{IVisitorProfile})"/>.
        /// </summary>
        public bool? VisitorServiceEnabled { get; set; }

        /// <summary>
        /// A key used to inspect the data layer for a stitching key to be used to store the current visitorId with.
        ///
        /// The current visitorId is stored with this key so if this key changes, we automatically reset it to a new value, and if it comes back to the old value we have a copy and don't have to generate a new one.
        /// Something like an email adress, or a unique identifier of the current user should be the field to which this key is pointing to.
        ///
        /// Note that the key is hashed and not saved in plain text when stored on disk.
        /// </summary>
        public string? VisitorIdentityKey { get; set; }

        /// <summary>
        /// Sets a list of IRemoteCommand objects to load on initialization.
        /// </summary>
        public IList<IRemoteCommand>? RemoteCommands { get; set; }

        /// <summary>
        /// A List of Dispatch Validators to control queueing and dropping of dispatches.
        /// </summary>
        public IList<IDispatchValidator> DispatchValidators { get; } = new List<IDispatchValidator>();

        /// <summary>
        /// A List of Tealium event listeners to be added to the Tealium instance.
        /// </summary>
        public IList<ITealiumEventListener> Listeners { get; } = new List<ITealiumEventListener>();

        /// <summary>
        /// Holds all relevant configuration information to construct an <see cref="ITealium"> object.
        /// </summary>
        /// <param name="account">The Tealium Account name</param>
        /// <param name="profile">The Tealium Profile name</param>
        /// <param name="env">The Tealium environment</param>
        /// <param name="dispatchers">A collection of required <see cref="Dispatchers"/></param>
        /// <param name="collectors">A collection of required <see cref="Collectors"/></param>
        /// <param name="dataSource">Optional - Data Source string to identify this app</param>
        /// <param name="customVisitorId">Optional - An known visitor id.</param>
        /// <param name="memoryReportingEnabled">Optional - whether to include memory usage statistics</param>
        /// <param name="overrideCollectURL">Optional - Overrides the default destination URL for the Collect Dispatcher</param>
        /// <param name="overrideCollectBatchURL">Optional - Overrides the default destination URL for batched events in the Collect Dispatcher</param>
        /// <param name="overrideCollectDomain">Optional - Optional - Overrides the default destination domain only for the Collect Dispatcher</param>
        /// <param name="overrideLibrarySettingsURL">Optional - Optional - Overrides the URL to pull remote library settings from</param>
        /// <param name="overrideTagManagementURL">Optional - Overrides the default TagManagement URL</param>
        /// <param name="deepLinkTrackingEnabled">Optional - enables/disables deep link tracking</param>
        /// <param name="qrTraceEnabled">Optional - enables/disables starting a trace from a QR codes</param>
        /// <param name="logLevel">Optional - sets the log level</param>
        /// <param name="consentLoggingEnabled">Optional - enables/disables whether to log consent updates</param>
        /// <param name="consentPolicy">Optional - sets the consent policy to use</param>
        /// <param name="consentExpiry">Optional - sets the consent expiry to adhere to</param>
        /// <param name="lifecycleAutotrackingEnabled">Optional - enables/disables lifecycle auto tracking</param>
        /// <param name="useRemoteLibrarySettings">Optional - enables/disbles fetching remote library settings</param>
        /// <param name="visitorServiceEnabled">Optional - enables/disables the visitor service</param>
        /// <param name="remoteCommands">Optional - collection of <see cref="IRemoteCommand"/> objects</param>
        /// <param name="visitorIdentityKey">Optional - the data layer key in which the application will store the identifier for the user, used for visitor switching</param>
        /// <param name="overrideConsentCategoriesKey">Optional - Overrides the consent categories key</param>
        public TealiumConfig(
            string account,
            string profile,
            Environment env,
            List<Dispatchers> dispatchers,
            List<Collectors> collectors,
            // optional params
            string? dataSource = null,
            string? customVisitorId = null,
            bool? memoryReportingEnabled = null,
            string? overrideCollectURL = null,
            string? overrideCollectBatchURL = null,
            string? overrideCollectDomain = null,
            string? overrideLibrarySettingsURL = null,
            string? overrideTagManagementURL = null,
            bool? deepLinkTrackingEnabled = null,
            bool? qrTraceEnabled = null,
            LogLevel? logLevel = null,
            bool? consentLoggingEnabled = null,
            ConsentPolicy? consentPolicy = null,
            ConsentExpiry? consentExpiry = null,
            //bool? batchingEnabled = null,
            bool? lifecycleAutotrackingEnabled = null,
            bool? useRemoteLibrarySettings = null,
            bool? visitorServiceEnabled = null,
            List<IRemoteCommand>? remoteCommands = null,
            string? visitorIdentityKey = null,
            string? overrideConsentCategoriesKey = null
     )
        {
            Account = account;
            Profile = profile;
            Environment = env;
            Dispatchers = dispatchers;
            Collectors = collectors;
            DataSource = dataSource;
            CustomVisitorId = customVisitorId;
            MemoryReportingEnabled = memoryReportingEnabled;
            OverrideCollectURL = overrideCollectURL;
            OverrideCollectBatchURL = overrideCollectBatchURL;
            OverrideCollectDomain = overrideCollectDomain;
            OverrideLibrarySettingsURL = overrideLibrarySettingsURL;
            OverrideTagManagementURL = overrideTagManagementURL;
            DeepLinkTrackingEnabled = deepLinkTrackingEnabled;
            OverrideConsentCategoriesKey = overrideConsentCategoriesKey;
            QrTraceEnabled = qrTraceEnabled;
            LogLevel = logLevel;
            ConsentLoggingEnabled = consentLoggingEnabled;
            ConsentPolicy = consentPolicy;
            ConsentExpiry = consentExpiry;
            LifecycleAutotrackingEnabled = lifecycleAutotrackingEnabled;
            UseRemoteLibrarySettings = useRemoteLibrarySettings;
            VisitorServiceEnabled = visitorServiceEnabled;
            RemoteCommands = remoteCommands;
            VisitorIdentityKey = visitorIdentityKey;
        }

    }

    /// <summary>
    /// The available data Collectors
    /// </summary>
    public enum Collectors
    {
        AppData, Connectivity, DeviceData, LifeCycle
    }

    /// <summary>
    /// The available event dispatchers
    /// </summary>
    public enum Dispatchers
    {
        TagManagement, Collect, RemoteCommands
    }

    /// <summary>
    /// The available Tealium environments
    /// </summary>
    public enum Environment
    {
        Dev, Qa, Prod
    }

    /// <summary>
    /// Expiry time for data stored in the Data Layer
    /// </summary>
    public enum Expiry
    {
        Forever, Session, UntilRestart
    }

    /// <summary>
    /// The available Log Levels
    /// </summary>
    public enum LogLevel
    {
        Dev, Qa, Prod, Silent
    }

    /// <summary>
    /// Available time units for <see cref="ConsentExpiry"/>
    /// </summary>
    public enum TimeUnit
    {
        Minutes, Hours, Days, Months
    }
}
