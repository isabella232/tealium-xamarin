using System;
namespace Tealium
{
    /// <summary>
    /// Delegate-based, customizable dispatch validator.
    /// </summary>
    public class DelegateDispatchValidator : IDispatchValidator
    {
        private string name;

        public DelegateDispatchValidator(string name)
        {
            this.name = name ?? throw new ArgumentNullException("name cannot be null");
        }

        public bool ShouldDrop(Dispatch dispatch)
        {
            if (ShouldDropDispatchDelegate != null)
            {
                return ShouldDropDispatchDelegate(dispatch);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Specifies the verification delegate method for dropping dispatches.
        /// </summary>
        public Func<Dispatch, bool> ShouldDropDispatchDelegate { get; set; }

        public bool ShouldQueue(Dispatch dispatch)
        {
            if (ShouldQueueDispatchDelegate != null)
            {
                return ShouldQueueDispatchDelegate(dispatch);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Specifies the verification delegate method for queuing dispatches.
        /// </summary>
        public Func<Dispatch, bool> ShouldQueueDispatchDelegate { get; set; }

        public string Name => name;
    }
}
