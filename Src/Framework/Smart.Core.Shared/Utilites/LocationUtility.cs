using System;

namespace Smart.Core.Utilites
{
    /// <summary>
    /// 位置信息帮助类
    /// </summary>
    public static class LocationUtility
    {
        /// <summary>
        /// 地球半径 (米)
        /// </summary>
        public const double EARTH_RADIUS = 6378137.0;

        /// <summary>
        /// 计算两个经纬度之间的直接距离
        /// </summary>
        public static double GetDistance(Geography start, Geography end)
        {
            double s = Math.Acos(Math.Cos(start.LatRadians) * Math.Cos(end.LatRadians) * Math.Cos(start.LngRadians - end.LngRadians) + Math.Sin(start.LatRadians) * Math.Sin(end.LatRadians));
            if (double.IsNaN(s)) return 0;
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        /// <summary>
        /// 以一个经纬度为中心计算出四个顶点
        /// </summary>
        /// <param name="point">半径(米)</param>
        /// <param name="distance">半径(米)</param>
        /// <returns></returns>
        public static Range GetCoordinates(Geography point, double distance)
        {
            double dlng = Math.Abs(ToDegrees(2 * Math.Asin(Math.Sin(distance / (2 * EARTH_RADIUS)) / Math.Cos(point.Latitude))));
            double dlat = Math.Abs(ToDegrees(distance / EARTH_RADIUS));
            return new Range
            {
                Left = Math.Round(point.Longitude - dlng, 6),
                Right = Math.Round(point.Longitude + dlng, 6),
                Top = Math.Round(point.Latitude + dlat, 6),
                Bottom = Math.Round(point.Latitude - dlat, 6)
            };
        }

        /// <summary>
        /// 弧度转换为角度数公式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static double ToDegrees(double value)
        {
            return value * (180 / Math.PI);
        }
    }

    /// <summary>
    /// 地理位置（经伟度）
    /// </summary>
    public class Geography
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public Geography(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }
        /// <summary>
        /// 获取或设置纬度 X
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// 获取或设置经度 Y
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度－弧度
        /// </summary>
        public double LatRadians { get { return this.GetRadians(this.Latitude); } }

        /// <summary>
        /// 经度－弧度
        /// </summary>
        public double LngRadians { get { return this.GetRadians(this.Longitude); } }

        /// <summary>
        /// 获取弧度
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double GetRadians(double value)
        {
            return value * Math.PI / 180.0;
        }
    }

    /// <summary>
    /// 范围 上下四周的界限
    /// </summary>
    public class Range
    {
        /// <summary>
        /// 左
        /// </summary>
        public double Left { get; set; }
        /// <summary>
        /// 上
        /// </summary>
        public double Top { get; set; }
        /// <summary>
        /// 右
        /// </summary>
        public double Right { get; set; }
        /// <summary>
        /// 下
        /// </summary>
        public double Bottom { get; set; }
    }
}
