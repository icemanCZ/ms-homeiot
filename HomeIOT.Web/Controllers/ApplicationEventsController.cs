using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeIOT.Data;
using AutoMapper;
using HomeIOT.Web.Models;

namespace HomeIOT.Web.Controllers
{
    public class ApplicationEventsController : Controller
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ApplicationEventsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: ApplicationEvents
        public async Task<IActionResult> Index(bool resolvedOnly = false)
        {
            var dBContext = _context.ApplicationEvents.Where(x => resolvedOnly == false || x.Resolved == resolvedOnly).Include(a => a.Sensor);
            var data = await dBContext.ToListAsync();
            return View(data.Select(x => _mapper.Map<ApplicationEventViewModel>(x)));
        }

        public async Task Resolve(int id)
        {
            var data = await _context.ApplicationEvents.FindAsync(id);
            if (data == null || data.Resolved == true)
                return;

            data.Resolved = true;
            data.ResolvedTimestamp = DateTime.Now;
            _context.SaveChanges();
        }
    }

    #region Components

    public class UnresolvedApplicationEventsViewComponent : ViewComponent
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public UnresolvedApplicationEventsViewComponent(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var dBContext = _context.ApplicationEvents.Where(x => x.Resolved == false).Include(a => a.Sensor);
            var data = await dBContext.ToListAsync();
            return View("~/Views/ApplicationEvents/Components/UnresolvedApplicationEvents.cshtml", data.Select(x => _mapper.Map<ApplicationEventViewModel>(x)));
        }
    }

    #endregion
}