using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeIOT.Data;

namespace home_iot.Controllers
{
    public class SensorGroupsController : Controller
    {
        private readonly DBContext _context;

        public SensorGroupsController(DBContext context)
        {
            _context = context;
        }

        // GET: SensorGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.SensorGroups.ToListAsync());
        }

        // GET: SensorGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorGroup = await _context.SensorGroups
                .FirstOrDefaultAsync(m => m.SensorGroupId == id);
            if (sensorGroup == null)
            {
                return NotFound();
            }

            return View(sensorGroup);
        }

        // GET: SensorGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SensorGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SensorGroupId,Name,Description")] SensorGroup sensorGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sensorGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sensorGroup);
        }

        // GET: SensorGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorGroup = await _context.SensorGroups.FindAsync(id);
            if (sensorGroup == null)
            {
                return NotFound();
            }
            return View(sensorGroup);
        }

        // POST: SensorGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SensorGroupId,Name,Description")] SensorGroup sensorGroup)
        {
            if (id != sensorGroup.SensorGroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sensorGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensorGroupExists(sensorGroup.SensorGroupId))
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
            return View(sensorGroup);
        }

        // GET: SensorGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensorGroup = await _context.SensorGroups
                .FirstOrDefaultAsync(m => m.SensorGroupId == id);
            if (sensorGroup == null)
            {
                return NotFound();
            }

            return View(sensorGroup);
        }

        // POST: SensorGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sensorGroup = await _context.SensorGroups.FindAsync(id);
            _context.SensorGroups.Remove(sensorGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SensorGroupExists(int id)
        {
            return _context.SensorGroups.Any(e => e.SensorGroupId == id);
        }
    }
}