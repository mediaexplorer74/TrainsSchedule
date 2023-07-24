using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.DTO
{
    public class Pagination
    {
        public bool has_next;
        public int per_page;
        public int page_count;
        public int total;
        public int page;
    }
}