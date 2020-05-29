using Android.App;
using Android.Content;
using Android.OS;
using System.Threading.Tasks;

namespace JTSK.Droid
{
    [Activity(Label = "JTSK", Icon = "@mipmap/jtsk", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Task startupWork = new Task(() => { StartActivity(new Intent(Application.Context, typeof(MainActivity))); });
            startupWork.Start();
        }

    }
}