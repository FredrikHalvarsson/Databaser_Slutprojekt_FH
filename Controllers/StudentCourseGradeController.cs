using Databaser_Slutprojekt_FH.Models;
using Labb3Skolan.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Skolan.Controllers
{
    internal class StudentCourseGradeController : BaseController
    {
        internal CourseController courseController = new CourseController();
        internal IOrderedEnumerable<StudentCourseGrade> GetStudentGrades(int studentId, int? courseId=null)
        {
            var allStudentGrades = context.Set<StudentCourseGrade>()
                .Include(x => x.Fkstudent)
                .Include(x => x.Fkgrade)
                .Include(X => X.Fkcourse);
            List<StudentCourseGrade> studentGrades;
            if (courseId == null)
            {
                studentGrades = allStudentGrades.Where(x => x.FkstudentId == studentId).ToList();

            }
            else
            {
                studentGrades = allStudentGrades.Where(x => x.FkstudentId == studentId && x.FkcourseId == courseId).ToList();
            }

            var x = studentGrades.OrderBy(x => x.Fkcourse.CourseName);

            return x;
        }
        //This was a test 
        //internal IEnumerable<Student> EFStoredProcedureTest(int studentId)
        //{
        //    var allStudentGrades = context.Database
        //        .SqlQuery<Student>($"PR_ViewRawStudent @ID={studentId}");

        //    var student = allStudentGrades.ToList();

        //    return student;
        //}
        //This was a test 
        //internal void ADOStoredProcedureTest(int studentId)
        //{
        //    SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Database=SlotteGymnasiet;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    SqlCommand cmd = new SqlCommand("PR_ViewRawStudent", con);
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@ID", studentId);
        //    try
        //    {
        //        con.Open();

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            Console.WriteLine(reader[1]+ " " + reader[2]);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}
        //Created for possible future use
        internal IOrderedEnumerable<StudentCourseGrade> GetStudentGradesInCourse(int courseId, int order)
        {
            var allCourseGrades = context.Set<StudentCourseGrade>()
                .Include(x => x.Fkstudent)
                .Include(x => x.Fkgrade)
                .Include(X => X.Fkcourse)
                .Where(x => x.FkcourseId == courseId)
                .ToList();

            var courseGrades=allCourseGrades.OrderBy(x => x.Fkgrade.GradeName);
            if (order == 1/*"gradeasc"*/)
            {
                courseGrades=allCourseGrades.OrderBy(x => x.Fkgrade.GradeName);
            }
            if (order == 2/*"gradedesc"*/)
            {
                courseGrades=allCourseGrades.OrderByDescending(x => x.Fkgrade.GradeName);
            }
            if (order == 3/*courseasc*/)
            {
                courseGrades=allCourseGrades.OrderBy(x => x.Fkcourse.CourseName);
            }
            if (order == 4/*"coursedesc"*/)
            {
                courseGrades=allCourseGrades.OrderByDescending(x => x.Fkcourse.CourseName);
            }
            return courseGrades;
        }
        internal List<StudentCourseGrade> GetGradesAndCourses()
        {
            var courseGrades = context.Set<StudentCourseGrade>()
                .Include(x => x.Fkgrade)
                .Include(X => X.Fkcourse)
                .ToList();
            return courseGrades;
        }
        internal IOrderedEnumerable<StudentCourseGrade> GetStudentsInCourse(int courseId, int order)
        {
            var allStudentsInCourse = context.Set<StudentCourseGrade>()
                .Include(x => x.Fkstudent)
                .Include(X => X.Fkcourse)
                .Where(x => x.FkcourseId == courseId)
                .ToList();

            var studentsInCourse=allStudentsInCourse.OrderBy(x => x.Fkstudent.LastName);
            if (order == 1/*"gradeasc"*/)
            {
                studentsInCourse=allStudentsInCourse.OrderBy(x => x.Fkstudent.LastName);
            }
            if (order == 2/*"gradedesc"*/)
            {
                studentsInCourse=allStudentsInCourse.OrderBy(x => x.Fkstudent.FirstName);
            }
            if (order == 3/*courseasc*/)
            {
                studentsInCourse=allStudentsInCourse.OrderByDescending(x => x.Fkstudent.LastName);
            }
            if (order == 4/*"coursedesc"*/)
            {
                studentsInCourse=allStudentsInCourse.OrderByDescending(x => x.Fkstudent.FirstName);
            }
            return studentsInCourse;
        }
        internal IOrderedEnumerable<StudentCourseGrade> GetRecentGrades()
        {
            var allRecentGrades = context.Set<StudentCourseGrade>()
                .Include(x => x.Fkstudent)
                .Include(x => x.Fkgrade)
                .Include(X => X.Fkcourse)
                .Where(x => x.GradeDate > DateTime.Now.AddMonths(-1))
                .ToList();
            var recentGrades=allRecentGrades.OrderBy(x => x.Fkcourse.CourseName);
           
            return recentGrades;
        }
        internal void PrintStudentGrades()
        {
            Console.WriteLine("Input Student ID...");
            int studentId;
            int.TryParse(Console.ReadLine(), out studentId);
            var studentGrades = GetStudentGrades(studentId);
            var student = studentGrades.ToList().First();
            Console.WriteLine($"" +
                $"-------| Student |-------\n" +
                $"Name: {student.Fkstudent.FirstName} {student.Fkstudent.LastName}\n" +
                $"Age {DateTime.Now.Year - ((DateTime)student.Fkstudent.BirthDate).Year}\n" +
                $"Gender: {student.Fkstudent.Gender}\n" +
                $"-------| Courses |-------");
            foreach (var item in studentGrades)
            {
                string date = string.Empty;
                if (item.GradeDate != null) { date = ((DateTime)item.GradeDate).ToShortDateString(); }
                Console.WriteLine(item.Fkcourse.CourseName + "\t" + item.Fkgrade.GradeName + "\n" + item.GradeTeacher + "\t" + date );
                Console.WriteLine("-------------------------");
            }
        }
        internal void PrintStudentGrades(int studentId)
        {
            var studentGrades = GetStudentGrades(studentId);
            var student = studentGrades.ToList().First();
            Console.WriteLine($"" +
                $"-------| Student |-------\n" +
                $"Name: {student.Fkstudent.FirstName} {student.Fkstudent.LastName}\n" +
                $"Age {DateTime.Now.Year - ((DateTime)student.Fkstudent.BirthDate).Year}\n" +
                $"Gender: {student.Fkstudent.Gender}\n" +
                $"-------| Courses |-------");
            foreach (var item in studentGrades)
            {
                string date = string.Empty;
                if (item.GradeDate != null) { date = ((DateTime)item.GradeDate).ToShortDateString(); }
                Console.WriteLine(item.Fkcourse.CourseName + "\t" + item.Fkgrade.GradeName + "\n" + item.GradeTeacher + "\t" + date);
                Console.WriteLine("-------------------------");
            }
        }
        internal void PrintAverageCourseGrades()
        {
            
            var courseGrades = GetGradesAndCourses();
            var groupedGrades = courseGrades.GroupBy(x => x.Fkcourse.CourseId);
            foreach (var course in groupedGrades)
            {
                decimal totalGrades = 0;
                decimal decimalGrade;
                decimal lowestGrade = 5;
                decimal highestGrade = 0;
                var x = course.ToList();
                Console.WriteLine(x.First().Fkcourse.CourseName);
                
                foreach (var grade in course)
                {
                    if (grade.Fkgrade.GradeName != "N/A")
                    {
                        totalGrades += GradeToDecimal(grade.Fkgrade.GradeName);
                        decimalGrade = GradeToDecimal(grade.Fkgrade.GradeName);
                        if (decimalGrade <= lowestGrade) { lowestGrade = decimalGrade; }
                        if (decimalGrade >= highestGrade) { highestGrade = decimalGrade; }
                    }
                }
                string avgGrade = GradeToString(Math.Round(totalGrades / x.Count()));
                if (lowestGrade < 5 && lowestGrade!=0) { Console.WriteLine($"Lowest grade: {GradeToString(lowestGrade)}"); }
                if (highestGrade > 0) { Console.WriteLine($"Highest grade: {GradeToString(highestGrade)}"); }
                Console.WriteLine("Average grade: "+avgGrade +"\n"); 
            }
        }
        internal void PrintRecentGrades()
        {
            var recentGrades = GetRecentGrades();
            foreach (var item in recentGrades)
            {
                Console.WriteLine("Course: "+item.Fkcourse.CourseName+" | Student: "+item.Fkstudent.FirstName+" "+item.Fkstudent.LastName+" | Grade: "+item.Fkgrade.GradeName);
            }
        }
        internal void PrintStudentsInCourse()
        {
            Console.WriteLine("Courses:");
            foreach (var item in context.Courses) 
            {
                Console.WriteLine("ID: " + item.CourseId + " Name: " + item.CourseName);
            }
            Console.Write("Please enter course ID: ");
            int courseId;
            int.TryParse(Console.ReadLine(), out courseId);
            if(courseController.GetCourses().Select(x => x.CourseId).Contains(courseId))
            {
                int order = MenuController.Menu("last name. ascending", "first name. ascending", "last name. descending", "first name. descending");
                var students = GetStudentsInCourse(courseId, order);
                Console.WriteLine("Course: " + students.FirstOrDefault().Fkcourse.CourseName);
                foreach (var item in students)
                {
                    Console.WriteLine("ID: " + item.Fkstudent.StudentId + " Name: " + item.Fkstudent.FirstName + " " + item.Fkstudent.LastName);
                }
            }
            else { Console.WriteLine("Selected Course NOT FOUND"); }
        }
        internal void PrintStudentsInCourse(int courseId)
        {
            if (courseController.GetCourses().Select(x => x.CourseId).Contains(courseId))
            {
                
                var students = GetStudentsInCourse(courseId, 1);
                Console.WriteLine("Course: " + students.FirstOrDefault().Fkcourse.CourseName);
                foreach (var item in students)
                {
                    Console.WriteLine("ID: " + item.Fkstudent.StudentId + " Name: " + item.Fkstudent.FirstName + " " + item.Fkstudent.LastName);
                }
            }
            else { Console.WriteLine("Selected Course NOT FOUND"); }
        }
        internal void AddStudentToCourse()
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {

                Console.WriteLine("Add Student to Course\n" +
        "-------------------------");
                Console.WriteLine("Enter Student ID: ");
                int.TryParse(Console.ReadLine(), out int sID);

                Console.WriteLine("-------------------------");
                Console.WriteLine("Enter Course ID: ");
                int.TryParse(Console.ReadLine(), out int cID);

                StudentCourseGrade studentToCourse = new StudentCourseGrade(sID, cID);
                context.Add(studentToCourse);
                Console.WriteLine("Student Added to Course!");
                context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Unable to add Student to Course, invalid input");
                throw;
            }

        }
        internal void SetGrade()
        {
            using var transaction = context.Database.BeginTransaction();
            Console.WriteLine("Select a Course");
            Console.WriteLine("Courses: \n");
            foreach (var item in context.Courses)
            {
                Console.WriteLine("ID: " + item.CourseId + " Name: " + item.CourseName);
            }
            Console.Write("\nPlease enter course ID: ");
            int.TryParse(Console.ReadLine(), out int courseId);
            Console.Clear();
            PrintStudentsInCourse(courseId);
            Console.WriteLine("\nEnter a Student ID: ");
            int.TryParse(Console.ReadLine(), out int studentId);
            Console.Clear();
            PrintStudentGrades(studentId);
            try
            {
                var GradeConnectionTable = GetStudentGrades(studentId, courseId);
                Console.WriteLine("Set Grade: ");
                int grade = MenuController.Menu("-","G", "VG", "MVG", "IG");
                GradeConnectionTable.FirstOrDefault().FkgradeId = grade;
                GradeConnectionTable.FirstOrDefault().GradeDate = DateTime.Now;
                Console.WriteLine("Enter Name of Teacher");
                GradeConnectionTable.FirstOrDefault().GradeTeacher = Console.ReadLine();
                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
