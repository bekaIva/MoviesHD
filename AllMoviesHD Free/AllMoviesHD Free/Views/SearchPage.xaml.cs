using MoviesHD.Model;
using MoviesHD.Models;
using MoviesHD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesHD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        MainViewModel mainviewmodel;
        public SearchPage()
        {
            InitializeComponent();
            mainviewmodel = this.BindingContext as MainViewModel;
            mainviewmodel.SContext = new SearchContext();
           
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override void OnAppearing()
        {
            //<models:IronSourceBanner x:Name="isBanner" Grid.Row="0" Grid.RowSpan="2" BanerAdLoaded="BanerLoaded" BanerAdLoadFailed="BanerLoadFailed"  WidthRequest="330" HeightRequest="60" VerticalOptions="End"></models:IronSourceBanner>
            base.OnAppearing();

        }
        private void GenreCheckboxChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            mainviewmodel?.SearchCriteryaChanged();
        }

        private void RatingSliderChanging(object sender, Syncfusion.SfRangeSlider.XForms.RangeEventArgs e)
        {
            mainviewmodel?.SearchCriteryaChanged();
        }

        private void CountryCheckboxChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            mainviewmodel?.SearchCriteryaChanged();
        }

        private void DateSliderChanging(object sender, Syncfusion.SfRangeSlider.XForms.RangeEventArgs e)
        {
            mainviewmodel?.SearchCriteryaChanged();
        }

        private async void MovieItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            if (mainviewmodel.sv.IsNewVersionAvailable)
            {
                var res = await DisplayAlert("Update required!", mainviewmodel.sv.Message, "Update", "Cancel");
                if (res)
                {
                    Launcher.TryOpenAsync(mainviewmodel.sv.UpdateUrl);
                }
                return;
            }

            Navigation.PushAsync(new PlayerPage(e.ItemData as Movie));
        }

        private void ResultsListview_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {

        }

        private void BanerLoaded()
        {
            BanerRow.Height = new GridLength(60, GridUnitType.Absolute);
        }

        private void BanerLoadFailed(string obj)
        {
            BanerRow.Height = new GridLength(0, GridUnitType.Absolute);
        }
    }
}