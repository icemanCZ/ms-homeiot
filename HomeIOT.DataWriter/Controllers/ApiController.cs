using HomeIOT.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.DataWriter.Controllers
{
    [ApiController]
    [Route("")]
    public class ApiController : ControllerBase
    {
        private readonly DBContext _context;

        public ApiController(DBContext context)
        {
            _context = context;
        }

        [Route("Write")]
        public IActionResult Write(string sensorIdentificator, float value)
        {
            if (string.IsNullOrWhiteSpace(sensorIdentificator))
                return UnprocessableEntity();

            if (value == 85.5)
                return Forbid();

            var sensorId = _context.Sensors.FirstOrDefault(x => x.Identificator == sensorIdentificator)?.SensorId;
            if (sensorId == null)
            {
                var sensor = new Sensor();
                sensor.Name = "new sensor";
                sensor.Identificator = sensorIdentificator;
                sensor.Units = Unit.Unknown;
                _context.Sensors.Add(sensor);
                _context.SaveChanges();
                sensorId = sensor.SensorId;

                //_eventService.NewSensorRegistered(sensorId.Value);
            }

            var data = new SensorData();
            data.SensorId = sensorId.Value;
            data.Timestamp = DateTime.Now;
            data.Value = value;
            _context.SensorData.Add(data);
            _context.SaveChanges();

            //_eventService.NotifySensorConnected(sensorId.Value);

            return Ok();
        }
    }
}