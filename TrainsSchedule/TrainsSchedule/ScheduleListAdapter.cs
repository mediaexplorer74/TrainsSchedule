using Android.Content;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.View;
using Android.Widget;
using TrainsSchedule.Model;
using TrainsSchedule.Model.Stop;
using TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class ScheduleListAdapter : Adapter<ScheduleListAdapter.ViewHolder>
    {
        private Stop.List mStops;
        private int mCurrentTimePosition = -1;
        private int mCurrentTimeSec = 0;
        private int mRegularColor = -1;
        private int mExpressColor = -1;
        private int mStopsColor = -1;
        public ScheduleListAdapter(Context context, IList mStops, int currentPositionInStops, int currentTimeSec)
        {
            this.mStops = mStops;
            this.mExpressColor = ContextCompat.GetColor(context, R.color.textExpress);
            this.mRegularColor = ContextCompat.GetColor(context, R.color.textMinor);
            this.mStopsColor = ContextCompat.GetColor(context, R.color.textStops);
            this.mCurrentTimePosition = currentPositionInStops;
            this.mCurrentTimeSec = currentTimeSec;
        }

        public virtual void SetCurrentTime(int currentPositionInStops, int currentTimeSec)
        {
            mCurrentTimePosition = currentPositionInStops;
            mCurrentTimeSec = currentTimeSec;
        }

        public class ViewHolder : ViewHolder
        {
            public readonly TextView departure;
            public readonly TextView arrive;
            public readonly TextView duration;
            public readonly TextView exclude;
            public readonly TextView route;
            public readonly TextView timeToWait;
            public ViewHolder(View v) : base(v)
            {
                departure = (TextView)v.FindViewById(R.id.tvDeparture);
                arrive = (TextView)v.FindViewById(R.id.tvArrive);
                exclude = (TextView)v.FindViewById(R.id.tvExclude);
                route = (TextView)v.FindViewById(R.id.tvRoute);
                duration = (TextView)v.FindViewById(R.id.tvDuration);
                timeToWait = (TextView)v.FindViewById(R.id.tvTimeToWait);
            }
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.GetContext()).Inflate(R.layout.shedule_list_item, parent, false);
            return new ViewHolder(view);
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            Stop currentStop = mStops[position];
            if (mCurrentTimePosition >= 0 && position < mCurrentTimePosition)
            {
                holder.itemView.SetAlpha(0.2F);
            }
            else
            {
                holder.itemView.SetAlpha(1F);
            }

            holder.arrive.SetText(GetTimeStr(currentStop.arrival));
            holder.departure.SetText(GetTimeStr(currentStop.departure));
            holder.duration.SetText("(" + currentStop.duration + ")");
            bool visibleExclude = false;
            if (currentStop.exclude.IsEmpty())
            {
                holder.exclude.SetVisibility(View.GONE);
            }
            else
            {
                visibleExclude = true;
                holder.exclude.SetText(currentStop.exclude);
                holder.exclude.SetVisibility(View.VISIBLE);
            }

            holder.route.SetText(currentStop.route);
            if (currentStop.express)
            {
                holder.route.SetTextColor(mExpressColor);
                if (visibleExclude)
                    holder.exclude.SetTextColor(mExpressColor);
            }
            else
            {
                holder.route.SetTextColor(mRegularColor);
                if (visibleExclude)
                {
                    holder.exclude.SetTextColor(mStopsColor);
                }
            }

            int timeToWaitPos = position - mCurrentTimePosition;
            if (timeToWaitPos >= 0 && timeToWaitPos < 5)
            {
                holder.timeToWait.SetText(GetTimeToWaitStr(currentStop.departure));
            }
            else
                holder.timeToWait.SetText("");
        }

        private string GetTimeStr(int timeInMins)
        {
            int mins = timeInMins % 60;
            int hours = (timeInMins - mins) / 60;
            mins = timeInMins - hours * 60;
            string hourStr = "0" + hours;
            string minStr = "0" + mins;
            return hourStr.Substring(hourStr.Length() - 2) + ":" + minStr.Substring(minStr.Length() - 2);
        }

        public virtual string GetTimeToWaitStr(int departure)
        {
            int diff = departure - mCurrentTimeSec;
            int min = diff % 60;
            int hr = (diff - min) / 60;
            min = diff - hr * 60;
            if (hr == 0)
                return Multilanguage.@in + " " + min + Multilanguage.min;
            return Multilanguage.@in + " " + hr + Multilanguage.h + " " + min + Multilanguage.min;
        }

        public override int GetItemCount()
        {
            return mStops.Count;
        }
    }
}