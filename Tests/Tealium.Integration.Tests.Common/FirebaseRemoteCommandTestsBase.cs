using System;
using System.Collections.Generic;
//using Tealium.RemoteCommands.Firebase;

using NUnit.Framework;

namespace Tealium.Integration.Tests.Common
{

    public abstract class FirebaseRemoteCommandTestsBase
    {
        protected abstract IRemoteCommand GetFirebaseRemoteCommand();
        protected abstract IMockFirebaseRemoteCommand GetMockFirebaseRemoteCommand();
        public abstract IRemoteCommandResponse GetResponseForPayload(Payloads payloadType);
        //public abstract IRemoteCommandPayload InitPayload { get; }

        [Test]
        public void TestEmptyConfiguration()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();

            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.InitWithNoProperties));

            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls > 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestAllConfiguration()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.InitWithAllProperties));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestIncorrectConfiguration()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.InitWithIncorrectProperties));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestLogEventWithNoParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.LogEventWithNoParams));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;

            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestLogEventWithTestParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.LogEventWithTestParams));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestLogEventWithNullParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.LogEventWithNullParams));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestSetScreenNameWithValidParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.SetScreenWithValidParams));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestSetScreenNameWithInvalidParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.SetScreenWithInvalidParams));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestSetUserIdWithValidParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.SetUserIdValid));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestSetUserIdWithInvalidParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.SetUserIdInvalid));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestSetUserPropertyWithValidParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.SetUserPropertyValid));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 1);
        }

        [Test]
        public void TestSetUserPropertyWithInvalidParameters()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.SetUserPropertyInvalid));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 0);
        }

        [Test]
        public void TestCompositeWithAllValidCommands()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.CompositeAllValidCommands));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 2);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 1);
        }

        [Test]
        public void TestCompositeWithSomeValidCommands()
        {
            IRemoteCommand remoteCommand = GetFirebaseRemoteCommand();
            Assert.DoesNotThrow(() =>
            {
                remoteCommand.HandleResponse(GetResponseForPayload(Payloads.CompositeSomeInvalidCommands));
            });

            IMockFirebaseRemoteCommand mockFirebaseRemoteCommand = (IMockFirebaseRemoteCommand)remoteCommand;
            Assert.True(mockFirebaseRemoteCommand.GetConfigCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetUserIdCalls == 0);//User Id is missing in test
            Assert.True(mockFirebaseRemoteCommand.GetLogEventCalls == 1);
            Assert.True(mockFirebaseRemoteCommand.GetScreenNameCalls == 0);//Screen name and class are both null
            Assert.True(mockFirebaseRemoteCommand.GetUserPropertyCalls == 1);
        }

        public enum Payloads
        {
            InitWithNoProperties,
            InitWithAllProperties,
            InitWithIncorrectProperties,
            LogEventWithNoParams,
            LogEventWithTestParams,
            LogEventWithNullParams,
            SetScreenWithValidParams,
            SetScreenWithInvalidParams,
            SetUserIdValid,
            SetUserIdInvalid,
            SetUserPropertyValid,
            SetUserPropertyInvalid,
            CompositeAllValidCommands,
            CompositeSomeInvalidCommands
        }

        public interface IMockFirebaseRemoteCommand
        {
            int GetConfigCalls { get; }
            int GetUserIdCalls { get; }
            int GetUserPropertyCalls { get; }
            int GetLogEventCalls { get; }
            int GetScreenNameCalls { get; }
        }
    }
}