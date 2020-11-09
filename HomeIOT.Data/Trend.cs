using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Data
{
    public enum Trend
    {
        Increasing,
        Decreasing,
        Consistent
    }

    public static class TrendExtensions
    {
        public static string GetIconPath(this Trend t)
        {
            switch (t)
            {
                case Trend.Increasing:
                    return "/images/trend_increasing.png";

                case Trend.Decreasing:
                    return "/images/trend_decreasing.png";

                case Trend.Consistent:
                    return "/images/trend_consistent.png";

                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetText(this Trend t)
        {
            switch (t)
            {
                case Trend.Increasing:
                    return "Stoupající";

                case Trend.Decreasing:
                    return "Klesající";

                case Trend.Consistent:
                    return "Ustálený";

                default:
                    throw new NotImplementedException();
            }
        }

        public static Trend GetTrend(float current, float avg)
        {
            if (current > avg)
                return Trend.Increasing;
            else if (current < avg)
                return Trend.Decreasing;
            else
                return Trend.Consistent;
        }
    }
}