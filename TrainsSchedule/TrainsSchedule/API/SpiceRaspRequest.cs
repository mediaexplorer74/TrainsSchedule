using Com.Octo.Android.Robospice.Request.Retrofit;
using TrainsSchedule.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.API
{
    public class SpiceRaspRequest : RetrofitSpiceRequest<RaspRequest, YandexRaspAPI>
    {
        private string from;
        private string to;
        private string date;
        public int page;
        public SpiceRaspRequest(string from, string to, string date, int page) : base(typeof(RaspRequest), typeof(YandexRaspAPI))
        {
            this.from = from;
            this.to = to;
            this.date = date;
            this.page = page;
        }

        public override RaspRequest LoadDataFromNetwork()
        {
            StopsList stopsList;
            if (page == 1)
            {
                stopsList = GetService().Search(from, to, date);
            }
            else
            {
                stopsList = GetService().Search(from, to, date, page);
            }

            return new RaspRequest(stopsList, page);
        }

        public virtual string CreateCacheKey()
        {
            return "search." + from + " " + to + " " + date + " " + page;
        }
    }
}