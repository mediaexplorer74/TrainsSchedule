using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Model
{
    public class Favorite
    {
        public string fromCode;
        public string fromName;
        public string toCode;
        public string toName;
        public Favorite(string fromCode, string fromName, string toCode, string toName)
        {
            this.fromCode = fromCode;
            this.fromName = fromName;
            this.toCode = toCode;
            this.toName = toName;
        }
    }
}