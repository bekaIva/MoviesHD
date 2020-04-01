using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoviesHD.Droid;
using MoviesHD.Models;
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
[assembly: Xamarin.Forms.Dependency(typeof(AdMobInterstitialDependency))]
namespace MoviesHD.Droid
{
    class AdMobInterstitialDependency : IAdmobInterstitial
    {
        private string _AdUnitId;

        public string AdUnitId
        {
            get { return _AdUnitId; }
            set { _AdUnitId = value; ad.AdUnitId = value; }
        }

        InterstitialAd ad;

        public event Action AdLoaded;

        public AdMobInterstitialDependency()
        {
            ad = new InterstitialAd(MainActivity.Activity);
            ad.RewardedVideoAdLoaded += Ad_RewardedVideoAdLoaded;
            ad.RewardedVideoAdClosed += Ad_RewardedVideoAdClosed;
        }

        private void Ad_RewardedVideoAdClosed(object sender, EventArgs e)
        {
            
        }

        private void Ad_RewardedVideoAdLoaded(object sender, EventArgs e)
        {
            AdLoaded?.Invoke();
        }

        public void Load()
        {
            ad.LoadAd(new AdRequest.Builder().Build());
        }

        public void Show()
        {
            if(ad.IsLoaded)
            {
                ad.Show();
            }
        }

        public bool IsLoaded()
        {
            return ad.IsLoaded;
        }

        public bool IsLoading()
        {
            return ad.IsLoading;
        }
    }
}