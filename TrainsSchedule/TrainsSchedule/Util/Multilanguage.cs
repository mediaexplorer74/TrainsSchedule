using Android.App;
using Android.Content.Res;
using Java.Util;
using TrainsSchedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms.Platform.Android;

namespace TrainsSchedule.Util
{
    public class Multilanguage
    {
        public static string app_name;
        public static string favorites;
        public static string _in;
        public static string min;
        public static string h;
        public static string express;
        public static string onTomorrow;
        public static string onToday;
        public static string enterStation;
        public static string stationNotFound;
        public static string on;
        public static string addedToFav;
        public static string nonstop;
        public static string lang;
        public static void Init(Activity activity)
        {
            Resources res = activity.GetResource();
            lang = Locale.GetDefault().GetLanguage();
            app_name = res.GetString(R.@string.app_name);
            favorites = res.GetString(R.@string.favorites);
            @_in = res.GetString(R.@string.@in);
            min = res.GetString(R.@string.min);
            h = res.GetString(R.@string.hr);
            express = res.GetString(R.@string.express);
            onToday = res.GetString(R.@string.onToday);
            onTomorrow = res.GetString(R.@string.onTomorrow);
            enterStation = res.GetString(R.@string.enterStation);
            stationNotFound = res.GetString(R.@string.stationNotFound);
            addedToFav = res.GetString(R.@string.addedToFav);
            nonstop = res.GetString(R.@string.nonStop);
            on = res.GetString(R.@string.on);
        }
    }
}