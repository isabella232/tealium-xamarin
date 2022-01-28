using System;
using System.Collections.Generic;
using System.Collections;
using Foundation;
using System.Linq;

namespace Tealium.iOS.NativeInterop
{
    /// <summary>
    /// NSObject to CLR object converter. Supports also conversion back from
    /// CLR object to NSObject.
    /// </summary>
    static internal class NSObjectConverter
    {
        /// <summary>
        /// Converts any supported NSObject to it's equivalent in .NET.
        /// </summary>
        /// <returns>NSObject converted to CLR object.</returns>
        /// <param name="nsObj">Java object to be converted.</param>
        static internal object Convert(NSObject nsObj)
        {
            if (nsObj is NSString)
            {
                return nsObj.ToString();
            }
            else if (nsObj is NSArray || nsObj is NSDictionary)
            {
                return ConvertGenericArrayOrDictionary(nsObj);
            }
            else if (nsObj is NSNumber number)
            {
                return ConvertNSNumber(number);
            }
            else if (nsObj is NSDate date)
            {
                return (DateTime)date;
            }
            else
            {
                throw new InvalidOperationException($"Unable to create CLR object from {nsObj.GetType()}!");
            }
        }

        static internal object ConvertNSNumber(NSNumber number)
        {
            switch (number.ObjCType)
            {
                case "c":
                case "B":
                    return number.BoolValue;
                case "i":
                case "s":
                case "l":
                case "q":
                    return number.Int64Value;
                case "f":
                    return number.FloatValue;
                case "d":
                    return number.DoubleValue;
                case "I":
                case "S":
                case "L":
                case "Q":
                    return number.UInt64Value;
                default:
                    // We can't determine here what's the target type: int, float, long or even bool! Decimal is the default as it will cover all cases...
                    return (decimal)number.NSDecimalValue;
            }
        }

        /// <summary>
        /// Converts NSObject to desired CLR object. Basically NSObjects are
        /// converted to their CLR equivalents and conversion to CLR string is possible
        /// for all of the supported NSObject types.
        /// </summary>
        /// <returns>Java object converted to CLR object.</returns>
        /// <param name="nsObj">NSObject to be converted.</param>
        /// <typeparam name="T">Conversion CLR target type.</typeparam>
        static internal T ConvertToTargetType<T>(NSObject nsObj)
        {
            //NOTE: in case more conversion examples are needed: https://github.com/xamarin/ios-samples/blob/master/HomeKit/HomeKitIntro/HomeKitIntro/Classes/NSObjectConverter.cs
            object result = default(T);
            Type t = typeof(T);
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.String:
                    if (nsObj is NSNumber number)
                    {
                        result = ConvertNSNumber(number).ToString();
                    }
                    else
                    {
                        result = nsObj.ToString();
                    }
                    break;
                case TypeCode.Boolean:
                    result = ((NSNumber)nsObj).BoolValue;
                    break;
                case TypeCode.Int32:
                    result = ((NSNumber)nsObj).Int32Value;
                    break;
                case TypeCode.Int64:
                    result = ((NSNumber)nsObj).Int64Value;
                    break;
                case TypeCode.Decimal:
                    result = (decimal)((NSNumber)nsObj).NSDecimalValue;
                    break;
                case TypeCode.Single:
                    result = ((NSNumber)nsObj).FloatValue;
                    break;
                case TypeCode.Double:
                    result = ((NSNumber)nsObj).DoubleValue;
                    break;
                case TypeCode.DateTime:
                    result = (DateTime)(NSDate)nsObj;
                    break;
                case TypeCode.Object:
                    if (t == typeof(string[]))
                    {
                        //if the resulting type is string[] we have to convert the list to array
                        result = ConvertFromNSArrayToTargetType<string>((NSArray)nsObj).ToArray();
                    }
                    else // This is an object and we don't know what kind of object... fallback to try and convert by nsObj type
                    {
                        result = Convert(nsObj);
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Unable to create {t} from {nsObj.GetType()}!");
            }

            return (T)result;
        }

        static object ConvertGenericArrayOrDictionary(NSObject nsObj)
        {
            object result;
            if (nsObj is NSArray || nsObj is NSDictionary)
            {
                if (nsObj.GetType().IsGenericType)
                {
                    var genericArguments = nsObj.GetType().GetGenericArguments();
                    var argument = nsObj is NSArray ? genericArguments[0] : genericArguments[1];
                    if (argument == typeof(NSString))
                    {
                        result = ConvertFromNSArrayOrNSDictionaryToTargetType<string>(nsObj);
                    }
                    else if (argument == typeof(NSNumber)) 
                    {
                        // Unfortunately we have no idea of what type of NSNumber are we talking about, so we default to any object
                        result = ConvertFromNSArrayOrNSDictionaryToTargetType<object>(nsObj);
                    }
                    else if (argument == typeof(NSDate))
                    {
                        result = ConvertFromNSArrayOrNSDictionaryToTargetType<DateTime>(nsObj);
                    }
                    else
                    {
                        result = ConvertFromNSArrayOrNSDictionaryToTargetType<object>(nsObj);
                    }
                }
                else
                {
                    result = ConvertFromNSArrayOrNSDictionaryToTargetType<object>(nsObj);
                }
            } else
            {
                throw new InvalidOperationException($"Unable to create Array or Dictionary from {nsObj.GetType()}!");
            }
            return result;
        }

        static object ConvertFromNSArrayOrNSDictionaryToTargetType<T>(NSObject nsObj)
        {
            if (nsObj is NSArray)
            {
                return ConvertFromNSArrayToTargetType<T>((NSArray)nsObj);
            }
            else if (nsObj is NSDictionary)
            {
                return ConvertFromNSDictionaryToTargetType<T>((NSDictionary)nsObj);
            }
            throw new InvalidOperationException($"Unable to create Array or Dict from {nsObj.GetType()}!");
        }

        /// <summary>
        /// Converts CLR object to it's NSObject equivalent.
        /// </summary>
        /// <returns>CLR object converted to NSObject.</returns>
        /// <param name="obj">CLR object to be converted.</param>
        static internal NSObject ConvertBack(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Null values are not supported while converting CLR object to NSObject!");
            }
            else if (obj is ICollection) //this should do for List<string> and string[]
            {
                if (obj is IDictionary)
                {
                    return ConvertToNSDictionary((IDictionary)obj);
                }
                else
                {
                    return ConvertToNSArray((ICollection)obj);
                }
            }
            else
            {
                return NSObject.FromObject(obj);
            }
        }

        /// <summary>
        /// Converts CLR string collections to NSArrays.
        /// </summary>
        /// <returns>NSArray of strings.</returns>
        /// <param name="strings">CLR string collection.</param>
        static NSArray ConvertToNSArray(ICollection items)
        {
            if (items == null)
            {
                return null;
            }
            var clrArray = new ArrayList((ICollection)items).ToArray().ToList().ConvertAll(i =>
            {
                NSObject nso = ConvertBack(i);
                return nso;
            }).ToArray();

            NSArray array = NSArray.FromObjects(clrArray);
            return array;
        }

        /// <summary>
        /// Converts NSArrays to CLR string collections.
        /// </summary>
        /// <returns>Collection of CLR strings.</returns>
        /// <param name="nsArray">NSArray of strings.</param>
        static internal List<T> ConvertFromNSArrayToTargetType<T>(NSArray nsArray)
        {
            if (nsArray == null)
            {
                return null;    
            }

            List<T> result = new List<T>((int)nsArray.Count);
            for (nuint i = 0; i < nsArray.Count; i++)
            {
                T converted = ConvertToTargetType<T>(nsArray.GetItem<NSObject>(i));
                result.Add(converted);
            }
            return result;
        }

        /// <summary>
        /// Converts CLR string - string dictionaries to NSDictionaries.
        /// </summary>
        /// <returns>NSDictionary containing string - string (key - value) data.</returns>
        /// <param name="strings">CLR string - string dictionary.</param>
        static NSDictionary ConvertToNSDictionary(IDictionary dict)
        {
            if (dict == null)
            {
                return null;
            }

            NSMutableDictionary result = new NSMutableDictionary();
            foreach (string key in dict.Keys)
            {
                result.Add(new NSString(key), ConvertBack(dict[key]));
            }
            return result;
        }

        /// <summary>
        /// Converts NSDictionary to CLR string - string dictionary.
        /// </summary>
        /// <returns>CLR string - string dictionary.</returns>
        /// <param name="nsDict">NSDictionary containing string - string (key - value) data.</param>
        static internal Dictionary<string, T> ConvertFromNSDictionaryToTargetType<T>(NSDictionary nsDict)
        {
            if (nsDict == null)
            {
                return null;
            }

            Dictionary<string, T> result = new Dictionary<string, T>((int)nsDict.Count);
            foreach(NSObject key in nsDict.Keys)
            {
                result.Add((NSString)key, ConvertToTargetType<T>(nsDict[key]));    
            }
            return result;
        }
    }
}
