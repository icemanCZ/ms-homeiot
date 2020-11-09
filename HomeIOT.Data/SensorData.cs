using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Data
{
    public class SensorData
    {
        public int ID { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
    }
}