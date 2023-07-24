using Com.Google.Gson;
using TrainsSchedule.API;
using TrainsSchedule.Base;
using TrainsSchedule.Util;
using Retrofit;
using Retrofit.RestAdapter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.TrainsSchedule.Services
{
    public class SuggestsRaspService : BaseNetworkService
    {
        private static readonly string MAX_SUGGETS = "10";
        protected override Builder CreateRetrofitBuilder()
        {
            RequestInterceptor requestInterceptor = new AnonymousRequestInterceptor(this);
            return new Builder().SetLogLevel(LogLevel.NONE).SetEndpoint(API.SUGGEST_BASE).SetConverter(new SuggestGsonConverter(new Gson())).SetRequestInterceptor(requestInterceptor);
        }

        private sealed class AnonymousRequestInterceptor : RequestInterceptor
        {
            public AnonymousRequestInterceptor(SuggestsRaspService parent)
            {
                this.parent = parent;
            }

            private readonly SuggestsRaspService parent;
            public override void Intercept(RequestFacade request)
            {
                request.AddQueryParam("format", "old");
                request.AddQueryParam("field", "from");
                request.AddQueryParam("limit", MAX_SUGGETS);
                request.AddQueryParam("client_city", "213");
                request.AddQueryParam("lang", Multilanguage.lang);
            }
        }
    }
}