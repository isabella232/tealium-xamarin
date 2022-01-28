using ObjCRuntime;

namespace Tealium.Platform.iOS
{
	[Native]
	public enum CollectorType : long
	{
		AppData = 0,
		Connectivity = 1,
		DeviceData = 2,
		Lifecycle = 3,
		VisitorService = 4
	}

	[Native]
	public enum DispatcherType : long
	{
		Collect = 0,
		RemoteCommands = 1,
		TagManagement = 2
	}

	[Native]
	public enum ExpiryWrapper : long
	{
		Session = 0,
		UntilRestart = 1,
		Forever = 2
	}

	[Native]
	public enum RefreshTimeWrapper : long
	{
		Seconds = 0,
		Minutes = 1,
		Hours = 2
	}

	[Native]
	public enum TealiumConsentCategoriesWrappers : long
	{
		Analytics = 0,
		Affiliates = 1,
		DisplayAds = 2,
		Email = 3,
		Personalization = 4,
		Search = 5,
		Social = 6,
		BigData = 7,
		Mobile = 8,
		Engagement = 9,
		Monitoring = 10,
		Crm = 11,
		Cdp = 12,
		CookieMatch = 13,
		Misc = 14
	}

	[Native]
	public enum TealiumConsentPolicyWrapper : long
	{
		Ccpa = 0,
		Gdpr = 1,
		None = 2
	}

	[Native]
	public enum TealiumConsentStatusWrapper : long
	{
		Unknown = 0,
		Consented = 1,
		NotConsented = 2
	}

	[Native]
	public enum TealiumConsentTrackAction : long
	{
		Allowed = 0,
		Forbidden = 1,
		Queued = 2
	}

	[Native]
	public enum TealiumLogLevelWrapper : long
	{
		Info = 0,
		Debug = 100,
		Error = 200,
		Fault = 300,
		Silent = -9999,
		Undefined = -99999
	}

	[Native]
	public enum TimeUnitWrapper : long
	{
		Minutes = 0,
		Hours = 1,
		Days = 2,
		Months = 3,
		Years = 4
	}
}
