using Labb3Skolan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Skolan.Controllers
{
    internal class CourseController : BaseController
    {
        internal List<Course> GetCourses()
        {
            var courses = context.Set<Course>().ToList();
            return courses;
        }
        internal void PrintActiveCourses()
        {
            var activeCourses = GetCourses().Where(x=>x.EndDate>=DateTime.Now);
            Console.WriteLine($"------------------|Active Courses |------------------");
            foreach (var item in activeCourses)
            {
                Console.WriteLine($"Course: {item.CourseName}\nEnd Date: {DateOnly.FromDateTime((DateTime)item.EndDate)}\n");
                Console.WriteLine($"-----------------------------------------------------");
            }
        }
    }
}
