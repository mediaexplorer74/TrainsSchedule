using TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Model
{
    public class SearchParams
    {
        public string fromCode;
        public string fromName;
        public string toCode;
        public string toName;
        public long date;
        public int dayWeek;
        public SearchParams(long date, string fromCode, string fromName, 
            string toCode, string toName)
        {
            this.date = date;
            this.fromCode = fromCode;
            this.fromName = fromName;
            this.toCode = toCode;
            this.toName = toName;
            this.dayWeek = Util.GetDayOfWeek(date);
        }
    }
}