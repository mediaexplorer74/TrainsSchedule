using Android.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Util
{
    public class S
    {
        public class ResultContainer
        {
            public bool result;
            public string errorString;
            public string userID;
            public string resultSessionID;
        }

        public static void L(object @object)
        {
            Log.D("mylog", "" + @object);
        }

        public static void L(string @string, Exception e)
        {
            Log.D("mylog", "" + @string + " " + Log.GetStackTraceString(e));
        }
    }
}