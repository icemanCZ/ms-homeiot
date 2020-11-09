using HomeIOT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Models
{
    public class SensorDataViewModel
    {
        public int SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }

        public SensorDataViewModel(int sensorId, DateTime timestamp, float value)
        {
            SensorId = sensorId;
            Timestamp = timestamp;
            Value = value;
        }
    }
}