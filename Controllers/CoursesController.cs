using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NC6.Data;
using NC6.Models;

namespace NC6.Controllers
{
    public class CoursesController : Controller
    {
        private readonly NC6Context _context;

        public CoursesController(NC6Context context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var nC6Context = _context.Course.Include(c => c.Faculty);
            return View(await nC6Context.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Faculty)
                .Include(c => c.Groups)
                .ThenInclude(g=>g.Students)
                .Include(c=>c.Grades)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ects,FacultyId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name", course.FacultyId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name", course.FacultyId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ects,FacultyId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["FacultyId"] = new SelectList(_context.Faculty, "Id", "Name", course.FacultyId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Faculty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'NC6Context.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        // GET Courses/TakeGrade/5
        public async Task<IActionResult> TakeGrade (int? id, int? groupid)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FirstOrDefaultAsync(c=>c.Id==id);
            var group = await _context.Group.Include(g=>g.Students).ThenInclude(s=>s.Grades.Where(g=>g.CourseId==id)).FirstOrDefaultAsync(g=>g.Id==groupid);            
            var grade = 0.0;
            var gradelist = new List<Grade>();
            foreach(var s in group.Students)
            {
                if (s.Grades.Count > 0) { grade = s.Grades.First().grade; } else { grade = 0.0; }
                gradelist.Add(
                    new Grade
                    {
                        CourseId = course.Id,
                        StudentId = s.Id,
                        Course = course,
                        Student = s,
                        grade = grade
                    }
                    );
            }
            ViewData["group"] = group;
            ViewData["grades"] = gradelist;            
            return View(course);
        }


        // POST: Courses/TakeGrade        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeGrade(int id )
        {           
            var course = await _context.Course
                .Include(c => c.Faculty)
                .Include(c => c.Groups)
                .ThenInclude(g => g.Students)
                .Include(c=>c.Grades)
                .FirstOrDefaultAsync(m => m.Id == id);
            var groupid = int.Parse(HttpContext.Request.Form["grids"]);
            var ids = HttpContext.Request.Form["ids"];
            var grades = HttpContext.Request.Form["oceny"];
            var xid = 0;
            var i = 0;            
            foreach ( var sid in ids)
            {
                xid = int.Parse(sid);
                var xgr = _context.Grade.Where(g => g.StudentId == xid & g.CourseId == id);
                if (xgr.Count()>0)
                {
                    var ocena = _context.Grade.Where(g => g.StudentId == xid & g.CourseId == id).Single();
                    ocena.grade = double.Parse(grades[i]);
                    _context.Update(ocena);
                }
                else
                {
                    var grade = new Grade()
                    {
                        StudentId = xid,
                        CourseId = id,
                        grade = double.Parse(grades[i])
                    };
                    _context.Add(grade);
                }                                
                i++;
            }
            
            await _context.SaveChangesAsync();
            return View("Details", course);
        }





        private bool CourseExists(int id)
        {
          return (_context.Course?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
