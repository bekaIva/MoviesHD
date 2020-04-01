using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoviesHD.Models
{
    public interface IAdmobInterstitial
    {
        event Action AdLoaded;
        void Show();
        string AdUnitId { get; set; }
        void Load();
        bool IsLoaded();
        bool IsLoading();
    }
  
    public interface IAlertDialog
    {
       
        void ShowDialog(string v1, string message, string v2);
        void ShowDialog(string title, string message, string positive, string negative, Action<string> action);
    }
    public interface IShowRateDialog
    {
        void ShowRateDialog();
    }
    public interface IStatusBar
    {
        void HideStatusBar();
        void ShowStatusBar();
    }
    public class GetImageSource : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var isource = ImageSource.FromFile(Source);
            return isource;
        }
    }
}
