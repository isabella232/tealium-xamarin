
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xunit;
using Xunit.Runners.UI;

namespace TealiumXamarinExample.Droid
{
    [Activity(Label = "TestsActivity")]
    public class TestsActivity : RunnerActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            AddTestAssembly(Assembly.GetExecutingAssembly());

            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}
