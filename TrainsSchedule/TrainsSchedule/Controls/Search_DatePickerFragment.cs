//using Android.App;
//using Android.Os;
//using Android.Widget;
//using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class Search_DatePickerFragment : DialogFragment, OnDateSetListener
    {
        private OnDatePicked mOnDatePicked;
        public interface OnDatePicked
        {
            void OnDatePicked(int year, int monthOfYear, int dayOfMonth);
        }

        private long date;
        public override void OnAttach(Activity activity)
        {
            base.OnAttach(activity);
            try
            {
                mOnDatePicked = (OnDatePicked)activity;
            }
            catch (ClassCastException e)
            {
                throw new ClassCastException(activity.ToString() + " must implement onDatePicked");
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Bundle args = GetArguments();
            if (args != null)
            {
                date = args.GetLong("DATE");
            }
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Calendar c = Calendar.GetInstance();
            Date dDate = new Date(date);
            c.SetTime(dDate);
            int year = c[Calendar.YEAR];
            int month = c[Calendar.MONTH];
            int day = c[Calendar.DAY_OF_MONTH];
            return new DatePickerDialog(GetActivity(), this, year, month, day);
        }

        public virtual void OnDateSet(DatePicker view, int year, int month, int day)
        {
            mOnDatePicked.OnDatePicked(year, month, day);
        }
    }
}