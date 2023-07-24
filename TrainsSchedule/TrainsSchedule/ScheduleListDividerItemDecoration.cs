using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawable;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class ScheduleListDividerItemDecoration : ItemDecoration
    {
        private Drawable mDivider;
        private bool mShowFirstDivider = false;
        private bool mShowLastDivider = false;
        int mOrientation = -1;
        public ScheduleListDividerItemDecoration(Context context, 
            AttributeSet attrs)
        {
            TypedArray a = context.ObtainStyledAttributes(attrs, new int 
            { android.R.attr.listDivider });

            mDivider = a.GetDrawable(0);
            a.Recycle();
        }

        public ScheduleListDividerItemDecoration(Context context, 
            AttributeSet attrs, bool showFirstDivider, bool showLastDivider)
            : this(context, attrs)
        {
            mShowFirstDivider = showFirstDivider;
            mShowLastDivider = showLastDivider;
        }

        public ScheduleListDividerItemDecoration(Context context, int resId)
        {
            mDivider = ContextCompat.GetDrawable(context, resId);
        }

        public ScheduleListDividerItemDecoration(Context context, int resId, 
            bool showFirstDivider, bool showLastDivider) : this(context, resId)
        {
            mShowFirstDivider = showFirstDivider;
            mShowLastDivider = showLastDivider;
        }

        public ScheduleListDividerItemDecoration(Drawable divider)
        {
            mDivider = divider;
        }

        public ScheduleListDividerItemDecoration(Drawable divider, 
            bool showFirstDivider, bool showLastDivider) : this(divider)
        {
            mShowFirstDivider = showFirstDivider;
            mShowLastDivider = showLastDivider;
        }

        public override void GetItemOffsets(Rect outRect, View view, 
            RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);
            if (mDivider == null)
            {
                return;
            }

            int position = parent.GetChildAdapterPosition(view);
            if (position == RecyclerView.NO_POSITION || (position == 0 && 
                !mShowFirstDivider))
            {
                return;
            }

            if (mOrientation == -1)
                GetOrientation(parent);
            if (mOrientation == LinearLayoutManager.VERTICAL)
            {
                outRect.top = mDivider.GetIntrinsicHeight();
                if (mShowLastDivider && position == (state.GetItemCount() - 1))
                {
                    outRect.bottom = outRect.top;
                }
            }
            else
            {
                outRect.left = mDivider.GetIntrinsicWidth();
                if (mShowLastDivider && position == (state.GetItemCount() - 1))
                {
                    outRect.right = outRect.left;
                }
            }
        }

        public override void OnDrawOver(Canvas c, RecyclerView parent, 
            RecyclerView.State state)
        {
            if (mDivider == null)
            {
                base.OnDrawOver(c, parent, state);
                return;
            }


            // Initialization needed to avoid compiler warning
            int left = 0, right = 0, top = 0, bottom = 0, size;
            int orientation = mOrientation != -1 ? mOrientation
                : GetOrientation(parent);

            int childCount = parent.GetChildCount();

            if (orientation == LinearLayoutManager.VERTICAL)
            {
                size = mDivider.GetIntrinsicHeight();
                left = parent.GetPaddingLeft();
                right = parent.GetWidth() - parent.GetPaddingRight();
            } //horizontal
            else
            {

                //horizontal
                size = mDivider.GetIntrinsicWidth();
                top = parent.GetPaddingTop();
                bottom = parent.GetHeight() - parent.GetPaddingBottom();
            }

            for (int i = mShowFirstDivider ? 0 : 1; i < childCount; i++)
            {
                View child = parent.GetChildAt(i);
                RecyclerView.LayoutParams params = 
                    (RecyclerView.LayoutParams)child.GetLayoutParams();

                if (orientation == LinearLayoutManager.VERTICAL)
                {
                    top = child.GetTop() - @params.topMargin - size;
                    bottom = top + size;
                } //horizontal
                else
                {

                    //horizontal
                    left = child.GetLeft() - @params.leftMargin;
                    right = left + size;
                }

                mDivider.SetBounds(left, top, right, bottom);
                mDivider.Draw(c);
            }


            // show last divider
            if (mShowLastDivider && childCount > 0)
            {
                View child = parent.GetChildAt(childCount - 1);
                if (parent.GetChildAdapterPosition(child) == (state.GetItemCount() - 1))
                {
                    RecyclerView.LayoutParams params = 
                        (RecyclerView.LayoutParams)child.GetLayoutParams();
                    if (orientation == LinearLayoutManager.VERTICAL)
                    {
                        top = child.GetBottom() + @params.bottomMargin;
                        bottom = top + size;
                    } // horizontal
                    else
                    {

                        // horizontal
                        left = child.GetRight() + @params.rightMargin;
                        right = left + size;
                    }

                    mDivider.SetBounds(left, top, right, bottom);
                    mDivider.Draw(c);
                }
            }
        }

        private int GetOrientation(RecyclerView parent)
        {
            if (mOrientation == -1)
            {
                if (parent.GetLayoutManager() is LinearLayoutManager)
                {
                    LinearLayoutManager layoutManager = (LinearLayoutManager)parent.GetLayoutManager();
                    mOrientation = layoutManager.GetOrientation();
                }
                else
                {
                    throw new InvalidOperationException("DividerItemDecoration can only be used with a LinearLayoutManager.");
                }
            }

            return mOrientation;
        }
    }
}