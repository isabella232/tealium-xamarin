using System;
using System.Collections.Generic;

namespace Tealium
{
    public interface IVisitorProfile
    {
        /// <summary>
        /// Returns a dictionary of Audiences that this visitor belongs to,
        /// indexed by the id. 
        /// </summary>
        public IDictionary<string, string> Audiences { get; }

        /// <summary>
        /// Returns a dictionary of Badges assigned to the visitor, indexed by
        /// the badge id.
        /// </summary>
        public IDictionary<string, bool> Badges { get; }

        /// <summary>
        /// Returns a dictionary of Dates (epoch time) indexed by the attirbute
        /// id
        /// </summary>
        public IDictionary<string, long> Dates { get; }

        /// <summary>
        /// Returns a dictionary of Booleans indexed by the attribute id
        /// </summary>
        public IDictionary<string, bool> Booleans { get; }

        /// <summary>
        /// Returns a dictionary of Arrays of Booleans indexed by the attribute
        /// id
        /// </summary>
        public IDictionary<string, IList<bool>> ArraysOfBooleans { get; }

        /// <summary>
        /// Returns a dictionary of Numbers indexed by the attribute id
        /// </summary>
        public IDictionary<string, double> Numbers { get; }

        /// <summary>
        /// Returns a dictionary of Arrays of Numbers indexed by the attribute
        /// id
        /// </summary>
        public IDictionary<string, IList<double>> ArraysOfNumbers { get; }

        /// <summary>
        /// Returns a dictionary of Tallies indexed by the attribute id
        /// </summary>
        public IDictionary<string, IDictionary<string, double>> Tallies { get; }

        /// <summary>
        /// Returns a dictionary of Strings indexed by the attribute id
        /// </summary>
        public IDictionary<string, string> Strings { get; }

        /// <summary>
        /// Returns a dictionary of Arrays of Strings indexed by the attribute
        /// id
        /// </summary>
        public IDictionary<string, IList<string>> ArraysOfStrings { get; }

        /// <summary>
        /// Returns a dictionary of Sets of Strings indexed by the attribute id
        /// </summary>
        public IDictionary<string, ISet<string>> SetsOfStrings { get; }

        /// <summary>
        /// Returns the total event count for this visitor.
        /// </summary>
        public int TotalEventCount { get; }

        /// <summary>
        /// Returns data relating specifically relating to Visit scoped
        /// attributes
        /// </summary>
        public ICurrentVisit CurrentVisit { get; }
    }

    public interface ICurrentVisit
    {
        /// <summary>
        /// Returns the unix epoch time that this visit was created.
        /// </summary>
        public long CreatedAt { get; }

        /// <summary>
        /// Returns the total event count recorded in this visit
        /// </summary>
        public long TotalEventCount { get; }


        /// <summary>
        /// Returns a dictionary of Dates (epoch time) indexed by the attirbute
        /// id
        /// </summary>
        public IDictionary<string, long> Dates { get; }

        /// <summary>
        /// Returns a dictionary of Booleans indexed by the attribute id
        /// </summary>
        public IDictionary<string, bool> Booleans { get; }

        /// <summary>
        /// Returns a dictionary of Arrays of Booleans indexed by the attribute
        /// id
        /// </summary>
        public IDictionary<string, IList<bool>> ArraysOfBooleans { get; }

        /// <summary>
        /// Returns a dictionary of Numbers indexed by the attribute id
        /// </summary>
        public IDictionary<string, double> Numbers { get; }

        /// <summary>
        /// Returns a dictionary of Arrays of Numbers indexed by the attribute
        /// id
        /// </summary>
        public IDictionary<string, IList<double>> ArraysOfNumbers { get; }

        /// <summary>
        /// Returns a dictionary of Tallies indexed by the attribute id
        /// </summary>
        public IDictionary<string, IDictionary<string, double>> Tallies { get; }

        /// <summary>
        /// Returns a dictionary of Strings indexed by the attribute id
        /// </summary>
        public IDictionary<string, string> Strings { get; }

        /// <summary>
        /// Returns a dictionary of Arrays of Strings indexed by the attribute
        /// id
        /// </summary>
        public IDictionary<string, IList<string>> ArraysOfStrings { get; }

        /// <summary>
        /// Returns a dictionary of Sets of Strings indexed by the attribute id
        /// </summary>
        public IDictionary<string, ISet<string>> SetsOfStrings { get; }
    }
}
