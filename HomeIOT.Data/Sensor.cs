using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Data
{
    public class Sensor
    {
        public int SensorId { get; set; }
        public string Identificator { get; set; }
        public Unit Units { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFavorited { get; set; }

        public ICollection<SensorData> Data { get; set; }
        public ICollection<SensorInSensorGroup> Groups { get; set; }
        public ICollection<ApplicationEvent> Events { get; set; }
    }
}