using System;
using System.Threading.Tasks;
using Android.OS;
using AndroidX.AppCompat.App;
using Android.App;
using Android.Content;

namespace Hangman.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        async void SimulateStartup()
        {
            await Task.Delay(4000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}
