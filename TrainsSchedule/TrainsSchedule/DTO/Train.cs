using Java;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.DTO
{
    public class Train
    {
        //    Station from;
        //    Station to;
        string departure;
        string arrival;
        int duration;
        string departure_terminal;
        string departure_platform;
        string arrival_terminal;
        string arrival_platform;
        Thread thread;
        string stops;
        public Train(string arrival, string arrival_platform)
        {
            this.arrival = arrival;
            this.arrival_platform = arrival_platform;
        }

        public virtual string GetTime()
        {
            return arrival;
        }

        public virtual string GetName()
        {
            return arrival_platform;
        }
    }
}