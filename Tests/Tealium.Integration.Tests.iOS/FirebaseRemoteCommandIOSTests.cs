using System;
using NUnit.Framework;
using Tealium.iOS;
using Tealium.Platform.iOS;
using Tealium.RemoteCommands.Firebase.iOS;
using Foundation;
using Tealium.Integration.Tests.Common;
using Tealium.RemoteCommands.Firebase;
using System.Collections.Generic;

namespace Tealium.Integration.Tests.iOS
{
    [TestFixture]

    public class FirebaseRemoteCommandTestsIOS : FirebaseRemoteCommandTestsBase
    {
        [Test]
        public void TestWebTypeParameter()
        {
            var command = new FirebaseRemoteCommandIOS(new RemoteCommandTypeWrapper());
            Assert.Null(command.Path);
            Assert.Null(command.Url);
        }

        [Test]
        public void TestUrlTypeParameter()
        {
            string url = "https://www.google.com";
            var command = new FirebaseRemoteCommandIOS(new RemoteCommandTypeWrapper(url));
            Assert.Null(command.Path);
            Assert.AreEqual(command.Url, url);
        }

        [Test]
        public void TestLocalTypeParameter()
        {
            string path = "file://path";
            var command = new FirebaseRemoteCommandIOS(new RemoteCommandTypeWrapper(path, null));
            Assert.Null(command.Url);
            Assert.AreEqual(command.Path, path);
        }

        [Test]
        public void TestNameAndVersion()
        {
            var command = new FirebaseRemoteCommandIOS(new RemoteCommandTypeWrapper());
            Assert.NotNull(command.Name);
            Assert.NotNull(command.Version);
            Assert.True(command.Name.StartsWith("xamarin-"));
        }
        public static readonly String CommandId = "firebaseanalytics";
        public static readonly String ResponseId = "none";

        public NSMutableDictionary<NSString, NSObject> InitPayloadWithNoProperties
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.Config), new NSString(FirebaseConstants.KeyCommandName));
                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> InitPayloadWithAllProperties
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = InitPayloadWithNoProperties;
                json.SetValueForKey(NSObject.FromObject(10000), new NSString(FirebaseConstants.KeySessionTimeout));
                json.SetValueForKey(NSObject.FromObject(false), new NSString(FirebaseConstants.KeyAnalyticsEnabled));
                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> InitPayloadWithIncorrectProperties
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = InitPayloadWithNoProperties;
                json.SetValueForKey(NSObject.FromObject("string"), new NSString(FirebaseConstants.KeySessionTimeout));
                json.SetValueForKey(NSObject.FromObject("test"), new NSString(FirebaseConstants.KeyAnalyticsEnabled));
                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> LogEventWithNoParams
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.LogEvent), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject("event_campaign_details"), new NSString(FirebaseConstants.KeyEventName));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> LogEventWithTestParams
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = LogEventWithNoParams;
                NSMutableDictionary<NSString, NSObject> parameters = new NSMutableDictionary<NSString, NSObject>();
                parameters.SetValueForKey(NSObject.FromObject("my content"), new NSString("param_content"));
                parameters.SetValueForKey(NSObject.FromObject("my content type"), new NSString("param_content_type"));

                json.SetValueForKey(NSObject.FromObject("event_campaign_details"), new NSString(FirebaseConstants.KeyEventName));
                json.SetValueForKey(NSObject.FromObject(parameters), new NSString(FirebaseConstants.JSONKeyEventParams));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> LogEventWithNullParams
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = LogEventWithNoParams;
                json.SetValueForKey(NSObject.FromObject("event_campaign_details"), new NSString(FirebaseConstants.KeyEventName));
                json.SetValueForKey(NSObject.FromObject(null), new NSString(FirebaseConstants.JSONKeyEventParams));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> SetScreenWithValidProperties
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.SetScreenName), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject("some screen name"), new NSString(FirebaseConstants.KeyScreenName));
                json.SetValueForKey(NSObject.FromObject("screen class"), new NSString(FirebaseConstants.KeyScreenClass));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> SetScreenWithInvalidProperties
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.SetScreenName), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject(10), new NSString(FirebaseConstants.KeyScreenName));
                json.SetValueForKey(NSObject.FromObject(true), new NSString(FirebaseConstants.KeyScreenClass));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> SetUserIdValid
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.SetUserId), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject("James"), new NSString(FirebaseConstants.KeyUserId));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> SetUserIdInvalid
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.SetUserId), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject(null), new NSString(FirebaseConstants.KeyUserId));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> SetUserPropertyValid
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.SetUserProperty), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject("property name"), new NSString(FirebaseConstants.KeyUserPropertyName));
                json.SetValueForKey(NSObject.FromObject("property value"), new NSString(FirebaseConstants.KeyUserPropertyValue));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> SetUserPropertyInvalid
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(FirebaseConstants.Commands.SetUserProperty), new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject(null), new NSString(FirebaseConstants.KeyUserPropertyName));
                json.SetValueForKey(NSObject.FromObject(null), new NSString(FirebaseConstants.KeyUserPropertyValue));

                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> CompositeAllValidCommands
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(String.Join(',',
                            new string[] { FirebaseConstants.Commands.Config,
                                            FirebaseConstants.Commands.SetUserId,
                                            FirebaseConstants.Commands.SetUserProperty,
                                            FirebaseConstants.Commands.SetScreenName,
                                            FirebaseConstants.Commands.LogEvent })),
                                             new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject(false), new NSString(FirebaseConstants.KeyAnalyticsEnabled));
                json.SetValueForKey(NSObject.FromObject("property name"), new NSString(FirebaseConstants.KeyUserPropertyName));
                json.SetValueForKey(NSObject.FromObject("property value"), new NSString(FirebaseConstants.KeyUserPropertyValue));
                json.SetValueForKey(NSObject.FromObject("James"), new NSString(FirebaseConstants.KeyUserId));
                json.SetValueForKey(NSObject.FromObject("screen_name"), new NSString(FirebaseConstants.KeyScreenName));
                json.SetValueForKey(NSObject.FromObject("screen class"), new NSString(FirebaseConstants.KeyScreenClass));
                NSMutableDictionary parameters = new NSMutableDictionary();
                parameters.SetValueForKey(NSObject.FromObject("my content"), new NSString("param_content"));
                parameters.SetValueForKey(NSObject.FromObject("my content type"), new NSString("param_content_type"));

                json.SetValueForKey(NSObject.FromObject("event_campaign_details"), new NSString(FirebaseConstants.KeyEventName));
                json.SetValueForKey(NSObject.FromObject(parameters), new NSString(FirebaseConstants.JSONKeyEventParams));
                return json;
            }
        }

        public NSMutableDictionary<NSString, NSObject> CompositeSomeInvalidCommands
        {
            get
            {
                NSMutableDictionary<NSString, NSObject> json = new NSMutableDictionary<NSString, NSObject>();
                json.SetValueForKey(NSObject.FromObject(String.Join(',',
                            new string[] { FirebaseConstants.Commands.Config,
                                            FirebaseConstants.Commands.SetUserId,
                                            FirebaseConstants.Commands.SetUserProperty,
                                            FirebaseConstants.Commands.SetScreenName,
                                            FirebaseConstants.Commands.LogEvent })),
                                             new NSString(FirebaseConstants.KeyCommandName));
                json.SetValueForKey(NSObject.FromObject(false), new NSString(FirebaseConstants.KeyAnalyticsEnabled));
                json.SetValueForKey(NSObject.FromObject("property name"), new NSString(FirebaseConstants.KeyUserPropertyName));
                json.SetValueForKey(NSObject.FromObject("property value"), new NSString(FirebaseConstants.KeyUserPropertyValue));
                //json.SetValueForKey(FirebaseRemoteCommand.KeyUserId, "James");//missing
                json.SetValueForKey(NSObject.FromObject(null), new NSString(FirebaseConstants.KeyScreenName));
                json.SetValueForKey(NSObject.FromObject(null), new NSString(FirebaseConstants.KeyScreenClass));

                NSMutableDictionary parameters = new NSMutableDictionary();

                //parameters.SetValueForKey(NSObject.FromObject("my content"), new NSString("param_content"));

                //parameters.SetValueForKey(NSObject.FromObject("my content type"), new NSString("param_content_type"));

                //test empty params.

                json.SetValueForKey(NSObject.FromObject("event_campaign_details"), new NSString(FirebaseConstants.KeyEventName));
                json.SetValueForKey(NSObject.FromObject(parameters), new NSString(FirebaseConstants.JSONKeyEventParams));
                return json;
            }
        }

        public override IRemoteCommandResponse GetResponseForPayload(Payloads payloadType)
        {
            NSMutableDictionary<NSString, NSObject> json;

            switch (payloadType)
            {
                case Payloads.InitWithNoProperties:
                    json = InitPayloadWithNoProperties;
                    break;
                case Payloads.InitWithAllProperties:
                    json = InitPayloadWithAllProperties;
                    break;
                case Payloads.InitWithIncorrectProperties:
                    json = InitPayloadWithIncorrectProperties;
                    break;
                case Payloads.LogEventWithNoParams:
                    json = LogEventWithNoParams;
                    break;
                case Payloads.LogEventWithTestParams:
                    json = LogEventWithNoParams;
                    break;
                case Payloads.LogEventWithNullParams:
                    json = LogEventWithNullParams;
                    break;
                case Payloads.SetScreenWithValidParams:
                    json = SetScreenWithValidProperties;
                    break;
                case Payloads.SetScreenWithInvalidParams:
                    json = SetScreenWithInvalidProperties;
                    break;
                case Payloads.SetUserIdValid:
                    json = SetUserIdValid;
                    break;
                case Payloads.SetUserIdInvalid:
                    json = SetUserIdInvalid;
                    break;
                case Payloads.SetUserPropertyValid:
                    json = SetUserPropertyValid;
                    break;
                case Payloads.SetUserPropertyInvalid:
                    json = SetUserPropertyInvalid;
                    break;
                case Payloads.CompositeAllValidCommands:
                    json = CompositeAllValidCommands;
                    break;
                case Payloads.CompositeSomeInvalidCommands:
                    json = CompositeSomeInvalidCommands;
                    break;
                default:
                    json = new NSMutableDictionary<NSString, NSObject>();
                    break;
            }
            return new RemoteCommandResponseIOS(new RemoteCommandResponseWrapper(new NSDictionary<NSString, NSObject>(json.Keys, json.Values)), CommandId);
        }

        protected override IRemoteCommand GetFirebaseRemoteCommand()
        {
            return new MockFirebaseRemoteCommandIOS();
        }

        protected override IMockFirebaseRemoteCommand GetMockFirebaseRemoteCommand()
        {
            return new MockFirebaseRemoteCommandIOS();
        }

        public class MockFirebaseRemoteCommandIOS : FirebaseRemoteCommandIOS, IMockFirebaseRemoteCommand
        {
            public MockFirebaseRemoteCommandIOS()
                : this(new RemoteCommandTypeWrapper())
            {

            }

            public MockFirebaseRemoteCommandIOS(RemoteCommandTypeWrapper type)
                : base(type)
            {

            }

            public int ConfiguredCalled = 0;
            public int LogEventCalled = 0;
            public int SetScreenCalled = 0;
            public int SetUserIdCalled = 0;
            public int SetUserPropertyCalled = 0;

            public int GetConfigCalls => ConfiguredCalled;
            public int GetUserIdCalls => SetUserIdCalled;
            public int GetUserPropertyCalls => SetUserPropertyCalled;
            public int GetLogEventCalls => LogEventCalled;
            public int GetScreenNameCalls => SetScreenCalled;

            protected override void Configure(int? sessionTimeoutDuration, bool? analyticsEnabled, string loggerLevel)
            {
                base.Configure(sessionTimeoutDuration, analyticsEnabled, loggerLevel);
                ConfiguredCalled++;
            }

            protected override void LogEvent(string eventName, Dictionary<string, object> eventParams)
            {
                base.LogEvent(eventName, eventParams);
                LogEventCalled++;
            }

            protected override void SetScreenName(string screenName, string screenClass)
            {
                base.SetScreenName(screenName, screenClass);
                SetScreenCalled++;
            }

            protected override void SetUserId(string userId)
            {
                base.SetUserId(userId);
                SetUserIdCalled++;
            }

            protected override void SetUserProperty(string propertyName, string propertyValue)
            {
                base.SetUserProperty(propertyName, propertyValue);
                SetUserPropertyCalled++;
            }
        }
    }
}