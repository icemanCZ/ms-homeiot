using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Data
{
    public enum Unit : byte
    {
        DegreeCelsius = 0,
        Unknown = byte.MaxValue,
    }

    public static class UnitExtensions
    {
        public static string GetText(this Unit t)
        {
            switch (t)
            {
                case Unit.DegreeCelsius:
                    return "°C";

                case Unit.Unknown:
                    return "";

                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetIconPath(this Unit t)
        {
            switch (t)
            {
                case Unit.DegreeCelsius:
                    return "/images/sensor_temperature.png";

                case Unit.Unknown:
                    return "/images/sensor_unknown.png";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}