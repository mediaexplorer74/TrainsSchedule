using Android.App;
//using Android.Support.V7.Widget;
using Android.Widget;
using Java.Util;
using TrainsSchedule.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Android.Views;

namespace TrainsSchedule
{
    public class FavoritesListAdapter : Adapter<FavoritesListAdapter.ViewHolder>
    {
        public interface OnRemoveClick
        {
            void OnRemoveClick(int position);
        }

        public interface OnFavoriteClick
        {
            void OnFavoriteClick(Favorite favorite);
        }

        readonly OnRemoveClick mOnRemoveClickCallback;
        readonly OnFavoriteClick mOnFavoriteClickCallback;
        public FavoritesListAdapter(Activity activity, MainActivityTabFragment context)
        {
            mOnRemoveClickCallback = context;
            mOnFavoriteClickCallback = (OnFavoriteClick)activity;
        }

        public IList<Favorite> mFavorites;
        public class ViewHolder : ViewHolder
        {
            public readonly TextView favText;
            public readonly ImageView favImg;
            public ViewHolder(View v) : base(v)
            {
                favText = (TextView)v.FindViewById(R.id.tvFavFrom);
                favImg = (ImageView)v.FindViewById(R.id.imgFavClear);
            }
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.GetContext()).Inflate(R.layout.favorites_list_item, parent, false);
            ViewHolder mViewHolder = new ViewHolder(view);
            mViewHolder.favImg.SetOnClickListener(new AnonymousOnClickListener(this));
            mViewHolder.favText.SetOnClickListener(new AnonymousOnClickListener1(this));
            return mViewHolder;
        }

        private sealed class AnonymousOnClickListener : OnClickListener
        {
            public AnonymousOnClickListener(ViewHolder parent)
            {
                this.parent = parent;
            }

            private readonly ViewHolder parent;
            public override void OnClick(View v)
            {
                mOnRemoveClickCallback.OnRemoveClick(mViewHolder.GetAdapterPosition());
            }
        }

        private sealed class AnonymousOnClickListener1 : OnClickListener
        {
            public AnonymousOnClickListener1(ViewHolder parent)
            {
                this.parent = parent;
            }

            private readonly ViewHolder parent;
            public override void OnClick(View v)
            {
                mOnFavoriteClickCallback.OnFavoriteClick(mFavorites[mViewHolder.GetAdapterPosition()]);
            }
        }

        public override void OnBindViewHolder(ViewHolder holder, int position)
        {
            Favorite currentStop = mFavorites[position];
            holder.favText.SetText(currentStop.fromName + " - " + currentStop.toName);
        }

        public override int GetItemCount()
        {
            return mFavorites.Count;
        }
    }
}