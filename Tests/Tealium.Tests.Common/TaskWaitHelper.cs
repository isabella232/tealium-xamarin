using System;
namespace Tealium.Tests.Common
{
    public static class TaskWaitHelper
    {
        public static void Wait(int delayMillis = 5)
        {
#if __IOS__

            //On iOS almost every operation is an async operation, but we have no chance
            //of awaiting for this operation to end - we guess it won't take longer than the
            //delay below:
            System.Threading.Tasks.Task.Delay(delayMillis).Wait();
#endif
        }
    }
}
