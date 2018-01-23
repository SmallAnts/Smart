using System;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 日期时间扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 计算年龄 (DateTime.Now-birthDate)
        /// </summary>
        /// <param name="birthDate">出生日期</param>
        /// <param name="calcTime">是事计算时间部分，默认false</param>
        /// <param name="calcSecond">是否计算秒，默认false</param>
        /// <returns>年龄</returns>
        public static TimeInfo GetAge(this DateTime birthDate, bool calcTime = false, bool calcSecond = false)
        {
            return birthDate.GetAge(DateTime.Now, calcTime, calcSecond);
        }

        /// <summary>
        /// 计算年龄 (now-birthDate)
        /// </summary>
        /// <param name="birthDate">出生日期</param>
        /// <param name="now">当前日期</param>
        /// <param name="calcTime">是事计算时间部分，默认false</param>
        /// <param name="calcSecond">是否计算秒，默认false</param>
        /// <returns>年龄</returns>
        public static TimeInfo GetAge(this DateTime birthDate, DateTime now, bool calcTime = false, bool calcSecond = false)
        {
            if (now < birthDate)
            {
                throw new ArgumentException("年龄计算日期不能小于出生日期！");
            }
            return GetTimeDiff(birthDate, now, calcTime: calcTime, calcSecond: calcSecond);
        }

        /// <summary>
        /// 与当前日期计算日期差（DateTime.Now－theDate）
        /// </summary>
        /// <param name="theDate">被计算日期</param>
        /// <returns></returns>
        public static TimeInfo GetDateDiff(this DateTime theDate)
        {
            return GetTimeDiff(theDate, DateTime.Now, false, false, false, false);
        }

        /// <summary>
        /// 与指定日期计算日期差（date－theDate）
        /// </summary>
        /// <param name="theDate">被计算日期</param>
        /// <param name="date">计算日期</param>
        /// <returns></returns>
        public static TimeInfo GetDateDiff(this DateTime theDate, DateTime date)
        {
            return GetTimeDiff(theDate, date, false, false, false, false);
        }

        /// <summary>
        ///  与当前日期计算时间差（DateTime.Now－theDate）
        /// </summary>
        /// <param name="theTime">被计算日期</param>
        /// <param name="calcTime">是事计算时间部分，默认true</param>
        /// <param name="calcHour">是否计算小时，默认true</param>
        /// <param name="calcMinute">是否计算分，默认true</param>
        /// <param name="calcSecond">是否计算秒，默认true</param>
        /// <returns></returns>
        public static TimeInfo GetTimeDiff(this DateTime theTime,
            bool calcTime = true, bool calcHour = true, bool calcMinute = true, bool calcSecond = true)
        {
            return GetTimeDiff(theTime, DateTime.Now, calcTime, calcHour, calcMinute, calcSecond);
        }

        /// <summary>
        /// 与指定的时间计算时间差（date－theDate）
        /// </summary>
        /// <param name="theTime">被计算日期</param>
        /// <param name="date">计算日期</param>
        /// <param name="calcTime">是事计算时间部分，默认true</param>
        /// <param name="calcHour">是否计算小时，默认true</param>
        /// <param name="calcMinute">是否计算分，默认true</param>
        /// <param name="calcSecond">是否计算秒，默认true</param>
        /// <returns></returns>
        public static TimeInfo GetTimeDiff(this DateTime theTime, DateTime date,
            bool calcTime = true, bool calcHour = true, bool calcMinute = true, bool calcSecond = true)
        {
            var timeinfo = new TimeInfo();
            timeinfo.Source = theTime;
            timeinfo.Now = date;
            DateTime? temp = null;
            if (date < theTime)
            {
                temp = theTime;
                theTime = date;
                date = temp.Value;
            }

            #region 计算秒
            if (calcSecond && calcTime)
            {
                timeinfo.Second = date.Second - theTime.Second;
                if (timeinfo.Second < 0)
                {
                    timeinfo.Second += 60;
                    timeinfo.Minute -= 1;
                }
            }
            #endregion

            #region 计算分
            if (calcSecond && calcTime)
            {
                timeinfo.Minute += date.Minute - theTime.Minute;
                if (timeinfo.Minute < 0)
                {
                    timeinfo.Minute += 60;
                    timeinfo.Hour -= 1;
                }
            }
            #endregion

            #region 计算时
            if (calcHour && calcTime)
            {
                timeinfo.Hour += date.Hour - theTime.Hour;
                if (timeinfo.Hour < 0)
                {
                    timeinfo.Hour += 24;
                    timeinfo.Day -= 1;
                }
            }
            #endregion

            #region 计算天
            var theMonthDays = DateTime.DaysInMonth(theTime.Year, theTime.Month);
            // 判断出生日期和计算日期是不是都是月份的最后一天
            // 出生月总天数大于当前月总天数的月底做满月处理,
            // 如: 2000-03-31 到 2000-04-30 做为满月，5月份则要31号才做为满月,不计算天数
            if (!(theTime.Day + timeinfo.Day == theMonthDays && date.Day == DateTime.DaysInMonth(date.Year, date.Month)))
            {
                timeinfo.Day += date.Day - theTime.Day;
            }
            if (timeinfo.Day < 0)
            {
                var nowPrevMonth = date.AddMonths(-1); // 当前日期上月日期
                var prevMonthDays = DateTime.DaysInMonth(nowPrevMonth.Year, nowPrevMonth.Month);
                // 如果借过来的月的总天数小于被减月份的总天数，则以被减月份的总天数为准
                // 如:  2013-01-20 到 2013-03-10 , 此时借的是2月份, 小于出生月1月份的总天数,则天数为 10-20+31=21天
                timeinfo.Day += Math.Max(prevMonthDays, theMonthDays);
                timeinfo.Month -= 1;
            }
            #endregion

            #region 计算月
            timeinfo.Month += date.Month - theTime.Month;
            if (timeinfo.Month < 0) // 月数不足借年
            {
                timeinfo.Month += 12;
                timeinfo.Year -= 1;
            }
            #endregion

            #region 计算年
            timeinfo.Year += date.Year - theTime.Year;
            #endregion

            if (temp.HasValue)
            {
                timeinfo.Second *= -1;
                timeinfo.Minute *= -1;
                timeinfo.Hour *= -1;
                timeinfo.Day *= -1;
                timeinfo.Month *= -1;
                timeinfo.Year *= -1;
            }
            return timeinfo;
        }

        /// <summary>
        /// 设置时间到系统时间
        /// </summary>
        /// <param name="theTime"></param>
        /// <returns></returns>
        public static bool SetLocalTime(this DateTime theTime)
        {
            var systemtime = new NativeMethods.SystemTimeInfo(theTime);
            return NativeMethods.SetLocalTime(ref systemtime);
        }

        /// <summary>
        /// 将日期转换为 javascript 时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToJsTicks(this DateTime time)
        {
            return (time - new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks / 10000;
        }

        /// <summary>
        /// 将日期转换为指定格式的字符串,如果日期为 null, 则返回string.Empty
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="format">格式化字符串</param>
        /// <returns></returns>
        public static string ToString(this DateTime? dt, string format)
        {
            if (dt.HasValue) return dt.Value.ToString(format);
            else return string.Empty;
        }

        /// <summary>
        /// 将日期转换为 yyyy-MM-dd 格式的字符串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 将日期转换为 yyyy-MM-dd HH:mm:ss 格式的字符串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 将日期转换为 yyyy-MM-dd 格式的字符串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString("yyyy-MM-dd") : null;
        }

        /// <summary>
        /// 将日期转换为 yyyy-MM-dd  HH:mm:ss格式的字符串
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
        }
    }

    /// <summary>
    /// 时间信息
    /// </summary>
    public class TimeInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public TimeInfo() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        public TimeInfo(int year, int month, int day) : this(year, month, day, 0, 0, 0) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        public TimeInfo(int year, int month, int day, int hour, int minute, int second)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }
        /// <summary>
        /// 被计算的时间
        /// </summary>
        public DateTime Source { get; set; }
        /// <summary>
        /// 计算时的时间
        /// </summary>
        public DateTime Now { get; set; }
        /// <summary>
        ///  年
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 天
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 小时
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        ///  分
        /// </summary>
        public int Minute { get; set; }
        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get; set; }
        /// <summary>
        /// 返回 {0}年{1}月{2}日{3}时{4}分{5}秒 格式的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Year}年{Month}月{Day}天{Hour}时{Minute}分{Second}秒";
        }
        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Year.GetHashCode() + this.Month.GetHashCode() + this.Month.GetHashCode()
                + this.Hour.GetHashCode() + this.Minute.GetHashCode() + this.Second.GetHashCode();
        }
        /// <summary>
        ///  返回一个值，该值指示此实例是否与指定的对象相等。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
    }
}
