using System;
using System.Linq;
using System.Collections.Generic;
using Org.Json;
using System.Collections;
using Java.Lang;
using Tealium.Droid.NativeInterop;

namespace Tealium.Droid.NativeInterop
{
    /// <summary>
    /// Java object to CLR object converter. Supports also conversion back from
    /// CLR object to Java object.
    /// </summary>
    public static class JavaToClrConverter
    {
        /// <summary>
        /// Converts any supported Java object to it's equivalent in .NET.
        /// </summary>
        /// <returns>Java object converted to CLR object.</returns>
        /// <param name="jObj">Java object to be converted.</param>
        public static object Convert(Java.Lang.Object jObj)
        {
            if (jObj == null) return null;

            if (jObj is Java.Lang.String || jObj is Java.Lang.ICharSequence)
            {
                return jObj.ToString();
            }
            else if (jObj is Android.Runtime.JavaCollection)
            {
                return ConvertFromJavaCollection((Android.Runtime.JavaCollection)jObj);
            }
            else if (jObj is JSONArray)
            {
                return ConvertFromJavaJSONArray((JSONArray)jObj);
            }
            else if (jObj is JSONObject)
            {
                return ConvertFromJavaJSONObject((JSONObject)jObj);
            }
            else if (jObj is Java.Lang.Integer)
            {
                return (int)jObj;
            }
            else if (jObj is Java.Lang.Long)
            {
                return (long)jObj;
            }
            else if (jObj is Java.Lang.Float)
            {
                return (float)jObj;
            }
            else if (jObj is Java.Lang.Double)
            {
                return (double)jObj;
            }
            else if (jObj is Java.Lang.Boolean)
            {
                return (bool)jObj;
            }
            else if (jObj is ICollection)
            {
                return jObj;
            }
            else if (jObj.Class.IsArray)
            {
                if (jObj.Class.ComponentType == Class.FromType(typeof(Java.Lang.String)))
                {
                    return ((string[])jObj).ToList();
                }
                else if (jObj.Class.ComponentType == Class.FromType(typeof(Java.Lang.Integer)))
                {
                    return jObj.ToArray<Java.Lang.Integer>().Select(i => i.IntValue()).ToList();
                }
                else if (jObj.Class.ComponentType == Class.FromType(typeof(Java.Lang.Double)))
                {
                    return jObj.ToArray<Java.Lang.Double>().Select(i => i.DoubleValue()).ToList();
                }
                else if (jObj.Class.ComponentType == Class.FromType(typeof(Java.Lang.Long)))
                {
                    return jObj.ToArray<Java.Lang.Long>().Select(i => i.LongValue()).ToList();
                }
                else if (jObj.Class.ComponentType == Class.FromType(typeof(Java.Lang.Boolean)))
                {
                    return jObj.ToArray<Java.Lang.Boolean>().Select(i => i.BooleanValue()).ToList();
                }
                else
                {
                    return jObj.ToArray<Java.Lang.Object>().Select(i => Convert(i)).ToList();
                }
            }
            else
            {
                return jObj.ToString();
            }
        }

        /// <summary>
        /// Converts Java object to desired CLR object. Basically Java objects are
        /// converted to their CLR equivalents and conversion to CLR string is possible
        /// for all of the supported Java object types.
        /// </summary>
        /// <returns>Java object converted to CLR object.</returns>
        /// <param name="jObj">Java object to be converted.</param>
        /// <typeparam name="T">Conversion CLR target type.</typeparam>
        public static T Convert<T>(Java.Lang.Object jObj)
        {
            Type targetType = typeof(T);

            //any object can be converted to string
            if (targetType == typeof(string))
            {
                if (jObj is Java.Lang.Boolean)
                {
                    //In .NET booleans are title case ("True") and in Java they're not ("true")
                    return (T)(object)System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(jObj.ToString());
                }
                else
                {
                    if (jObj is Java.Lang.Double || jObj is Java.Lang.Float)
                    {
                        //Exponent notation is different in .NET ("E+xxx) and in Java ("Exxx")
                        return (T)(object)jObj.ToString().Replace("E", "E+");
                    }
                    else
                    {
                        return (T)(object)jObj.ToString();
                    }
                }
            }
            else
            {
                if (targetType.IsArray)
                {
                    //if the resulting type is string[] we have to convert the list to array
                    return (T)(object)((List<object>)Convert(jObj)).ToArray();
                }
                else
                {
                    return (T)Convert(jObj);
                }
            }
        }

        /// <summary>
        /// Converts CLR object to it's Java equivalent.
        /// </summary>
        /// <returns>CLR object converted to Java object.</returns>
        /// <param name="obj">CLR object to be converted.</param>
        public static Java.Lang.Object ConvertBack(object obj)
        {
            Java.Lang.Object jObj;
            if (obj is string)
            {
                jObj = new Java.Lang.String((string)obj);
            }
            else if (obj.GetType().GetInterfaces().Any(i => i.IsGenericType &&
                     i.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
            {
                IDictionary dictionary = obj as IDictionary;
                var copy = new Dictionary<string, object>(dictionary.Count);
                foreach (DictionaryEntry d in dictionary)
                {
                    copy.Add(d.Key.ToString(), d.Value);
                }
                jObj = ConvertToJavaJSONObject(copy);
            }
            else if (obj.GetType().GetInterfaces().Any(i => i.IsGenericType &&
                     i.GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                ICollection collection = obj as ICollection;
                jObj = ConvertToJavaJSONArray(collection.Cast<object>().ToList());
            }
            else if (obj is int)
            {
                jObj = new Java.Lang.Integer((int)obj);
            }
            else if (obj is long)
            {
                jObj = new Java.Lang.Long((long)obj);
            }
            else if (obj is double)
            {
                jObj = new Java.Lang.Double((double)obj);
            }
            else if (obj is float)
            {
                jObj = new Java.Lang.Float((float)obj);
            }
            else if (obj is bool)
            {
                jObj = new Java.Lang.Boolean((bool)obj);
            }
            else
            {
                jObj = obj.ToString();
            }
            return jObj;
        }

        /// <summary>
        /// Converts <see cref="Android.Runtime.JavaCollection"/> to list of strings.
        /// Useful when dealing with <see cref="Android.Content.ISharedPreferences"/>
        /// and string sets.
        /// </summary>
        /// <returns>CLR list of strings.</returns>
        /// <param name="jCol">Input Java collection.</param>
        static List<object> ConvertFromJavaCollection(Android.Runtime.JavaCollection jCol)
        {
            List<object> result = new List<object>(jCol.Count);
            foreach (Java.Lang.Object jObj in jCol)
            {
                result.Add(Convert(jObj));
            }
            return result;
        }

        /// <summary>
        /// Converts JSONArrays to CLR string collections.
        /// </summary>
        /// <returns>Collection of CLR strings.</returns>
        /// <param name="jsonArray">JSONArray of strings.</param>
        static ICollection<object> ConvertFromJavaJSONArray(JSONArray jsonArray)
        {
            if (jsonArray == null)
            {
                return null;
            }

            List<object> result = new List<object>(jsonArray.Length());
            for (int i = 0; i < jsonArray.Length(); i++)
            {
                result.Add(Convert(jsonArray.Get(i)));
            }
            return result;
        }

        /// <summary>
        /// Converts CLR string collections to JSONArrays.
        /// </summary>
        /// <returns>JSONArray of strings.</returns>
        /// <param name="strings">CLR string collection.</param>
        static JSONArray ConvertToJavaJSONArray(System.Collections.ICollection objects)
        {
            if (objects == null)
            {
                return null;
            }

            JSONArray result = new JSONArray();
            foreach (object obj in objects)
            {
                result.Put(ConvertBack(obj));
            }
            return result;
        }

        /// <summary>
        /// Converts JSONObject to CLR string - string dictionary.
        /// </summary>
        /// <returns>CLR string - string dictionary.</returns>
        /// <param name="jsonObject">JSONObject containing string - string (key - value) data.</param>
        static IDictionary<string, object> ConvertFromJavaJSONObject(JSONObject jsonObject)
        {
            if (jsonObject == null)
            {
                return null;
            }

            Dictionary<string, object> result = new Dictionary<string, object>(jsonObject.Length());
            var jsonKeys = jsonObject.Keys();
            while (jsonKeys != null && jsonKeys.HasNext)
            {
                var clrKey = Convert<string>(jsonKeys.Next());
                var value = jsonObject.Get(clrKey);
                result.Add(clrKey, Convert(value));
            }
            return result;
        }

        /// <summary>
        /// Converts CLR dictionaries to JSONObjects.
        /// </summary>
        /// <returns>JSONObject containing string - object (key - value) data.</returns>
        /// <param name="strings">CLR string - object dictionary.</param>
        static JSONObject ConvertToJavaJSONObject(IDictionary<string, object> objects)
        {
            if (objects == null)
            {
                return null;
            }

            JSONObject result = new JSONObject();
            foreach (string key in objects.Keys)
            {
                result.Put(key, ConvertBack(objects[key]));
            }
            return result;
        }
    }
}
