using Android.App;
using Android.Content;
using Android.Graphics.Drawable;
using Android.Os;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.View;
using Android.View.View;
using Android.View.Inputmethod;
using Android.Widget;
using Android.Widget.AdapterView;
using Com.Octo.Android.Robospice;
using Java.Util;
using TrainsSchedule.Model;
using TrainsSchedule.Search_DatePickerFragment;
using TrainsSchedule.Services;
using TrainsSchedule.TrainsSchedule.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class Search_Activity : AppCompatActivity, OnDatePicked
    {
        public static readonly string ARGS_FROM_CODE = "ARGS_FROM_CODE";
        public static readonly string ARGS_FROM_NAME = "ARGS_FROM_NAME";
        public static readonly string ARGS_TO_CODE = "ARGS_TO_CODE";
        public static readonly string ARGS_TO_NAME = "ARGS_TO_NAME";
        public static readonly string ARGS_DATE = "ARGS_DATE";
        protected SpiceManager mSpiceManager = new SpiceManager(typeof(SuggestsRaspService));
        private Search_AutoCompleteAdapter mSearchAdapterFrom;
        private Search_AutoCompleteAdapter mSearchAdapterTo;
        private AutoCompleteTextView tvSearchFrom;
        private AutoCompleteTextView tvSearchTo;
        private Button mBtnAddToFavorites;
        private EditText tvDate;
        private TextInputLayout tilSearchFrom;
        private TextInputLayout tilSearchTo;
        private SearchParams mCurrentSearch;
        private int imgWidth;
        private DB mainDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(R.layout.search_activity_main);
            Toolbar toolbar = (Toolbar)FindViewById(R.id.search_toolbar);
            SetSupportActionBar(toolbar);
            Init_LoadParams();
            Init_FAB();
            Init_TV();
        }

        private void Init_LoadParams()
        {
            Bundle args = GetIntent().GetExtras();
            if (args == null)
                return;
            string currentFromCode = args.GetString(ARGS_FROM_CODE, "");
            string currentFromName = args.GetString(ARGS_FROM_NAME, "");
            string currentToCode = args.GetString(ARGS_TO_CODE, "");
            string currentToName = args.GetString(ARGS_TO_NAME, "");
            long date = args.GetLong(ARGS_DATE, 0);
            mCurrentSearch = new SearchParams(date, currentFromCode, currentFromName, currentToCode, currentToName);
            mainDB = new DB(this);
        }

        private void Init_TV()
        {
            mSearchAdapterFrom = new Search_AutoCompleteAdapter(this, mSpiceManager);
            mSearchAdapterTo = new Search_AutoCompleteAdapter(this, mSpiceManager);
            mBtnAddToFavorites = (Button)FindViewById(R.id.btnAddToFavorites);
            UpdateFavVisibility(mCurrentSearch.fromCode, mCurrentSearch.toCode);
            tvSearchFrom = (AutoCompleteTextView)FindViewById(R.id.tvSearchFrom);
            tvSearchTo = (AutoCompleteTextView)FindViewById(R.id.tvSearchTo);
            tvDate = (EditText)FindViewById(R.id.tvDate);
            tvSearchFrom.SetOnItemClickListener(new AnonymousOnItemClickListener(this));
            tvSearchFrom.SetOnFocusChangeListener(new AnonymousOnFocusChangeListener(this));
            tvSearchTo.SetOnFocusChangeListener(new AnonymousOnFocusChangeListener1(this));
            tvSearchTo.SetOnItemClickListener(new AnonymousOnItemClickListener1(this));
            tilSearchFrom = (TextInputLayout)FindViewById(R.id.textInputLayoutFrom);
            tilSearchTo = (TextInputLayout)FindViewById(R.id.textInputLayoutTo);
            Button btnSwitch = (Button)FindViewById(R.id.btnSwitch);
            Drawable imgClear = GetResources().GetDrawable(R.mipmap.ic_clear_search_api_holo_light);
            imgWidth = tvSearchFrom.GetPaddingRight() + imgClear.GetIntrinsicWidth();
            tvSearchFrom.SetOnTouchListener(new OnClear());
            tvSearchTo.SetOnTouchListener(new OnClear());
            if (Util.NotEmpty(mCurrentSearch.fromName))
            {
                mSearchAdapterFrom.mSelectedItem = new Station(mCurrentSearch.fromCode, mCurrentSearch.fromName);
                tvSearchFrom.SetText(mCurrentSearch.fromName);
            }

            if (Util.NotEmpty(mCurrentSearch.toName))
            {
                mSearchAdapterTo.mSelectedItem = new Station(mCurrentSearch.toCode, mCurrentSearch.toName);
                tvSearchTo.SetText(mCurrentSearch.toName);
            }

            tvSearchFrom.SetAdapter(mSearchAdapterFrom);
            tvSearchTo.SetAdapter(mSearchAdapterTo);
            tvDate.SetOnFocusChangeListener(new AnonymousOnFocusChangeListener2(this));
            tvDate.SetOnClickListener(new AnonymousOnClickListener(this));
            btnSwitch.SetOnClickListener(new AnonymousOnClickListener1(this));
            tvSearchTo.SetOnEditorActionListener(new AnonymousOnEditorActionListener(this));
            tvDate.SetOnEditorActionListener(new AnonymousOnEditorActionListener1(this));
            mBtnAddToFavorites.SetOnClickListener(new AnonymousOnClickListener2(this));
            SetDataText();
        }

        private sealed class AnonymousOnItemClickListener : OnItemClickListener
        {
            public AnonymousOnItemClickListener(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnItemClick(AdapterView<TWildcardTodo> parent, View view, int position, long id)
            {
                mSearchAdapterFrom.mSelectedItem = mSearchAdapterFrom.GetItem(position);
                UpdateFavVisibility(mSearchAdapterFrom.mSelectedItem, mSearchAdapterTo.mSelectedItem);
            }
        }

        private sealed class AnonymousOnFocusChangeListener : OnFocusChangeListener
        {
            public AnonymousOnFocusChangeListener(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnFocusChange(View v, bool hasFocus)
            {
                if (hasFocus)
                {
                    tilSearchFrom.SetError("");
                    tilSearchFrom.SetErrorEnabled(false);
                }
            }
        }

        private sealed class AnonymousOnFocusChangeListener1 : OnFocusChangeListener
        {
            public AnonymousOnFocusChangeListener1(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnFocusChange(View v, bool hasFocus)
            {
                if (hasFocus)
                {
                    tilSearchTo.SetError("");
                    tilSearchTo.SetErrorEnabled(false);
                }
            }
        }

        private sealed class AnonymousOnItemClickListener1 : OnItemClickListener
        {
            public AnonymousOnItemClickListener1(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnItemClick(AdapterView<TWildcardTodo> parent, View view, int position, long id)
            {
                mSearchAdapterTo.mSelectedItem = mSearchAdapterTo.GetItem(position);
                UpdateFavVisibility(mSearchAdapterFrom.mSelectedItem, mSearchAdapterTo.mSelectedItem);
            }
        }

        private sealed class AnonymousOnFocusChangeListener2 : OnFocusChangeListener
        {
            public AnonymousOnFocusChangeListener2(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnFocusChange(View v, bool hasFocus)
            {
                if (hasFocus)
                {
                    ShowDataPicker();
                }
            }
        }

        private sealed class AnonymousOnClickListener : OnClickListener
        {
            public AnonymousOnClickListener(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnClick(View v)
            {
                ShowDataPicker();
            }
        }

        private sealed class AnonymousOnClickListener1 : OnClickListener
        {
            public AnonymousOnClickListener1(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnClick(View v)
            {
                Station tmpFrom = mSearchAdapterFrom.mSelectedItem;
                mSearchAdapterFrom.mSelectedItem = mSearchAdapterTo.mSelectedItem;
                mSearchAdapterTo.mSelectedItem = tmpFrom;
                mSearchAdapterFrom.needAutocomplete = false;
                mSearchAdapterTo.needAutocomplete = false;
                string textFrom = tvSearchFrom.GetText().ToString();
                tvSearchFrom.SetText(tvSearchTo.GetText().ToString());
                tvSearchTo.SetText(textFrom);
                mSearchAdapterFrom.needAutocomplete = true;
                mSearchAdapterTo.needAutocomplete = true;
            }
        }

        private sealed class AnonymousOnEditorActionListener : OnEditorActionListener
        {
            public AnonymousOnEditorActionListener(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override bool OnEditorAction(TextView v, int actionId, KeyEvent @event)
            {
                if (actionId == EditorInfo.IME_ACTION_SEARCH)
                {
                    DoSearch();
                    return true;
                }

                return false;
            }
        }

        private sealed class AnonymousOnEditorActionListener1 : OnEditorActionListener
        {
            public AnonymousOnEditorActionListener1(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override bool OnEditorAction(TextView v, int actionId, KeyEvent @event)
            {
                if (actionId == EditorInfo.IME_ACTION_SEARCH)
                {
                    DoSearch();
                    return true;
                }

                return false;
            }
        }

        private sealed class AnonymousOnClickListener2 : OnClickListener
        {
            public AnonymousOnClickListener2(Search_Activity parent)
            {
                this.parent = parent;
            }

            private readonly Search_Activity parent;
            public override void OnClick(View v)
            {
                AddToFavorites();
            }
        }

        private void UpdateFavVisibility(string codeFrom, string codeTo)
        {
            if (mainDB.IsInFavorites(codeFrom, codeTo))
                mBtnAddToFavorites.SetVisibility(View.INVISIBLE);
            else
                mBtnAddToFavorites.SetVisibility(View.VISIBLE);
        }

        private void UpdateFavVisibility(Station from, Station to)
        {
            if (from == null || to == null || from.code.IsEmpty() || to.code.IsEmpty() || mainDB.IsInFavorites(from.code, to.code))
                mBtnAddToFavorites.SetVisibility(View.INVISIBLE);
            else
                mBtnAddToFavorites.SetVisibility(View.VISIBLE);
        }

        private bool CheckErrors(Station curFrom, Station curTo)
        {
            tilSearchFrom.SetError("");
            tilSearchFrom.SetErrorEnabled(false);
            if (tvSearchFrom.GetText().ToString().IsEmpty())
            {
                tilSearchFrom.SetErrorEnabled(true);
                tilSearchFrom.SetError(Multilanguage.enterStation);
                return true;
            }

            if (curFrom.code.IsEmpty())
            {
                tilSearchFrom.SetErrorEnabled(true);
                tilSearchFrom.SetError(Multilanguage.stationNotFound);
                return true;
            }

            if (tvSearchTo.GetText().ToString().IsEmpty())
            {
                tilSearchTo.SetErrorEnabled(true);
                tilSearchTo.SetError(Multilanguage.enterStation);
                return true;
            }

            if (curTo.code.IsEmpty())
            {
                tilSearchTo.SetErrorEnabled(true);
                tilSearchTo.SetError(Multilanguage.stationNotFound);
                return true;
            }

            return false;
        }

        private void AddToFavorites()
        {
            Station curFrom = mSearchAdapterFrom.mSelectedItem;
            Station curTo = mSearchAdapterTo.mSelectedItem;
            if (CheckErrors(curFrom, curTo))
                return;
            DB mainDB = new DB(this);
            mainDB.AddToFavorite(new Favorite(curFrom.code, curFrom.name, curTo.code, curTo.name));
            mainDB.Close();
            Toast.MakeText(GetApplication(), Multilanguage.addedToFav, Toast.LENGTH_SHORT).Show();
        }

        private void SetDataText()
        {
            Calendar c = Calendar.GetInstance();
            c.SetTimeInMillis(mCurrentSearch.date);
            tvDate.SetText(Util.GetDateRepresentation(c[Calendar.YEAR], c[Calendar.MONTH], c[Calendar.DAY_OF_MONTH]));
        }

        private void ShowDataPicker()
        {
            DialogFragment newFragment = new Search_DatePickerFragment();
            Bundle args = new Bundle();
            args.PutLong("DATE", mCurrentSearch.date);
            newFragment.SetArguments(args);
            newFragment.Show(GetFragmentManager(), "date");
        }

        public override void OnDatePicked(int year, int monthOfYear, int dayOfMonth)
        {
            Calendar c = Calendar.GetInstance();
            c.Set(year, monthOfYear, dayOfMonth);
            mCurrentSearch.date = c.GetTimeInMillis();
            tvDate.SetText(Util.GetDateRepresentation(year, monthOfYear, dayOfMonth));
        }

        class OnClear : OnTouchListener
        {
            public override bool OnTouch(View v, MotionEvent @event)
            {
                if (@event.GetAction() != MotionEvent.ACTION_UP)
                    return false;
                if (@event.GetX() > v.GetWidth() - imgWidth)
                {
                    if (v == tvSearchFrom)
                    {
                        tvSearchFrom.SetText("");
                        mSearchAdapterFrom.mSelectedItem.Clear();
                        mBtnAddToFavorites.SetVisibility(View.INVISIBLE);
                    }
                    else
                    {
                        tvSearchTo.SetText("");
                        mSearchAdapterTo.mSelectedItem.Clear();
                        mBtnAddToFavorites.SetVisibility(View.INVISIBLE);
                    }
                }

                return false;
            }
        }

        private void Init_FAB()
        {
            FloatingActionButton fab = (FloatingActionButton)FindViewById(R.id.search_fab);
            fab.SetOnClickListener(new AnonymousOnClickListener3(this));
        }

        private sealed class AnonymousOnClickListener3 : OnClickListener
        {
            public AnonymousOnClickListener3(OnClear parent)
            {
                this.parent = parent;
            }

            private readonly OnClear parent;
            public override void OnClick(View view)
            {
                DoSearch();
            }
        }

        private void DoSearch()
        {
            Station curFrom = mSearchAdapterFrom.mSelectedItem;
            Station curTo = mSearchAdapterTo.mSelectedItem;
            if (CheckErrors(curFrom, curTo))
                return;
            if (curFrom == null || curTo == null || curFrom.code.IsEmpty() || curTo.code.IsEmpty())
                return;
            Intent intent = new Intent();
            intent.PutExtra(ARGS_FROM_CODE, curFrom.code);
            intent.PutExtra(ARGS_FROM_NAME, curFrom.name);
            intent.PutExtra(ARGS_TO_CODE, curTo.code);
            intent.PutExtra(ARGS_TO_NAME, curTo.name);
            intent.PutExtra(ARGS_DATE, mCurrentSearch.date);
            SetResult(RESULT_OK, intent);

            //        forceCloseKeyboard();
            Finish();
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
            base.OnDestroy();
            mainDB.Close();
        }
    }
}