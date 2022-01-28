using System;
using System.Collections.Generic;

namespace Tealium.Droid.NativeInterop
{
    public static class JavaDictionaryToClrDictionaryConverter
    {
        public static Dictionary<string, object> Convert(IDictionary<string, Java.Lang.Object> javaDict)
        {
            if (javaDict == null)
            {
                return null;
            }

            Dictionary<string, object> dict = new Dictionary<string, object>(javaDict.Count);
            foreach (var key in javaDict.Keys)
            {
                dict.Add(key, JavaToClrConverter.Convert(javaDict[key]));
            }

            return dict;
        }

        public static Dictionary<string, Java.Lang.Object> ConvertBack(IDictionary<string, object> dict)
        {
            if (dict == null)
            {
                return null;
            }

            Dictionary<string, Java.Lang.Object> javaDict = new Dictionary<string, Java.Lang.Object>(dict.Count);
            foreach (var key in dict.Keys)
            {
                javaDict.Add(key, JavaToClrConverter.ConvertBack(dict[key]));
            }
            return javaDict;
        }
    }
}
