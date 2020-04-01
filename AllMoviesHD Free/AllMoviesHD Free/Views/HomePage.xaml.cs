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
    public partial class HomePage : ContentPage
    {
        MainViewModel mainviewmodel;
        public HomePage()
        {
            InitializeComponent();
            mainviewmodel = this.BindingContext as MainViewModel; 
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void MovieTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
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