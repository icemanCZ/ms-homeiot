using System;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HomeIOT.TemperatureTransfer
{
    public static class TemperatureTransfer
    {
        [FunctionName("TemperatureTransfer")]
        public static void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            using (var wc = new WebClient())
            {
                var rawData = wc.DownloadString("http://api.openweathermap.org/data/2.5/weather?lon=15.4301626&lat=50.2905017&appid=3d280fac1cb2bf1738bf16302a9bdf5e&units=metric");
                var parsedData = JsonConvert.DeserializeObject<dynamic>(rawData);
                wc.DownloadString("http://homeiot.aspifyhost.cz/api/write?sensoridentificator=temperatureExt&value=" + ((float)parsedData.main.temp).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
        }
    }
}