using TrainsSchedule.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.API
{
    public class RaspRequest
    {
        public StopsList stopsList;
        public int page;
        public RaspRequest()
        {
        }

        public RaspRequest(StopsList stopsList, int page)
        {
            this.stopsList = stopsList;
            this.page = page;
        }
    }
}