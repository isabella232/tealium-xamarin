using System;
namespace Tealium
{
    /// <summary>
    /// Validates dispatches. Allows to indicate whether to queue or to drop
    /// a dispatch.
    /// </summary>
    public interface IDispatchValidator
    {
        string Name { get; }

        /// <summary>
        /// Indicates whether to queue the dispatch. Properties and payload data 
        /// manipulated here will overwrite this dispatch’s outbound data 
        /// - effects not persistent.
        /// </summary>
        /// <returns><c>true</c>, if the dispatch should be queued, <c>false</c> otherwise.</returns>
        /// <param name="dispatch">Dispatch object.</param>
        bool ShouldQueue(Dispatch dispatch);

        /// <summary>
        /// Indicates whether to drop the dispatch. Properties and payload data 
        /// manipulated here will overwrite this dispatch’s outbound data 
        /// - effects not persistent.
        /// </summary>
        /// <returns><c>true</c>, if the dispatch should be dropped, <c>false</c> otherwise.</returns>
        /// <param name="dispatch">Dispatch object.</param>
        bool ShouldDrop(Dispatch dispatch);
    }
}
