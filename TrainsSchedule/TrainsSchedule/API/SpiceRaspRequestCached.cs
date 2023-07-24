using Com.Octo.Android.Robospice.Request.Retrofit;
using TrainsSchedule.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.API
{
    public class SpiceRaspRequestCached : RetrofitSpiceRequest<RaspRequest, YandexRaspAPI>
    {
        private string from;
        private string to;
        private int day;
        public int page;
        public SpiceRaspRequestCached(string from, string to, int day, int page) : base(typeof(RaspRequest), typeof(YandexRaspAPI))
        {
            this.from = from;
            this.to = to;
            this.day = day;
            this.page = page;
        }

        public override RaspRequest LoadDataFromNetwork()
        {
            return new RaspRequest(new StopsList(), page);
        }

        public virtual object CreateCacheKey()
        {
            return "searchDay." + from + " " + to + " " + day + " " + page;
        }
    }
}