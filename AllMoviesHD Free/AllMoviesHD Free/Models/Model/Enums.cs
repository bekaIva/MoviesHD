using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesHD.Model
{
    public enum MovieType
    {
        Movie,
        TVShow
    }
    public enum Genre
    {
        Action = 248,
        Adventure = 266,
        Animation = 265,
        Biography = 253,
        Comedy = 258,
        Crime = 259,
        Documentary = 252,
        Drama = 249,
        Family = 263,
        Fantasy = 261,
        History = 264,
        Horror = 255,
        Music = 262,
        Musical = 268,
        Mystery = 256,
        Romance = 260,
        Sci_Fi = 257,
        Sport = 254,
        Thriller = 250,
        War = 251,
        Western = 267
    }
    enum themoviedbGenres
    {
        Action = 28,
        Adventure = 12,
        Animation = 16,
        Comedy = 35,
        Crime = 80,
        Documentary = 99,
        Drama = 18,
        Family = 10751,
        Fantasy = 14,
        History = 36,
        Horror = 27,
        Music = 10402,
        Mystery = 9648,
        Romance = 10749,
        Science_Fiction = 878,
        TV_Movie = 10770,
        Thriller = 53,
        War = 10752,
        Western = 37
    }
    public enum TopType
    {
        Featured,


        TopMovies,
        Premiere,
        LastAdded,
        NewEpisodes,
        TopTVShows
    }
}
