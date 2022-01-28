using System;
namespace Tealium.Tests.iOS.NativeInteropTests
{
    public static class Utils
    {
        internal static string TypeMismatchMessage(object expected, Type resulted)
        {
            return $"The resulting type {expected.GetType()} should be {resulted} (or a subclass).";
        }
    }
}
