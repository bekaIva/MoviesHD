using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MoviesHD.Models
{
    public class AdMobSquareView : Xamarin.Forms.View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
                       nameof(AdUnitId),
                       typeof(string),
                       typeof(AdMobView),
                       string.Empty);

        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }

    }
    public class AdMobView : Xamarin.Forms.View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
                       nameof(AdUnitId),
                       typeof(string),
                       typeof(AdMobView),
                       string.Empty);

        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }


        public static readonly BindableProperty AdUnitHeightProperty = BindableProperty.Create("AdUnitHeight", typeof(int), typeof(AdMobView), 0);
       
        public int AdUnitHeight
        {
            get { return (int)GetValue(AdUnitHeightProperty); }
            set { SetValue(AdUnitIdProperty, value); }
        }

        public static readonly BindableProperty AdUnitWidthProperty = BindableProperty.Create("AdUnitHeight", typeof(int), typeof(AdMobView), 0);

        public int AdUnitWidth
        {
            get { return (int)GetValue(AdUnitWidthProperty); }
            set { SetValue(AdUnitWidthProperty, value); }
        }

    }
    public class AdControlView : Xamarin.Forms.View
    {
        
    }
}
