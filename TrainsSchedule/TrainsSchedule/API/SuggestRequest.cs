using Com.Octo.Android.Robospice.Request.Retrofit;
using TrainsSchedule.Model.Station;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.API
{
    public class SuggestRequest : RetrofitSpiceRequest<IList, YandexSuggestAPI>
    {
        private string searchStr;
        public SuggestRequest(string searchStr) : base(typeof(IList), typeof(YandexSuggestAPI))
        {
            this.searchStr = searchStr;
        }

        public override IList LoadDataFromNetwork()
        {
            return GetService().Search(searchStr);
        }

        public virtual string CreateCacheKey()
        {
            return "suggest." + searchStr;
        }
    }
}