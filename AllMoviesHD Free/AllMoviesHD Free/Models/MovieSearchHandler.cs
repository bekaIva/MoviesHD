using MoviesHD.Model;
using MoviesHD.ViewModel;
using MoviesHD.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xamarin.Forms;
namespace MoviesHD.Models
{
    public class MovieSearchHandler:SearchHandler
    {
        public MainViewModel ViewModel { get; set; }
        public Page CurrentPage { get; set; }
        long lastTick;
        CancellationTokenSource ts;
        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            try
            {
                base.OnQueryChanged(oldValue, newValue);
                if (DateTime.Now.Ticks - lastTick > 3000)
                {
                    ts?.Cancel();
                    ts = new CancellationTokenSource();
                    lastTick = DateTime.Now.Ticks;
                    ViewModel.SearchQuick(ts.Token).ContinueWith((res)=> 
                    {
                        ViewModel.BeginInvokeOnMainThreadAsync(() => 
                        {
                            this.ItemsSource = res.Result;
                        });                        
                    });
                    
                }
                
            }
            catch 
            {
                lastTick = DateTime.Now.Ticks;
            }
            finally {  }
        }
        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            CurrentPage.Navigation.PushAsync(new PlayerPage(item as Movie));
        }
        protected override async void OnQueryConfirmed()
        {
            try
            {
                base.OnQueryConfirmed();
                await ViewModel.SearchCriteryaChanged();
            }
            catch(HttpRequestException hre)
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error", "Couldn't connect to the server", "Ok");
            }
            catch (Exception ee)
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error", ee.Message, "Ok");
            }
        }
    }
}
