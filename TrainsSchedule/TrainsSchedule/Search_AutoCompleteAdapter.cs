using Android.Content;
using Android.View;
using Android.Widget;
using Com.Octo.Android.Robospice;
using Com.Octo.Android.Robospice.Exception;
using Com.Octo.Android.Robospice.Persistence;
using Com.Octo.Android.Robospice.Persistence.Exception;
using Com.Octo.Android.Robospice.Request.Listener;
using Java.Util;
using TrainsSchedule.TrainsSchedule.API;
using TrainsSchedule.TrainsSchedule.Model;
using TrainsSchedule.Model.Station;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.TrainsSchedule
{
    public class Search_AutoCompleteAdapter : ArrayAdapter<Station>, Filterable
    {
        public List<Station> resultList;
        public Station mSelectedItem;
        protected SpiceManager mSpiceManager;
        private readonly LayoutInflater mInflater;
        private SuggestRequest mRequest;
        private Context mContext;
        public bool needAutocomplete;
        public Search_AutoCompleteAdapter(Context context, SpiceManager spiceManager)
            : base(context, R.layout.search_autocomplete_list_item)
        {
            mSpiceManager = spiceManager;
            mInflater = LayoutInflater.From(context);
            mContext = context;
            mSelectedItem = new Station("", "");
            needAutocomplete = true;
            this.SetNotifyOnChange(false);
        }

        public override int GetCount()
        {
            return resultList.Count;
        }

        public override Station GetItem(int index)
        {
            return resultList[index];
        }

        class ViewHolder
        {
            public TextView text;
            public TextView textDescr;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;
            View v = convertView;
            Station item = GetItem(position);
            if (v == null)
            {
                v = mInflater.Inflate(R.layout.search_autocomplete_list_item, parent, false);
                holder = new ViewHolder();
                holder.text = (TextView)v.FindViewById(R.id.suggetItem0);
                holder.textDescr = (TextView)v.FindViewById(R.id.suggetItem1);
                v.SetTag(holder);
            }
            else
            {
                holder = (ViewHolder)v.GetTag();
            }

            holder.text.SetText(item.name);
            holder.textDescr.SetText(item.descr);
            return v;
        }

        public override Filter GetFilter()
        {
            return new AnonymousFilter(this);
        }

        private sealed class AnonymousFilter : Filter
        {
            public AnonymousFilter(ViewHolder parent)
            {
                this.parent = parent;
            }

            private readonly ViewHolder parent;
            protected override FilterResults PerformFiltering(CharSequence constraint)
            {
                AutocompleteAsync(constraint);
                return null;
            }

            protected override void PublishResults(CharSequence constraint, FilterResults results)
            {
            }

            public override CharSequence ConvertResultToString(object resultValue)
            {
                Station station = (Station)resultValue;
                if (station == null)
                    return "";
                return station.name;
            }
        }

        private void AutocompleteAsync(CharSequence input)
        {
            if (mRequest != null && !mRequest.IsCancelled())
            {
                mRequest.Cancel();
            }

            if (!needAutocomplete || input == null || input.Length() == 0)
                return;
            mRequest = new SuggestRequest(input.ToString());
            mSpiceManager.Execute(mRequest, mRequest.CreateCacheKey(), 
                DurationInMillis.ALWAYS_RETURNED, new SuggestRequestResultListener());
        }

        private class SuggestRequestResultListener : RequestListener<IList>
        {
            public override void OnRequestFailure(SpiceException spiceException)
            {
                if (spiceException is RequestCancelledException)
                    return;
                Toast.MakeText(mContext, spiceException.GetLocalizedMessage(), Toast.LENGTH_SHORT).Show();
            }

            public override void OnRequestSuccess(IList suggests)
            {
                resultList = new List(suggests.Count);
                for (int i = 0; i < suggests.Count; i++)
                {
                    resultList.Add(i, suggests[i]);
                }

                NotifyDataSetChanged();
            }
        }
    }
}