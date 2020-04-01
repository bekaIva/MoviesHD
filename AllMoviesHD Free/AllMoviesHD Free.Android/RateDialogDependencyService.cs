using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoviesHD.Droid;
using MoviesHD.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
[assembly: Xamarin.Forms.Dependency(typeof(RateDialogDependencyService))]
namespace MoviesHD.Droid
{
    public class RateDialogDependencyService : IShowRateDialog
    {
        public void ShowRateDialog()
        {
            Android.Net.Uri uri = Android.Net.Uri.Parse("market://details?id=" + MainActivity.Activity.PackageName);
            Intent goToMarket = new Intent(Intent.ActionView, uri);
            // To count with Play market backstack, After pressing back button, 
            // to taken back to our application, we need to add following flags to intent. 
            goToMarket.AddFlags(ActivityFlags.NoHistory |
                            ActivityFlags.NewDocument |
                            ActivityFlags.MultipleTask);
            try
            {
               MainActivity.Activity.StartActivity(goToMarket);
            }
            catch (ActivityNotFoundException e)
            {
                //MainActivity.Activity.StartActivity(new Intent(Intent.ActionView),
                //        Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + MainActivity.Activity.PackageName));
            }
        }
    }
}