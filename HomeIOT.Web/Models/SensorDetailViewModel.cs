using HomeIOT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Models
{
    public class SensorDetailViewModel
    {
        public int SensorId { get; set; }

        [Display(Name = "Senzor")]
        public string SensorName { get; set; }

        public string SensorDescription { get; set; }
        public Unit Units { get; set; }
        public bool IsFavorited { get; set; }

        [Display(Name = "Aktuální hodnota")]
        public SensorDataViewModel ActualValue { get; set; }

        [Display(Name = "Dnešní maximum")]
        public SensorDataViewModel TodayMax { get; set; }

        [Display(Name = "Dnešní minimum")]
        public SensorDataViewModel TodayMin { get; set; }

        [Display(Name = "Poslední spojení")]
        public DateTime LastConnection { get; set; }

        [Display(Name = "Trend")]
        public Trend Trend { get; set; }

        public DateTime ChartFrom { get; set; }
        public DateTime ChartTo { get; set; }

        public string FavoriteIcon()
        {
            if (IsFavorited)
                return "/images/unfavorite.png";
            else
                return "/images/favorite.png";
        }

        public string FavoriteText()
        {
            if (IsFavorited)
                return "Odebrat z oblíbených";
            else
                return "Přidat do oblíbených";
        }
    }
}