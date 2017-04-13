using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Harder
{
    [Activity(Label = "Harder"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        public static int SizeOfDeviceX;
        public static int SizeOfDeviceY;
         
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var g = new Run();
            SetContentView((View)g.Services.GetService(typeof(View)));

            var metrics = Resources.DisplayMetrics;
            SizeOfDeviceX = metrics.WidthPixels;
            SizeOfDeviceY = metrics.HeightPixels;
            //var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            //var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            g.Run();
        }

        /*
         private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
         */
    }
}

