using Android.App;
using Android.Os;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.View;
using Android.Widget;
using Java.Util;
using TrainsSchedule.FavoritesListAdapter;
using TrainsSchedule.Model;
using TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class MainActivityTabFragment : Fragment, OnRemoveClick
    {
        public string mTitle;
        public int mImage;
        public bool mIsFav;
        private ScheduleListAdapter mSchedulesAdapter;
        private FavoritesListAdapter mFavoritesAdapter;
        private Stop.List mStops;
        private RelativeLayout mRootView;
        ScheduleSnappingLinearLayoutManager mLinearLayoutManager;
        private DB mainDB;
        private CurrentTime mCurrentTime;
        private readonly Handler mHandler = new Handler(Looper.GetMainLooper(), new AnonymousCallback(this));
        private sealed class AnonymousCallback : Callback
        {
            public AnonymousCallback(MainActivityTabFragment parent)
            {
                this.parent = parent;
            }

            private readonly MainActivityTabFragment parent;
            public override bool HandleMessage(Message message)
            {
                if (message.what == 1)
                {
                    ScrollToCurrentTimePosition();
                    return true;
                }

                return false;
            }
        }

        private class CurrentTime
        {
            public int currentTimeSec;
            public int currentPositionInStops;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mRootView = (RelativeLayout)inflater.Inflate(R.layout.main_activity_content, container, false);
            Init_vars();
            Init_Lists();
            return mRootView;
        }

        public override void OnDestroyView()
        {
            if (mainDB != null)
                mainDB.Close();
            base.OnDestroyView();
        }

        private void Init_vars()
        {
            if (!mIsFav)
            {
                mStops = new IList(null);
                GetCurrentTime();
            }
            else
            {
                mainDB = new DB(GetActivity());
            }
        }

        private void Init_Lists()
        {
            Activity activity = GetActivity();
            RecyclerView mRecyclerView = (RecyclerView)mRootView.FindViewById(R.id.recyclerview);
            mRecyclerView.AddItemDecoration(new ScheduleListDividerItemDecoration(GetActivity(), R.drawable.divider_horizontal_bright_opaque, false, false));
            mLinearLayoutManager = new ScheduleSnappingLinearLayoutManager(mRecyclerView.GetContext());
            mRecyclerView.SetLayoutManager(mLinearLayoutManager);
            if (mIsFav)
            {
                mFavoritesAdapter = new FavoritesListAdapter(activity, this);
                mFavoritesAdapter.mFavorites = mainDB.GetFavoritesList();
                mRecyclerView.SetAdapter(mFavoritesAdapter);
            }
            else
            {
                mSchedulesAdapter = new ScheduleListAdapter(activity, mStops, mCurrentTime.currentPositionInStops, mCurrentTime.currentTimeSec);
                mRecyclerView.SetAdapter(mSchedulesAdapter);
            }
        }

        private void ScrollToCurrentTimePosition()
        {
            GetCurrentTime();
            mSchedulesAdapter.SetCurrentTime(mCurrentTime.currentPositionInStops, mCurrentTime.currentTimeSec);
            mLinearLayoutManager.ScrollToPositionWithOffset(mCurrentTime.currentPositionInStops, 0); //        mRecyclerView.smoothScrollToPosition(mCurrentTime.currentPositionInStops);
        }

        public virtual void UpdateFavorites()
        {
            mFavoritesAdapter.mFavorites = mainDB.GetFavoritesList();
            mFavoritesAdapter.NotifyDataSetChanged();
        }

        public override void OnRemoveClick(int position)
        {
            Favorite favorite = mFavoritesAdapter.mFavorites[position];
            mainDB.DeleteFavorite(favorite);
            mFavoritesAdapter.mFavorites.Remove(position);
            mFavoritesAdapter.NotifyDataSetChanged();
        }

        private void GetCurrentTime()
        {
            mCurrentTime = new CurrentTime();
            if (mStops == null || mStops.Count == 0)
                return;
            Calendar calendar = Calendar.GetInstance();
            int timeInt = calendar[Calendar.HOUR_OF_DAY] * 60 + calendar[Calendar.MINUTE];
            mCurrentTime.currentTimeSec = timeInt;
            for (int i = 0; i < mStops.Count - 1; i++)
            {
                if (mStops[i].departure >= timeInt)
                {
                    mCurrentTime.currentPositionInStops = i;
                    return;
                }
            }

            mCurrentTime.currentPositionInStops = mStops.Count - 1;
        }

        public virtual void NewSchedule(Stop.List stopsList, bool hasNextPage)
        {
            mStops.NewStops(stopsList);
            mSchedulesAdapter.NotifyDataSetChanged();
            if (!hasNextPage)
                ScrollToCurrentTimePositionRunnable();
        }

        public virtual void AppendSchedule(Stop.List stopsList, bool hasNextPage)
        {
            mStops.AddStops(stopsList);
            mSchedulesAdapter.NotifyDataSetChanged();
            if (!hasNextPage)
                ScrollToCurrentTimePositionRunnable();
        }

        private void ScrollToCurrentTimePositionRunnable()
        {
            mHandler.SendMessage(mHandler.ObtainMessage(1, this));
        }

        public virtual void OnSelect()
        {
            if (!mIsFav)
                ScrollToCurrentTimePositionRunnable();
        }
    }
}