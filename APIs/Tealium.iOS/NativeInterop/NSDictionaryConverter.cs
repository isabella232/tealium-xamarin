using System;
using System.Collections.Generic;
using Foundation;

namespace Tealium.iOS.NativeInterop
{
    static internal class NSDictionaryConverter
    {
        [Obsolete("This method should not be used as only strongly typed data access is fully supported.")]
        static internal Dictionary<string, object> Convert(NSDictionary nsDict)
        {
            if (nsDict == null)
            {
                return null;
            }
            Dictionary<string, object> dict = new Dictionary<string, object>((int)nsDict.Count);
            foreach (var nsKey in nsDict.Keys)
            {
                dict.Add(((NSString)nsKey), NSObjectConverter.Convert(nsDict[nsKey]));
            }
            return dict;
        }

        static internal NSDictionary<NSString, T> ConvertBack<T>(IDictionary<string, object> dict) where T : NSObject
        {
            if (dict == null)
            {
                return null;
            }
            if (dict.Keys.Count == 0)
            {
                return new NSDictionary<NSString,T>();
            }
            NSMutableDictionary<NSString, T> nsDict = new NSMutableDictionary<NSString, T>();
            foreach (var key in dict.Keys)
            {
                NSString nsKey = NSObjectConverter.ConvertBack(key) as NSString;
                var value = dict[key];
                if (value == null)
                {
                    continue;
                }
                NSObject nsValue = NSObjectConverter.ConvertBack(value);
                nsDict.Add(nsKey, nsValue);
            }
            return NSDictionary<NSString, T>.FromObjectsAndKeys(nsDict.Values, nsDict.Keys);
        }
    }
}
