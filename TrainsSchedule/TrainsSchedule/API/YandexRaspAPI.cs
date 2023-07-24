using TrainsSchedule.DTO;
using Retrofit.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.API
{
    public interface YandexRaspAPI
    {
        // https://api.rasp.yandex.net/v1.0/search/
        // ?apikey=cb12e7a7-4f68-437c-9733-c8e39bc98046&from=s9601627&to=s9602231
        // &date=2015-10-18
        StopsList Search(string from, string to, string date, int page);
        StopsList Search(string from, string to, string date);
    }
}