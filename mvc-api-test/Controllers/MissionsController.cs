using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc_api_test.Data;
using mvc_api_test.Models;
using Newtonsoft.Json;

namespace mvc_api_test.Controllers
{
    public class MissionsController : Controller
    {
        private readonly mvc_api_testContext _context;

        public MissionsController(mvc_api_testContext context)
        {
            _context = context;
        }

        // GET: Missions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Mission.ToListAsync());
        }

        // GET: Missions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mission = await _context.Mission
                .FirstOrDefaultAsync(m => m.id == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // GET: Missions/GetAllFromApi
        public IActionResult Create()
        {
            return View();
        }

        // POST: Missions/GetAllFromApi
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpGet]
        //public async Task<ActionResult> Get()
        //{
            
        //    var result = await JsonConvert("https://dummyjson.com/todos/random");
        //    return Ok(result);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRandomFromApi([Bind("id,todo,completed,userId")] )
        {

            HttpResponseMessage response = await Client.GetAsync("https://dummyjson.com/todos/random");
            Mission mission = await Json.Deserialize(response);
            _context.Add(mission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> GetAllFromApi([Bind("id,todo,completed,userId")] )
        {

            HttpResponseMessage response = await Client.GetAsync("fetch('https://dummyjson.com/todos')\r\n.then(res => res.json())\r\n.then(console.log);");
            Mission missions = await Json.Deserialize(response).tolist;
            return View(missions);

        }
       

        // GET: Missions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mission = await _context.Mission.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }
            return View(mission);
        }

        // POST: Missions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,todo,completed,userId")] Mission mission)
        {
            if (id != mission.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MissionExists(mission.id))
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
            return View(mission);
        }

        // GET: Missions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mission = await _context.Mission
                .FirstOrDefaultAsync(m => m.id == id);
            if (mission == null)
            {
                return NotFound();
            }

            return View(mission);
        }

        // POST: Missions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mission = await _context.Mission.FindAsync(id);
            if (mission != null)
            {
                _context.Mission.Remove(mission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MissionExists(int id)
        {
            return _context.Mission.Any(e => e.id == id);
        }
    }
}
