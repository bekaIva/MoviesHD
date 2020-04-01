
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesHD.Model
{
     
    public class Movie:PropertyChangedBase
    {
        private bool _Updating;

        public bool Updating
        {
            get { return _Updating; }
            set { _Updating = value; OnPropertyChanged(); }
        }

        public string listViewName;
        public string ID { get; set; }

        public string AdjaraID { get; set; }

        private string _Genres;

        public string Genres
        {
            get { return _Genres; }
            set { _Genres = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Actor> _Actors;
        public ObservableCollection<Actor> Actors
        {
            get { return _Actors; }
            set { _Actors = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Country> _Countries;
        public ObservableCollection<Country> Countries
        {
            get { return _Countries; }
            set { _Countries = value; OnPropertyChanged(); }
        }

        private string _Directors;
        public string Directors
        {
            get { return _Directors; }
            set { _Directors = value; OnPropertyChanged(); }
        }

        public string Referer { get; set; }

        private string _title_en;
        public string title_en
        {
            get { return _title_en; }
            set { _title_en = value; OnPropertyChanged(); }
        }

        private string _title_ge;
        public string title_ge
        {
            get { return _title_ge; }
            set { _title_ge = value; OnPropertyChanged(); }
        }

        public string link { get; set; }

        private string _poster;
        public string poster
        {
            get { return _poster; }
            set { _poster = value; OnPropertyChanged(); }
        }




        private string _imdb;
        public string imdb
        {
            get { return _imdb; }
            set { _imdb = value; OnPropertyChanged(); }
        }

        private string _releaseDate;

        public string releaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; OnPropertyChanged(); }
        }


        private string _imdb_id;
        public string imdb_id
        {
            get { return _imdb_id; }
            set { _imdb_id = value; OnPropertyChanged(); }
        }

        private string _ImdbUrl;

        public string ImdbUrl
        {
            get { return _ImdbUrl; }
            set { _ImdbUrl = value; OnPropertyChanged(); }
        }


        private string _imdb_votes;
        public string imdb_votes
        {
            get { return _imdb_votes; }
            set { _imdb_votes = value; OnPropertyChanged(); }
        }

        private string _DescriptionEng;
        public string DescriptionEng
        {
            get { return _DescriptionEng; }
            set { _DescriptionEng = value; OnPropertyChanged(); }
        }

        private string _DescriptionGeo;
        public string DescriptionGeo
        {
            get { return _DescriptionGeo; }
            set { _DescriptionGeo = value; OnPropertyChanged(); }
        }

        private string _Duration;
        public string Duration
        {
            get { return _Duration; }
            set { _Duration = value; OnPropertyChanged(); }
        }
        private ObservableCollection<Season> _Seasons;

        public ObservableCollection<Season> Seasons
        {
            get { return _Seasons; }
            set { _Seasons = value; OnPropertyChanged(); }
        }

        public Movie()
        {
            Seasons = new ObservableCollection<Season>();
            Actors = new ObservableCollection<Actor>();
            Countries = new ObservableCollection<Country>();
        }
    }
    public class Season : PropertyChangedBase
    {
        private bool _Updating;

        public bool Updating
        {
            get { return _Updating; }
            set { _Updating = value; OnPropertyChanged(); }
        }

        public string MovieId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string EpisodeCount { get; set; }
        private string _SeasonName;
        public string SeasonName
        {
            get { return _SeasonName; }
            set { _SeasonName = value; OnPropertyChanged(); }
        }


        private ObservableCollection<DirectLink> _Episodes;

        public ObservableCollection<DirectLink> Episodes
        {
            get { return _Episodes; }
            set { _Episodes = value; OnPropertyChanged(); }
        }


    }
    public class Episode :PropertyChangedBase
    {
        private ObservableCollection<DirectLink> _DirectLinks;

        public ObservableCollection<DirectLink> DirectLinks
        {
            get { return _DirectLinks; }
            set { _DirectLinks = value; OnPropertyChanged(); }
        }

    }
    public class DirectLink : PropertyChangedBase
    {
        public Movie Movie { get; set; }
        private string _Link;

        public string Link
        {
            get { return _Link; }
            set { _Link = value; OnPropertyChanged(); }
        }

        private long _Size;
        public long Size
        {
            get { return _Size; }
            set { _Size = value; OnPropertyChanged(); }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }
        public DirectLink(Movie baseMovie)
        {
            Movie = baseMovie;
        }
    }
    public class Actor : PropertyChangedBase
    {
        private string _Actor_Eng;
        public string Actor_Eng
        {
            get { return _Actor_Eng; }
            set { _Actor_Eng = value; OnPropertyChanged(); }
        }


        private string _Actor_Ka;
        public string Actor_Ka
        {
            get { return _Actor_Ka; }
            set { _Actor_Ka = value; OnPropertyChanged(); }
        }
        public string Link { get; set; }
        public string Id { get; set; }

    }
    public class Director : PropertyChangedBase
    {
        private string _Name_Eng;
        public string Name_Eng
        {
            get { return _Name_Eng; }
            set { _Name_Eng = value; OnPropertyChanged(); }
        }

        private string _Name_Geo;
        public string Name_Geo
        {
            get { return _Name_Geo; }
            set { _Name_Geo = value; OnPropertyChanged(); }
        }

        public string ImdbUrl { get; set; }


    }
    public class Country : PropertyChangedBase
    {
        public string Name { get; set; }
        public string ID { get; set; }
        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; OnPropertyChanged(); }
        }
        public Country()
        {
            IsSelected = false;
        }
    }

    public class CheckBoxGenre : PropertyChangedBase
    {
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set { _IsChecked = value; OnPropertyChanged(); }
        }
        public Genre Genre { get; set; }
    }

    public class RatingRange
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public RatingRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
    public class YearRange
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public YearRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

  



    public class SearchContext : PropertyChangedBase
    {
        private ObservableCollection<Movie> _Results;
        public ObservableCollection<Movie> Results
        {
            get { return _Results; }
            set { _Results = value; OnPropertyChanged(); }
        }


        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                _IsBusy = value;
                OnPropertyChanged();
            }
        }

        string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }

        public SearchContext(bool istvshow=false, string keyword = null, RatingRange ratingrange = null, YearRange yearRange = null, List<Country> countries = null, List<Genre> genres = null)
        {


          
            
            Results = new ObservableCollection<Movie>();
            if (keyword != null && keyword!= string.Empty)
            {
                Url = "https://api.adjaranet.com/api/v1/search-advanced?";
                string genresString = "movie_filters%5Bgenre%5D=";
                if (genres?.Count > 0)
                {
                    foreach (Genre g in genres)
                    {
                        genresString += (int)g;
                        if (!genres[genres.Count - 1].Equals(g)) genresString += "%2C";
                    }
                    Url += genresString;
                }
                Url += "movie_filters%5Bwith_actors%5D=3&movie_filters%5Bwith_directors%5D=1";
                if(istvshow)
                {

                }
                string keywordString = "&keywords=" + keyword.Replace(" ", "%20");
                Url += keywordString;
                Url += "&page=1&per_page=15&source=adjaranet";
            }
            else
            {
                Url = "https://api.adjaranet.com/api/v1/movies?page=1&per_page=15&filters%5Btype%5D=movie&filters%5Blanguage%5D=ENG";

                if (yearRange == null)
                {
                    Url += "&filters%5Byear_range%5D=1900%2C2019";
                    
                }
                else
                {
                    Url += "&filters%5Byear_range%5D=" + yearRange.Min + "%2C" + yearRange.Max;
                    

                }
               



                string genresString = "&filters%5Bgenre%5D=";
                if (genres?.Count > 0)
                {
                    foreach (Genre g in genres)
                    {
                        genresString += (int)g;
                        if (!genres[genres.Count - 1].Equals(g)) genresString += "%2C";
                    }
                    Url += genresString;
                }

                string ctrs = "";
                if (countries?.Count > 0)
                {
                    ctrs = "&filters%5Bcountry%5D=";
                    foreach (var country in countries)
                    {
                        if (country.IsSelected)
                        {
                            ctrs += country.ID;
                            if (!countries[countries.Count - 1].Equals(country)) ctrs += "%2C";
                        }
                    }
                    url += ctrs;
                }

                if (ratingrange != null)
                {
                    Url += "&filters%5Bimdb_rating_range%5D="+ratingrange.Min+ "%2C"+ratingrange.Max;
                }
                else
                {

                }

                Url += "&filters%5Binit%5D=true&filters%5Badvanced%5D=false&filters%5Bwith_actors%5D=3&filters%5Bwith_directors%5D=1&filters%5Bwith_files%5D=yes&sort=-upload_date&source=adjaranet";


               
            }

           
            




            //string episode = "";
            //switch (type)
            //{
            //    case MovieType.Movie:
            //        {
            //            episode = "&episode=0";
            //            break;
            //        }
            //    case MovieType.TVShow:
            //        {
            //            episode = "&episode=1";
            //            break;
            //        }
            //}

            
            url += "movie_filters%5Bwith_actors%5D=3&movie_filters%5Bwith_directors%5D=1";
            if(istvshow)
            {
                url += "&filters%5Btype%5D=series";
            }
            else
            {
                url += "&filters%5Btype%5D=movie";
            }
            //if (genresString?.Length > 0) Url += genresString;

           
            Url += "&page=1&per_page=15&source=adjaranet";
            //else
            //{

            //}
            //string order = "";
            //if (yearRange == null)
            //{
            //    Url += "&startYear=1900&endYear=2018";
            //    order = "&offset=0&isnew=0&needtags=0&orderBy=date&order%5Border%5D=data&order%5Bdata%5D=published&language=english";
            //}
            //else
            //{
            //    Url += "&startYear=" + yearRange.Min + "&endYear=" + yearRange.Max;
            //    order = "&offset=0&isnew=0&needtags=0&orderBy=date&order%5Border%5D=data&order%5Bdata%5D=published&language=english";

            //}


            //string ctrs = "";
            //if (countries?.Count > 0)
            //{
            //    ctrs = "&country=";
            //    foreach (var country in countries)
            //    {
            //        if (country.IsSelected)
            //        {
            //            ctrs += country.ID;
            //            if (!countries[countries.Count - 1].Equals(country)) ctrs += ",";
            //        }
            //    }
            //}
            //else
            //{
            //    ctrs = "&country=false";
            //}

            //if (ratingrange != null)
            //{
            //    order = "&offset=0&isnew=0&needtags=0&orderBy=date&order%5Border%5D=data&order%5Bdata%5D=rating&order%5Bstart%5D=" + ratingrange.Min + "&order%5Bend%5D=" + ratingrange.Max + "&order%5Bmeta%5D=imdb&language=english";
            //}
            //else
            //{

            //}
            //Url += order;
            //Url += ctrs;
            //Url += keywordString;
            //Url += "&game=0&softs=0&videos=0&xvideos=0&vvideos=0&dvideos=0&xphotos=0&vphotos=0&dphotos=0&trailers=0" + episode + "&tvshow=0&flashgames=0";

        }

        #region OldContext
        //public SearchContext(MovieType type, string keyword = null, RatingRange ratingrange = null, YearRange yearRange = null, List<Country> countries = null, List<Genre> genres = null)
        //{


        //    Results = new ObservableCollection<Movie>();

        //    string genresString = "";
        //    if (genres?.Count > 0)
        //    {

        //        foreach (Genre g in genres)
        //        {
        //            genresString += "&searchTags%5B%5D=" + (int)g;
        //        }
        //    }

        //    string episode = "";
        //    switch (type)
        //    {
        //        case MovieType.Movie:
        //            {
        //                episode = "&episode=0";
        //                break;
        //            }
        //        case MovieType.TVShow:
        //            {
        //                episode = "&episode=1";
        //                break;
        //            }
        //    }

        //    Url = "http://net.adjara.com/Search/SearchResults?ajax=1";
        //    if (genresString?.Length > 0) Url += genresString;
        //    Url += "&display=15";
        //    string keywordString = "";
        //    if (keyword != null)
        //    {
        //        keywordString = "&keyword=" + keyword.Replace(" ", "%20");
        //    }
        //    else
        //    {

        //    }
        //    string order = "";
        //    if (yearRange == null)
        //    {
        //        Url += "&startYear=1900&endYear=2018";
        //        order = "&offset=0&isnew=0&needtags=0&orderBy=date&order%5Border%5D=data&order%5Bdata%5D=published&language=english";
        //    }
        //    else
        //    {
        //        Url += "&startYear=" + yearRange.Min + "&endYear=" + yearRange.Max;
        //        order = "&offset=0&isnew=0&needtags=0&orderBy=date&order%5Border%5D=data&order%5Bdata%5D=published&language=english";

        //    }


        //    string ctrs = "";
        //    if (countries?.Count > 0)
        //    {
        //        ctrs = "&country=";
        //        foreach (var country in countries)
        //        {
        //            if (country.IsSelected)
        //            {
        //                ctrs += country.ID;
        //                if (!countries[countries.Count - 1].Equals(country)) ctrs += ",";
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ctrs = "&country=false";
        //    }

        //    if (ratingrange != null)
        //    {
        //        order = "&offset=0&isnew=0&needtags=0&orderBy=date&order%5Border%5D=data&order%5Bdata%5D=rating&order%5Bstart%5D=" + ratingrange.Min + "&order%5Bend%5D=" + ratingrange.Max + "&order%5Bmeta%5D=imdb&language=english";
        //    }
        //    else
        //    {

        //    }
        //    Url += order;
        //    Url += ctrs;
        //    Url += keywordString;
        //    Url += "&game=0&softs=0&videos=0&xvideos=0&vvideos=0&dvideos=0&xphotos=0&vphotos=0&dphotos=0&trailers=0" + episode + "&tvshow=0&flashgames=0";

        //} 
        #endregion

    }
}
