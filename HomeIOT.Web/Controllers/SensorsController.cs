using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HomeIOT.Web.Models;
using HomeIOT.Data;

namespace home_iot.Controllers
{
    public class SensorsController : Controller
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SensorsController> _logger;

        public SensorsController(DBContext context, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<SensorsController>();
        }

        // GET: Sensors
        public async Task<IActionResult> Index()
        {
            var data = await _context.Sensors.ToListAsync();
            return View(data.Select(x => _mapper.Map<SensorViewModel>(x)).ToList());
        }

        public async Task<IActionResult> Charts()
        {
            var data = await _context.Sensors.Select(x => x.SensorId).ToListAsync();
            return View("charts", data);
        }

        public ActionResult Favorites()
        {
            return ViewComponent("DataChart");
        }

        public async Task<IActionResult> Group(int groupId)
        {
            var data = await _context.Sensors.Where(x => x.Groups.Any(g => g.SensorGroupId == groupId)).Select(x => x.SensorId).ToListAsync();
            return View("charts", data);
        }

        public async Task<IActionResult> Detail(int sensorId)
        {
            var sensor = await _context.Sensors
                .FirstOrDefaultAsync(m => m.SensorId == sensorId);
            if (sensor == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<SensorViewModel>(sensor));
        }

        // GET: Sensors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensor = await _context.Sensors.Include(x => x.Groups).FirstAsync(x => x.SensorId == id);
            if (sensor == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<SensorViewModel>(sensor));
        }

        // POST: Sensors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SensorId,Identificator,Units,Name,Description,Groups")] SensorViewModel sensor)
        {
            var sensorData = await _context.Sensors.Include(x => x.Groups).FirstAsync(x => x.SensorId == id);
            if (sensorData == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _mapper.Map(sensor, sensorData);
                    _context.Update(sensorData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensorExists(sensor.SensorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sensor);
        }

        private bool SensorExists(int id)
        {
            return _context.Sensors.Any(e => e.SensorId == id);
        }

        public async Task Favorite(int sensorId, bool favorite)
        {
            var sensorData = await _context.Sensors.FindAsync(sensorId);
            sensorData.IsFavorited = favorite;
            _context.SaveChanges();
        }

        public ActionResult DataChartComponent(int sensor, long from, long to)
        {
            return ViewComponent("DataChart", new { sensor = sensor, from = new DateTime(from), to = new DateTime(to) });
        }
    }

    #region Components

    public class FavoritedSensorsViewComponent : ViewComponent
    {
        private const int MAX_SAMPLES_COUNT = 200;

        private readonly DBContext _context;

        public FavoritedSensorsViewComponent(DBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int sensor, DateTime from, DateTime to)
        {
            var data = await _context.Sensors.Where(x => x.IsFavorited).Select(x => x.SensorId).ToListAsync();
            return View("~/Views/Sensors/Components/FavoritedSensorsComponent.cshtml", data);
        }
    }

    public class DataChartViewComponent : ViewComponent
    {
        private const int MAX_SAMPLES_COUNT = 200;

        private readonly DBContext _context;

        public DataChartViewComponent(DBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int sensor, DateTime from, DateTime to)
        {
            var model = new ChartDataViewModel()
            {
                SensorId = sensor,
                Data = await _context.SensorData
                    .Where(x => x.SensorId == sensor && x.Timestamp >= from && x.Timestamp <= to)
                    .OrderBy(x => x.Timestamp)
                    .Select(x => new SensorDataViewModel(sensor, x.Timestamp, x.Value))
                    .ToListAsync()
            };

            // TODO: tohle nejak udelat uz pri nacitani z DB. Nebo jeste lepe udelat nejakou intepolaci
            var indexModulo = Math.Ceiling(model.Data.Count() / (float)MAX_SAMPLES_COUNT);
            model.Data = model.Data.Where((x, index) => index % indexModulo == 0);

            return View("~/Views/Sensors/Components/DataChartComponent.cshtml", model);
        }
    }

    public class SensorDataDetailViewComponent : ViewComponent
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public SensorDataDetailViewComponent(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int sensor)
        {
            var dbData = await _context.Sensors.FindAsync(sensor);
            var model = _mapper.Map<SensorDetailViewModel>(dbData);
            var now = DateTime.Now;
            model.ActualValue = await _context.SensorData
                .Where(x => x.SensorId == sensor)
                .OrderByDescending(x => x.Timestamp)
                .Take(1)
                .Select(x => new SensorDataViewModel(sensor, x.Timestamp, x.Value))
                .FirstOrDefaultAsync();
            model.TodayMin = await _context.SensorData
                .Where(x => x.SensorId == sensor && x.Timestamp >= now.Date && x.Timestamp < now.Date.AddDays(1))
                .OrderBy(x => x.Value)
                .Take(1)
                .Select(x => new SensorDataViewModel(sensor, x.Timestamp, x.Value))
                .FirstOrDefaultAsync();
            model.TodayMax = await _context.SensorData
                .Where(x => x.SensorId == sensor && x.Timestamp >= now.Date && x.Timestamp < now.Date.AddDays(1))
                .OrderByDescending(x => x.Value)
                .Take(1)
                .Select(x => new SensorDataViewModel(sensor, x.Timestamp, x.Value))
                .FirstOrDefaultAsync();
            model.LastConnection = await _context.SensorData
                .Where(x => x.SensorId == sensor)
                .MaxAsync(x => (DateTime?)x.Timestamp) ?? DateTime.MinValue;
            model.ChartFrom = now.AddHours(-6);
            model.ChartTo = now;

            var avg = await _context.SensorData
                    .Where(x => x.SensorId == sensor)
                    .OrderByDescending(x => x.Timestamp)
                    .Take(3)
                    .AverageAsync(x => (float?)x.Value) ?? 0;
            model.Trend = TrendExtensions.GetTrend(model.ActualValue?.Value ?? 0, avg);

            return View("~/Views/Sensors/Components/SensorDataDetailComponent.cshtml", model);
        }
    }

    public class SensorsOverviewViewComponent : ViewComponent
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public SensorsOverviewViewComponent(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dbData = await _context
                .Sensors
                .Select(s =>
                    new
                    {
                        Sensor = s,
                        ActualValue = s.Data.OrderByDescending(x => x.Timestamp).FirstOrDefault(),
                        Last3 = s.Data.OrderByDescending(x => x.Timestamp).Take(3),
                    })
                .ToListAsync();

            var models = dbData.Select(d => _mapper.Map<SensorDetailViewModel>(d.Sensor)).ToList();
            for (int i = 0; i < dbData.Count(); i++)
            {
                models[i].ActualValue = _mapper.Map<SensorDataViewModel>(dbData[i].ActualValue);
                models[i].Trend = TrendExtensions.GetTrend(models[i].ActualValue?.Value ?? 0, dbData[i].Last3.Average(x => (float?)x.Value) ?? 0);
            }

            return View("~/Views/Sensors/Components/SensorsOverviewComponent.cshtml", models);
        }
    }

    #endregion
}