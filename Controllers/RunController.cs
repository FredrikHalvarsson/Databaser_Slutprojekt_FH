using Labb3Skolan.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Skolan.Controllers
{
    internal class RunController
    {
        internal StudentCourseGradeController sCGController = new StudentCourseGradeController();
        internal StudentController studentController = new StudentController();
        internal EmployeeController employeeController = new EmployeeController();
        internal CourseController courseController = new CourseController();
        internal void Run()
        {
            do
            {
                switch (MenuController.Menu("Student Info", "Employee Info", "Grade Statistics" , "Add"))
                {
                    case 1:
                        Console.WriteLine("Student Info");
                        switch (MenuController.Menu("List All Students", "List Students by course", "View Student", "Return"))
                        {
                           
                            case 1:
                                studentController.PrintStudents();
                                break;
                            case 2:
                                sCGController.PrintStudentsInCourse();
                                break;
                            case 3:
                                sCGController.PrintStudentGrades();
                                break;
                            case 4:
                                break;
                        }
                        break;
                    case 2:
                        switch(MenuController.Menu("All Employes", "Dept. Employee Count", "Salary Statistics", "Employees by role", "Return"))
                        {
                            case 1:
                                employeeController.Print();
                                break;
                            case 2:
                                employeeController.CountEmpInDept();
                                break;
                            case 3:
                                employeeController.CalcDeptSalary();
                                break;
                            case 4:
                                employeeController.PrintByRole();
                                break;
                            case 5: 
                                break;
                        }
                        break;
                    case 3:
                        switch (MenuController.Menu("Grades from latest month", "Course with avg. Grades","List Active Courses", "Return"))
                        {
                            case 1:
                                sCGController.PrintRecentGrades();
                                break;
                            case 2:
                                sCGController.PrintAverageCourseGrades();
                                break;
                            case 3:
                                courseController.PrintActiveCourses();
                                break;
                            case 4:
                                break;
                        }
                        break;
                    case 4:
                        Console.WriteLine("Add...");
                        switch (MenuController.Menu("New Employee", "New Student", "Student to Course", "Set Grade", "Return"))
                        {
                            case 1:
                                employeeController.AddEmployee();
                                break;
                            case 2:
                                studentController.AddStudent();
                                break;
                            case 3:
                                sCGController.AddStudentToCourse();
                                break;
                            case 4:
                                sCGController.SetGrade();
                                break;
                            case 5:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            } while (true);
        }
    }
}
