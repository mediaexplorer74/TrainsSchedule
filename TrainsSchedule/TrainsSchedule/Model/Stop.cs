using Java.Text;
using Java.Util;
using Java.Util.Concurrent;
using TrainsSchedule.DTO;
using TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Model
{
    public class Stop
    {
        public int dayOfWeek;
        public string fromStation;
        public string toStation;
        public int arrival; //in minutes
        public int departure; //in minutes
        public string duration;
        public string route;
        public string exclude;
        public bool express;
        public virtual void Fill(Station from, Station to, int dayOfWeek, string departure, string arrival, int duration, string route, string exclude, string express)
        {
            this.fromStation = from.code;
            this.toStation = to.code;
            this.dayOfWeek = dayOfWeek;
            this.departure = GetTimeInt(departure);
            this.arrival = GetTimeInt(arrival);
            if (exclude == null || exclude.IsEmpty() || exclude.EqualsIgnoreCase("везде"))
            {
                this.exclude = "";
            }
            else if (exclude.EqualsIgnoreCase("без остановок"))
            {
                this.exclude = Multilanguage.nonstop;
            }
            else
                this.exclude = exclude.Substring(0, 1).ToUpperCase() + exclude.Substring(1);
            if (express == null)
            {
                this.express = false;
                this.route = route;
            }
            else
            {
                this.express = true;
                this.route = route + " (" + Multilanguage.express + ")";
            }

            this.duration = DurationStr(duration);
        }

        private int GetTimeInt(string inStr)
        {

            //2016-01-25 01:07:00 -> 1*60+7 = 67
            DateFormat format = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.ENGLISH);
            Calendar c = Calendar.GetInstance();
            try
            {
                c.SetTime(format.Parse(inStr));
            }
            catch (ParseException e)
            {
                e.PrintStackTrace();
            }

            return c[Calendar.HOUR_OF_DAY] * 60 + c[Calendar.MINUTE];
        }

        private static string DurationStr(int seconds)
        {
            long hr = TimeUnit.SECONDS.ToHours(seconds);
            long sec = seconds - hr * 3600;
            long min = TimeUnit.SECONDS.ToMinutes(sec);
            if (hr == 0)
                return "" + min + " " + Multilanguage.min;
            return "" + hr + " " + Multilanguage.h + " " + min + " " + Multilanguage.min;
        }

        public class List
        {
            private List<Stop> stops;
            public List(List<Stop> stops)
            {
                if (stops == null)
                {
                    stops = new List();
                }

                this.stops = stops;
            }

            public virtual List<Stop> GetStops()
            {
                return stops;
            }

            public virtual int Size()
            {
                return stops.Count;
            }

            public virtual Stop Get(int position)
            {
                return stops[position];
            }

            public virtual void NewStops(IList stopsList)
            {
                stops.Clear();
                foreach (Stop stop in stopsList.stops)
                {
                    stops.Add(stop);
                }
            }

            public virtual void AddStops(IList stopsList)
            {
                foreach (Stop stop in stopsList.stops)
                {
                    stops.Add(stop);
                }
            }
        }
    }
}