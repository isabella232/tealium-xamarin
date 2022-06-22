using System;
using NUnit.Framework;
using Tealium.Droid;
using Tealium.RemoteCommands.Firebase;
using Tealium.RemoteCommands.Firebase.Droid;
using Tealium.Tests.Common;
using Tealium.Integration.Tests.Common;
using Android.App;
using System.Collections.Generic;

using Com.Tealium.Remotecommands;

namespace Tealium.Integration.Tests.Droid
{
    [TestFixture]
    public class FirebaseRemoteCommandTestsDroid : FirebaseRemoteCommandTestsBase
    {

        [Test]
        public void TestWebTypeParameter()
        {
            var command = new FirebaseRemoteCommandDroid(MainActivity.CurrentApplication, null, null);
            Assert.Null(command.Path);
            Assert.Null(command.Url);
        }

        [Test]
        public void TestUrlTypeParameter()
        {
            string url = "https://www.google.com";
            var command = new FirebaseRemoteCommandDroid(MainActivity.CurrentApplication, null, url);
            Assert.Null(command.Path);
            Assert.AreEqual(command.Url, url);
        }

        [Test]
        public void TestLocalTypeParameter()
        {
            string path = "file://path";
            var command = new FirebaseRemoteCommandDroid(MainActivity.CurrentApplication, path, null);
            Assert.Null(command.Url);
            Assert.AreEqual(command.Path, path);
        }

        [Test]
        public void TestNameAndVersion()
        {
            var command = new FirebaseRemoteCommandDroid(MainActivity.CurrentApplication, null, null);
            Assert.NotNull(command.Name);
            Assert.NotNull(command.Version);
            Assert.True(command.Name.StartsWith("xamarin-"));
        }

        public static readonly String CommandId = "firebaseanalytics";
        public static readonly String ResponseId = "none";

        public Org.Json.JSONObject InitPayloadWithNoProperties
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.Config);
                return json;
            }
        }

        public Org.Json.JSONObject InitPayloadWithAllProperties
        {
            get
            {
                Org.Json.JSONObject json = InitPayloadWithNoProperties;
                json.Put(FirebaseConstants.KeySessionTimeout, 10000);
                json.Put(FirebaseConstants.KeyAnalyticsEnabled, false);
                return json;
            }
        }

        public Org.Json.JSONObject InitPayloadWithIncorrectProperties
        {
            get
            {
                Org.Json.JSONObject json = InitPayloadWithNoProperties;
                json.Put(FirebaseConstants.KeySessionTimeout, "string");
                json.Put(FirebaseConstants.KeyAnalyticsEnabled, "test");
                return json;
            }
        }


        public Org.Json.JSONObject LogEventWithNoParams
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.LogEvent);
                json.Put(FirebaseConstants.KeyEventName, "event_campaign_details");

                return json;
            }
        }

        public Org.Json.JSONObject LogEventWithTestParams
        {
            get
            {
                Org.Json.JSONObject json = LogEventWithNoParams;
                Org.Json.JSONObject parameters = new Org.Json.JSONObject();
                parameters.Put("param_content", "my content");
                parameters.Put("param_content_type", "my content type");

                json.Put(FirebaseConstants.KeyEventName, "event_campaign_details");
                json.Put(FirebaseConstants.KeyEventParams, parameters);

                return json;
            }
        }

        public Org.Json.JSONObject LogEventWithNullParams
        {
            get
            {
                Org.Json.JSONObject json = LogEventWithNoParams;
                json.Put(FirebaseConstants.KeyEventName, "event_campaign_details");
                json.Put(FirebaseConstants.KeyEventParams, null);

                return json;
            }
        }

        public Org.Json.JSONObject SetScreenWithValidProperties
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.SetScreenName);
                json.Put(FirebaseConstants.KeyScreenName, "some screen name");
                json.Put(FirebaseConstants.KeyScreenClass, "screen class");

                return json;
            }
        }

        public Org.Json.JSONObject SetScreenWithInvalidProperties
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.SetScreenName);
                json.Put(FirebaseConstants.KeyScreenName, 10);
                json.Put(FirebaseConstants.KeyScreenClass, true);

                return json;
            }
        }

        public Org.Json.JSONObject SetUserIdValid
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.SetUserId);
                json.Put(FirebaseConstants.KeyUserId, "James");

                return json;
            }
        }

        public Org.Json.JSONObject SetUserIdInvalid
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.SetUserId);
                json.Put(FirebaseConstants.KeyUserId, null);


                return json;
            }
        }

        public Org.Json.JSONObject SetUserPropertyValid
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.SetUserProperty);
                json.Put(FirebaseConstants.KeyUserPropertyName, "property name");
                json.Put(FirebaseConstants.KeyUserPropertyValue, "property value");


                return json;
            }
        }

        public Org.Json.JSONObject SetUserPropertyInvalid
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, FirebaseConstants.Commands.SetUserProperty);
                json.Put(FirebaseConstants.KeyUserPropertyName, null);
                json.Put(FirebaseConstants.KeyUserPropertyValue, null);


                return json;
            }
        }

        public Org.Json.JSONObject CompositeAllValidCommands
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, String.Join(',',
                            new string[] { FirebaseConstants.Commands.Config,
                                            FirebaseConstants.Commands.SetUserId,
                                            FirebaseConstants.Commands.SetUserProperty,
                                            FirebaseConstants.Commands.SetScreenName,
                                            FirebaseConstants.Commands.LogEvent }));
                json.Put(FirebaseConstants.KeySessionTimeout, 10000);
                json.Put(FirebaseConstants.KeyAnalyticsEnabled, false);
                json.Put(FirebaseConstants.KeyUserPropertyName, "property name");
                json.Put(FirebaseConstants.KeyUserPropertyValue, "property value");
                json.Put(FirebaseConstants.KeyUserId, "James");
                json.Put(FirebaseConstants.KeyScreenName, "some screen name");
                json.Put(FirebaseConstants.KeyScreenClass, "screen class");
                Org.Json.JSONObject parameters = new Org.Json.JSONObject();
                parameters.Put("param_content", "my content");
                parameters.Put("param_content_type", "my content type");

                json.Put(FirebaseConstants.KeyEventName, "event_campaign_details");
                json.Put(FirebaseConstants.KeyEventParams, parameters);
                return json;
            }
        }

        public Org.Json.JSONObject CompositeSomeInvalidCommands
        {
            get
            {
                Org.Json.JSONObject json = new Org.Json.JSONObject();
                json.Put(FirebaseConstants.KeyCommandName, String.Join(',',
                            new string[] { FirebaseConstants.Commands.Config,
                                            FirebaseConstants.Commands.SetUserId,
                                            FirebaseConstants.Commands.SetUserProperty,
                                            FirebaseConstants.Commands.SetScreenName,
                                            FirebaseConstants.Commands.LogEvent }));
                json.Put(FirebaseConstants.KeyAnalyticsEnabled, false);
                json.Put(FirebaseConstants.KeyUserPropertyName, "property name");
                json.Put(FirebaseConstants.KeyUserPropertyValue, "property value");
                //json.Put(FirebaseRemoteCommand.KeyUserId, "James");//missing
                json.Put(FirebaseConstants.KeyScreenName, null);
                json.Put(FirebaseConstants.KeyScreenClass, null);
                Org.Json.JSONObject parameters = new Org.Json.JSONObject();
                //parameters.Put("param_content", "my content");
                //parameters.Put("param_content_type", "my content type");
                //test empty params.

                json.Put(FirebaseConstants.KeyEventName, "event_campaign_details");
                json.Put(FirebaseConstants.KeyEventParams, parameters);
                return json;
            }
        }

        public override IRemoteCommandResponse GetResponseForPayload(Payloads payloadType)
        {
            Org.Json.JSONObject json;

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
                    json = new Org.Json.JSONObject();
                    break;
            }


            RemoteCommand.Response response = new RemoteCommand.Response(null, CommandId, ResponseId, json);

            return new RemoteCommandResponseDroid(response);
        }

        protected override IRemoteCommand GetFirebaseRemoteCommand()
        {
            return new MockFirebaseRemoteCommandDroid(MainActivity.CurrentApplication);
        }

        protected override IMockFirebaseRemoteCommand GetMockFirebaseRemoteCommand()
        {
            return new MockFirebaseRemoteCommandDroid(MainActivity.CurrentApplication);
        }

        public class MockFirebaseRemoteCommandDroid : FirebaseRemoteCommandDroid, IMockFirebaseRemoteCommand
        {
            public int ConfiguredCalled = 0;
            public int LogEventCalled = 0;
            public int SetScreenCalled = 0;
            public int SetUserIdCalled = 0;
            public int SetUserPropertyCalled = 0;

            public MockFirebaseRemoteCommandDroid(Application app) : this(app, null, null)
            {

            }

            public MockFirebaseRemoteCommandDroid(Application app, string path, string url) : base(app, path, url)
            {

            }

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
