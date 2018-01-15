using System;
using System.Security.Cryptography;

namespace Smart.Core.Extensions
{
    /// <summary>
    /// 随机数扩展方法
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary> 
        /// 产生一个非负数且最小值在 min 以上最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="random"></param> 
        /// <param name="min">最小值</param> 
        /// <param name="max">最大值</param> 
        /// <returns></returns>
        public static long Next(this Random random, long min, long max)
        {
            return RNG.Next(min, max);
        }

        /// <summary> 
        /// 产生一个非负数且最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="random"></param> 
        /// <param name="max">最大值</param> 
        public static long Next(this Random random, long max)
        {
            return RNG.Next(max);
        }
    }


    internal class RNG
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[8];

        /// <summary> 
        /// 产生一个非负数的乱数 
        /// </summary> 
        public static long Next()
        {
            rngp.GetBytes(rb);
            long value = BitConverter.ToInt64(rb, 0);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary> 
        /// 产生一个非负数且最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="max">最大值</param> 
        public static long Next(long max)
        {
            rngp.GetBytes(rb);
            long value = BitConverter.ToInt64(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary> 
        /// 产生一个非负数且最小值在 min 以上最大值在 max 以下的乱数 
        /// </summary> 
        /// <param name="min">最小值</param> 
        /// <param name="max">最大值</param> 
        public static long Next(long min, long max)
        {
            long value = Next(max - min) + min;
            return value;
        }
    }
}
