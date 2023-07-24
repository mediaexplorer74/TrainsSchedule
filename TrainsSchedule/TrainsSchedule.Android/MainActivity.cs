// MainActivity

using Android.Content;
//using Android.Content.SharedPreferences;
//using Android.Graphics.PorterDuff;
//using Android.Os;
//using Android.Support.Design.Widget;
//using Android.Support.Design.Widget.TabLayout;
//using Android.Support.V4.View;
//using Android.Support.V7.App;
//using Android.Support.V7.Widget;
//using Android.View;
using Android.Widget;
//using Com.Octo.Android.Robospice;
//using Com.Octo.Android.Robospice.Exception;
//using Com.Octo.Android.Robospice.Persistence;
//using Com.Octo.Android.Robospice.Persistence.Exception;
//using Com.Octo.Android.Robospice.Request.Listener;
using Java.Util;
using TrainsSchedule.API;
//using TrainsSchedule.FavoritesListAdapter;
using TrainsSchedule.Model;
using TrainsSchedule.Services;
using TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.AppCompat.App;

//namespace TrainsSchedule.TrainsSchedule
namespace TrainsSchedule.Droid
{
    //public class MainActivity : AppCompatActivity, OnFavoriteClick
    [Activity(Label = "TrainsSchedule", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected SpiceManager mSpiceManager = new SpiceManager(typeof(RaspServiceNetworkService));
        private static readonly int SEARCH_ACTIVITY_RESULT_CODE = 1;
        private static readonly string SETTINGS_STATION_FROM_NAME = "SETTINGS_STATION_FROM_NAME";
        private static readonly string SETTINGS_STATION_FROM_CODE = "SETTINGS_STATION_FROM_CODE";
        private static readonly string SETTINGS_STATION_TO_NAME = "SETTINGS_STATION_TO_NAME";
        private static readonly string SETTINGS_STATION_TO_CODE = "SETTINGS_STATION_TO_CODE";
        private static readonly string SETTINGS_DATE = "SETTINGS_DATE";
        private MainActivityTabsAdapter mTabsAdapter;
        private ViewPager mViewPager;
        private SearchParams mCurrentSearch;
        private Toolbar mToolbar;
        private ProgressBar mProgressBar;
        private TabLayout mTabLayout;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            


            Multilanguage.Init(this);
            SetContentView(R.layout.main_activity_main);
            mToolbar = (Toolbar)FindViewById(R.id.toolbar);
            mToolbar.SetTitle(Multilanguage.app_name);
            SetSupportActionBar(mToolbar);
            Init_vars();
            Init_Tabs();
            Init_FAB();

            LoadApplication(new App());
        }

        private void Init_vars()
        {
            SharedPreferences sPref = GetPreferences(MODE_PRIVATE);
            string stationFromName = sPref.GetString(SETTINGS_STATION_FROM_NAME, "");
            string stationFromCode = sPref.GetString(SETTINGS_STATION_FROM_CODE, "");
            string stationToName = sPref.GetString(SETTINGS_STATION_TO_NAME, "");
            string stationToCode = sPref.GetString(SETTINGS_STATION_TO_CODE, "");
            long date = sPref.GetLong(SETTINGS_DATE, 0);
            Calendar c = Calendar.GetInstance();
            c[Calendar.HOUR_OF_DAY] = 0;
            c[Calendar.MINUTE] = 0;
            c[Calendar.SECOND] = 0;
            long beginOfToday = c.GetTimeInMillis();
            date = (date < beginOfToday) ? beginOfToday : date;
            mCurrentSearch = new SearchParams(date, stationFromCode, stationFromName, stationToCode, stationToName);
        }

        private void StartSearch(bool force)
        {
            if (mCurrentSearch.fromCode.IsEmpty() || mCurrentSearch.toCode.IsEmpty())
                return;
            mToolbar.SetSubtitle(mCurrentSearch.fromName + " - " + mCurrentSearch.toName);
            DoSearchRequest(force);
        }

        private void Init_Tabs()
        {
            mProgressBar = (ProgressBar)FindViewById(R.id.toolbar_progress_bar);
            if (mProgressBar.GetIndeterminateDrawable() != null)
            {
                mProgressBar.GetIndeterminateDrawable().SetColorFilter(GetResources().GetColor(R.color.colorAccent), Mode.SRC_IN);
            }

            mTabLayout = (TabLayout)FindViewById(R.id.tabs);
            mViewPager = (ViewPager)FindViewById(R.id.viewpager);
            mTabsAdapter = new MainActivityTabsAdapter(GetSupportFragmentManager());
            mTabsAdapter.AddFragment(Util.GetDateRepresentation(mCurrentSearch.date));
            mTabsAdapter.AddFragment(Multilanguage.favorites, R.drawable.ic_action_important, true);
            mViewPager.SetAdapter(mTabsAdapter);
            mTabLayout.SetupWithViewPager(mViewPager);
            mTabsAdapter.UpdateImages(this, mTabLayout);
            mTabLayout.SetOnTabSelectedListener(new AnonymousViewPagerOnTabSelectedListener(mViewPager));
            StartSearch(false);
        }

        private sealed class AnonymousViewPagerOnTabSelectedListener : ViewPagerOnTabSelectedListener
        {
            public AnonymousViewPagerOnTabSelectedListener(MainActivity parent)
            {
                this.parent = parent;
            }

            private readonly MainActivity parent;
            public override void OnTabSelected(Tab tab)
            {
                base.OnTabSelected(tab);
                mTabsAdapter.OnTabClick(tab.GetPosition());
            }

            public override void OnTabReselected(Tab tab)
            {
                base.OnTabReselected(tab);
                mTabsAdapter.OnTabClick(tab.GetPosition());
            }
        }

        private void Init_FAB()
        {
            FloatingActionButton fab = (FloatingActionButton)FindViewById(R.id.fab);
            fab.SetOnClickListener(new AnonymousOnClickListener(this));
        }

        private sealed class AnonymousOnClickListener : OnClickListener
        {
            public AnonymousOnClickListener(MainActivity parent)
            {
                this.parent = parent;
            }

            private readonly MainActivity parent;
            public override void OnClick(View view)
            {
                StartSearchActivity();
            }
        }

        private void StartSearchActivity()
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(Search_Activity));
            intent.PutExtra(Search_Activity.ARGS_FROM_CODE, mCurrentSearch.fromCode);
            intent.PutExtra(Search_Activity.ARGS_FROM_NAME, mCurrentSearch.fromName);
            intent.PutExtra(Search_Activity.ARGS_TO_CODE, mCurrentSearch.toCode);
            intent.PutExtra(Search_Activity.ARGS_TO_NAME, mCurrentSearch.toName);
            intent.PutExtra(Search_Activity.ARGS_DATE, mCurrentSearch.date);
            StartActivityForResult(intent, SEARCH_ACTIVITY_RESULT_CODE);
        }

        protected override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == SEARCH_ACTIVITY_RESULT_CODE)
            {
                if (resultCode == RESULT_OK)
                {
                    Bundle args = data.GetExtras();
                    if (args == null)
                        return;
                    mCurrentSearch.fromCode = args.GetString(Search_Activity.ARGS_FROM_CODE, "");
                    mCurrentSearch.fromName = args.GetString(Search_Activity.ARGS_FROM_NAME, "");
                    mCurrentSearch.toCode = args.GetString(Search_Activity.ARGS_TO_CODE, "");
                    mCurrentSearch.toName = args.GetString(Search_Activity.ARGS_TO_NAME, "");
                    mCurrentSearch.date = args.GetLong(Search_Activity.ARGS_DATE, 0);
                    int pageIndex = mViewPager.GetCurrentItem();
                    string pageTitle = Util.GetDateRepresentation(mCurrentSearch.date);
                    mTabsAdapter.SetTitle(pageIndex, pageTitle);
                    Tab tab = mTabLayout.GetTabAt(0);
                    if (tab != null)
                    {
                        tab.SetText(pageTitle);
                        tab.Select();
                    }

                    StartSearch(true);
                }

                mTabsAdapter.UpdateFavorites();
            }
        }

        public override void OnFavoriteClick(Favorite favorite)
        {
            mCurrentSearch.fromCode = favorite.fromCode;
            mCurrentSearch.fromName = favorite.fromName;
            mCurrentSearch.toCode = favorite.toCode;
            mCurrentSearch.toName = favorite.toName;
            Tab tab = mTabLayout.GetTabAt(0);
            if (tab != null)
            {
                tab.Select();
            }

            StartSearch(false);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mSpiceManager.Start(this);
        }

        protected override void OnStop()
        {
            mSpiceManager.ShouldStop();
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            SharedPreferences sPref = GetPreferences(MODE_PRIVATE);
            Editor ed = sPref.Edit();
            ed.Clear();
            ed.PutString(SETTINGS_STATION_FROM_NAME, mCurrentSearch.fromName);
            ed.PutString(SETTINGS_STATION_FROM_CODE, mCurrentSearch.fromCode);
            ed.PutString(SETTINGS_STATION_TO_NAME, mCurrentSearch.toName);
            ed.PutString(SETTINGS_STATION_TO_CODE, mCurrentSearch.toCode);
            ed.PutLong(SETTINGS_DATE, mCurrentSearch.date);
            ed.Apply();
            base.OnDestroy();
        }

        public override bool OnCreateOptionsMenu(Menu menu)
        {
            GetMenuInflater().Inflate(R.menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(MenuItem item)
        {
            int id = item.GetItemId();
            if (id == R.id.action_refresh)
            {
                StartSearch(true);
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void DoSearchRequest(bool force)
        {
            DoSearchRequest(1, force);
        }

        private void DoSearchRequest(int page, bool force)
        {
            if (page == 1 && mSpiceManager.IsStarted())
                mSpiceManager.CancelAllRequests();
            if (force)
            {
                LoadFromNetwork(mCurrentSearch, page);
            }
            else
            {
                LoadFromCache(mCurrentSearch, page);
            }
        }

        private void LoadFromNetwork(SearchParams searchParams, int page)
        {
            mProgressBar.SetVisibility(View.VISIBLE);
            SpiceRaspRequest mRequest = new SpiceRaspRequest(searchParams.fromCode, searchParams.toCode, Util.GetDateForSearch(searchParams.date), page);
            mSpiceManager.Execute(mRequest, mRequest.CreateCacheKey(), DurationInMillis.ALWAYS_EXPIRED, new SearchRequestResultListener());
        }

        private void LoadFromCache(SearchParams searchParams, int page)
        {
            SpiceRaspRequestCached mRequestCached = new SpiceRaspRequestCached(searchParams.fromCode, searchParams.toCode, searchParams.dayWeek, page);
            mSpiceManager.GetFromCache(typeof(RaspRequest), mRequestCached.CreateCacheKey(), DurationInMillis.ALWAYS_RETURNED, new SearchRequestResultListenerCached(searchParams, page));
        }

        private void PutInCache(RaspRequest request, SearchParams search)
        {
            SpiceRaspRequestCached mRequestCached = new SpiceRaspRequestCached(mCurrentSearch.fromCode, mCurrentSearch.toCode, mCurrentSearch.dayWeek, request.page);
            mSpiceManager.PutInCache(mRequestCached.CreateCacheKey(), request);
        }

        private class SearchRequestResultListener : RequestListener<RaspRequest>
        {
            public override void OnRequestFailure(SpiceException e)
            {
                mProgressBar.SetVisibility(View.INVISIBLE);
                if (e is RequestCancelledException)
                    return;
                Snackbar.Make(FindViewById(R.id.fab), e.GetMessage(), Snackbar.LENGTH_LONG).Show();
            }

            public override void OnRequestSuccess(RaspRequest raspRequest)
            {
                mProgressBar.SetVisibility(View.INVISIBLE);
                PutInCache(raspRequest, mCurrentSearch);
                LoadNextPage(raspRequest, true);
            }
        }

        private class SearchRequestResultListenerCached : RequestListener<RaspRequest>
        {
            private int page;
            private SearchParams mParentSearch;
            public SearchRequestResultListenerCached(SearchParams currentSearch, int page)
            {
                this.page = page;
                this.mParentSearch = currentSearch;
            }

            public override void OnRequestFailure(SpiceException e)
            {
                if (e is RequestCancelledException)
                    return;
                LoadFromNetwork(mParentSearch, page);
            }

            public override void OnRequestSuccess(RaspRequest raspRequest)
            {
                if (raspRequest == null)
                {
                    LoadFromNetwork(mParentSearch, page);
                }
                else
                {
                    LoadNextPage(raspRequest, false);
                }
            }
        }


        // LoadNextPage
        private void LoadNextPage(RaspRequest raspRequest, bool force)
        {
            bool hasNextPage = raspRequest.stopsList.HasNextPage();
            if (raspRequest.stopsList.IsStartPage())
            {
                mTabsAdapter.NewSchedule(raspRequest.stopsList.GetStops(), hasNextPage);
            }
            else
            {
                mTabsAdapter.AppendSchedule(raspRequest.stopsList.GetStops(), hasNextPage);
            }

            if (hasNextPage)
            {
                DoSearchRequest(raspRequest.stopsList.GetNextPage(), force);
            }
        }//LoadNextPage


        // OnRequestPermissionsResult
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }//OnRequestPermissionsResult
    }//class end
}