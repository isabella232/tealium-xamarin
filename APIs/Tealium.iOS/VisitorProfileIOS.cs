using System;
using Tealium.Platform.iOS;
using Tealium;
using System.Collections.Generic;
using Tealium.iOS.NativeInterop.Extensions;
using Foundation;

namespace Tealium.iOS
{
    public class VisitorProfileIOS : IVisitorProfile
    {
        readonly TealiumVisitorProfileWrapper nativeProfile;
        ICurrentVisit currentVisit;
        public VisitorProfileIOS(TealiumVisitorProfileWrapper profile)
        {
            nativeProfile = profile;
        }

        public int TotalEventCount => (int)nativeProfile.TotalEventsCount;

        public IDictionary<string, string> Audiences => nativeProfile.Audiences?.ToDictionary<string, NSString>();

        public IDictionary<string, bool> Badges => nativeProfile.Badges?.ToDictionary<bool, NSNumber>();

        public IDictionary<string, long> Dates => nativeProfile.Dates?.ToDictionary<long, NSNumber>();

        public IDictionary<string, bool> Booleans => nativeProfile.Booleans?.ToDictionary<bool, NSNumber>();

        public IDictionary<string, IList<bool>> ArraysOfBooleans => nativeProfile.ArraysOfBooleans?.ToDictionaryOfLists<bool, NSNumber>();

        public IDictionary<string, double> Numbers => nativeProfile.Numbers?.ToDictionary<double, NSNumber>();

        public IDictionary<string, IList<double>> ArraysOfNumbers => nativeProfile.ArraysOfNumbers?.ToDictionaryOfLists<double, NSNumber>();

        public IDictionary<string, IDictionary<string, double>> Tallies => nativeProfile.Tallies?.ToDictionaryOfDictionaries<double, NSNumber>();

        public IDictionary<string, string> Strings => nativeProfile.Strings?.ToDictionary<string, NSString>();

        public IDictionary<string, IList<string>> ArraysOfStrings => nativeProfile.ArraysOfStrings?.ToDictionaryOfLists<string, NSString>();

        public IDictionary<string, ISet<string>> SetsOfStrings => nativeProfile.SetsOfStrings?.ToDictionaryOfSets<string, NSString>();

        public ICurrentVisit CurrentVisit
        {
            get
            {
                if (currentVisit == null)
                {
                    currentVisit = new CurrentVisitIOS(nativeProfile.CurrentVisit);
                }
                return currentVisit;
            }
        }
    }

    public class CurrentVisitIOS : ICurrentVisit
    {
        readonly TealiumCurrentVisitProfileWrapper nativeVisit;
        public CurrentVisitIOS(TealiumCurrentVisitProfileWrapper currentVisit)
        {
            nativeVisit = currentVisit;
        }

        public long CreatedAt => nativeVisit.CreatedAt;

        public long TotalEventCount => 0;

        public IDictionary<string, long> Dates => nativeVisit.Dates?.ToDictionary<long, NSNumber>();

        public IDictionary<string, bool> Booleans => nativeVisit.Booleans?.ToDictionary<bool, NSNumber>();

        public IDictionary<string, IList<bool>> ArraysOfBooleans => nativeVisit.ArraysOfBooleans?.ToDictionaryOfLists<bool, NSNumber>();

        public IDictionary<string, double> Numbers => nativeVisit.Numbers?.ToDictionary<double, NSNumber>();

        public IDictionary<string, IList<double>> ArraysOfNumbers => nativeVisit.ArraysOfNumbers?.ToDictionaryOfLists<double, NSNumber>();

        public IDictionary<string, IDictionary<string, double>> Tallies => nativeVisit.Tallies?.ToDictionaryOfDictionaries<double, NSNumber>();

        public IDictionary<string, string> Strings => nativeVisit.Strings?.ToDictionary<string, NSString>();

        public IDictionary<string, IList<string>> ArraysOfStrings => nativeVisit.ArraysOfStrings?.ToDictionaryOfLists<string, NSString>();

        public IDictionary<string, ISet<string>> SetsOfStrings => nativeVisit.SetsOfStrings?.ToDictionaryOfSets<string, NSString>();
    }
}
