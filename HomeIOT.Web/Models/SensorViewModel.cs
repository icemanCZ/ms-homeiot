using HomeIOT.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Models
{
    public class SensorViewModel
    {
        public int SensorId { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Unit Units { get; set; }
        public IEnumerable<int> Groups { get; set; }
        public SelectList AllGroups { get; set; }
    }
}