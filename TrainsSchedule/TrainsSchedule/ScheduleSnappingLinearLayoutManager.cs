using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule
{
    public class ScheduleSnappingLinearLayoutManager : LinearLayoutManager
    {
        private static readonly float MILLISECONDS_PER_INCH = 5F;
        public ScheduleSnappingLinearLayoutManager(Context context) : base(context)
        {
        }

        public override void SmoothScrollToPosition(RecyclerView recyclerView, RecyclerView.State state, int position)
        {
            RecyclerView.SmoothScroller smoothScroller = new TopSnappedSmoothScroller(recyclerView.GetContext());
            smoothScroller.SetTargetPosition(position);
            StartSmoothScroll(smoothScroller);
        }

        private class TopSnappedSmoothScroller : LinearSmoothScroller
        {
            public TopSnappedSmoothScroller(Context context) : base(context)
            {
            }

            protected override float CalculateSpeedPerPixel(DisplayMetrics displayMetrics)
            {
                return MILLISECONDS_PER_INCH / displayMetrics.densityDpi; //           return super.calculateSpeedPerPixel(displayMetrics);
            }

            public override PointF ComputeScrollVectorForPosition(int targetPosition)
            {
                return this.ComputeScrollVectorForPosition(targetPosition);
            }

            protected override int GetVerticalSnapPreference()
            {
                return SNAP_TO_START;
            }
        }
    }
}