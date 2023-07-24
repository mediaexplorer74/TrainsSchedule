using TrainsSchedule.API;
using TrainsSchedule.Base;
using Retrofit;
using Retrofit.RestAdapter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Services
{
    public class RaspServiceNetworkService : BaseNetworkService
    {
        protected override Builder CreateRetrofitBuilder()
        {
            RequestInterceptor requestInterceptor = new AnonymousRequestInterceptor(this);
            return new Builder().SetLogLevel(LogLevel.NONE)
                .SetEndpoint(API.RASP_BASE).SetConverter(GetConverter())
                .SetRequestInterceptor(requestInterceptor);
        }

        private sealed class AnonymousRequestInterceptor : RequestInterceptor
        {
            public AnonymousRequestInterceptor(RaspServiceNetworkService parent)
            {
                this.parent = parent;
            }

            private readonly RaspServiceNetworkService parent;
            public override void Intercept(RequestFacade request)
            {
                request.AddQueryParam("apikey", Key.KEY);
                request.AddQueryParam("format", "json");
                request.AddQueryParam("transport_types", "suburban");
                request.AddQueryParam("lang", "ru");
            }
        }
    }
}