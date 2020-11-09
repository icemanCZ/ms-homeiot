using HomeIOT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Models
{
    public class ApplicationEventViewModel
    {
        public int ApplicationEventId { get; set; }
        public ApplicationEventType EventType { get; set; }

        [Display(Name = "Čas")]
        public DateTime Timestamp { get; set; }

        public int SensorId { get; set; }

        [Display(Name = "Senzor")]
        public string SensorName { get; set; }

        public Unit SensorUnits { get; set; }
        public bool Resolved { get; set; }
        public DateTime? ResolvedTimestamp { get; set; }

        public string GetIconPath()
        {
            switch (EventType)
            {
                case ApplicationEventType.NewSensorRegistered:
                    switch (SensorUnits)
                    {
                        case Unit.DegreeCelsius:
                            return Resolved ? "/images/temperatureSensor_registered_resolved.png" : "/images/temperatureSensor_registered.png";

                        case Unit.Unknown:
                            return Resolved ? "/images/unknownSensor_registered_resolved.png" : "/images/unknownSensor_registered.png";
                    }
                    break;

                case ApplicationEventType.SensorConnectionLost:
                    switch (SensorUnits)
                    {
                        case Unit.DegreeCelsius:
                            return Resolved ? "/images/temperatureSensor_disconnected_resolved.png" : "/images/temperatureSensor_disconnected.png";

                        case Unit.Unknown:
                            return Resolved ? "/images/unknownSensor_disconnected_resolved.png" : "/images/unknownSensor_disconnected.png";
                    }
                    break;
            }

            return string.Empty;
        }
    }
}