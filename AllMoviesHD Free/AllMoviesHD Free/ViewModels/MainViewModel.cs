using HtmlAgilityPack;
using MoviesHD.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HttpControl;
using System.Windows.Input;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using VersionControl;
using MoviesHD.Models;
using Xamarin.Forms;
using Syncfusion.SfPullToRefresh.XForms;

namespace MoviesHD.ViewModel
{

    public class MainViewModel : PropertyChangedBase
    {
        #region Fields

       


        SimpleHttpControl shc = new SimpleHttpControl();

        public Movie PreviousMovie { get; set; }
        public CancellationTokenSource ts = new CancellationTokenSource();
        RateControl RControl { get; set; } = new RateControl(10);
        #endregion
        #region Propertyes
        private IAdmobInterstitial interstitial;

        public IAdmobInterstitial Interstitial
        {
            get { return interstitial; }
            set { interstitial = value; }
        }
        public Action<Movie> MovieTapped { get; set; }

        public CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        private SearchContext _SContext;
        public SearchContext SContext
        {
            get { return _SContext; }
            set { _SContext = value; OnPropertyChanged(); }
        }
       

        public ObservableCollection<Movie> TopMoviesResults { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> PremiereResults { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> LastAddedResults { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> NewEpisodesResults { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> TopTVShowsResults { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> FeaturedResults { get; set; } = new ObservableCollection<Movie>();


        public ObservableCollection<Country> Countries { get; set; } = new ObservableCollection<Country>();
        public ObservableCollection<CheckBoxGenre> Genres { get; set; } = new ObservableCollection<CheckBoxGenre>();
        public Command<object> RefreshListing { get; set; }
        private string _PolicySource;

        public string PolicySource
        {
            get { return _PolicySource; }
            set { _PolicySource = value; OnPropertyChanged(); }
        }

        private string _DMCASource;

        public string DMCASource
        {
            get { return _DMCASource; }
            set { _DMCASource = value; OnPropertyChanged(); }
        }


        private string _SearchQuery;
        public string SearchQuery
        {
            get { return _SearchQuery; }
            set { _SearchQuery = value; OnPropertyChanged(); }
        }


      

        private bool _Nothingfound;
        public bool Nothingfound
        {
            get { return _Nothingfound; }
            set { _Nothingfound = value; OnPropertyChanged(); }
        }

        private bool _IsPolicyAccepted;

        public bool IsPolicyAccepted
        {
            get { return _IsPolicyAccepted; }
            set { _IsPolicyAccepted = value; OnPropertyChanged(); }
        }


        private bool _IsRatingEnabled;
        public bool IsRatingEnabled
        {
            get { return _IsRatingEnabled; }
            set
            {
                _IsRatingEnabled = value; OnPropertyChanged(); if (!IsTVShow) SearchCriteryaChanged();
            }
        }

        private bool _IsTVShow;
        public bool IsTVShow
        {
            get { return _IsTVShow; }
            set { _IsTVShow = value; OnPropertyChanged(); if (value) { _IsRatingEnabled = false; OnPropertyChanged("IsRatingEnabled"); } SearchCriteryaChanged(); }
        }


        private bool _IsGenreEnabled;
        public bool IsGenreEnabled
        {
            get { return _IsGenreEnabled; }
            set { _IsGenreEnabled = value; OnPropertyChanged(); SearchCriteryaChanged(); }
        }


        private bool _IsCountryEnabled;
        public bool IsCountryEnabled
        {
            get { return _IsCountryEnabled; }
            set {
                _IsCountryEnabled = value; OnPropertyChanged(); SearchCriteryaChanged(); }
        }

        private bool _IsDateEnabled;
        public bool IsDateEnabled
        {
            get { return _IsDateEnabled; }
            set { _IsDateEnabled = value; OnPropertyChanged(); SearchCriteryaChanged(); }
        }

        private int _ImdbFilterStart;
        public int ImdbFilterStart
        {
            get { return _ImdbFilterStart; }
            set { _ImdbFilterStart = value; OnPropertyChanged(); }
        }

        private int _ImdbFilterEnd;
        public int ImdbFilterEnd
        {
            get { return _ImdbFilterEnd; }
            set { _ImdbFilterEnd = value; OnPropertyChanged(); }
        }

        private int _DateFilterStart;
        public int DateFilterStart
        {
            get { return _DateFilterStart; }
            set { _DateFilterStart = value; OnPropertyChanged(); }
        }

        private int _DateFilterEnd;
        public int DateFilterEnd
        {
            get { return _DateFilterEnd; }
            set { _DateFilterEnd = value; OnPropertyChanged(); }
        }

        private bool _FeaturesLoading;

        public bool FeaturesLoading
        {
            get { return _FeaturesLoading; }
            set { _FeaturesLoading = value; OnPropertyChanged(); }
        }


        private bool _TopMoviesLoading;
        public bool TopMoviesLoading
        {
            get { return _TopMoviesLoading; }
            set { _TopMoviesLoading = value; OnPropertyChanged(); }
        }

        private bool _PremiereLoading;
        public bool PremiereLoading
        {
            get { return _PremiereLoading; }
            set { _PremiereLoading = value; OnPropertyChanged(); }
        }

        private bool _LastAddedLoading;

        public bool LastAddedLoading
        {
            get { return _LastAddedLoading; }
            set { _LastAddedLoading = value; OnPropertyChanged(); }
        }

        private bool _NewEpisodesLoading;

        public bool NewEpisodesLoading
        {
            get { return _NewEpisodesLoading; }
            set { _NewEpisodesLoading = value; OnPropertyChanged(); }
        }

        private bool _TopTVShowsLoading;

        public bool TopTVShowsLoading
        {
            get { return _TopTVShowsLoading; }
            set { _TopTVShowsLoading = value; OnPropertyChanged(); }
        }

        private string _Policy;

        public string Policy
        {
            get { return _Policy; }
            set { _Policy = value; OnPropertyChanged(); }
        }


        private Command<object> _LoadMoreCommand;
        public Command<object> LoadMoreCommand
        {
            get
            {
                if (_LoadMoreCommand == null) _LoadMoreCommand = new Command<object>(LoadMore, (sender) => { if (SContext?.Url.Length > 0) return true; else return false; });
                return _LoadMoreCommand;
            }

        }

        public Action<DirectLink> link_DirectlinkDelegate;
        #endregion
        #region Constructors

        public StandartVersionControl sv { get; set; }
        public MainViewModel()
        {
            Policy = Encoding.UTF8.GetString(Convert.FromBase64String("PCFET0NUWVBFIGh0bWw+CiAgICA8aHRtbD4KICAgIDxoZWFkPgogICAgICA8bWV0YSBjaGFyc2V0PSd1dGYtOCc+CiAgICAgIDxtZXRhIG5hbWU9J3ZpZXdwb3J0JyBjb250ZW50PSd3aWR0aD1kZXZpY2Utd2lkdGgnPgogICAgICA8dGl0bGU+UHJpdmFjeSBQb2xpY3k8L3RpdGxlPgogICAgICA8c3R5bGU+IGJvZHkgeyBmb250LWZhbWlseTogJ0hlbHZldGljYSBOZXVlJywgSGVsdmV0aWNhLCBBcmlhbCwgc2Fucy1zZXJpZjsgcGFkZGluZzoxZW07IH0gPC9zdHlsZT4KICAgIDwvaGVhZD4KICAgIDxib2R5PgogICAgPGgyPlByaXZhY3kgUG9saWN5PC9oMj4gPHA+CiAgICAgICAgICAgICAgICAgICAgIGJ1aWx0IHRoZSBBbGxNb3ZpZXNIRCBhcHAgYXMKICAgICAgICAgICAgICAgICAgICBhIEZyZWUgYXBwLiBUaGlzIFNFUlZJQ0UgaXMgcHJvdmlkZWQgYnkKICAgICAgICAgICAgICAgICAgICAgYXQgbm8gY29zdCBhbmQgaXMgaW50ZW5kZWQgZm9yCiAgICAgICAgICAgICAgICAgICAgdXNlIGFzIGlzLgogICAgICAgICAgICAgICAgICA8L3A+IDxwPgogICAgICAgICAgICAgICAgICAgIFRoaXMgcGFnZSBpcyB1c2VkIHRvIGluZm9ybSB2aXNpdG9ycyByZWdhcmRpbmcKICAgICAgICAgICAgICAgICAgICBteSBwb2xpY2llcyB3aXRoIHRoZSBjb2xsZWN0aW9uLCB1c2UsIGFuZAogICAgICAgICAgICAgICAgICAgIGRpc2Nsb3N1cmUgb2YgUGVyc29uYWwgSW5mb3JtYXRpb24gaWYgYW55b25lIGRlY2lkZWQgdG8gdXNlCiAgICAgICAgICAgICAgICAgICAgbXkgU2VydmljZS4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD4KICAgICAgICAgICAgICAgICAgICBJZiB5b3UgY2hvb3NlIHRvIHVzZSBteSBTZXJ2aWNlLCB0aGVuIHlvdSBhZ3JlZQogICAgICAgICAgICAgICAgICAgIHRvIHRoZSBjb2xsZWN0aW9uIGFuZCB1c2Ugb2YgaW5mb3JtYXRpb24gaW4gcmVsYXRpb24gdG8gdGhpcwogICAgICAgICAgICAgICAgICAgIHBvbGljeS4gVGhlIFBlcnNvbmFsIEluZm9ybWF0aW9uIHRoYXQgSSBjb2xsZWN0IGlzCiAgICAgICAgICAgICAgICAgICAgdXNlZCBmb3IgcHJvdmlkaW5nIGFuZCBpbXByb3ZpbmcgdGhlIFNlcnZpY2UuCiAgICAgICAgICAgICAgICAgICAgSSB3aWxsIG5vdCB1c2Ugb3Igc2hhcmUgeW91cgogICAgICAgICAgICAgICAgICAgIGluZm9ybWF0aW9uIHdpdGggYW55b25lIGV4Y2VwdCBhcyBkZXNjcmliZWQgaW4gdGhpcyBQcml2YWN5CiAgICAgICAgICAgICAgICAgICAgUG9saWN5LgogICAgICAgICAgICAgICAgICA8L3A+IDxwPgogICAgICAgICAgICAgICAgICAgIFRoZSB0ZXJtcyB1c2VkIGluIHRoaXMgUHJpdmFjeSBQb2xpY3kgaGF2ZSB0aGUgc2FtZSBtZWFuaW5ncwogICAgICAgICAgICAgICAgICAgIGFzIGluIG91ciBUZXJtcyBhbmQgQ29uZGl0aW9ucywgd2hpY2ggaXMgYWNjZXNzaWJsZSBhdAogICAgICAgICAgICAgICAgICAgIEFsbE1vdmllc0hEIEZyZWUgdW5sZXNzIG90aGVyd2lzZSBkZWZpbmVkIGluIHRoaXMgUHJpdmFjeQogICAgICAgICAgICAgICAgICAgIFBvbGljeS4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkluZm9ybWF0aW9uIENvbGxlY3Rpb24gYW5kIFVzZTwvc3Ryb25nPjwvcD4gPHA+CiAgICAgICAgICAgICAgICAgICAgRm9yIGEgYmV0dGVyIGV4cGVyaWVuY2UsIHdoaWxlIHVzaW5nIG91ciBTZXJ2aWNlLAogICAgICAgICAgICAgICAgICAgIEkgbWF5IHJlcXVpcmUgeW91IHRvIHByb3ZpZGUgdXMgd2l0aCBjZXJ0YWluCiAgICAgICAgICAgICAgICAgICAgcGVyc29uYWxseSBpZGVudGlmaWFibGUgaW5mb3JtYXRpb24uIFRoZQogICAgICAgICAgICAgICAgICAgIGluZm9ybWF0aW9uIHRoYXQgSSByZXF1ZXN0IHdpbGwgYmUKICAgICAgICAgICAgICAgICAgICByZXRhaW5lZCBvbiB5b3VyIGRldmljZSBhbmQgaXMgbm90IGNvbGxlY3RlZCBieSBtZSBpbiBhbnkgd2F5LgogICAgICAgICAgICAgICAgICA8L3A+IDxwPgogICAgICAgICAgICAgICAgICAgIFRoZSBhcHAgZG9lcyB1c2UgdGhpcmQgcGFydHkgc2VydmljZXMgdGhhdCBtYXkgY29sbGVjdAogICAgICAgICAgICAgICAgICAgIGluZm9ybWF0aW9uIHVzZWQgdG8gaWRlbnRpZnkgeW91LgogICAgICAgICAgICAgICAgICA8L3A+IDxkaXY+PHA+CiAgICAgICAgICAgICAgICAgICAgICBMaW5rIHRvIHByaXZhY3kgcG9saWN5IG9mIHRoaXJkIHBhcnR5IHNlcnZpY2UgcHJvdmlkZXJzCiAgICAgICAgICAgICAgICAgICAgICB1c2VkIGJ5IHRoZSBhcHAKICAgICAgICAgICAgICAgICAgICA8L3A+IDx1bD48IS0tLS0+PCEtLS0tPjwhLS0tLT48IS0tLS0+PCEtLS0tPjwhLS0tLT48IS0tLS0+PCEtLS0tPjwhLS0tLT48IS0tLS0+PCEtLS0tPjwhLS0tLT48IS0tLS0+PCEtLS0tPjwvdWw+PC9kaXY+IDxwPjxzdHJvbmc+TG9nIERhdGE8L3N0cm9uZz48L3A+IDxwPgogICAgICAgICAgICAgICAgICAgIEkgd2FudCB0byBpbmZvcm0geW91IHRoYXQgd2hlbmV2ZXIKICAgICAgICAgICAgICAgICAgICB5b3UgdXNlIG15IFNlcnZpY2UsIGluIGEgY2FzZSBvZiBhbiBlcnJvciBpbiB0aGUKICAgICAgICAgICAgICAgICAgICBhcHAgSSBjb2xsZWN0IGRhdGEgYW5kIGluZm9ybWF0aW9uICh0aHJvdWdoIHRoaXJkCiAgICAgICAgICAgICAgICAgICAgcGFydHkgcHJvZHVjdHMpIG9uIHlvdXIgcGhvbmUgY2FsbGVkIExvZyBEYXRhLiBUaGlzIExvZyBEYXRhCiAgICAgICAgICAgICAgICAgICAgbWF5IGluY2x1ZGUgaW5mb3JtYXRpb24gc3VjaCBhcyB5b3VyIGRldmljZSBJbnRlcm5ldAogICAgICAgICAgICAgICAgICAgIFByb3RvY29sICjigJxJUOKAnSkgYWRkcmVzcywgZGV2aWNlIG5hbWUsIG9wZXJhdGluZyBzeXN0ZW0KICAgICAgICAgICAgICAgICAgICB2ZXJzaW9uLCB0aGUgY29uZmlndXJhdGlvbiBvZiB0aGUgYXBwIHdoZW4gdXRpbGl6aW5nCiAgICAgICAgICAgICAgICAgICAgbXkgU2VydmljZSwgdGhlIHRpbWUgYW5kIGRhdGUgb2YgeW91ciB1c2Ugb2YgdGhlCiAgICAgICAgICAgICAgICAgICAgU2VydmljZSwgYW5kIG90aGVyIHN0YXRpc3RpY3MuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+PHN0cm9uZz5Db29raWVzPC9zdHJvbmc+PC9wPiA8cD4KICAgICAgICAgICAgICAgICAgICBDb29raWVzIGFyZSBmaWxlcyB3aXRoIGEgc21hbGwgYW1vdW50IG9mIGRhdGEgdGhhdCBhcmUKICAgICAgICAgICAgICAgICAgICBjb21tb25seSB1c2VkIGFzIGFub255bW91cyB1bmlxdWUgaWRlbnRpZmllcnMuIFRoZXNlIGFyZQogICAgICAgICAgICAgICAgICAgIHNlbnQgdG8geW91ciBicm93c2VyIGZyb20gdGhlIHdlYnNpdGVzIHRoYXQgeW91IHZpc2l0IGFuZAogICAgICAgICAgICAgICAgICAgIGFyZSBzdG9yZWQgb24geW91ciBkZXZpY2UncyBpbnRlcm5hbCBtZW1vcnkuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+CiAgICAgICAgICAgICAgICAgICAgVGhpcyBTZXJ2aWNlIGRvZXMgbm90IHVzZSB0aGVzZSDigJxjb29raWVz4oCdIGV4cGxpY2l0bHkuCiAgICAgICAgICAgICAgICAgICAgSG93ZXZlciwgdGhlIGFwcCBtYXkgdXNlIHRoaXJkIHBhcnR5IGNvZGUgYW5kIGxpYnJhcmllcyB0aGF0CiAgICAgICAgICAgICAgICAgICAgdXNlIOKAnGNvb2tpZXPigJ0gdG8gY29sbGVjdCBpbmZvcm1hdGlvbiBhbmQgaW1wcm92ZSB0aGVpcgogICAgICAgICAgICAgICAgICAgIHNlcnZpY2VzLiBZb3UgaGF2ZSB0aGUgb3B0aW9uIHRvIGVpdGhlciBhY2NlcHQgb3IgcmVmdXNlCiAgICAgICAgICAgICAgICAgICAgdGhlc2UgY29va2llcyBhbmQga25vdyB3aGVuIGEgY29va2llIGlzIGJlaW5nIHNlbnQgdG8geW91cgogICAgICAgICAgICAgICAgICAgIGRldmljZS4gSWYgeW91IGNob29zZSB0byByZWZ1c2Ugb3VyIGNvb2tpZXMsIHlvdSBtYXkgbm90IGJlCiAgICAgICAgICAgICAgICAgICAgYWJsZSB0byB1c2Ugc29tZSBwb3J0aW9ucyBvZiB0aGlzIFNlcnZpY2UuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+PHN0cm9uZz5TZXJ2aWNlIFByb3ZpZGVyczwvc3Ryb25nPjwvcD4gPHA+CiAgICAgICAgICAgICAgICAgICAgSSBtYXkgZW1wbG95IHRoaXJkLXBhcnR5IGNvbXBhbmllcwogICAgICAgICAgICAgICAgICAgIGFuZCBpbmRpdmlkdWFscyBkdWUgdG8gdGhlIGZvbGxvd2luZyByZWFzb25zOgogICAgICAgICAgICAgICAgICA8L3A+IDx1bD48bGk+VG8gZmFjaWxpdGF0ZSBvdXIgU2VydmljZTs8L2xpPiA8bGk+VG8gcHJvdmlkZSB0aGUgU2VydmljZSBvbiBvdXIgYmVoYWxmOzwvbGk+IDxsaT5UbyBwZXJmb3JtIFNlcnZpY2UtcmVsYXRlZCBzZXJ2aWNlczsgb3I8L2xpPiA8bGk+VG8gYXNzaXN0IHVzIGluIGFuYWx5emluZyBob3cgb3VyIFNlcnZpY2UgaXMgdXNlZC48L2xpPjwvdWw+IDxwPgogICAgICAgICAgICAgICAgICAgIEkgd2FudCB0byBpbmZvcm0gdXNlcnMgb2YgdGhpcwogICAgICAgICAgICAgICAgICAgIFNlcnZpY2UgdGhhdCB0aGVzZSB0aGlyZCBwYXJ0aWVzIGhhdmUgYWNjZXNzIHRvIHlvdXIKICAgICAgICAgICAgICAgICAgICBQZXJzb25hbCBJbmZvcm1hdGlvbi4gVGhlIHJlYXNvbiBpcyB0byBwZXJmb3JtIHRoZSB0YXNrcwogICAgICAgICAgICAgICAgICAgIGFzc2lnbmVkIHRvIHRoZW0gb24gb3VyIGJlaGFsZi4gSG93ZXZlciwgdGhleSBhcmUgb2JsaWdhdGVkCiAgICAgICAgICAgICAgICAgICAgbm90IHRvIGRpc2Nsb3NlIG9yIHVzZSB0aGUgaW5mb3JtYXRpb24gZm9yIGFueSBvdGhlcgogICAgICAgICAgICAgICAgICAgIHB1cnBvc2UuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+PHN0cm9uZz5TZWN1cml0eTwvc3Ryb25nPjwvcD4gPHA+CiAgICAgICAgICAgICAgICAgICAgSSB2YWx1ZSB5b3VyIHRydXN0IGluIHByb3ZpZGluZyB1cwogICAgICAgICAgICAgICAgICAgIHlvdXIgUGVyc29uYWwgSW5mb3JtYXRpb24sIHRodXMgd2UgYXJlIHN0cml2aW5nIHRvIHVzZQogICAgICAgICAgICAgICAgICAgIGNvbW1lcmNpYWxseSBhY2NlcHRhYmxlIG1lYW5zIG9mIHByb3RlY3RpbmcgaXQuIEJ1dCByZW1lbWJlcgogICAgICAgICAgICAgICAgICAgIHRoYXQgbm8gbWV0aG9kIG9mIHRyYW5zbWlzc2lvbiBvdmVyIHRoZSBpbnRlcm5ldCwgb3IgbWV0aG9kCiAgICAgICAgICAgICAgICAgICAgb2YgZWxlY3Ryb25pYyBzdG9yYWdlIGlzIDEwMCUgc2VjdXJlIGFuZCByZWxpYWJsZSwgYW5kCiAgICAgICAgICAgICAgICAgICAgSSBjYW5ub3QgZ3VhcmFudGVlIGl0cyBhYnNvbHV0ZSBzZWN1cml0eS4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkxpbmtzIHRvIE90aGVyIFNpdGVzPC9zdHJvbmc+PC9wPiA8cD4KICAgICAgICAgICAgICAgICAgICBUaGlzIFNlcnZpY2UgbWF5IGNvbnRhaW4gbGlua3MgdG8gb3RoZXIgc2l0ZXMuIElmIHlvdSBjbGljawogICAgICAgICAgICAgICAgICAgIG9uIGEgdGhpcmQtcGFydHkgbGluaywgeW91IHdpbGwgYmUgZGlyZWN0ZWQgdG8gdGhhdCBzaXRlLgogICAgICAgICAgICAgICAgICAgIE5vdGUgdGhhdCB0aGVzZSBleHRlcm5hbCBzaXRlcyBhcmUgbm90IG9wZXJhdGVkIGJ5CiAgICAgICAgICAgICAgICAgICAgbWUuIFRoZXJlZm9yZSwgSSBzdHJvbmdseSBhZHZpc2UgeW91IHRvCiAgICAgICAgICAgICAgICAgICAgcmV2aWV3IHRoZSBQcml2YWN5IFBvbGljeSBvZiB0aGVzZSB3ZWJzaXRlcy4KICAgICAgICAgICAgICAgICAgICBJIGhhdmUgbm8gY29udHJvbCBvdmVyIGFuZCBhc3N1bWUgbm8KICAgICAgICAgICAgICAgICAgICByZXNwb25zaWJpbGl0eSBmb3IgdGhlIGNvbnRlbnQsIHByaXZhY3kgcG9saWNpZXMsIG9yCiAgICAgICAgICAgICAgICAgICAgcHJhY3RpY2VzIG9mIGFueSB0aGlyZC1wYXJ0eSBzaXRlcyBvciBzZXJ2aWNlcy4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkNoaWxkcmVu4oCZcyBQcml2YWN5PC9zdHJvbmc+PC9wPiA8cD4KICAgICAgICAgICAgICAgICAgICBUaGVzZSBTZXJ2aWNlcyBkbyBub3QgYWRkcmVzcyBhbnlvbmUgdW5kZXIgdGhlIGFnZSBvZiAxMy4KICAgICAgICAgICAgICAgICAgICBJIGRvIG5vdCBrbm93aW5nbHkgY29sbGVjdCBwZXJzb25hbGx5CiAgICAgICAgICAgICAgICAgICAgaWRlbnRpZmlhYmxlIGluZm9ybWF0aW9uIGZyb20gY2hpbGRyZW4gdW5kZXIgMTMuIEluIHRoZSBjYXNlCiAgICAgICAgICAgICAgICAgICAgSSBkaXNjb3ZlciB0aGF0IGEgY2hpbGQgdW5kZXIgMTMgaGFzIHByb3ZpZGVkCiAgICAgICAgICAgICAgICAgICAgbWUgd2l0aCBwZXJzb25hbCBpbmZvcm1hdGlvbiwKICAgICAgICAgICAgICAgICAgICBJIGltbWVkaWF0ZWx5IGRlbGV0ZSB0aGlzIGZyb20gb3VyIHNlcnZlcnMuIElmIHlvdQogICAgICAgICAgICAgICAgICAgIGFyZSBhIHBhcmVudCBvciBndWFyZGlhbiBhbmQgeW91IGFyZSBhd2FyZSB0aGF0IHlvdXIgY2hpbGQKICAgICAgICAgICAgICAgICAgICBoYXMgcHJvdmlkZWQgdXMgd2l0aCBwZXJzb25hbCBpbmZvcm1hdGlvbiwgcGxlYXNlIGNvbnRhY3QKICAgICAgICAgICAgICAgICAgICBtZSBzbyB0aGF0IEkgd2lsbCBiZSBhYmxlIHRvIGRvCiAgICAgICAgICAgICAgICAgICAgbmVjZXNzYXJ5IGFjdGlvbnMuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+PHN0cm9uZz5DaGFuZ2VzIHRvIFRoaXMgUHJpdmFjeSBQb2xpY3k8L3N0cm9uZz48L3A+IDxwPgogICAgICAgICAgICAgICAgICAgIEkgbWF5IHVwZGF0ZSBvdXIgUHJpdmFjeSBQb2xpY3kgZnJvbQogICAgICAgICAgICAgICAgICAgIHRpbWUgdG8gdGltZS4gVGh1cywgeW91IGFyZSBhZHZpc2VkIHRvIHJldmlldyB0aGlzIHBhZ2UKICAgICAgICAgICAgICAgICAgICBwZXJpb2RpY2FsbHkgZm9yIGFueSBjaGFuZ2VzLiBJIHdpbGwKICAgICAgICAgICAgICAgICAgICBub3RpZnkgeW91IG9mIGFueSBjaGFuZ2VzIGJ5IHBvc3RpbmcgdGhlIG5ldyBQcml2YWN5IFBvbGljeQogICAgICAgICAgICAgICAgICAgIG9uIHRoaXMgcGFnZS4gVGhlc2UgY2hhbmdlcyBhcmUgZWZmZWN0aXZlIGltbWVkaWF0ZWx5IGFmdGVyCiAgICAgICAgICAgICAgICAgICAgdGhleSBhcmUgcG9zdGVkIG9uIHRoaXMgcGFnZS4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkNvbnRhY3QgVXM8L3N0cm9uZz48L3A+IDxwPgogICAgICAgICAgICAgICAgICAgIElmIHlvdSBoYXZlIGFueSBxdWVzdGlvbnMgb3Igc3VnZ2VzdGlvbnMgYWJvdXQKICAgICAgICAgICAgICAgICAgICBteSBQcml2YWN5IFBvbGljeSwgZG8gbm90IGhlc2l0YXRlIHRvIGNvbnRhY3QKICAgICAgICAgICAgICAgICAgICBtZSBhdCBkZXZlbG9wZXJiaXZhbmlkemVAZ21haWwuY29tLgogICAgICAgICAgICAgICAgICA8L3A+IDxwPiAgICAgICAgICAgICAgICAgIAogICAgPC9ib2R5PgogICAgPC9odG1sPgogICAgICA="));
            Interstitial = DependencyService.Get<IAdmobInterstitial>();
            Interstitial.AdUnitId = "ca-app-pub-3909212246838265/8947795159";
            Interstitial.AdLoaded += Interstitial_AdLoaded;
            sv = new StandartVersionControl(1, "https://dl.dropboxusercontent.com/s/8k29y6xiseajst7/MoviesHD.txt");
            sv.NewVersionAvailable += Sv_NewVersionAvailable;
            PolicySource = Encoding.ASCII.GetString(Convert.FromBase64String("PCFET0NUWVBFIGh0bWw+CiAgICA8aHRtbD4KICAgIDxoZWFkPgogICAgICA8bWV0YSBjaGFyc2V0PSd1dGYtOCc+CiAgICAgIDxtZXRhIG5hbWU9J3ZpZXdwb3J0JyBjb250ZW50PSd3aWR0aD1kZXZpY2Utd2lkdGgnPgogICAgICA8dGl0bGU+UHJpdmFjeSBQb2xpY3k8L3RpdGxlPgogICAgICA8c3R5bGU+IGJvZHkgeyBmb250LWZhbWlseTogJ0hlbHZldGljYSBOZXVlJywgSGVsdmV0aWNhLCBBcmlhbCwgc2Fucy1zZXJpZjsgcGFkZGluZzoxZW07IH0gPC9zdHlsZT4KICAgIDwvaGVhZD4KICAgIDxib2R5PgogICAgPGgyPlByaXZhY3kgUG9saWN5PC9oMj4gPHA+IGJTb2Z0IFN0dWRpbyBidWlsdCB0aGUgQWxsTW92aWVzSEQgYXBwIGFzIGEgRnJlZSBhcHAuIFRoaXMgU0VSVklDRSBpcyBwcm92aWRlZCBieQogICAgICAgICAgICAgICAgICAgIGJTb2Z0IFN0dWRpbyBhdCBubyBjb3N0IGFuZCBpcyBpbnRlbmRlZCBmb3IgdXNlIGFzIGlzLgogICAgICAgICAgICAgICAgICA8L3A+IDxwPlRoaXMgcGFnZSBpcyB1c2VkIHRvIGluZm9ybSB2aXNpdG9ycyByZWdhcmRpbmcgbXkgcG9saWNpZXMgd2l0aCB0aGUgY29sbGVjdGlvbiwgdXNlLCBhbmQgZGlzY2xvc3VyZQogICAgICAgICAgICAgICAgICAgIG9mIFBlcnNvbmFsIEluZm9ybWF0aW9uIGlmIGFueW9uZSBkZWNpZGVkIHRvIHVzZSBteSBTZXJ2aWNlLgogICAgICAgICAgICAgICAgICA8L3A+IDxwPklmIHlvdSBjaG9vc2UgdG8gdXNlIG15IFNlcnZpY2UsIHRoZW4geW91IGFncmVlIHRvIHRoZSBjb2xsZWN0aW9uIGFuZCB1c2Ugb2YgaW5mb3JtYXRpb24gaW4KICAgICAgICAgICAgICAgICAgICByZWxhdGlvbiB0byB0aGlzIHBvbGljeS4gVGhlIFBlcnNvbmFsIEluZm9ybWF0aW9uIHRoYXQgSSBjb2xsZWN0IGlzIHVzZWQgZm9yIHByb3ZpZGluZyBhbmQgaW1wcm92aW5nCiAgICAgICAgICAgICAgICAgICAgdGhlIFNlcnZpY2UuIEkgd2lsbCBub3QgdXNlIG9yIHNoYXJlIHlvdXIgaW5mb3JtYXRpb24gd2l0aCBhbnlvbmUgZXhjZXB0IGFzIGRlc2NyaWJlZAogICAgICAgICAgICAgICAgICAgIGluIHRoaXMgUHJpdmFjeSBQb2xpY3kuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+VGhlIHRlcm1zIHVzZWQgaW4gdGhpcyBQcml2YWN5IFBvbGljeSBoYXZlIHRoZSBzYW1lIG1lYW5pbmdzIGFzIGluIG91ciBUZXJtcyBhbmQgQ29uZGl0aW9ucywgd2hpY2ggaXMKICAgICAgICAgICAgICAgICAgICBhY2Nlc3NpYmxlIGF0IEFsbE1vdmllc0hEIHVubGVzcyBvdGhlcndpc2UgZGVmaW5lZCBpbiB0aGlzIFByaXZhY3kgUG9saWN5LgogICAgICAgICAgICAgICAgICA8L3A+IDxwPjxzdHJvbmc+SW5mb3JtYXRpb24gQ29sbGVjdGlvbiBhbmQgVXNlPC9zdHJvbmc+PC9wPiA8cD5Gb3IgYSBiZXR0ZXIgZXhwZXJpZW5jZSwgd2hpbGUgdXNpbmcgb3VyIFNlcnZpY2UsIEkgbWF5IHJlcXVpcmUgeW91IHRvIHByb3ZpZGUgdXMgd2l0aCBjZXJ0YWluCiAgICAgICAgICAgICAgICAgICAgcGVyc29uYWxseSBpZGVudGlmaWFibGUgaW5mb3JtYXRpb24uIFRoZSBpbmZvcm1hdGlvbiB0aGF0IEkgcmVxdWVzdCB3aWxsIGJlIHJldGFpbmVkIG9uIHlvdXIgZGV2aWNlIGFuZCBpcyBub3QgY29sbGVjdGVkIGJ5IG1lIGluIGFueSB3YXkuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+VGhlIGFwcCBkb2VzIHVzZSB0aGlyZCBwYXJ0eSBzZXJ2aWNlcyB0aGF0IG1heSBjb2xsZWN0IGluZm9ybWF0aW9uIHVzZWQgdG8gaWRlbnRpZnkgeW91LjwvcD4gPGRpdj48cD5MaW5rIHRvIHByaXZhY3kgcG9saWN5IG9mIHRoaXJkIHBhcnR5IHNlcnZpY2UgcHJvdmlkZXJzIHVzZWQgYnkgdGhlIGFwcDwvcD4gPHVsPjxsaT48YSBocmVmPSJodHRwczovL3d3dy5nb29nbGUuY29tL3BvbGljaWVzL3ByaXZhY3kvIiB0YXJnZXQ9Il9ibGFuayI+R29vZ2xlIFBsYXkgU2VydmljZXM8L2E+PC9saT48bGk+PGEgaHJlZj0iaHR0cHM6Ly9zdXBwb3J0Lmdvb2dsZS5jb20vYWRtb2IvYW5zd2VyLzYxMjg1NDM/aGw9ZW4iIHRhcmdldD0iX2JsYW5rIj5BZE1vYjwvYT48L2xpPjxsaT48YSBocmVmPSJodHRwczovL2ZpcmViYXNlLmdvb2dsZS5jb20vcG9saWNpZXMvYW5hbHl0aWNzIiB0YXJnZXQ9Il9ibGFuayI+RmlyZWJhc2UgQW5hbHl0aWNzPC9hPjwvbGk+PCEtLS0tPjwhLS0tLT48IS0tLS0+PCEtLS0tPjwhLS0tLT48L3VsPjwvZGl2PiA8cD48c3Ryb25nPkxvZyBEYXRhPC9zdHJvbmc+PC9wPiA8cD4gSSB3YW50IHRvIGluZm9ybSB5b3UgdGhhdCB3aGVuZXZlciB5b3UgdXNlIG15IFNlcnZpY2UsIGluIGEgY2FzZSBvZgogICAgICAgICAgICAgICAgICAgIGFuIGVycm9yIGluIHRoZSBhcHAgSSBjb2xsZWN0IGRhdGEgYW5kIGluZm9ybWF0aW9uICh0aHJvdWdoIHRoaXJkIHBhcnR5IHByb2R1Y3RzKSBvbiB5b3VyIHBob25lCiAgICAgICAgICAgICAgICAgICAgY2FsbGVkIExvZyBEYXRhLiBUaGlzIExvZyBEYXRhIG1heSBpbmNsdWRlIGluZm9ybWF0aW9uIHN1Y2ggYXMgeW91ciBkZXZpY2UgSW50ZXJuZXQgUHJvdG9jb2wgKElQKSBhZGRyZXNzLAogICAgICAgICAgICAgICAgICAgIGRldmljZSBuYW1lLCBvcGVyYXRpbmcgc3lzdGVtIHZlcnNpb24sIHRoZSBjb25maWd1cmF0aW9uIG9mIHRoZSBhcHAgd2hlbiB1dGlsaXppbmcgbXkgU2VydmljZSwKICAgICAgICAgICAgICAgICAgICB0aGUgdGltZSBhbmQgZGF0ZSBvZiB5b3VyIHVzZSBvZiB0aGUgU2VydmljZSwgYW5kIG90aGVyIHN0YXRpc3RpY3MuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+PHN0cm9uZz5Db29raWVzPC9zdHJvbmc+PC9wPiA8cD5Db29raWVzIGFyZSBmaWxlcyB3aXRoIGEgc21hbGwgYW1vdW50IG9mIGRhdGEgdGhhdCBhcmUgY29tbW9ubHkgdXNlZCBhcyBhbm9ueW1vdXMgdW5pcXVlIGlkZW50aWZpZXJzLgogICAgICAgICAgICAgICAgICAgIFRoZXNlIGFyZSBzZW50IHRvIHlvdXIgYnJvd3NlciBmcm9tIHRoZSB3ZWJzaXRlcyB0aGF0IHlvdSB2aXNpdCBhbmQgYXJlIHN0b3JlZCBvbiB5b3VyIGRldmljZSdzIGludGVybmFsCiAgICAgICAgICAgICAgICAgICAgbWVtb3J5LgogICAgICAgICAgICAgICAgICA8L3A+IDxwPlRoaXMgU2VydmljZSBkb2VzIG5vdCB1c2UgdGhlc2UgY29va2llcyBleHBsaWNpdGx5LiBIb3dldmVyLCB0aGUgYXBwIG1heSB1c2UgdGhpcmQgcGFydHkgY29kZSBhbmQKICAgICAgICAgICAgICAgICAgICBsaWJyYXJpZXMgdGhhdCB1c2UgY29va2llcyB0byBjb2xsZWN0IGluZm9ybWF0aW9uIGFuZCBpbXByb3ZlIHRoZWlyIHNlcnZpY2VzLiBZb3UgaGF2ZSB0aGUgb3B0aW9uIHRvCiAgICAgICAgICAgICAgICAgICAgZWl0aGVyIGFjY2VwdCBvciByZWZ1c2UgdGhlc2UgY29va2llcyBhbmQga25vdyB3aGVuIGEgY29va2llIGlzIGJlaW5nIHNlbnQgdG8geW91ciBkZXZpY2UuIElmIHlvdSBjaG9vc2UKICAgICAgICAgICAgICAgICAgICB0byByZWZ1c2Ugb3VyIGNvb2tpZXMsIHlvdSBtYXkgbm90IGJlIGFibGUgdG8gdXNlIHNvbWUgcG9ydGlvbnMgb2YgdGhpcyBTZXJ2aWNlLgogICAgICAgICAgICAgICAgICA8L3A+IDxwPjxzdHJvbmc+U2VydmljZSBQcm92aWRlcnM8L3N0cm9uZz48L3A+IDxwPiBJIG1heSBlbXBsb3kgdGhpcmQtcGFydHkgY29tcGFuaWVzIGFuZCBpbmRpdmlkdWFscyBkdWUgdG8gdGhlIGZvbGxvd2luZyByZWFzb25zOjwvcD4gPHVsPjxsaT5UbyBmYWNpbGl0YXRlIG91ciBTZXJ2aWNlOzwvbGk+IDxsaT5UbyBwcm92aWRlIHRoZSBTZXJ2aWNlIG9uIG91ciBiZWhhbGY7PC9saT4gPGxpPlRvIHBlcmZvcm0gU2VydmljZS1yZWxhdGVkIHNlcnZpY2VzOyBvcjwvbGk+IDxsaT5UbyBhc3Npc3QgdXMgaW4gYW5hbHl6aW5nIGhvdyBvdXIgU2VydmljZSBpcyB1c2VkLjwvbGk+PC91bD4gPHA+IEkgd2FudCB0byBpbmZvcm0gdXNlcnMgb2YgdGhpcyBTZXJ2aWNlIHRoYXQgdGhlc2UgdGhpcmQgcGFydGllcyBoYXZlIGFjY2VzcyB0bwogICAgICAgICAgICAgICAgICAgIHlvdXIgUGVyc29uYWwgSW5mb3JtYXRpb24uIFRoZSByZWFzb24gaXMgdG8gcGVyZm9ybSB0aGUgdGFza3MgYXNzaWduZWQgdG8gdGhlbSBvbiBvdXIgYmVoYWxmLiBIb3dldmVyLAogICAgICAgICAgICAgICAgICAgIHRoZXkgYXJlIG9ibGlnYXRlZCBub3QgdG8gZGlzY2xvc2Ugb3IgdXNlIHRoZSBpbmZvcm1hdGlvbiBmb3IgYW55IG90aGVyIHB1cnBvc2UuCiAgICAgICAgICAgICAgICAgIDwvcD4gPHA+PHN0cm9uZz5TZWN1cml0eTwvc3Ryb25nPjwvcD4gPHA+IEkgdmFsdWUgeW91ciB0cnVzdCBpbiBwcm92aWRpbmcgdXMgeW91ciBQZXJzb25hbCBJbmZvcm1hdGlvbiwgdGh1cyB3ZSBhcmUgc3RyaXZpbmcKICAgICAgICAgICAgICAgICAgICB0byB1c2UgY29tbWVyY2lhbGx5IGFjY2VwdGFibGUgbWVhbnMgb2YgcHJvdGVjdGluZyBpdC4gQnV0IHJlbWVtYmVyIHRoYXQgbm8gbWV0aG9kIG9mIHRyYW5zbWlzc2lvbiBvdmVyCiAgICAgICAgICAgICAgICAgICAgdGhlIGludGVybmV0LCBvciBtZXRob2Qgb2YgZWxlY3Ryb25pYyBzdG9yYWdlIGlzIDEwMCUgc2VjdXJlIGFuZCByZWxpYWJsZSwgYW5kIEkgY2Fubm90IGd1YXJhbnRlZQogICAgICAgICAgICAgICAgICAgIGl0cyBhYnNvbHV0ZSBzZWN1cml0eS4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkxpbmtzIHRvIE90aGVyIFNpdGVzPC9zdHJvbmc+PC9wPiA8cD5UaGlzIFNlcnZpY2UgbWF5IGNvbnRhaW4gbGlua3MgdG8gb3RoZXIgc2l0ZXMuIElmIHlvdSBjbGljayBvbiBhIHRoaXJkLXBhcnR5IGxpbmssIHlvdSB3aWxsIGJlIGRpcmVjdGVkCiAgICAgICAgICAgICAgICAgICAgdG8gdGhhdCBzaXRlLiBOb3RlIHRoYXQgdGhlc2UgZXh0ZXJuYWwgc2l0ZXMgYXJlIG5vdCBvcGVyYXRlZCBieSBtZS4gVGhlcmVmb3JlLCBJIHN0cm9uZ2x5CiAgICAgICAgICAgICAgICAgICAgYWR2aXNlIHlvdSB0byByZXZpZXcgdGhlIFByaXZhY3kgUG9saWN5IG9mIHRoZXNlIHdlYnNpdGVzLiBJIGhhdmUgbm8gY29udHJvbCBvdmVyCiAgICAgICAgICAgICAgICAgICAgYW5kIGFzc3VtZSBubyByZXNwb25zaWJpbGl0eSBmb3IgdGhlIGNvbnRlbnQsIHByaXZhY3kgcG9saWNpZXMsIG9yIHByYWN0aWNlcyBvZiBhbnkgdGhpcmQtcGFydHkgc2l0ZXMKICAgICAgICAgICAgICAgICAgICBvciBzZXJ2aWNlcy4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkNoaWxkcmVucyBQcml2YWN5PC9zdHJvbmc+PC9wPiA8cD5UaGVzZSBTZXJ2aWNlcyBkbyBub3QgYWRkcmVzcyBhbnlvbmUgdW5kZXIgdGhlIGFnZSBvZiAxMy4gSSBkbyBub3Qga25vd2luZ2x5IGNvbGxlY3QKICAgICAgICAgICAgICAgICAgICBwZXJzb25hbGx5IGlkZW50aWZpYWJsZSBpbmZvcm1hdGlvbiBmcm9tIGNoaWxkcmVuIHVuZGVyIDEzLiBJbiB0aGUgY2FzZSBJIGRpc2NvdmVyIHRoYXQgYSBjaGlsZAogICAgICAgICAgICAgICAgICAgIHVuZGVyIDEzIGhhcyBwcm92aWRlZCBtZSB3aXRoIHBlcnNvbmFsIGluZm9ybWF0aW9uLCBJIGltbWVkaWF0ZWx5IGRlbGV0ZSB0aGlzIGZyb20KICAgICAgICAgICAgICAgICAgICBvdXIgc2VydmVycy4gSWYgeW91IGFyZSBhIHBhcmVudCBvciBndWFyZGlhbiBhbmQgeW91IGFyZSBhd2FyZSB0aGF0IHlvdXIgY2hpbGQgaGFzIHByb3ZpZGVkIHVzIHdpdGggcGVyc29uYWwKICAgICAgICAgICAgICAgICAgICBpbmZvcm1hdGlvbiwgcGxlYXNlIGNvbnRhY3QgbWUgc28gdGhhdCBJIHdpbGwgYmUgYWJsZSB0byBkbyBuZWNlc3NhcnkgYWN0aW9ucy4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD48c3Ryb25nPkNoYW5nZXMgdG8gVGhpcyBQcml2YWN5IFBvbGljeTwvc3Ryb25nPjwvcD4gPHA+IEkgbWF5IHVwZGF0ZSBvdXIgUHJpdmFjeSBQb2xpY3kgZnJvbSB0aW1lIHRvIHRpbWUuIFRodXMsIHlvdSBhcmUgYWR2aXNlZCB0byByZXZpZXcKICAgICAgICAgICAgICAgICAgICB0aGlzIHBhZ2UgcGVyaW9kaWNhbGx5IGZvciBhbnkgY2hhbmdlcy4gSSB3aWxsIG5vdGlmeSB5b3Ugb2YgYW55IGNoYW5nZXMgYnkgcG9zdGluZwogICAgICAgICAgICAgICAgICAgIHRoZSBuZXcgUHJpdmFjeSBQb2xpY3kgb24gdGhpcyBwYWdlLiBUaGVzZSBjaGFuZ2VzIGFyZSBlZmZlY3RpdmUgaW1tZWRpYXRlbHkgYWZ0ZXIgdGhleSBhcmUgcG9zdGVkIG9uCiAgICAgICAgICAgICAgICAgICAgdGhpcyBwYWdlLgogICAgICAgICAgICAgICAgICA8L3A+IDxwPjxzdHJvbmc+Q29udGFjdCBVczwvc3Ryb25nPjwvcD4gPHA+SWYgeW91IGhhdmUgYW55IHF1ZXN0aW9ucyBvciBzdWdnZXN0aW9ucyBhYm91dCBteSBQcml2YWN5IFBvbGljeSwgZG8gbm90IGhlc2l0YXRlIHRvIGNvbnRhY3QKICAgICAgICAgICAgICAgICAgICBtZS4KICAgICAgICAgICAgICAgICAgPC9wPiA8cD5UaGlzIHByaXZhY3kgcG9saWN5IHBhZ2Ugd2FzIGNyZWF0ZWQgYXQgPGEgaHJlZj0iaHR0cHM6Ly9wcml2YWN5cG9saWN5dGVtcGxhdGUubmV0IiB0YXJnZXQ9Il9ibGFuayI+cHJpdmFjeXBvbGljeXRlbXBsYXRlLm5ldDwvYT4KICAgICAgICAgICAgICAgICAgICBhbmQgbW9kaWZpZWQvZ2VuZXJhdGVkIGJ5IDxhIGhyZWY9Imh0dHBzOi8vYXBwLXByaXZhY3ktcG9saWN5LWdlbmVyYXRvci5maXJlYmFzZWFwcC5jb20vIiB0YXJnZXQ9Il9ibGFuayI+QXBwCiAgICAgICAgICAgICAgICAgICAgICBQcml2YWN5IFBvbGljeSBHZW5lcmF0b3I8L2E+PC9wPgogICAgPC9ib2R5PgogICAgPC9odG1sPg=="));
            DMCASource = Encoding.ASCII.GetString(Convert.FromBase64String("PGRpdiBkaXI9ImF1dG8iPjxzdHJvbmc+QWxsTW92aWVzSEQmbmJzcDs8L3N0cm9uZz5pcyBhbiBvbmxpbmUgd2Vic2l0ZSBzZXJ2aWNlIHByb3ZpZGVyIGxpa2UgZGVmaW5lZCBpbiBEaWdpdGFsIE1pbGxlbml1bSBDb3B5cmlnaHQgQWN0LiBXZSByZXNwZWN0IHRoZSBjb3B5cmlnaHQgbGF3cyBhbmQgd2lsbCBwcm90ZWN0IHRoZSByaWdodCBvZiBldmVyeSBjb3B5cmlnaHQgb3duZXIgc2VyaW91c2x5LiBJZiB5b3UgYXJlIHRoZSBvd25lciBvZiBhbnkgY29udGVudCBzaG93ZWQgb24gPGEgaHJlZj0iaHR0cDovL3B1dGxvY2tlcnMuZm0vIj5wdXRsb2NrZXJzJm5ic3A7PC9hPmFuZCB5b3UgZG9udCB3YW50IHRvIGFsbG93IHVzIHRvIHVzZSB0aGUgY29udGVudCwgdGhlbiB5b3UgYXJlIHZlcnkgYWJsZSB0byB0ZWxsIHVzIGJ5IGVtYWlsIHRvJm5ic3A7PHN0cm9uZz5kZXZlbG9wZXJiaXZhbmlkemVAZ21haWwuY29tPC9zdHJvbmc+Jm5ic3A7c28gdGhhdCB3ZSBjYW4gaWRlbnRpZnkgYW5kIHRha2UgbmVjZXNzYXJ5IGFjdGlvbi4gV2UgY2Fubm90IHRha2UgYW55IGFjdGlvbiBpZiB5b3UgZG9udCBnaXZlIHVzIGFueSBpbmZvcm1hdGlvbiBhYm91dCBpdCwgc28gcGxlYXNlIHNlbmQgdXMgYW4gZW1haWwgd2l0aCB0aGUgZGV0YWlscyBsaWtlOjxiciAvPjxiciAvPjwvZGl2Pgo8dWw+CjxsaSBkaXI9ImF1dG8iPlNwZWNpZmljYXRpb24gYWJvdXQgY29weXJpZ2h0IG9mIGNvbnRlbnQgd2hpY2ggY2xhaW1lZCB0byBiZSBpbmZyaW5nZWQsJm5ic3A7PC9saT4KPGxpIGRpcj0iYXV0byI+SWYgeW91IGNsYWltZWQgYWJvdXQgaW5mcmluZ2VtZW50IGZyb20gc29tZSBjb3B5cmlnaHQgd29ya3MgaW4gb25lIGVtYWlsLCBwbGVhc2Ugd3JpdGUgdGhlIGxpc3QgYWJvdXQgaXQgaW4gZGV0YWlsIGluY2x1ZGluZyB3ZWJzaXRlIHVybHMgY29udGFpbiBleGFjdCBjb250ZW50IHRoYXQgY2xhaW1lZCB0byBiZSBpbmZyaW5naW5nLDwvbGk+CjxsaSBkaXI9ImF1dG8iPkdpdmUgdXMgaW5mb3JtYXRpb24gYWJvdXQgeW91ciBuYW1lLCBwaG9uZSwgb2ZmaWNlIGFkZHJlc3MgYW5kIGVtYWlsIGFkZHJlc3MgYWxzbyB0byBhbGxvdyB1cyBjb250YWN0IHlvdSBpZiBuZWNjZXNzYXJ5LDwvbGk+CjxsaSBkaXI9ImF1dG8iPldlIHJlYWxseSBleHBlY3QgdGhhdCB0aGUgc2VuZGVyIHdvdWxkIGJlIHJlYWwgY29weXJpZ2h0IG93bmVyIGFuZCBub3QgdGhlIHRoaXJkIHBhcnR5IG9yIGFnZW50cyw8L2xpPgo8bGkgZGlyPSJhdXRvIj5JbmZvcm1hdGlvbiB3cml0dGVuIG11c3QgYmUgYWNjdXJhdGUgYW5kIHVuZGVyIHRoZSBsYXcgb2YgY291bnRlcmZlaXRpbmcuPC9saT4KPC91bD4KPHA+PHN0cm9uZz4mbmJzcDs8L3N0cm9uZz48L3A+"));
            DateFilterEnd = 2020;
            ImdbFilterStart = 0;
            ImdbFilterEnd = 10;
            Nothingfound = false;
            Countries.Add(new Country() { Name = "USA", ID = "125" });
            Countries.Add(new Country() { Name = "Great Britain", ID = "124" });
            Countries.Add(new Country() { Name = "France", ID = "123" });
            Countries.Add(new Country() { Name = "Germany", ID = "132" });
            Countries.Add(new Country() { Name = "Italy", ID = "135" });
            Countries.Add(new Country() { Name = "India", ID = "152" });
            Countries.Add(new Country() { Name = "Japan", ID = "133" });
            Countries.Add(new Country() { Name = "Korea", ID = "142" });
            Countries.Add(new Country() { Name = "Russia", ID = "139" });
            Countries.Add(new Country() { Name = "Turkey", ID = "336" });
            foreach (Genre g in Enum.GetValues(typeof(Genre)).Cast<Genre>()) Genres.Add(new CheckBoxGenre() { Genre = g, IsChecked = false });
            RefreshListing =
            new Command<object>(async (sender) =>
            {
                var ptf = sender as SfPullToRefresh;
                try
                {
                    ptf.IsRefreshing = true;
                    await SearchTopContents();
                }
                catch { }
                finally
                {
                    ptf.IsRefreshing = false;
                }
            });
            SearchTopContents();
        }

        private void Sv_NewVersionAvailable(NewVersionAvailableEventsArg arg, object sender)
        {
            
        }

        #endregion
        #region Methods
        private void Interstitial_AdLoaded()
        {
            
        }
        public async Task<List<Movie>> SearchQuick(CancellationToken token)
        {
            return await Task.Run(async () =>
             {
                 string url = "https://api.adjaranet.com/api/v1/search?filters%5Btype%5D=movie%2Ccast&keywords=" + System.Net.WebUtility.UrlEncode(SearchQuery) + "&source=adjaranet";
                 var res = await GetHtml(token, url);
                 token.ThrowIfCancellationRequested();
                 return await ParseMovies(res, token);
             });
        }
        public async Task Search(SearchContext context, CancellationToken token)
        {
            try
            {
                context.IsBusy = true;
                await Task.Run(async () =>
                {
                    token.ThrowIfCancellationRequested();
                    var res = await GetHtml(token, context.Url);
                    token.ThrowIfCancellationRequested();
                    var movies = await ParseMovies(res, token);
                    if (movies.Count > 0)
                    {
                        Nothingfound = false;
                        var match = Regex.Match(context.Url, "&page=(?<page>.*?)&");
                        int currentOffset;
                        if (match.Success && int.TryParse(match.Groups["page"].Value, out currentOffset))
                        {
                            currentOffset += 1;
                            context.Url = context.Url.Replace(match.Value, "&page=" + currentOffset.ToString() + "&");
                        }
                        else
                        {
                            context.Url = string.Empty;
                        }
                        await BeginInvokeOnMainThreadAsync(async () =>
                        {
                            foreach (var movie in movies)
                            {
                                try
                                {                                    
                                    context.Results.Add(movie);
                                }
                                catch
                                {
                                }
                            }
                        });
                    }
                    else
                    {

                    }
                });
            }
            catch (Exception e)
            {
                context.Url = string.Empty;

                throw e;
            }
            finally
            {
                if (context?.Results.Count == 0)
                {
                    Nothingfound = true;
                    SContext.Url = string.Empty;
                }
                else
                {

                }
                context.IsBusy = false;
            }

        }

        CancellationTokenSource topTS = new CancellationTokenSource();
        public async Task SearchTopContents()
        {
            topTS.Cancel();
            topTS = new CancellationTokenSource();
            await Task.Run(async () =>
            {
                List<Task> tasksToWait = new List<Task>();
                foreach (TopType type in Enum.GetValues(typeof(TopType)).Cast<TopType>())
                {
                    var t = SearchTopContent(type, topTS.Token);
                    await t;
                    tasksToWait.Add(t);
                }
                await Task.WhenAll(tasksToWait.ToArray());
                string allDone = "alldone";
            });
        }

        public async Task SearchTopContent(TopType type, CancellationToken token)
        {
            void SetTopMoviesprogress(bool value)
            {
                try
                {
                    switch (type)
                    {
                        
                        case TopType.Featured:
                            FeaturesLoading = value;
                            break;
                        case TopType.TopMovies:
                            TopMoviesLoading = value;
                            break;
                        case TopType.Premiere:
                            PremiereLoading = value;
                            break;
                        case TopType.LastAdded:
                            LastAddedLoading = value;
                            break;
                        case TopType.NewEpisodes:
                            NewEpisodesLoading = value;
                            break;
                        case TopType.TopTVShows:
                            TopTVShowsLoading = value;
                            break;
                        default:
                            break;
                    }
                }
                catch
                {

                }
            }
            await Task.Run(async () =>
            {
                try
                {

                    SetTopMoviesprogress(true);
                    string url = "";
                    switch (type)
                    {
                        case TopType.Featured:
                            {
                                url = "https://api.adjaranet.com/api/v1/movies/featured?source=adjaranet";
                                break;
                            }
                        case TopType.TopMovies:
                            url = "https://api.adjaranet.com/api/v1/movies/top?type=movie&period=day&page=1&per_page=20&filters%5Bwith_actors%5D=3&filters%5Bwith_directors%5D=1&source=adjaranet";
                            break;
                        case TopType.Premiere:
                            url = "https://api.adjaranet.com/api/v1/movies/premiere-day?page=1&per_page=20&filters=&source=adjaranet";
                            break;
                        case TopType.LastAdded:
                            url = "https://api.adjaranet.com/api/v1/movies?page=1&per_page=20&filters%5Bwith_files%5D=yes&filters%5Btype%5D=movie&filters%5Bwith_actors%5D=3&filters%5Bwith_directors%5D=1&sort=-upload_date&source=adjaranet";
                            break;
                        case TopType.NewEpisodes:
                            url = "https://api.adjaranet.com/api/v1/movies/latest-episodes?source=adjaranet";
                            break;
                        case TopType.TopTVShows:
                            url = "https://api.adjaranet.com/api/v1/movies/top?type=series&period=day&page=1&per_page=20&filters%5Bwith_actors%5D=3&filters%5Bwith_directors%5D=1&source=adjaranet";
                            break;
                    }
                    var res = await GetHtml(url: url, token: token);
                    var Results = await ParseMovies(res,token);                      
                        if (Results.Count > 0)
                        {
                            try
                            {
                                await BeginInvokeOnMainThreadAsync(() =>
                                {

                                    ObservableCollection<Movie> FillSource;
                                    switch (type)
                                    {
                                        case TopType.TopMovies:
                                            {
                                                FillSource = TopMoviesResults;
                                                break;
                                            }
                                        case TopType.Premiere:
                                            {
                                                FillSource = PremiereResults;
                                            }
                                            break;
                                        case TopType.LastAdded:
                                            {
                                                FillSource = LastAddedResults;
                                            }
                                            break;
                                        case TopType.NewEpisodes:
                                            {
                                                FillSource = NewEpisodesResults;
                                            }
                                            break;
                                        case TopType.TopTVShows:
                                            {
                                                FillSource = TopTVShowsResults;
                                            }
                                            break;
                                        case TopType.Featured:
                                            {
                                                FillSource = FeaturedResults;
                                            }
                                            break;
                                        default:
                                            {
                                                throw new Exception("No Res");
                                            }
                                    }
                                    FillSource.Clear();
                                    foreach (var mov in Results)
                                    {
                                        try
                                        {
                                            token.ThrowIfCancellationRequested();
                                            FillSource.Add(mov);
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                    }
                                });
                            }
                            catch
                            {

                            }

                        }
                    
                }
                catch (Exception ee)
                {

                }
                finally
                {
                    SetTopMoviesprogress(false);
                }
            });
        }
        public async Task<IEnumerable<DirectLink>> GetEpisodeDirectLinks(CancellationToken token, Movie mbase, Season season)
        {
            season.Updating = true;
            try
            {
                string res = await GetHtml(token, "https://api.adjaranet.com/api/v1/movies/" + season.MovieId + "/season-files/" + season.Number + "?source=adjaranet");
                var episodeSourceModel = EpisodeSourceModel.EpisodeSourceModel.FromJson(res);
                List<DirectLink> dlinks = new List<DirectLink>();
                foreach (var m in episodeSourceModel.Data)
                {
                    string title = m.Title;
                    string Episode = m.Episode.ToString() ;
                    var dlks = m.Files.Where(df => df.Lang?.ToLower() == "eng").SelectMany(df => df.Files.Select(f => new DirectLink(mbase) { Link = f.Src.AbsoluteUri, Name =  "Episode "+Episode.ToString()+", "+ title+" " +f.Quality }));
                    if(dlks.Count()==0)
                    {
                        dlks= m.Files.Where(df => df.Lang?.ToLower() != "geo").SelectMany(df => df.Files.Select(f => new DirectLink(mbase) { Link = f.Src.AbsoluteUri, Name = "Episode " + Episode.ToString() + ", " + title + " " + f.Quality }));
                    }
                    if (dlks.Count() == 0)
                    {
                        dlks = m.Files.SelectMany(df => df.Files.Select(f => new DirectLink(mbase) { Link = f.Src.AbsoluteUri, Name = "Episode " + Episode.ToString() + ", " + title + " " + f.Quality }));
                    }
                    dlinks.AddRange(dlks);
                }
                BeginInvokeOnMainThreadAsync(() => 
                {
                    season.Episodes = new ObservableCollection<DirectLink>(dlinks);
                });
                
                return dlinks;
            }
            finally
            {
                season.Updating = false;
            }
        }

        public async Task<Movie> UpdateMovie(Movie baseMovie, CancellationToken token)
        {
            return await Task.Run<Movie>(async () =>
            {
                //https://api.adjaranet.com/api/v1/movies/878413396?filters%5Bwith_directors%5D=3&source=adjaranet
                string res = await GetHtml(token, "https://api.adjaranet.com/api/v1/movies/" + (baseMovie.AdjaraID?.Length > 0 ? baseMovie.AdjaraID : baseMovie.ID) + "?filters%5Bwith_directors%5D=3&source=adjaranet");
                MovieSerialized mModel = null;
                try
                {
                    mModel = JsonConvert.DeserializeObject<MovieSerialized>(res);
                }
                catch (Exception e)
                {
                }
                baseMovie.ID = mModel.Data.id;
                baseMovie.AdjaraID = mModel.Data.adjaraId;
                if ((mModel.Data.originalName?.Length > 0 ? mModel.Data.originalName : mModel.Data.secondaryName) is string ns && ns?.Length < 0)
                {
                    baseMovie.title_en = ns;
                }
                foreach (var c in mModel.Data.Countries.Data)
                {
                    baseMovie.Countries.Add(new Country() { ID = c.Id.ToString(), Name = c.SecondaryName });
                }
                baseMovie.DescriptionEng = mModel.Data.Plots.Data.FirstOrDefault(d => d.Language == "ENG")?.Description;
                baseMovie.Directors = string.Join(", ", mModel.Data.Directors.Data.Select(dd => dd.OriginalName));
                baseMovie.Duration = mModel.Data.duration.ToString();
                baseMovie.Genres = string.Join(", ", mModel.Data.Genres.Data.Select(g => g.SecondaryName));
                baseMovie.poster = (mModel.Data.poster == string.Empty) ? baseMovie.poster : mModel.Data.poster;
                baseMovie.imdb = mModel.Data.Rating.Imdb.Score.ToString();
                baseMovie.ImdbUrl = mModel.Data.ImdbUrl;
                baseMovie.imdb_votes = mModel.Data.Rating.Imdb.Voters.ToString();


                baseMovie.Seasons = new ObservableCollection<Season>(mModel.Data.Seasons.Data.Select(sd => new Season() { EpisodeCount = sd.EpisodesCount.ToString(), MovieId = sd.MovieId.ToString(), Name = sd.Name, Number = sd.Number.ToString() }));
                return baseMovie;



                throw new Exception("Could not get movie");
            });
        }

        SearchContext CreateContext()
        {

            var context = new SearchContext(IsTVShow, SearchQuery,
                IsRatingEnabled ? new RatingRange(ImdbFilterStart, ImdbFilterEnd) : null,
                IsDateEnabled ? new YearRange(DateFilterStart, DateFilterEnd) : null,
                IsCountryEnabled ? Countries.Where((citem) => { return citem.IsSelected; }).ToList() : null,
                IsGenreEnabled ? Genres.Where((chGTenre) =>
                {
                    if (chGTenre.IsChecked)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }).Select(chGenre => chGenre.Genre).ToList() : null);

            return context;
        }

        public async Task SearchCriteryaChanged()
        {
            try
            {
                var context = CreateContext();
                SContext = context;
                CTS.Cancel();
                CTS = new CancellationTokenSource();
                await Search(SContext, CTS.Token);
            }
            catch (OperationCanceledException oce)
            {

            }
            catch(HttpRequestException hre)
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error", "Couldn't connect to the server", "Ok");
            }
            catch(Exception ee)
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error",ee.Message, "Ok");
            }
           
        }

        public async void LoadMore(object sender = null)
        {
            try
            {
                if (SContext.IsBusy) return;
                await Search(SContext, CTS.Token);
                if (SContext.Results?.Count > 0)
                {
                    Random r = new Random();
                    if (r.Next(0, 10) == 1)
                    {
                        //BeginInvokeOnMainThreadAsync(() =>
                        //{
                        //    try
                        //    {
                        //        if (Interstitial == null) return;
                        //        if (Interstitial.IsLoading() || interstitial.IsLoaded())
                        //        {
                        //        }
                        //        else
                        //        {
                        //            Interstitial.Load();
                        //        }
                        //    }
                        //    catch
                        //    {

                        //    }                         

                        //});

                    }
                    else
                    {
                        BeginInvokeOnMainThreadAsync(() =>
                        {
                            RControl.ShowRateDialog();
                        });

                    }
                }
            }
            catch (OperationCanceledException oce)
            {

            }
            catch (HttpRequestException hre)
            {
                DependencyService.Get<IAlertDialog>().ShowDialog("Error", "Couldn't connect to the server", "Ok");
            }
            catch (Exception ee)
            {

            }
        }
        string GetIMDbIDFromUrl(string Url)
        {
            //https://www.imdb.com/title/tt10016704

            var match = Regex.Match(Url, "https://www.imdb.com/title/(?<id>tt.*)");
            if (match.Groups["id"]?.Success ?? false)
            {
                return match.Groups["id"].Value;
            }
            else
            {
                throw new Exception("No id");
            }

        }
        async Task<Movie> ParseMovieData(JObject j)
        {
            Movie m = new Movie();
            m.releaseDate = j["year"]?.ToString();
            m.title_en = j["originalName"]?.ToString();
            if(!(m.title_en?.Length>0)) m.title_en = j["secondaryName"]?.ToString();
            m.title_ge = j["primaryName"]?.ToString();
            m.ID = j["id"].ToString();
            m.AdjaraID = j["adjaraId"].ToString();
            var cs = j["countries"]?["data"]?.Select(cT => new Country() { ID = cT["id"].ToString(), Name = cT["secondaryName"].ToString() });
            if(cs!=null) m.Countries = new ObservableCollection<Country>(cs);
            if(m.Countries.Any(c=>c.Name?.ToLower()=="turkey"))
            {
                throw new Exception("Turkey shit!");
            }
            if (j["rating"] is JObject ratingsObj && ratingsObj["imdb"] is JObject imdbObj && imdbObj["score"] is JValue scoreValue)
            {
                m.imdb = scoreValue.ToString();
            }
            m.link = "https://www.adjaranet.com/Movie/main?id=" + (m.AdjaraID?.Length > 0 ? m.AdjaraID : m.ID);
            //m.poster = j["poster"].ToString();
            //m.imdb = j["imdb"]?.ToString();

            if (j["posters"] is JToken pToken && pToken["data"] is JToken posters && posters["240"] is JValue poster)
            {
                m.poster = poster.Value.ToString();
            }
            m.ImdbUrl = j["imdbUrl"]?.ToString();
           
            m.Duration = j["duration"]?.ToString();
            return m;
        }
        private async Task<List<Movie>> ParseMovies(string res, CancellationToken token)
        {
            List<Movie> retValue = new List<Movie>();
            JObject ob = JObject.Parse(res);
            if (ob["data"] is JArray moviesjData)
            {
                foreach (var moviejData in moviesjData)
                {


                    if (moviejData is JObject j)
                    {
                        if (j["movies"] is JObject mobject && mobject["data"] is JArray moviesjData2)
                        {
                            foreach(var moviejData2 in moviesjData2)
                            {
                                if (moviejData2 is JObject j2)
                                {
                                    try
                                    {
                                        var ms = await ParseMovieData(j2);
                                        retValue.Add(ms);
                                    }
                                    catch
                                    {}
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                var ms = await ParseMovieData(j);
                                retValue.Add(ms);
                            }
                            catch 
                            {
                            }
                        }
                    }
                    if (token.IsCancellationRequested) break;

                }
            }
            return retValue;
        }
        public async Task UpdateIMDb(Movie mb, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                try
                {
                    string link = "https://api.themoviedb.org/3/find/" + mb.imdb_id + "?api_key=07824c019b81ecf7ad094a66f6410cc9&language=en-US&external_source=imdb_id";
                    string jsonRes = await GetHtml(token, link, host: "no", referer: "no");
                    var jobject = JObject.Parse(jsonRes);
                    if (jobject["movie_results"] is JArray arr)
                    {
                        foreach (var resOb in arr)
                        {
                            if (resOb is JObject obj)
                            {
                                mb.DescriptionEng = obj["overview"]?.ToString();
                                mb.title_en = obj["original_title"]?.ToString();
                                if (obj["genre_ids"] is JArray genresArr)
                                {
                                    await BeginInvokeOnMainThreadAsync(() =>
                                    {
                                        List<Genre> genres = new List<Genre>();
                                        foreach (var genrObj in genresArr)
                                        {
                                            try
                                            {
                                                themoviedbGenres g = (themoviedbGenres)Convert.ToInt32(genrObj.ToString());
                                                Genre fg;
                                                if (Enum.TryParse<Genre>(g.ToString().Replace("_", ""), out fg))
                                                {
                                                    genres.Add(fg);
                                                }

                                            }
                                            catch
                                            {

                                            }
                                        }
                                        StringBuilder strb = new StringBuilder();
                                        if (genres.Count > 0)
                                        {
                                            strb.Append(" | ");
                                            foreach (Genre g in genres)
                                            {
                                                strb.Append(g.ToString());
                                                strb.Append(" | ");
                                            }
                                            mb.Genres = strb.ToString();
                                        }
                                    });
                                }
                            }
                        }
                    }
                    if (jobject["tv_results"] is JArray tvarr)
                    {
                        foreach (var resOb in tvarr)
                        {
                            if (resOb is JObject obj)
                            {
                                mb.DescriptionEng = obj["overview"]?.ToString();
                                mb.title_en = (obj["original_title"] ?? obj["original_name"])?.ToString();
                                if (obj["genre_ids"] is JArray genresArr)
                                {
                                    await BeginInvokeOnMainThreadAsync(() =>
                                    {
                                        List<Genre> genres = new List<Genre>();
                                        foreach (var genrObj in genresArr)
                                        {
                                            try
                                            {
                                                themoviedbGenres g = (themoviedbGenres)Convert.ToInt32(genrObj.ToString());
                                                Genre fg;
                                                if (Enum.TryParse<Genre>(g.ToString().Replace("_", ""), out fg))
                                                {
                                                    genres.Add(fg);
                                                }

                                            }
                                            catch
                                            {

                                            }
                                        }
                                        StringBuilder strb = new StringBuilder();
                                        if (genres.Count > 0)
                                        {
                                            strb.Append(" | ");
                                            foreach (Genre g in genres)
                                            {
                                                strb.Append(g.ToString());
                                                strb.Append(" | ");
                                            }
                                            mb.Genres = strb.ToString();
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
                catch
                {

                }


            });
        }
        private async Task<string> GetHtml(CancellationToken token, string url, string referer = null, string accept = null, string host = null)
        {
            return await Task.Run(async () =>
            {
                var uri = new Uri(url);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true };
                if (host != "no") client.DefaultRequestHeaders.Add("Host", uri.Host);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Pragma", "no-cache");
                client.DefaultRequestHeaders.Add("User-Agent", Strings.UserAgent);
                client.DefaultRequestHeaders.Add("Accept", accept ?? "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                //if (referer != "no") client.DefaultRequestHeaders.Add("Referer", referer ?? "http:/adjaranet.com/Search");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, sdch");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8,ru;q=0.4");

                var bres = (await (await client.GetAsync(uri, token)).Content.ReadAsByteArrayAsync());
                url = string.Empty;

                string res = string.Empty;
                //try
                //{
                //    var decoded = Decompress(bres);
                //    res = Encoding.ASCII.GetString(decoded);
                //}
                //catch
                //{
                //    res = Encoding.ASCII.GetString(bres);
                //}
                res = Encoding.UTF8.GetString(bres);
                return res;
            });
        }
        private async Task UpdateSize(DirectLink link, string referer = null, string accept = null, string host = null)
        {
            await Task.Run(async () =>
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { NoCache = true };
                    if (host != "no") client.DefaultRequestHeaders.Add("Host", "net.adjara.com");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("Pragma", "no-cache");
                    client.DefaultRequestHeaders.Add("User-Agent", Strings.UserAgent);
                    client.DefaultRequestHeaders.Add("Accept", accept ?? "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    if (referer != "no") client.DefaultRequestHeaders.Add("Referer", referer ?? "http://net.adjara.com/Search");
                    client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, sdch");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8,ru;q=0.4");

                    HttpRequestMessage rm = new HttpRequestMessage(HttpMethod.Head, new Uri(link.Link));
                    var res = await client.SendAsync(rm, HttpCompletionOption.ResponseHeadersRead);
                    if (res.IsSuccessStatusCode)
                    {
                        link.Size = (long)res.Content.Headers.ContentLength;
                    }

                }
                catch
                {

                }
            });
        }
        byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip),
                CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
        public Task BeginInvokeOnMainThreadAsync(Action a)
        {
            var tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    a();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
        public Task BeginInvokeOnMainThreadAsync(Func<Task> a)
        {
            var tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await a();
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }
        #endregion
    }
}
