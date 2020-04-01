using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Admob.Droid;
using MoviesHD.Models;
using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdMobSquareView), typeof(AdMobSquareViewRenderer))]
namespace Admob.Droid
{
    public class AdMobSquareViewRenderer : ViewRenderer<AdMobSquareView, AdView>
    {
        public AdMobSquareViewRenderer(Context context) : base(context) { }

      
        protected override void OnElementChanged(ElementChangedEventArgs<AdMobSquareView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null && Control == null)
                SetNativeControl(CreateAdView());
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(AdView.AdUnitId))
                Control.AdUnitId = Element.AdUnitId;
        }
        private AdView CreateAdView()
        {
            var adView = new AdView(Context)
            {
                AdSize = AdSize.MediumRectangle,
                AdUnitId = Element.AdUnitId
            };            
            adView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
         
            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;
        }
    }
}