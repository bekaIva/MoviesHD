using MoviesHD.Model;
using MoviesHD.Models;
using MoviesHD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesHD.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        MainViewModel mvm = (MainViewModel)Application.Current.Resources["MainViewModel"];
        Movie mBase;
        Random Rand = new Random();
        CancellationTokenSource ts;
        public PlayerPage()
        {
            InitializeComponent();
        }
        public PlayerPage(Movie movie)
        {
            InitializeComponent();
            mBase = movie;
            this.BindingContext = movie;
            Init();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private void BanerLoaded()
        {
            AdRow.Height = new GridLength(60, GridUnitType.Absolute);
        }

        private void BanerLoadFailed(string obj)
        {
            AdRow.Height = new GridLength(0, GridUnitType.Absolute);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Player.Release();
        }
        async Task Init()
        {
            try
            {
                ts?.Cancel();
                ts = new CancellationTokenSource();
                mBase.Updating = true;
                await mvm.UpdateMovie(mBase, ts.Token);
                if (!((mvm?.Interstitial?.IsLoading() ?? true) || (mvm?.Interstitial?.IsLoaded() ?? true)))
                {
                    mvm?.Interstitial?.Load();
                }
                else
                {

                }
                if (mBase.Seasons.Count == 0) throw new Exception("Couldn't retrieve streams!");
                StartupPlay();
            }
            catch (Exception e)
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error", e.Message, "Ok");
            }
            finally
            {
                mBase.Updating = false;
            }
        }
        async Task StartupPlay()
        {
            try
            {
                var dlinks = await GetDirectLinks(mBase.Seasons.First());
                if (dlinks.Count() > 0)
                {
                    await DirectLinkTapped(dlinks.First());
                }
                else
                {
                    throw new Exception("Failed to retrieve streams!");
                }
            }
            catch(Exception e) 
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error", e.Message, "Ok");
            }
        }
        public async Task DirectLinkTapped(DirectLink dlink)
        {




            Player.Play(dlink.Link, dlink.Movie.link);



        }
        public async Task<IEnumerable<DirectLink>> GetDirectLinks(Season s)
        {

            ts?.Cancel();
            ts = new CancellationTokenSource();
            return await mvm.GetEpisodeDirectLinks(ts.Token, mBase, s);


        }
        private void Player_DoubleClick()
        {
            if (Player.IsFullScreen)
            {
                Shell.SetNavBarIsVisible(this, false);
                PlayerRow.Height = new GridLength(1, GridUnitType.Star);
                DetailsRow.Height = new GridLength(0, GridUnitType.Star);
                AdRow.Height = new GridLength(0, GridUnitType.Absolute);
                DependencyService.Get<IStatusBar>().HideStatusBar();
            }
            else
            {
                Shell.SetNavBarIsVisible(this, true);
                PlayerRow.Height = new GridLength(1, GridUnitType.Star);
                DetailsRow.Height = new GridLength(1, GridUnitType.Star);
                DependencyService.Get<IStatusBar>().ShowStatusBar();
                AdRow.Height = new GridLength(55, GridUnitType.Absolute);
            }
        }

        private void Player_SeekProcessed()
        {
            try
            {
                if (Rand.Next(0, 6) == 1)
                {
                    if (!((mvm?.Interstitial?.IsLoading() ?? true) || (mvm?.Interstitial?.IsLoaded() ?? true)))
                    {
                        mvm?.Interstitial?.Load();
                    }
                    else
                    {

                    }
                }
            }
            catch
            {
            }
        }

        private void Player_PlayerError(Exception obj)
        {
            DependencyService.Get<IAlertDialog>().ShowDialog("Error", obj.Message, "Ok");
        }

        private void FullScreenButtonClick(bool obj)
        {
            if (Player.IsFullScreen)
            {
                Shell.SetNavBarIsVisible(this, false);
                PlayerRow.Height = new GridLength(1, GridUnitType.Star);
                DetailsRow.Height = new GridLength(0, GridUnitType.Star);
                AdRow.Height = new GridLength(0, GridUnitType.Absolute);
                DependencyService.Get<IStatusBar>().HideStatusBar();
            }
            else
            {
                Shell.SetNavBarIsVisible(this, true);
                PlayerRow.Height = new GridLength(1, GridUnitType.Star);
                DetailsRow.Height = new GridLength(1, GridUnitType.Star);
                DependencyService.Get<IStatusBar>().ShowStatusBar();
                AdRow.Height = new GridLength(55, GridUnitType.Absolute);
            }
        }

        private async void SeasonTapped(object sender, EventArgs e)
        {
            try
            {
                Season s = (sender as BindableObject)?.BindingContext as Season;
                var direclinks = await GetDirectLinks(s);
                if (direclinks.Count() == 0) throw new Exception("Couldn't retrieve streams!");
            }
            catch (Exception ee)
            {
                
            }
        }

        private async void EpisodeTapped(object sender, EventArgs e)
        {
            try
            {
                DirectLink dlink = (DirectLink)((Grid)sender).BindingContext;
                await DirectLinkTapped(dlink);




            }
            catch (Exception ee)
            {
                
            }
        }
    }
}