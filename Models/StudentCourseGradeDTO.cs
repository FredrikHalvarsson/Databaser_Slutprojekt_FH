using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databaser_Slutprojekt_FH.Models
{
    internal class StudentCourseGradeDTO //Not in use
    {
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string PersonalNr { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Gender { get; set; }

        public string? CourseName { get; set; }

        public string? Grade { get; set; }

        public DateTime? GradeDate { get; set; }

        public string? GradeTeacher { get; set; }
    }
}
