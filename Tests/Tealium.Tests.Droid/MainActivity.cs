using System.Reflection;

using Android.App;
using Android.OS;
using Android.Widget;
using Xamarin.Android.NUnitLite;

namespace Tealium.Tests.Droid
{
    [Activity(Label = "Tealium.Tests.Droid", MainLauncher = true)]
    public class MainActivity : TestSuiteActivity
    {
        public static Application CurrentApplication { get; private set; }
        private Button button;

        protected override void OnCreate(Bundle bundle)
        {
            CurrentApplication = Application;

            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}
