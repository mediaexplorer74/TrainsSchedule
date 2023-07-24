using Android.Content;
using Android.Graphics.Drawable;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.View;
using Android.Widget;
using Java.Util;
using TrainsSchedule.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class MainActivityTabsAdapter : FragmentPagerAdapter
    {
        private readonly IList<MainActivityTabFragment> mFragments = new List();
        public MainActivityTabsAdapter(FragmentManager fragmentManager) : base(fragmentManager)
        {
        }

        public virtual void AddFragment(string title)
        {
            AddFragment(title, 0, false);
        }

        public virtual void AddFragment(string title, int resourceId, bool isFav)
        {
            MainActivityTabFragment fragment = new MainActivityTabFragment();
            fragment.mTitle = title;
            fragment.mImage = resourceId;
            fragment.mIsFav = isFav;
            mFragments.Add(fragment);
        }

        public override Fragment GetItem(int position)
        {
            return mFragments[position];
        }

        public override int GetCount()
        {
            return mFragments.Count;
        }

        public override CharSequence GetPageTitle(int position)
        {
            MainActivityTabFragment curFragment = mFragments[position];
            return curFragment.mTitle;
        }

        public virtual void NewSchedule(Stop.List stopsList, bool hasNextPage)
        {
            MainActivityTabFragment curFragment = mFragments[0];
            curFragment.NewSchedule(stopsList, hasNextPage);
        }

        public virtual void AppendSchedule(Stop.List stopsList, bool hasNextPage)
        {
            MainActivityTabFragment curFragment = mFragments[0];
            curFragment.AppendSchedule(stopsList, hasNextPage);
        }

        public virtual void OnTabClick(int curTab)
        {
            MainActivityTabFragment curFragment = mFragments[curTab];
            curFragment.OnSelect();
        }

        public virtual void UpdateImages(Context context, TabLayout tabLayout)
        {
            foreach (MainActivityTabFragment fragment in mFragments)
            {
                if (fragment.mImage != 0)
                {
                    LinearLayout customTabLayout = (LinearLayout)LayoutInflater.From(context).Inflate(R.layout.main_activity_tab_layout, null);
                    TextView t = (TextView)customTabLayout.FindViewById(R.id.tabText);
                    t.SetText(fragment.mTitle);
                    Drawable d = context.GetResources().GetDrawable(fragment.mImage);
                    d.SetBounds(0, 0, d.GetIntrinsicWidth(), d.GetIntrinsicHeight());
                    ImageView i = (ImageView)customTabLayout.FindViewById(R.id.tabImage);
                    i.SetImageDrawable(d);
                    tabLayout.GetTabAt(mFragments.Count - 1).SetCustomView(customTabLayout);
                }
            }
        }

        public virtual void SetTitle(int position, string dateRepresentation)
        {
            MainActivityTabFragment fragment = mFragments[position];
            fragment.mTitle = dateRepresentation;
        }

        public virtual void UpdateFavorites()
        {
            MainActivityTabFragment fragment = mFragments[1];
            fragment.UpdateFavorites();
        }
    }
}