using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Tealium;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TealiumXamarinExample.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            TrackEventCommand = new Command(() => Teal.Helper.DefaultInstance.Track(new TealiumEvent("Button Click")));
            TrackEcommercePurchaseCommand = new Command(() =>
            {
                Teal.Helper.DefaultInstance.Track(new TealiumEvent("ecommerce_purchase", new Dictionary<string, object>{
                    { "order_id", 679000 },
                    { "event_title", "event_purchase" },
                    { "product_name", new string[]{"some_name", "2" } },
                    { "product_id", new string[]{"some_id", "2" } },
                    { "product_unit_price", new long[] {20, 10} },
                    { "product_quantity", new long[] {3, 4 } },
                    { "currency_code", "USD" }
                }));
            });

            TrackViewCommand = new Command(() =>
            {
                Teal.Helper.DefaultInstance.Track(new TealiumView("AboutPage-ButtonClick"));
            });

            AddToDataLayerCommand = new Command(() =>
            {
                Teal.Helper.DefaultInstance.AddToDataLayer(new Dictionary<string, object>
                {
                    { "string", "value" },
                    { "int", 10 },
                    { "double", 10.1d },
                    { "long", long.MaxValue },
                    { "bool", true },
                    { "string_array", new string[] { "str_1", "str_2" } },
                    { "int_array", new int[] { 1, 2, 3, 4, 5 } },
                    { "double_array", new double[] { 1.1d, 2.2d, 3.3d, 4.4d, 5.5d } },
                    { "long_array", new long[] { 1L, 2L, 3L, 4L, 5L } },
                    { "bool_array", new bool[] { true, false, true } },
                    { "dictionary", new Dictionary<string, object> {
                        { "sub_string", "value" },
                        { "sub_int", 10 },
                        { "sub_dict", new Dictionary<String, String>
                            {
                            { "sub_sub_string", "value" },
                            { "sub_sub_int", "20" },
                            }
                        }
                    } },

                    { "float", 1.1f },
                    { "float_array", new float[] { 1.1f, 2.2f } },

                }, Expiry.Session);
            });

            GetFromDataLayerCommand = new Command(() =>
            {
                IDictionary<string, object> data = new Dictionary<string, object>()
                {
                    { "string", Teal.Helper.DefaultInstance.GetFromDataLayer("string") },
                    { "int", Teal.Helper.DefaultInstance.GetFromDataLayer("int") },
                    { "double", Teal.Helper.DefaultInstance.GetFromDataLayer("double") },
                    { "long", Teal.Helper.DefaultInstance.GetFromDataLayer("long") },
                    { "bool", Teal.Helper.DefaultInstance.GetFromDataLayer("bool") },
                    { "string_array", Teal.Helper.DefaultInstance.GetFromDataLayer("string_array") },
                    { "int_array", Teal.Helper.DefaultInstance.GetFromDataLayer("int_array") },
                    { "double_array", Teal.Helper.DefaultInstance.GetFromDataLayer("double_array") },
                    { "long_array", Teal.Helper.DefaultInstance.GetFromDataLayer("long_array") },
                    { "bool_array", Teal.Helper.DefaultInstance.GetFromDataLayer("bool_array") },
                    { "dictionary", Teal.Helper.DefaultInstance.GetFromDataLayer("dictionary") }
                };

                foreach (var entry in data)
                {
                    System.Diagnostics.Debug.WriteLine($"key ({entry.Key}) - value: {Stringify(entry.Value)}");
                }
            });
        }

        public ICommand TrackEventCommand { get; }
        public ICommand TrackEcommercePurchaseCommand { get; }
        public ICommand TrackViewCommand { get; }
        public ICommand AddToDataLayerCommand { get; }
        public ICommand GetFromDataLayerCommand { get; }

        private string Stringify(object obj)
        {
            if (obj is ICollection)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(" [ ");
                int i = 1;

                if (obj is IDictionary)
                {
                    var dict = (IDictionary)obj;
                    foreach (DictionaryEntry t in dict)
                    {
                        builder.Append($"{t.Key} : {Stringify(t.Value)}");

                        if (i != dict.Count)
                        {
                            builder.Append(", ");
                        }
                        i++;
                    }
                }
                else
                {
                    var collection = (ICollection)obj;
                    foreach (var t in collection)
                    {
                        builder.Append(Stringify(t));
                        if (i != collection.Count)
                        {
                            builder.Append(", ");
                        }
                        i++;
                    }
                }

                builder.Append(" ] ");
                return builder.ToString();
            }

            return obj.ToString();
        }
    }
}
