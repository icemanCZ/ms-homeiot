using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeIOT.Web.Models
{
    public class MenuItemViewModel
    {
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public Dictionary<string, string> Params { get; set; }

        public IEnumerable<MenuItemViewModel> Children { get; set; } = new List<MenuItemViewModel>();
    }
}