using System;
using Tealium.Platform.iOS;
using Tealium.iOS.NativeInterop.Extensions;
using Foundation;

namespace Tealium.iOS
{
    public class DispatchValidatorIOS: DispatchValidatorWrapper, IDispatchValidator
    {
        readonly IDispatchValidator validator;

        /**
         * Pass a validator in this method or override this class to provide your own implementation of ShouldDrop and ShouldQueue and pass null as validator.
         */
        public DispatchValidatorIOS(string id, IDispatchValidator validator)
            : base(id)
        {
            this.validator = validator;
        }

        public string Name => Id;

        sealed override public bool ShouldDropWithRequest(TealiumRequestWrapper request)
        {
            return ShouldDrop(DispatchFromRequest(request));
        }

        virtual public bool ShouldDrop(Dispatch dispatch)
        {
            return validator?.ShouldDrop(dispatch) ?? false;
        }

        sealed override public QueueRequestResponse ShouldQueueWithRequest(TealiumRequestWrapper request)
        {
            return new QueueRequestResponse(ShouldQueue(DispatchFromRequest(request)), null);
        }

        virtual public bool ShouldQueue(Dispatch dispatch)
        {
            
            return validator?.ShouldQueue(dispatch) ?? false;
        }

        private Dispatch DispatchFromRequest(TealiumRequestWrapper request)
        {
            if (request == null)
            {
                return null;
            }
            var dict = request.TrackDictionary.ToDictionary<object, NSObject>();
            var name = dict["tealium_event"] as string;
            if ((dict["tealium_event_type"] as string) == "view")
            {
                return new TealiumView(name, dict);
            }
            else
            {
                return new TealiumEvent(name, dict);
            }
        }
    }
}
