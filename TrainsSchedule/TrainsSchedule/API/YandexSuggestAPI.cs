using TrainsSchedule.Model.Station;
using Retrofit.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;

namespace TrainsSchedule.API
{
    public interface YandexSuggestAPI
    {
        //    https://suggests.rasp.yandex.ru/suburban?format=old&field=from&query=%D0%BC%D0%BE%D1%81%D0%BA%D0%B2%D0%B0&other_query=&other_point=&limit=11&lang=ru&client_city=213&national_version=ru&_=1446057073684
        //    https://suggests.rasp.yandex.ru/suburban?format=old&field=from&query=%D0%BC%D0%BE%D1%81%D0%BA%D0%B2%D0%B0&limit=3&lang=ru
        IList Search(string query);
    }
}