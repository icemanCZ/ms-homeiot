using HomeIOT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Models
{
    public class ChartDataViewModel
    {
        public int SensorId { get; set; }
        public string SensorName { get; set; }
        public Unit Unit { get; set; }
        public IEnumerable<SensorDataViewModel> Data { get; set; }
    }
}