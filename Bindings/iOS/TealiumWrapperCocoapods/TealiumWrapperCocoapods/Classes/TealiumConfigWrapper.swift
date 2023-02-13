//
//  TealiumConfigWrapper.swift
//  TealiumSwiftObjCWrapper
//
//  Created by Enrico Zannini on 21/07/21.
//

import UIKit
import WebKit
import TealiumSwift


@objc(TealiumConfigWrapper)
public class TealiumConfigWrapper: NSObject {
    
    let config: TealiumConfig
    @objc public init(account: String,
         profile: String,
         environment: String,
         dataSource: String? = nil,
         options: [String: Any]?) {
        config = TealiumConfig(account: account, profile: profile, environment: environment, dataSource: dataSource, options: options)
        config.remoteAPIEnabled = true
    }
    
    
    @objc public var account: String {
        config.account
    }

    @objc public var profile: String {
        config.profile
    }

    @objc public var environment: String {
        config.environment
    }
    
    private var requiredForBindingToWork: String?
    @objc public var dataSourceKey: String? {
        get {
            config.dataSource
        }
        set {
            requiredForBindingToWork = newValue // Needs to do this otherwise the binding won't work
            print("Not Implemented set DataSource:", newValue ?? "")
        }
        
    }

    @objc public var options: [String : Any] {
        get {
            config.options
        }
        set {
            config.options = newValue
        }
    }

    /// Define the conversion event and value if using `SKAdNetwork.updateConversionValue(_ value:)`
    /// The key in the dictionary is the `tealium_event` for which to count as a conversion and the value in the dictionary is the variable that holds the conversion value.
    /// Conversion value must be an `Int` and between `0-63`
    /// - Usage: `config.skAdConversionKeys = ["purchase": "order_subtotal"]`
    @objc public var skAdConversionKeys: [String : String]? {
        get {
            config.skAdConversionKeys
        }
        set {
            config.skAdConversionKeys = newValue
        }
    }

    /// Set to `false` to disable data migration from the Objective-c library
    /// Default `true`.
    @objc public var shouldMigratePersistentData: Bool {
        get {
            config.shouldMigratePersistentData
        }
        set {
            config.shouldMigratePersistentData = newValue
        }
    }

    /// Provides the option to add custom `DispatchValidator`s to control whether events should be dispatched, queued, or dropped
    @objc public var dispatchValidators: [DispatchValidatorWrapper]? {
        get {
            config.dispatchValidators as? [DispatchValidatorWrapper]
        }
        set {
            config.dispatchValidators = newValue
        }
    }

    /// Provides the option to add custom `DispatchListener`s to listen for tracking calls just prior to dispatch
    @objc public var dispatchListeners: [DispatchListenerWrapper]? {
        get {
            config.dispatchListeners as? [DispatchListenerWrapper]
        }
        set {
            config.dispatchListeners = newValue
        }
    }

    /// Allows configuration of optional Tealium Collectors
    @objc public var collectors: [Int]? { // Array of enums are not supported!?!?!?!?!
        get {
            config.collectors?.compactMap { CollectorType(collector: $0)?.rawValue }
        }
        set {
            config.collectors = newValue?.compactMap{ CollectorType(rawValue: $0)?.collector }
        }
    }

    /// Allows configuration of optional Tealium Dispatchers
    @objc public var dispatchers: [Int]? { // Array of enums are not supported!?!?!?!?!
        get {
            config.dispatchers?.compactMap { DispatcherType(dispatcher: $0)?.rawValue }
        }
        set {
            config.dispatchers = newValue?.compactMap{ DispatcherType(rawValue: $0)?.dispatcher }
        }
    }

    /// Returns a deep copy of the config object
    @objc public var deepCopy: TealiumConfigWrapper {
        return TealiumConfigWrapper(account: account,
                                    profile: profile,
                                    environment: environment,
                                    dataSource: dataSourceKey,
                                    options: options)
    }

    // This needs to hold the reference for the commands to be accessed from the outside
    @objc public var remoteCommands: [RemoteCommandWrapper]? {
        willSet {
            self.config.remoteCommands = newValue?.map{ $0.command }
        }
    }

    /// Returns a Boolean value indicating whether two values are equal.
    ///
    /// Equality is the inverse of inequality. For any values `a` and `b`,
    /// `a == b` implies that `a != b` is `false`.
    ///
    /// - Parameters:
    ///   - lhs: A value to compare.
    ///   - rhs: Another value to compare.
    public static func == (lhs: TealiumConfigWrapper, rhs: TealiumConfigWrapper) -> Bool {
        lhs.config == rhs.config
    }

    /// Sets a known visitor ID. Must be unique (i.e. UUID).
    /// Should only be used in cases where the user has an existing visitor ID
    @objc public var existingVisitorId: String? {
        get {
            config.existingVisitorId
        }
        set {
            config.existingVisitorId = newValue
        }
    }

    /// Whether or not remote publish settings should be used. Default `true`.
    @objc public var shouldUseRemotePublishSettings: Bool {
        get {
            config.shouldUseRemotePublishSettings
        }
        set {
            config.shouldMigratePersistentData = newValue
        }
    }

    /// Overrides the publish settings URL. Default is https://tags.tiqcdn.com/utag/ACCOUNT/PROFILE/ENVIRONMENT/mobile.html
    /// If overriding, you must provide the entire URL, not just the domain.
    /// Usage: `config.publishSettingsURL = "https://mycompany.org/utag/ACCOUNT/PROFILE/ENVIRONMENT/mobile.html"`
    /// Takes precendence over `publishSettingsProfile`
    @objc public var publishSettingsURL: String? {
        get {
            config.publishSettingsURL
        }
        set {
            config.publishSettingsURL = newValue
        }
    }

    /// Overrides the publish settings profile. Default is to use the profile set on the `TealiumConfig` object.
    /// Use this if you need to load the publish settings from a central profile that is different to the profile you're sending data to.
    /// Usage: `config.publishSettingsProfile = "myprofile"`
    @objc public var publishSettingsProfile: String? {
        get {
            config.publishSettingsProfile
        }
        set {
            config.publishSettingsProfile = newValue
        }
    }

    /// If `false`, the entire library is disabled, and no tracking calls are sent.
    @objc public var isEnabled: NSNumber? {
        get {
            guard let enabled = config.isEnabled else {
                return nil
            }
            return NSNumber(value: enabled)
        }
        set {
            config.isEnabled = newValue?.boolValue
        }
    }

    /// If `false`, the the tag management module is disabled and will not be used for dispatching events
    @objc public var isTagManagementEnabled: Bool {
        get {
            config.isTagManagementEnabled
        }
        set {
            config.isTagManagementEnabled = newValue
        }
    }

    /// If `false`, the the collect module is disabled and will not be used for dispatching events
    @objc public var isCollectEnabled: Bool {
        get {
            config.isCollectEnabled
        }
        set {
            config.isCollectEnabled = newValue
        }
    }

    /// If `true`, calls will only be sent if the device has sufficient battery levels (>20%).
    @objc public var batterySaverEnabled: NSNumber? {
        get {
            guard let batterySaverEnabled = config.batterySaverEnabled else {
                return nil
            }
            return NSNumber(value: batterySaverEnabled)
        }
        set {
            config.batterySaverEnabled = newValue?.boolValue
        }
    }

    /// How long the data persists in the app if no data has been sent back (`-1` = no dispatch expiration). Default value is `7` days.
    @objc public var dispatchExpiration: NSNumber? {
        get {
            guard let dispatchExpiration = config.dispatchExpiration else {
                return nil
            }
            return NSNumber(value: dispatchExpiration)
        }
        set {
            config.dispatchExpiration = newValue?.intValue
        }
    }

    /// Enables (`true`) or disables (`false`) event batching. Default `false`
    @objc public var batchingEnabled: NSNumber? {
        get {
            guard let batchingEnabled = config.batchingEnabled else {
                return nil
            }
            return NSNumber(value: batchingEnabled)
        }
        set {
            config.batchingEnabled = newValue?.boolValue
        }
    }

    /// How many events should be batched together
    /// If set to `1`, events will be sent individually
    @objc public var batchSize: Int {
        get {
            config.batchSize
        }
        set {
            config.batchSize = newValue
        }
    }

    /// The maximum amount of events that will be stored offline
    /// Oldest events are deleted to make way for new events if this limit is reached
    @objc public var dispatchQueueLimit: NSNumber? {
        get {
            guard let dispatchQueueLimit = config.dispatchQueueLimit else {
                return nil
            }
            return NSNumber(value: dispatchQueueLimit)
        }
        set {
            config.dispatchQueueLimit = newValue?.intValue
        }
    }

    /// Restricts event data transmission to wifi only
    /// Data will be queued if on cellular connection
    @objc public var wifiOnlySending: NSNumber? {
        get {
            guard let wifiOnlySending = config.wifiOnlySending else {
                return nil
            }
            return NSNumber(value: wifiOnlySending)
        }
        set {
            config.wifiOnlySending = newValue?.boolValue
        }
    }

    /// Determines how often the publish settings should be fetched from the CDN
    /// Usually set automatically by the response from the remote publish settings
    @objc public var minutesBetweenRefresh: NSNumber? {
        get {
            guard let minutesBetweenRefresh = config.minutesBetweenRefresh else {
                return nil
            }
            return NSNumber(value: minutesBetweenRefresh)
        }
        set {
            config.minutesBetweenRefresh = newValue?.doubleValue
        }
    }

    /// Sets the expiry for the Consent Manager preferences.
    @objc public var consentExpiry: ExpirationTime? {
        get {
            guard let expiry = config.consentExpiry else {
                return nil
            }
            return ExpirationTime(time: expiry.time, unit: TimeUnitWrapper(unit: expiry.unit))
        }
        set {
            guard let newValue = newValue else {
                config.consentExpiry = nil
                return
            }
            config.consentExpiry = (time: newValue.time, unit: newValue.unit.unit)
        }
    }

    /// Defines the consent expiration callback
    @objc public var onConsentExpiration: (() -> Void)? {
        get {
            config.onConsentExpiration
        }
        set {
            config.onConsentExpiration = newValue
        }
    }

    @objc public var deepLinkTrackingEnabled: Bool {
        get {
            config.deepLinkTrackingEnabled
        }
        set {
            config.deepLinkTrackingEnabled = newValue
        }
    }

    @objc public var qrTraceEnabled: Bool {
        get {
            config.qrTraceEnabled
        }
        set {
            config.qrTraceEnabled = newValue
        }
    }
    
    /// If enabled, this will add current memory reporting variables to the data layer
    /// Default is `false`
    @objc public var memoryReportingEnabled: Bool {
        get {
            config.memoryReportingEnabled
        }
        set {
            config.memoryReportingEnabled = newValue
        }
    }
    
    @objc public var overrideCollectURL: String? {
        get {
            config.overrideCollectURL
        }
        set {
            config.overrideCollectURL = newValue
        }
    }
    
    /// Overrides the default Collect endpoint URL￼.
    /// The full URL must be provided, including protocol and path.
    /// If using Tealium with a CNAMEd domain, the format would be: https://collect.mydomain.com/bulk-event (the path MUST be `/bulk-event`).
    /// If using your own custom endpoint, the URL can be any valid URL. Your endpoint must be prepared to accept batched events in Tealium's proprietary gzipped format.
    @objc public var overrideCollectBatchURL: String? {
        get {
            config.overrideCollectBatchURL
        }
        set {
            config.overrideCollectBatchURL = newValue
        }
    }

    /// Overrides the default Collect endpoint profile￼.
    @objc public var overrideCollectProfile: String? {
        get {
            config.overrideCollectProfile
        }
        set {
            config.overrideCollectProfile = newValue
        }
    }
    
    /// Overrides the default Collect domain only.
    /// Only the hostname should be provided, excluding the protocol, e.g. `my-company.com`
    @objc public var overrideCollectDomain: String? {
        get {
            config.overrideCollectDomain
        }
        set {
            config.overrideCollectDomain = newValue
        }
    }
    
    /// Adds optional delegates to the WebView instance.
    @objc public var webViewDelegates: [WKNavigationDelegate]? {
        get {
            config.webViewDelegates
        }
        set {
            config.webViewDelegates = newValue
        }
    }
    
    /// Optionally sets a `WKProcessPool` for the Tealium WKWebView to use.
    /// Required if multiple webviews are in use; prevents issues with cookie setting.
    /// A singleton WKProcessPool instance should be passed that is used for all `WKWebView`s in your app.
    @objc public var webviewProcessPool: WKProcessPool? {
        get {
            config.webviewProcessPool
        }
        set {
            config.webviewProcessPool = newValue
        }
    }
    
    /// Optionally sets a `WKWebViewConfiguration` for the Tealium WKWebView to use.
    /// Not normally required, but provides some additional customization options if requred.
    @objc public var webviewConfig: WKWebViewConfiguration {
        get {
            config.webviewConfig
        }
        set {
            config.webviewConfig = newValue
        }
    }

    /// Optional override for the tag management webview URL.
    @objc public var tagManagementOverrideURL: String? {
        get {
            config.tagManagementOverrideURL
        }
        set {
            config.tagManagementOverrideURL = newValue
        }
    }

    /// Gets the URL to be loaded by the webview (mobile.html).
    ///
    /// - Returns: `URL` representing either the custom URL provided in the `TealiumConfig` object, or the default Tealium mCDN URL
    @objc public var webviewURL: URL? {
        config.webviewURL
    }

    /// Sets a root view for `WKWebView` to be attached to. Only required for complex view hierarchies.
    @objc public var rootView: UIView? {
        get {
            config.rootView
        }
        set {
            config.rootView = newValue
        }
    }

    /// Sets the log level
    @objc public var logLevel: TealiumLogLevelWrapper {
        get {
            TealiumLogLevelWrapper(level: config.logLevel)
        }
        set {
            config.logLevel = newValue.logLevel
        }
    }

    /// Determines whether consent logging events should be sent to Tealium UDH￼.
    @objc public var consentLoggingEnabled: Bool {
        get {
            config.consentLoggingEnabled
        }
        set {
            config.consentLoggingEnabled = newValue
        }
    }

    /// Sets the consent policy (defaults to GDPR)￼. e.g. CCPA
    @objc public var consentPolicy: TealiumConsentPolicyWrapper {
        get {
            TealiumConsentPolicyWrapper(policy: config.consentPolicy)
        }
        set {
            config.consentPolicy = newValue.policy
        }
    }
    
    /// The number of events after which the queue will be flushed
    @objc public var dispatchAfter: Int {
        get {
            config.dispatchAfter
        }
        set {
            config.dispatchAfter = newValue
        }
    }

    @objc public var batchingBypassKeys: [String]? {
        get {
            config.batchingBypassKeys
        }
        set {
            config.batchingBypassKeys = newValue
        }
    }

    // config.dispatchExpiration in `Core` module, since it's required for remote publish settings

    #if os(iOS)
    /// Enables (`true`) or disables (`false`) `remote_api` event. Required for RemoteCommands module's JSON commands.
    @objc public var remoteAPIEnabled: NSNumber? {
        get {
            guard let remoteAPIEnabled = config.remoteAPIEnabled else {
                return nil
            }
            return NSNumber(value: remoteAPIEnabled)
        }
        set {
            config.remoteAPIEnabled = newValue?.boolValue
        }
    }
    #endif

    /// Enables (`true`) or disables (`false`) lifecycle auto tracking. Default is `true`. If set to `false` and lifecycle launch/sleep/wake events are desired, they will need to be manually called using the public methods in the `LifecycleModule`.
    @objc public var lifecycleAutoTrackingEnabled: Bool {
        get {
            config.lifecycleAutoTrackingEnabled
        }
        set {
            config.lifecycleAutoTrackingEnabled = newValue
        }
    }
    
    /// Enables or disables the built-in HTTP command. Default `false` (command is ENABLED). Set to `true` to disable
    @objc public var remoteHTTPCommandDisabled: Bool {
        get {
            config.remoteHTTPCommandDisabled
        }
        set {
            config.remoteHTTPCommandDisabled = newValue
        }
    }

    /// Sets the refresh interval for which to fetch the JSON remote command config
    /// - Returns: `TealiumRefreshInterval` default is `.every(1, .hours)`
    @objc public var remoteCommandConfigRefresh: TealiumRefreshIntervalWrapper {
        get {
            TealiumRefreshIntervalWrapper(refreshInterval: config.remoteCommandConfigRefresh)
        }
        set {
            config.remoteCommandConfigRefresh = newValue.refreshInterval
        }
    }
    
    /// A key used to inspect the data layer for a stitching key to be used to store the current visitorId with.
    ///
    /// The current visitorId is stored with this key so if this key changes, we automatically reset it to a new value, and if it comes back to the old value we have a copy and don't have to generate a new one.
    /// Something like an email adress, or a unique identifier of the current user should be the field to which this key is pointing to.
    ///
    /// Note that the key is hashed and not saved in plain text when stored on disk.
    @objc public var visitorIdentityKey: String? {
        get {
            config.visitorIdentityKey
        }
        set {
            config.visitorIdentityKey = newValue
        }
    }
    
    @objc public var overrideConsentCategoriesKey: String? {
        get {
            config.overrideConsentCategoriesKey
        }
        set {
            config.overrideConsentCategoriesKey = newValue
        }
    }
}
