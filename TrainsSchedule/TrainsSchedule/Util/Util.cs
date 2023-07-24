using System.Globalization;
using TrainsSchedule.Util;

//using Java.Text.DateFormat;
//using Java.Text.DateFormatSymbols;
//using Java.Text.ParseException;
//using Java.Text.SimpleDateFormat;
//using Java.Util.Calendar;
//using Java.Util.Date;
//using Java.Util.GregorianCalendar;
//using Java.Util.Locale;

public class Util
{

    public static string todayStr() 
    {
        Calendar calendar = new Calendar();//.getInstance();
        string sb = String.valueOf(calendar.get(Calendar.YEAR)) +
                "-" +
                (calendar.get(Calendar.MONTH) + 1) +
                "-" +
                calendar.get(Calendar.DAY_OF_MONTH);
        return sb;
    }

    public static string tomorrowStr()
    {
        Calendar calendar = Calendar.getInstance();
        calendar.add(Calendar.DAY_OF_MONTH, 1);

        string sb = String.valueOf(calendar.get(Calendar.YEAR)) +
                "-" +
                (calendar.get(Calendar.MONTH) + 1) +
                "-" +
                calendar.get(Calendar.DAY_OF_MONTH);
        return sb;
    }

    public static string todayDayOfWeek() 
    {
        Calendar calendar = Calendar.getInstance();
        return "" + calendar.get(Calendar.DAY_OF_WEEK);
    }

    public static string tomorrowDayOfWeek()
    {
        Calendar calendar = Calendar.getInstance();
        calendar.add(Calendar.DAY_OF_MONTH, 1);
        return "" + calendar.get(Calendar.DAY_OF_WEEK);
    }

    public static bool notEmpty(Object o) 
    {
        if (o == null) return false;
        return !(o instanceof String && ((String) o).isEmpty());

    }

    public static string getDateRepresentation(long date)
    {

        GregorianCalendar cur = new GregorianCalendar();
        GregorianCalendar c = new GregorianCalendar();
        c.setTimeInMillis(date);
        if (c.get(Calendar.YEAR) == cur.get(Calendar.YEAR) &&
                c.get(Calendar.MONTH) == cur.get(Calendar.MONTH) &&
                c.get(Calendar.DAY_OF_MONTH) == cur.get(Calendar.DAY_OF_MONTH)) 
        {
            return Multilanguage.onToday;
        }

        cur.add(Calendar.DAY_OF_MONTH, 1);
        if (c.get(Calendar.YEAR) == cur.get(Calendar.YEAR) &&
                c.get(Calendar.MONTH) == cur.get(Calendar.MONTH) &&
                c.get(Calendar.DAY_OF_MONTH) == cur.get(Calendar.DAY_OF_MONTH)) {
            return Multilanguage.onTomorrow;
        }

        Date dDate = new Date(date);
        SimpleDateFormat df = new SimpleDateFormat("dd MMMM", Locale.getDefault());
        return Multilanguage.on + " " + df.format(dDate);
    }

    public static string getDateRepresentation(int year, int month, int day) 
    {
        GregorianCalendar c = new GregorianCalendar();
        c.set(Calendar.YEAR, year);
        c.set(Calendar.MONTH, month);
        c.set(Calendar.DAY_OF_MONTH, day);

        return getDateRepresentation(c.getTimeInMillis());
    }

    public static string getDateForSearch(long date)
    {
        SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd", Locale.US);
        return df.format(new Date(date));
    }

    public static int getDayOfWeek(String date) 
    {
        DateFormat format = new SimpleDateFormat("yyyy-MM-dd", Locale.ENGLISH);
        Calendar c = Calendar.getInstance();
        try 
        {
            c.setTime(format.parse(date));
        } catch (ParseException ex) 
        {
            ex.printStackTrace();
        }
        return c.get(Calendar.DAY_OF_WEEK);
    }

    public static int getDayOfWeek(long date) 
    {
        Calendar c = Calendar.getInstance();
        c.setTimeInMillis(date);
        return c.get(Calendar.DAY_OF_WEEK);
    }
}
