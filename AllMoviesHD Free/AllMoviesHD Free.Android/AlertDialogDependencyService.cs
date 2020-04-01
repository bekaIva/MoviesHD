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
[assembly: Xamarin.Forms.Dependency(typeof(AlertDialogDependencyService))]
namespace MoviesHD.Droid
{
    class AlertDialogDependencyService : IAlertDialog
    {
        public void ShowDialog(string par)
        {
           
        }

        public void ShowDialog(string v1, string message, string v2)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.Activity);
            builder.SetMessage(message).SetPositiveButton(v2, new EventHandler<DialogClickEventArgs>((arg1,arg2)=> { })).SetTitle(v1);

            Dialog d = builder.Create();
            d.Show();


            //AlertDialog.Builder alert = new AlertDialog.Builder(MainActivity.Activity);
            //alert.SetTitle(v1);
            //alert.SetMessage(message);
            //alert.SetPositiveButton(v2, (senderAlert, args) => {
            //    Toast.MakeText(MainActivity.Activity, "Deleted!", ToastLength.Short).Show();
            //});
            //Dialog dialog = alert.Create();
            //dialog.Show();
        }

        public void ShowDialog(string title, string message, string positive, string negative,Action<string> action)
        {
           
            AlertDialog.Builder alert = new AlertDialog.Builder(MainActivity.Activity);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton(positive, (senderAlert, args) =>
            {
                action("positive");
                //Toast.MakeText(MainActivity.Activity, "Deleted!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton(negative, (senderAlert, args) =>
            {
                action("negative");
                //Toast.MakeText(MainActivity.Activity, "Deleted!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}