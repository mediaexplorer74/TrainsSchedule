using Com.Google.Gson;
using Java.Io;
using Java.Lang.Reflect;
using TrainsSchedule.Model;
using TrainsSchedule.Model.Station;
using Retrofit.Converter;
using Retrofit.Mime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.TrainsSchedule.API
{
    public class SuggestGsonConverter : GsonConverter
    {
        public SuggestGsonConverter(Gson gson) : base(gson)
        {
        }

        public override object FromBody(TypedInput body, Type type)
        {
            IList res = null;
            string dirty = ToString(body);
            if (dirty.IsEmpty())
                return null;
            string clean = dirty.ReplaceAll("]]]", "");
            clean = clean.ReplaceAll("\\[null,\\[\\[", "");
            String[] arr = clean.Split("],\\[");
            if (arr.length == 0)
                return null;
            res = new IList(arr.length);
            foreach (string substr in arr)
            {
                substr = substr.Substring(1, substr.Length() - 1);
                String[] arr2 = substr.Split("\",\"");
                if (arr2.length != 3)
                    continue;
                Station newSugg = new Station(arr2[0], arr2[1], arr2[2]);
                res.Add(newSugg);
            }

            return res;
        }

        private string ToString(TypedInput body)
        {
            string charset = "UTF-8";
            if (body.MimeType() != null)
            {
                charset = MimeUtil.ParseCharset(body.MimeType());
            }

            BufferedReader br = null;
            StringBuilder sb = new StringBuilder();
            string line;
            try
            {
                br = new BufferedReader(new InputStreamReader(body.In(), charset));
                while ((line = br.ReadLine()) != null)
                {
                    sb.Append(line);
                }
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
            finally
            {
                if (br != null)
                {
                    try
                    {
                        br.Close();
                    }
                    catch (IOException e)
                    {
                        e.PrintStackTrace();
                    }
                }
            }

            return sb.ToString();
        }
    }
}