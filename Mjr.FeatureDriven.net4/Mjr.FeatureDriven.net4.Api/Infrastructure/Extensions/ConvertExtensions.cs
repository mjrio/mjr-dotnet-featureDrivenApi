using System;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions
{
    public static class ConvertExtensions
    {
        public static DateTime AsDateTime(this object o)
        {
            if (o == null || o == DBNull.Value)
                return DateTime.MinValue;
            return Convert.ToDateTime(o);
        }

        public static DateTime? AsNullableDateTime(this object o)
        {
            if (o == null || o == DBNull.Value)
                return null;
            return Convert.ToDateTime(o);
        }
        public static int AsInt(this object o, int value = 0)
        {
            if (o == null || o == DBNull.Value)
                return value;
            return Convert.ToInt32(o);
        }
        public static Int64 AsLong(this object o, long value = 0)
        {
            if (o == null || o == DBNull.Value)
                return value;
            return Convert.ToInt64(o);
        }
        public static double AsDouble(this object o, double value = 0)
        {
            if (o == null || o == DBNull.Value)
                return value;
            return Convert.ToDouble(o);
        }
        public static string AsString(this object o, string value = "")
        {
            return o as string ?? value;
        }
        /// <summary>
        /// Performs a AsString with trim. so no nullreference could occur.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AsTrimmedString(this object o, string value = "")
        {
            return o.AsString().Trim();
        }
    }
}
