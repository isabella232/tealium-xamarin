using System;
using System.Collections.Generic;
using System.Linq;

namespace Tealium.Droid
{
    public class VisitorProfileDroid : IVisitorProfile
    {
        private readonly Com.Tealium.Visitorservice.VisitorProfile nativeProfile;
        private ICurrentVisit currentVisit;

        public VisitorProfileDroid(Com.Tealium.Visitorservice.VisitorProfile nativeProfile)
        {
            this.nativeProfile = nativeProfile ?? throw new ArgumentOutOfRangeException();
        }

        public IDictionary<string, string> Audiences => nativeProfile.Audiences;

        public IDictionary<string, bool> Badges => nativeProfile.Badges?.ToDictionary(p => p.Key, p => (bool)p.Value);

        public IDictionary<string, long> Dates => nativeProfile.Dates?.ToDictionary(p => p.Key, p => p.Value.LongValue());

        public IDictionary<string, bool> Booleans => nativeProfile.Booleans?.ToDictionary(p => p.Key, p => p.Value.BooleanValue());

        public IDictionary<string, IList<bool>> ArraysOfBooleans => nativeProfile.ArraysOfBooleans?.ToDictionary(p => p.Key, p => p.Value.Select(e => e.BooleanValue()).ToList()) as IDictionary<string, IList<bool>>;

        public IDictionary<string, double> Numbers => nativeProfile.Numbers?.ToDictionary(p => p.Key, p => p.Value.DoubleValue());

        public IDictionary<string, IList<double>> ArraysOfNumbers => nativeProfile.ArraysOfNumbers?.ToDictionary(p => p.Key, p => p.Value.Select(e => e.DoubleValue()).ToList()) as IDictionary<string, IList<double>>;

        public IDictionary<string, IDictionary<string, double>> Tallies => nativeProfile.Tallies?.ToDictionary(p => p.Key, p => p.Value.ToDictionary(p => p.Key, p => p.Value.DoubleValue())) as IDictionary<string, IDictionary<string, double>>;

        public IDictionary<string, string> Strings => nativeProfile.Strings;

        public IDictionary<string, IList<string>> ArraysOfStrings => nativeProfile.ArraysOfStrings;

        public IDictionary<string, ISet<string>> SetsOfStrings => nativeProfile.SetsOfStrings?.ToHashSet() as IDictionary<string, ISet<string>>;

        public int TotalEventCount => nativeProfile.TotalEventCount;

        public ICurrentVisit CurrentVisit
        {
            get
            {
                if (currentVisit != null)
                {
                    return currentVisit;
                }

                var nativeCurrentVisit = nativeProfile.CurrentVisit;
                if (nativeCurrentVisit == null)
                {
                    return null;
                }

                currentVisit = new CurrentVisitDroid(nativeProfile.CurrentVisit);
                return currentVisit;
            }
        }
    }


    public class CurrentVisitDroid : ICurrentVisit
    {
        private readonly Com.Tealium.Visitorservice.CurrentVisit nativeCurrentVisit;

        public CurrentVisitDroid(Com.Tealium.Visitorservice.CurrentVisit nativeCurrentVisit)
        {
            this.nativeCurrentVisit = nativeCurrentVisit ?? throw new ArgumentOutOfRangeException();
        }

        public long CreatedAt => nativeCurrentVisit.CreatedAt;

        public long TotalEventCount => nativeCurrentVisit.TotalEventCount;

        public IDictionary<string, long> Dates => nativeCurrentVisit.Dates?.ToDictionary(p => p.Key, p => p.Value.LongValue());

        public IDictionary<string, bool> Booleans => nativeCurrentVisit.Booleans?.ToDictionary(p => p.Key, p => p.Value.BooleanValue());

        public IDictionary<string, IList<bool>> ArraysOfBooleans => nativeCurrentVisit.ArraysOfBooleans?.ToDictionary(p => p.Key, p => p.Value.Select(e => e.BooleanValue()).ToList()) as IDictionary<string, IList<bool>>;

        public IDictionary<string, double> Numbers => nativeCurrentVisit.Numbers?.ToDictionary(p => p.Key, p => p.Value.DoubleValue());

        public IDictionary<string, IList<double>> ArraysOfNumbers => nativeCurrentVisit.ArraysOfNumbers?.ToDictionary(p => p.Key, p => p.Value?.Select(e => e.DoubleValue()).ToList()) as IDictionary<string, IList<double>>;

        public IDictionary<string, IDictionary<string, double>> Tallies => nativeCurrentVisit.Tallies?.ToDictionary(p => p.Key, p => p.Value.ToDictionary(p => p.Key, p => p.Value.DoubleValue())) as IDictionary<string, IDictionary<string, double>>;

        public IDictionary<string, string> Strings => nativeCurrentVisit.Strings;

        public IDictionary<string, IList<string>> ArraysOfStrings => nativeCurrentVisit.ArraysOfStrings;

        public IDictionary<string, ISet<string>> SetsOfStrings => nativeCurrentVisit.SetsOfStrings?.ToHashSet() as IDictionary<string, ISet<string>>;
    }
}
