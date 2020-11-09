using HomeIOT.Data;
using HomeIOT.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Services
{
    public class MenuService
    {
        private readonly DBContext _context;

        public MenuService(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<MenuItemViewModel> GetMenu()
        {
            var ret = new List<MenuItemViewModel>();
            ret.Add(new MenuItemViewModel()
            {
                Text = "Senzory",
                Controller = "Sensors",
                Action = "Index",
                Children = _context.Sensors
                    .Select(x => new MenuItemViewModel()
                    {
                        Text = x.Name,
                        Controller = "Sensors",
                        Action = "Detail",
                        Params = new Dictionary<string, string>() { { "sensorId", x.SensorId.ToString() } }
                    }).ToList()
            });
            ret.Add(new MenuItemViewModel()
            {
                Text = "Skupiny",
                Children = _context.SensorGroups
                    .Select(x => new MenuItemViewModel()
                    {
                        Text = x.Name,
                        Controller = "Sensors",
                        Action = "Group",
                        Params = new Dictionary<string, string>() { { "groupId", x.SensorGroupId.ToString() } }
                    }).ToList()
            });
            ret.Add(new MenuItemViewModel()
            {
                Text = "Administrace",
                Controller = "",
                Action = "",
                Children = new List<MenuItemViewModel>()
                {
                    new MenuItemViewModel()
                    {
                        Text = "Skupiny",
                        Controller = "SensorGroups",
                        Action = "Index"
                    }
                }
            });

            return ret;
        }
    }
}