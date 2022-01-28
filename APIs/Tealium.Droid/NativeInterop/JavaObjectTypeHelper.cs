using System;

namespace Tealium.Droid.NativeInterop
{
    /// <summary>
    /// Helps to cast Java objects to .NET objects.
    /// </summary>
    static class JavaObjectTypeHelper
    {
        //TODO: CastToRefType extension method may not be useful in our case...
        /// <summary>
        /// Tries to cast the given <see cref="Java.Lang.Object"/> to a CLR reference type.
        /// Can return null if cast is unsuccessful.
        /// </summary>
        /// <returns>Java object cast to desired CLR reference type.</returns>
        /// <param name="obj">Java object.</param>
        public static T CastToRefType<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }
}
