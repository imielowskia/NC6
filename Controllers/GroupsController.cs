using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NC6.Data;
using NC6.Models;

namespace NC6.Controllers
{
    public class GroupsController : Controller
    {
        private readonly NC6Context _context;

        public GroupsController(NC6Context context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
              return _context.Group != null ? 
                          View(await _context.Group.Include(g=>g.Faculty).ToListAsync()) :
                          Problem("Entity set 'NC6Context.Group'  is null.");
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g=>g.Faculty)
                .Include(g=>g.Students)
                .Include(g=>g.Courses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name");
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Sname,FacultyId")] Group @group)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name", group.FacultyId);
            return View(@group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var @group = await _context.Group.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name", group.FacultyId);
            GetCourseList(id);      //Wywołanie funkcji do stworzenia listy kursów 
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Sname,FacultyId")] Group @group)
        {
            if (id != @group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    // dodany fragment aktualizujący relację wiele do wielu
                    var SC = HttpContext.Request.Form["selectedCourses"];
                    var gr = _context.Group.Include(g => g.Courses).Single(g => g.Id == id);
                    if (gr.Courses != null) { gr.Courses.Clear(); }
                    foreach (var sc in SC)
                    {
                        var course = _context.Course.Single(course => course.Id == int.Parse(sc));
                        gr.Courses.Add(course);
                    }
                    _context.Update(gr);
                    //koniec aktualizacji relacji Course-Group
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.Id))
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
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name", group.FacultyId);
            GetCourseList(id);
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Group == null)
            {
                return NotFound();
            }

            var @group = await _context.Group
                .Include(g => g.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Group == null)
            {
                return Problem("Entity set 'NC6Context.Group'  is null.");
            }
            var @group = await _context.Group.FindAsync(id);
            if (@group != null)
            {
                _context.Group.Remove(@group);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
          return (_context.Group?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Funkcja tworząca liste kursów
        private void GetCourseList(int? id)
        {
            var Courses = _context.Course.ToList();
            var Selected = _context.Group.Include(g => g.Courses).Single(g => g.Id == id);
            var coursestocheck = new List<CGcheck>();
            var xcheck = "";
            foreach (var course in Courses)
            {
                if (Selected.Courses.Contains(course)) { xcheck = "checked"; } else { xcheck = ""; };
                coursestocheck.Add(
                   new CGcheck
                   {
                       CourseId = course.Id,
                       Name = course.Name,
                       Checked = xcheck,
                   }
                   );
            }
            ViewData["courses"] = coursestocheck;
        }
    }
}
