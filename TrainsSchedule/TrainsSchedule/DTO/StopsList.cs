using Java.Util;
using TrainsSchedule.Model;
using TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.DTO
{
    public class StopsList
    {
        public Pagination pagination;
        public IList<Train> threads;
        SearchDetail search;
        public virtual Stop.List GetStops()
        {
            if (threads == null)
                return null;
            int i = threads.Count;
            List<Stop> res = new List<Stop>(i);
            int dayOfWeek = Util.GetDayOfWeek(search.date);
            foreach (Train train in threads)
            {
                Stop stop = new Stop();
                stop.Fill(search.from, search.to, dayOfWeek, train.departure, train.arrival, train.duration, train.thread.title, train.stops, train.thread.express_type);
                res.Add(stop);
            }

            return new IList(res);
        }

        public virtual bool HasNextPage()
        {
            if (pagination == null)
                return false;
            return pagination.has_next && pagination.page < pagination.page_count;
        }

        public virtual int GetNextPage()
        {
            if (pagination == null)
                return 1;
            return pagination.page + 1;
        }

        public virtual bool IsStartPage()
        {
            if (pagination == null)
                return true;
            return pagination.page == 1;
        }
    }
}