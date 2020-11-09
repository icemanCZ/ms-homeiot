using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeIOT.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HomeIOT.Web.Models;

namespace home_iot.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(DBContext context, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}