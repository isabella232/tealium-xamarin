using System;
using Foundation;
using ObjCRuntime;
using UIKit;
using WebKit;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace Tealium.Platform.iOS
{
	// @interface ConsentManagerWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ConsentManagerWrapper
	{
		// @property (copy, nonatomic) void (^ _Nullable)(void) onConsentExpiration;
		[NullAllowed, Export ("onConsentExpiration", ArgumentSemantic.Copy)]
		Action OnConsentExpiration { get; set; }

		// @property (nonatomic) enum TealiumConsentStatusWrapper userConsentStatus;
		[Export ("userConsentStatus", ArgumentSemantic.Assign)]
		TealiumConsentStatusWrapper UserConsentStatus { get; set; }

		// @property (copy, nonatomic) NSArray<NSNumber *> * _Nullable userConsentCategories;
		[NullAllowed, Export ("userConsentCategories", ArgumentSemantic.Copy)]
		NSNumber[] UserConsentCategories { get; set; }

		// @property (readonly, nonatomic) BOOL consentLoggingEnabled;
		[Export ("consentLoggingEnabled")]
		bool ConsentLoggingEnabled { get; }

		// @property (readonly, nonatomic) BOOL consentManagerEnabled;
		[Export ("consentManagerEnabled")]
		bool ConsentManagerEnabled { get; }

		// @property (readonly, nonatomic) enum TealiumConsentPolicyWrapper policy;
		[Export ("policy")]
		TealiumConsentPolicyWrapper Policy { get; }

		// -(void)resetUserConsentPreferences;
		[Export ("resetUserConsentPreferences")]
		void ResetUserConsentPreferences ();

		// -(void)setUserConsentStatus:(enum TealiumConsentStatusWrapper)status withCategories:(NSArray<NSNumber *> * _Nonnull)categories;
		[Export ("setUserConsentStatus:withCategories:")]
		void SetUserConsentStatus (TealiumConsentStatusWrapper status, NSNumber[] categories);
	}

	// @interface DispatchListenerWrapper : NSObject
	[BaseType (typeof(NSObject))]
	interface DispatchListenerWrapper
	{
		// -(void)willTrackWithRequest:(TealiumRequestWrapper * _Nonnull)request;
		[Export ("willTrackWithRequest:")]
		void WillTrackWithRequest (TealiumRequestWrapper request);
	}

	// @interface DispatchValidatorWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface DispatchValidatorWrapper
	{
		// @property (copy, nonatomic) NSString * _Nonnull id;
		[Export ("id")]
		string Id { get; set; }

		// -(instancetype _Nonnull)initWithId:(NSString * _Nonnull)id __attribute__((objc_designated_initializer));
		[Export ("initWithId:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string id);

		// -(QueueRequestResponse * _Nonnull)shouldQueueWithRequest:(TealiumRequestWrapper * _Nonnull)request __attribute__((warn_unused_result("")));
		[Export ("shouldQueueWithRequest:")]
		QueueRequestResponse ShouldQueueWithRequest (TealiumRequestWrapper request);

		// -(BOOL)shouldDropWithRequest:(TealiumRequestWrapper * _Nonnull)request __attribute__((warn_unused_result("")));
		[Export ("shouldDropWithRequest:")]
		bool ShouldDropWithRequest (TealiumRequestWrapper request);

		// -(BOOL)shouldPurgeWithRequest:(TealiumRequestWrapper * _Nonnull)request __attribute__((warn_unused_result("")));
		[Export ("shouldPurgeWithRequest:")]
		bool ShouldPurgeWithRequest (TealiumRequestWrapper request);
	}

	// @interface ExpirationTime : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface ExpirationTime
	{
		// @property (readonly, nonatomic) NSInteger time;
		[Export ("time")]
		nint Time { get; }

		// @property (readonly, nonatomic) enum TimeUnitWrapper unit;
		[Export ("unit")]
		TimeUnitWrapper Unit { get; }

		// -(instancetype _Nonnull)initWithTime:(NSInteger)time unit:(enum TimeUnitWrapper)unit __attribute__((objc_designated_initializer));
		[Export ("initWithTime:unit:")]
		[DesignatedInitializer]
		NativeHandle Constructor (nint time, TimeUnitWrapper unit);
	}

	// @interface QueueRequestResponse : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface QueueRequestResponse
	{
		// @property (readonly, nonatomic) BOOL shouldQueue;
		[Export ("shouldQueue")]
		bool ShouldQueue { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,id> * _Nullable data;
		[NullAllowed, Export ("data", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Data { get; }

		// -(instancetype _Nonnull)initWithShouldQueue:(BOOL)shouldQueue data:(NSDictionary<NSString *,id> * _Nullable)data __attribute__((objc_designated_initializer));
		[Export ("initWithShouldQueue:data:")]
		[DesignatedInitializer]
		NativeHandle Constructor (bool shouldQueue, [NullAllowed] NSDictionary<NSString, NSObject> data);
	}

	// @interface RemoteCommandResponseWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface RemoteCommandResponseWrapper
	{
		// -(instancetype _Nonnull)initWithPayload:(NSDictionary<NSString *,id> * _Nonnull)payload __attribute__((objc_designated_initializer));
		[Export ("initWithPayload:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSDictionary<NSString, NSObject> payload);

		// @property (copy, nonatomic) NSDictionary<NSString *,id> * _Nullable payload;
		[NullAllowed, Export ("payload", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Payload { get; set; }

		// @property (nonatomic) NSError * _Nullable error;
		[NullAllowed, Export ("error", ArgumentSemantic.Assign)]
		NSError Error { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable status;
		[NullAllowed, Export ("status", ArgumentSemantic.Strong)]
		NSNumber Status { get; set; }

		// @property (copy, nonatomic) NSData * _Nullable data;
		[NullAllowed, Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; set; }

		// @property (nonatomic) BOOL hasCustomCompletionHandler;
		[Export ("hasCustomCompletionHandler")]
		bool HasCustomCompletionHandler { get; set; }
	}

	// @interface RemoteCommandTypeWrapper : NSObject
	[BaseType (typeof(NSObject))]
	interface RemoteCommandTypeWrapper
	{
		// @property (readonly, copy, nonatomic) NSString * _Nonnull typeString;
		[Export ("typeString")]
		string TypeString { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable url;
		[NullAllowed, Export ("url")]
		string Url { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable path;
		[NullAllowed, Export ("path")]
		string Path { get; }

		// -(instancetype _Nonnull)initWithUrl:(NSString * _Nonnull)url __attribute__((objc_designated_initializer));
		[Export ("initWithUrl:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string url);

		// -(instancetype _Nonnull)initWithFile:(NSString * _Nonnull)file bundle:(NSBundle * _Nullable)bundle __attribute__((objc_designated_initializer));
		[Export ("initWithFile:bundle:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string file, [NullAllowed] NSBundle bundle);
	}

	// @interface RemoteCommandWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface RemoteCommandWrapper
	{
		// @property (readonly, nonatomic, strong) RemoteCommandTypeWrapper * _Nonnull type;
		[Export ("type", ArgumentSemantic.Strong)]
		RemoteCommandTypeWrapper Type { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull commandId;
		[Export ("commandId")]
		string CommandId { get; }

		// @property (copy, nonatomic) NSString * _Nullable commandDescription;
		[NullAllowed, Export ("commandDescription")]
		string CommandDescription { get; set; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable version;
		[NullAllowed, Export ("version")]
		string Version { get; }

		// @property (copy, nonatomic) void (^ _Nonnull)(RemoteCommandResponseWrapper * _Nonnull) completion;
		[Export ("completion", ArgumentSemantic.Copy)]
		Action<RemoteCommandResponseWrapper> Completion { get; set; }

		// -(instancetype _Nonnull)initWithCommandId:(NSString * _Nonnull)commandId description:(NSString * _Nullable)description type:(RemoteCommandTypeWrapper * _Nonnull)type completion:(void (^ _Nonnull)(RemoteCommandResponseWrapper * _Nonnull))completion;
		[Export ("initWithCommandId:description:type:completion:")]
		NativeHandle Constructor (string commandId, [NullAllowed] string description, RemoteCommandTypeWrapper type, Action<RemoteCommandResponseWrapper> completion);

		// -(instancetype _Nonnull)initWithCommandId:(NSString * _Nonnull)commandId description:(NSString * _Nullable)description type:(RemoteCommandTypeWrapper * _Nonnull)type name:(NSString * _Nullable)name version:(NSString * _Nullable)version completion:(void (^ _Nonnull)(RemoteCommandResponseWrapper * _Nonnull))completion __attribute__((objc_designated_initializer));
		[Export ("initWithCommandId:description:type:name:version:completion:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string commandId, [NullAllowed] string description, RemoteCommandTypeWrapper type, [NullAllowed] string name, [NullAllowed] string version, Action<RemoteCommandResponseWrapper> completion);
	}

	// @interface TealiumConfigWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface TealiumConfigWrapper
	{
		// -(instancetype _Nonnull)initWithAccount:(NSString * _Nonnull)account profile:(NSString * _Nonnull)profile environment:(NSString * _Nonnull)environment dataSource:(NSString * _Nullable)dataSource options:(NSDictionary<NSString *,id> * _Nullable)options __attribute__((objc_designated_initializer));
		[Export ("initWithAccount:profile:environment:dataSource:options:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string account, string profile, string environment, [NullAllowed] string dataSource, [NullAllowed] NSDictionary<NSString, NSObject> options);

		// @property (readonly, copy, nonatomic) NSString * _Nonnull account;
		[Export ("account")]
		string Account { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull profile;
		[Export ("profile")]
		string Profile { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull environment;
		[Export ("environment")]
		string Environment { get; }

		// @property (copy, nonatomic) NSString * _Nullable dataSourceKey;
		[NullAllowed, Export ("dataSourceKey")]
		string DataSourceKey { get; set; }

		// @property (copy, nonatomic) NSDictionary<NSString *,id> * _Nonnull options;
		[Export ("options", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> Options { get; set; }

		// @property (copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable skAdConversionKeys;
		[NullAllowed, Export ("skAdConversionKeys", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> SkAdConversionKeys { get; set; }

		// @property (nonatomic) BOOL shouldMigratePersistentData;
		[Export ("shouldMigratePersistentData")]
		bool ShouldMigratePersistentData { get; set; }

		// @property (copy, nonatomic) NSArray<DispatchValidatorWrapper *> * _Nullable dispatchValidators;
		[NullAllowed, Export ("dispatchValidators", ArgumentSemantic.Copy)]
		DispatchValidatorWrapper[] DispatchValidators { get; set; }

		// @property (copy, nonatomic) NSArray<DispatchListenerWrapper *> * _Nullable dispatchListeners;
		[NullAllowed, Export ("dispatchListeners", ArgumentSemantic.Copy)]
		DispatchListenerWrapper[] DispatchListeners { get; set; }

		// @property (copy, nonatomic) NSArray<NSNumber *> * _Nullable collectors;
		[NullAllowed, Export ("collectors", ArgumentSemantic.Copy)]
		NSNumber[] Collectors { get; set; }

		// @property (copy, nonatomic) NSArray<NSNumber *> * _Nullable dispatchers;
		[NullAllowed, Export ("dispatchers", ArgumentSemantic.Copy)]
		NSNumber[] Dispatchers { get; set; }

		// @property (readonly, nonatomic, strong) TealiumConfigWrapper * _Nonnull deepCopy;
		[Export ("deepCopy", ArgumentSemantic.Strong)]
		TealiumConfigWrapper DeepCopy { get; }

		// @property (copy, nonatomic) NSArray<RemoteCommandWrapper *> * _Nullable remoteCommands;
		[NullAllowed, Export ("remoteCommands", ArgumentSemantic.Copy)]
		RemoteCommandWrapper[] RemoteCommands { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable existingVisitorId;
		[NullAllowed, Export ("existingVisitorId")]
		string ExistingVisitorId { get; set; }

		// @property (nonatomic) BOOL shouldUseRemotePublishSettings;
		[Export ("shouldUseRemotePublishSettings")]
		bool ShouldUseRemotePublishSettings { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable publishSettingsURL;
		[NullAllowed, Export ("publishSettingsURL")]
		string PublishSettingsURL { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable publishSettingsProfile;
		[NullAllowed, Export ("publishSettingsProfile")]
		string PublishSettingsProfile { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable isEnabled;
		[NullAllowed, Export ("isEnabled", ArgumentSemantic.Strong)]
		NSNumber IsEnabled { get; set; }

		// @property (nonatomic) BOOL isTagManagementEnabled;
		[Export ("isTagManagementEnabled")]
		bool IsTagManagementEnabled { get; set; }

		// @property (nonatomic) BOOL isCollectEnabled;
		[Export ("isCollectEnabled")]
		bool IsCollectEnabled { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable batterySaverEnabled;
		[NullAllowed, Export ("batterySaverEnabled", ArgumentSemantic.Strong)]
		NSNumber BatterySaverEnabled { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable dispatchExpiration;
		[NullAllowed, Export ("dispatchExpiration", ArgumentSemantic.Strong)]
		NSNumber DispatchExpiration { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable batchingEnabled;
		[NullAllowed, Export ("batchingEnabled", ArgumentSemantic.Strong)]
		NSNumber BatchingEnabled { get; set; }

		// @property (nonatomic) NSInteger batchSize;
		[Export ("batchSize")]
		nint BatchSize { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable dispatchQueueLimit;
		[NullAllowed, Export ("dispatchQueueLimit", ArgumentSemantic.Strong)]
		NSNumber DispatchQueueLimit { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable wifiOnlySending;
		[NullAllowed, Export ("wifiOnlySending", ArgumentSemantic.Strong)]
		NSNumber WifiOnlySending { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable minutesBetweenRefresh;
		[NullAllowed, Export ("minutesBetweenRefresh", ArgumentSemantic.Strong)]
		NSNumber MinutesBetweenRefresh { get; set; }

		// @property (nonatomic, strong) ExpirationTime * _Nullable consentExpiry;
		[NullAllowed, Export ("consentExpiry", ArgumentSemantic.Strong)]
		ExpirationTime ConsentExpiry { get; set; }

		// @property (copy, nonatomic) void (^ _Nullable)(void) onConsentExpiration;
		[NullAllowed, Export ("onConsentExpiration", ArgumentSemantic.Copy)]
		Action OnConsentExpiration { get; set; }

		// @property (nonatomic) BOOL deepLinkTrackingEnabled;
		[Export ("deepLinkTrackingEnabled")]
		bool DeepLinkTrackingEnabled { get; set; }

		// @property (nonatomic) BOOL qrTraceEnabled;
		[Export ("qrTraceEnabled")]
		bool QrTraceEnabled { get; set; }

		// @property (nonatomic) BOOL memoryReportingEnabled;
		[Export ("memoryReportingEnabled")]
		bool MemoryReportingEnabled { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable overrideCollectURL;
		[NullAllowed, Export ("overrideCollectURL")]
		string OverrideCollectURL { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable overrideCollectBatchURL;
		[NullAllowed, Export ("overrideCollectBatchURL")]
		string OverrideCollectBatchURL { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable overrideCollectProfile;
		[NullAllowed, Export ("overrideCollectProfile")]
		string OverrideCollectProfile { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable overrideCollectDomain;
		[NullAllowed, Export ("overrideCollectDomain")]
		string OverrideCollectDomain { get; set; }

		// @property (copy, nonatomic) NSArray<id<WKNavigationDelegate>> * _Nullable webViewDelegates;
		[NullAllowed, Export ("webViewDelegates", ArgumentSemantic.Copy)]
		WKNavigationDelegate[] WebViewDelegates { get; set; }

		// @property (nonatomic, strong) WKProcessPool * _Nullable webviewProcessPool;
		[NullAllowed, Export ("webviewProcessPool", ArgumentSemantic.Strong)]
		WKProcessPool WebviewProcessPool { get; set; }

		// @property (nonatomic, strong) WKWebViewConfiguration * _Nonnull webviewConfig;
		[Export ("webviewConfig", ArgumentSemantic.Strong)]
		WKWebViewConfiguration WebviewConfig { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable tagManagementOverrideURL;
		[NullAllowed, Export ("tagManagementOverrideURL")]
		string TagManagementOverrideURL { get; set; }

		// @property (readonly, copy, nonatomic) NSURL * _Nullable webviewURL;
		[NullAllowed, Export ("webviewURL", ArgumentSemantic.Copy)]
		NSUrl WebviewURL { get; }

		// @property (nonatomic, strong) UIView * _Nullable rootView;
		[NullAllowed, Export ("rootView", ArgumentSemantic.Strong)]
		UIView RootView { get; set; }

		// @property (nonatomic) enum TealiumLogLevelWrapper logLevel;
		[Export ("logLevel", ArgumentSemantic.Assign)]
		TealiumLogLevelWrapper LogLevel { get; set; }

		// @property (nonatomic) BOOL consentLoggingEnabled;
		[Export ("consentLoggingEnabled")]
		bool ConsentLoggingEnabled { get; set; }

		// @property (nonatomic) enum TealiumConsentPolicyWrapper consentPolicy;
		[Export ("consentPolicy", ArgumentSemantic.Assign)]
		TealiumConsentPolicyWrapper ConsentPolicy { get; set; }

		// @property (nonatomic) NSInteger dispatchAfter;
		[Export ("dispatchAfter")]
		nint DispatchAfter { get; set; }

		// @property (copy, nonatomic) NSArray<NSString *> * _Nullable batchingBypassKeys;
		[NullAllowed, Export ("batchingBypassKeys", ArgumentSemantic.Copy)]
		string[] BatchingBypassKeys { get; set; }

		// @property (nonatomic, strong) NSNumber * _Nullable remoteAPIEnabled;
		[NullAllowed, Export ("remoteAPIEnabled", ArgumentSemantic.Strong)]
		NSNumber RemoteAPIEnabled { get; set; }

		// @property (nonatomic) BOOL lifecycleAutoTrackingEnabled;
		[Export ("lifecycleAutoTrackingEnabled")]
		bool LifecycleAutoTrackingEnabled { get; set; }

		// @property (nonatomic) BOOL remoteHTTPCommandDisabled;
		[Export ("remoteHTTPCommandDisabled")]
		bool RemoteHTTPCommandDisabled { get; set; }

		// @property (nonatomic, strong) TealiumRefreshIntervalWrapper * _Nonnull remoteCommandConfigRefresh;
		[Export ("remoteCommandConfigRefresh", ArgumentSemantic.Strong)]
		TealiumRefreshIntervalWrapper RemoteCommandConfigRefresh { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable visitorIdentityKey;
		[NullAllowed, Export ("visitorIdentityKey")]
		string VisitorIdentityKey { get; set; }

		// @property (copy, nonatomic) NSString * _Nullable overrideConsentCategoriesKey;
		[NullAllowed, Export ("overrideConsentCategoriesKey")]
		string OverrideConsentCategoriesKey { get; set; }
	}

	// @interface TealiumCurrentVisitProfileWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface TealiumCurrentVisitProfileWrapper
	{
		// @property (readonly, nonatomic) int64_t createdAt;
		[Export ("createdAt")]
		long CreatedAt { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable dates;
		[NullAllowed, Export ("dates", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Dates { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable booleans;
		[NullAllowed, Export ("booleans", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Booleans { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSArray<NSNumber *> *> * _Nullable arraysOfBooleans;
		[NullAllowed, Export ("arraysOfBooleans", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<NSNumber>> ArraysOfBooleans { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable numbers;
		[NullAllowed, Export ("numbers", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Numbers { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSArray<NSNumber *> *> * _Nullable arraysOfNumbers;
		[NullAllowed, Export ("arraysOfNumbers", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<NSNumber>> ArraysOfNumbers { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSDictionary<NSString *,NSNumber *> *> * _Nullable tallies;
		[NullAllowed, Export ("tallies", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSDictionary<NSString, NSNumber>> Tallies { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable strings;
		[NullAllowed, Export ("strings", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> Strings { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSArray<NSString *> *> * _Nullable arraysOfStrings;
		[NullAllowed, Export ("arraysOfStrings", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<NSString>> ArraysOfStrings { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSSet<NSString *> *> * _Nullable setsOfStrings;
		[NullAllowed, Export ("setsOfStrings", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSSet<NSString>> SetsOfStrings { get; }
	}

	// @interface TealiumRefreshIntervalWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface TealiumRefreshIntervalWrapper
	{
		// @property (readonly, nonatomic) NSInteger amount;
		[Export ("amount")]
		nint Amount { get; }

		// @property (readonly, nonatomic) enum RefreshTimeWrapper unit;
		[Export ("unit")]
		RefreshTimeWrapper Unit { get; }

		// -(instancetype _Nonnull)initWithAmount:(NSInteger)amount unit:(enum RefreshTimeWrapper)unit __attribute__((objc_designated_initializer));
		[Export ("initWithAmount:unit:")]
		[DesignatedInitializer]
		NativeHandle Constructor (nint amount, RefreshTimeWrapper unit);
	}

	// @interface TealiumRequestWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface TealiumRequestWrapper
	{
		// @property (copy, nonatomic) NSString * _Nonnull typeId;
		[Export ("typeId")]
		string TypeId { get; set; }

		// +(NSString * _Nonnull)instanceTypeId __attribute__((warn_unused_result("")));
		[Static]
		[Export ("instanceTypeId")]
		string InstanceTypeId { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,id> * _Nonnull trackDictionary;
		[Export ("trackDictionary", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> TrackDictionary { get; }
	}

	// @interface TealiumVisitorProfileWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface TealiumVisitorProfileWrapper
	{
		// @property (readonly, nonatomic) NSInteger totalEventsCount;
		[Export ("totalEventsCount")]
		nint TotalEventsCount { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable audiences;
		[NullAllowed, Export ("audiences", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> Audiences { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable badges;
		[NullAllowed, Export ("badges", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Badges { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable dates;
		[NullAllowed, Export ("dates", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Dates { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable booleans;
		[NullAllowed, Export ("booleans", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Booleans { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSArray<NSNumber *> *> * _Nullable arraysOfBooleans;
		[NullAllowed, Export ("arraysOfBooleans", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<NSNumber>> ArraysOfBooleans { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSNumber *> * _Nullable numbers;
		[NullAllowed, Export ("numbers", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSNumber> Numbers { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSArray<NSNumber *> *> * _Nullable arraysOfNumbers;
		[NullAllowed, Export ("arraysOfNumbers", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<NSNumber>> ArraysOfNumbers { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSDictionary<NSString *,NSNumber *> *> * _Nullable tallies;
		[NullAllowed, Export ("tallies", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSDictionary<NSString, NSNumber>> Tallies { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSString *> * _Nullable strings;
		[NullAllowed, Export ("strings", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSString> Strings { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSArray<NSString *> *> * _Nullable arraysOfStrings;
		[NullAllowed, Export ("arraysOfStrings", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSArray<NSString>> ArraysOfStrings { get; }

		// @property (readonly, copy, nonatomic) NSDictionary<NSString *,NSSet<NSString *> *> * _Nullable setsOfStrings;
		[NullAllowed, Export ("setsOfStrings", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSSet<NSString>> SetsOfStrings { get; }

		// @property (readonly, nonatomic, strong) TealiumCurrentVisitProfileWrapper * _Nullable currentVisit;
		[NullAllowed, Export ("currentVisit", ArgumentSemantic.Strong)]
		TealiumCurrentVisitProfileWrapper CurrentVisit { get; }

		// -(instancetype _Nullable)initWithJsonData:(NSData * _Nonnull)jsonData error:(NSError * _Nullable * _Nullable)error __attribute__((objc_designated_initializer));
		[Export ("initWithJsonData:error:")]
		[DesignatedInitializer]
		NativeHandle Constructor (NSData jsonData, [NullAllowed] out NSError error);

		// @property (readonly, nonatomic) BOOL isEmpty;
		[Export ("isEmpty")]
		bool IsEmpty { get; }
	}

	// @interface TealiumWrapper : NSObject
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface TealiumWrapper
	{
		// @property (readonly, nonatomic, strong) ConsentManagerWrapper * _Nullable consentManager;
		[NullAllowed, Export ("consentManager", ArgumentSemantic.Strong)]
		ConsentManagerWrapper ConsentManager { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nonnull instanceId;
		[Export ("instanceId")]
		string InstanceId { get; }

		// @property (readonly, copy, nonatomic) NSString * _Nullable visitorId;
		[NullAllowed, Export ("visitorId")]
		string VisitorId { get; }

		// @property (nonatomic) enum TealiumConsentStatusWrapper userConsentStatus;
		[Export ("userConsentStatus", ArgumentSemantic.Assign)]
		TealiumConsentStatusWrapper UserConsentStatus { get; set; }

		// @property (copy, nonatomic) NSArray<NSNumber *> * _Nullable userConsentCategories;
		[NullAllowed, Export ("userConsentCategories", ArgumentSemantic.Copy)]
		NSNumber[] UserConsentCategories { get; set; }

		// @property (copy, nonatomic) void (^ _Nullable)(void) onConsentExpiration;
		[NullAllowed, Export ("onConsentExpiration", ArgumentSemantic.Copy)]
		Action OnConsentExpiration { get; set; }

		// @property (copy, nonatomic) void (^ _Nullable)(TealiumVisitorProfileWrapper * _Nonnull) onVisitorProfileUpdate;
		[NullAllowed, Export ("onVisitorProfileUpdate", ArgumentSemantic.Copy)]
		Action<TealiumVisitorProfileWrapper> OnVisitorProfileUpdate { get; set; }

		// @property (copy, nonatomic) void (^ _Nullable)(NSString * _Nonnull) onVisitorIdUpdate;
		[NullAllowed, Export ("onVisitorIdUpdate", ArgumentSemantic.Copy)]
		Action<NSString> OnVisitorIdUpdate { get; set; }

		// -(instancetype _Nonnull)initWithConfig:(TealiumConfigWrapper * _Nonnull)config enableCompletion:(void (^ _Nullable)(BOOL, NSError * _Nullable))enableCompletion __attribute__((objc_designated_initializer));
		[Export ("initWithConfig:enableCompletion:")]
		[DesignatedInitializer]
		NativeHandle Constructor (TealiumConfigWrapper config, [NullAllowed] Action<bool, NSError> enableCompletion);

		// -(void)resetConsentPreferences;
		[Export ("resetConsentPreferences")]
		void ResetConsentPreferences ();

		// -(void)trackWithTitle:(NSString * _Nonnull)title data:(NSDictionary<NSString *,id> * _Nullable)data;
		[Export ("trackWithTitle:data:")]
		void TrackWithTitle (string title, [NullAllowed] NSDictionary<NSString, NSObject> data);

		// -(void)trackViewWithTitle:(NSString * _Nonnull)title data:(NSDictionary<NSString *,id> * _Nullable)data;
		[Export ("trackViewWithTitle:data:")]
		void TrackViewWithTitle (string title, [NullAllowed] NSDictionary<NSString, NSObject> data);

		// -(void)joinTrace:(NSString * _Nonnull)traceID;
		[Export ("joinTrace:")]
		void JoinTrace (string traceID);

		// -(void)leaveTrace;
		[Export ("leaveTrace")]
		void LeaveTrace ();

		// -(void)addToDataLayerWithData:(NSDictionary<NSString *,id> * _Nonnull)data expiry:(enum ExpiryWrapper)expiry;
		[Export ("addToDataLayerWithData:expiry:")]
		void AddToDataLayerWithData (NSDictionary<NSString, NSObject> data, ExpiryWrapper expiry);

		// -(id _Nullable)getFromDataLayerWithKey:(NSString * _Nonnull)key __attribute__((warn_unused_result("")));
		[Export ("getFromDataLayerWithKey:")]
		[return: NullAllowed]
		NSObject GetFromDataLayerWithKey (string key);

		// -(void)removeFromDataLayerWithKeys:(NSArray<NSString *> * _Nonnull)keys;
		[Export ("removeFromDataLayerWithKeys:")]
		void RemoveFromDataLayerWithKeys (string[] keys);

		// -(void)addRemoteCommand:(RemoteCommandWrapper * _Nonnull)remoteCommand;
		[Export ("addRemoteCommand:")]
		void AddRemoteCommand (RemoteCommandWrapper remoteCommand);

		// -(void)removeRemoteCommandWithId:(NSString * _Nonnull)id;
		[Export ("removeRemoteCommandWithId:")]
		void RemoveRemoteCommandWithId (string id);

		// -(void)destroy;
		[Export ("destroy")]
		void Destroy ();

		// -(void)resetVisitorId;
		[Export ("resetVisitorId")]
		void ResetVisitorId ();

		// -(void)clearStoredVisitorIds;
		[Export ("clearStoredVisitorIds")]
		void ClearStoredVisitorIds ();
	}
}
