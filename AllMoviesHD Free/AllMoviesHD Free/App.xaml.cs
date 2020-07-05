using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MoviesHD.Views;
using Xamarin.Forms.Internals;
using System.Diagnostics;

namespace MoviesHD
{
    public class Strings
    {
        public static string EmptyBody { get; set; } = "<html><head></head><body></body></html>";
        public static string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36";
    }
    public partial class App : Application
    {

        public App()
        {
            Xamarin.Forms.Internals.Log.Listeners.Add(new DelegateLogListener((arg1, arg2) => Debug.WriteLine(arg2)));
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjc1NTQ1QDMxMzgyZTMxMmUzMEV6cVlSTlZzSkwyM0piWFAvSWc5U3JLdjQrS3RrNjAxUEpXdjJPeDBZRGc9");
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
