using Microsoft.AspNetCore.Mvc;
using NC6.Controllers;
using NC6.Models;
using NC6.Data;
using Microsoft.EntityFrameworkCore;


namespace NC6.ViewComponents
{
    public class LessonViewComponent:ViewComponent
    {
        private readonly NC6Context _context;

        public LessonViewComponent(NC6Context context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id,int courseid, Boolean add=false)
        {            
            if (add)
            {
                var students = await _context.Student.Where(student => student.GroupId == id).ToListAsync();
                var @group = await _context.Group.FindAsync(id);
                ViewData["students"] = students;
                ViewBag.courseid = courseid;
                return View("AddLesson", @group);
            }
            else
            {
                var lessonslist = await _context.Attendance.Where(a => a.CourseId == courseid && a.Student.GroupId == id).OrderBy(a=>a.DataS).ToListAsync();
                ViewData["lessons"]= lessonslist;
                return View("List");
            }

        }
    }
}
