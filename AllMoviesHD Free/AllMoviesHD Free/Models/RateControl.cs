using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoviesHD.Models
{
    public class RateControl
    {
        ISettings localsettings = CrossSettings.Current;
        bool isRated = false;
        int RateCount;
        public RateControl(int ratecount)
        {
            try
            {
                RateCount = ratecount;

                var ratvalue = localsettings.GetValueOrDefault("isRated", "NotRated");
                if (ratvalue == "NotRated") isRated = false;
                if (ratvalue == "Rated") isRated = true;
            }
            catch (Exception ee)
            {

            }
        }

        public async Task ShowRateDialogImediate()
        {
            try
            {
                DependencyService.Get<IShowRateDialog>().ShowRateDialog();
                localsettings.AddOrUpdateValue("isRated", "Rated");
            }
            catch (Exception)
            {

            }
        }
        public async Task ShowRateDialog()
        {
            try
            {
                if (isRated == false)
                {
                    int RateCounter = localsettings.GetValueOrDefault("RateCounter",0);
                   
                    if (RateCounter > RateCount)
                    {
                        localsettings.AddOrUpdateValue("RateCounter", 0);

                        DependencyService.Get<IAlertDialog>().ShowDialog("Information", "If you like our app please rate or review it and help us provide a better service.","Rate now","Not now",new Action<string>((arg)=> 
                        {
                            switch(arg)
                            {
                                case "positive":
                                    {
                                        localsettings.AddOrUpdateValue("isRated","Rated");
                                        isRated = true;
                                        DependencyService.Get<IShowRateDialog>().ShowRateDialog();
                                        break;
                                    }
                                case "negative":
                                    {
                                        
                                        break;
                                    }
                            }
                        }));
                    }
                    else
                    {
                        RateCounter += 1;
                        localsettings.AddOrUpdateValue("RateCounter", RateCounter);
                    }
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
