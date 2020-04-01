using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoviesHD.Model
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName = null)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
                }
                catch (Exception ex)
                {
                }
            });
            
           
           
            
        }
    }
}
