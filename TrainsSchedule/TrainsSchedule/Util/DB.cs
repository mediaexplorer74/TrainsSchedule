using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Java.Util;
using TrainsSchedule.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Util
{
    public class DB : SQLiteOpenHelper
    {
        private static readonly string DB_NAME = "main.db";
        private static readonly int DB_VERSION = 23;
        private static readonly string TABLE_FAVORITES = "favorites";
        private static readonly string FIELD_STATION_FROM_CODE = "from_code";
        private static readonly string FIELD_STATION_FROM_NAME = "from_name";
        private static readonly string FIELD_STATION_TO_CODE = "to_code";
        private static readonly string FIELD_STATION_TO_NAME = "to_name";
        public DB(Context context) : base(context, DB_NAME, null, DB_VERSION)
        {
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            string queryString = String.Format("CREATE TABLE %s (%s TEXT, %s TEXT, %s TEXT, %s TEXT)", TABLE_FAVORITES, FIELD_STATION_FROM_CODE, FIELD_STATION_FROM_NAME, FIELD_STATION_TO_CODE, FIELD_STATION_TO_NAME);
            db.ExecSQL(queryString);
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            if (oldVersion != newVersion)
            {
                db.ExecSQL("DROP TABLE IF EXISTS " + TABLE_FAVORITES);
                OnCreate(db);
            }
        }

        public virtual void AddToFavorite(Favorite favorite)
        {
            SQLiteDatabase db = this.GetWritableDatabase();
            ContentValues row = new ContentValues();
            row.Put(FIELD_STATION_FROM_CODE, favorite.fromCode);
            row.Put(FIELD_STATION_FROM_NAME, favorite.fromName);
            row.Put(FIELD_STATION_TO_CODE, favorite.toCode);
            row.Put(FIELD_STATION_TO_NAME, favorite.toName);
            int updated = db.Update(TABLE_FAVORITES, row, FIELD_STATION_FROM_CODE + " =? AND " + FIELD_STATION_TO_CODE + " =? ", new string { favorite.fromCode, favorite.toCode });
            S.L("update: " + updated);
            if (updated == 0)
            {
                long res = db.Insert(TABLE_FAVORITES, null, row);
                S.L("insert: " + res);
            }
        }

        public virtual IList<Favorite> GetFavoritesList()
        {
            SQLiteDatabase db = this.GetReadableDatabase();
            Cursor cursor = db.RawQuery("SELECT " + FIELD_STATION_FROM_CODE + ", " + FIELD_STATION_FROM_NAME + ", " + FIELD_STATION_TO_CODE + ", " + FIELD_STATION_TO_NAME + " FROM " + TABLE_FAVORITES + " ORDER BY " + FIELD_STATION_FROM_NAME + " DESC", null);
            IList<Favorite> result = new List(cursor.GetCount());
            while (cursor.MoveToNext())
            {
                result.Add(new Favorite(cursor.GetString(0), cursor.GetString(1), cursor.GetString(2), cursor.GetString(3)));
            }

            cursor.Close();
            return result;
        }

        public virtual void DeleteFavorite(Favorite favorite)
        {
            SQLiteDatabase db = this.GetWritableDatabase();
            long res = db.Delete(TABLE_FAVORITES, FIELD_STATION_FROM_CODE + " =? AND " + FIELD_STATION_TO_CODE + " =? ", new string { favorite.fromCode, favorite.toCode });
            S.L("delete: " + res);
        }

        public virtual bool IsInFavorites(string codeFrom, string codeTo)
        {
            SQLiteDatabase db = this.GetReadableDatabase();
            Cursor cursor = db.RawQuery("SELECT 1 FROM " + TABLE_FAVORITES + " WHERE " + FIELD_STATION_FROM_CODE + " = ? AND " + FIELD_STATION_TO_CODE + " =? LIMIT 1", new string { codeFrom, codeTo });
            bool res = false;
            if (cursor.GetCount() > 0)
            {
                res = true;
            }

            cursor.Close();
            return res;
        }
    }
}