using Labb3Skolan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3Skolan.Controllers
{
    internal class EmployeeController : BaseController
    {
        internal IOrderedEnumerable<Employee> GetEmployeesByRole(int order, string role)
        {
            var allEmployees = context.Set<Employee>()
                .Include(x=>x.Role)
                .Where(x=>x.Role.RoleName.Contains($"{role}"))
                .ToList();

            var employeesByRole = allEmployees.OrderBy(x => x.LastName);
            if (order == 1/*"lastnameasc"*/)
            {
                employeesByRole=allEmployees.OrderBy(x => x.LastName);
            }
            if (order == 2/*"firstnameasc"*/)
            {
                employeesByRole = allEmployees.OrderBy(x => x.FirstName);
            }
            if (order == 3/*lastnamedesc*/)
            {
                employeesByRole = allEmployees.OrderByDescending(x => x.LastName);
            }
            if (order == 4/*"firstnamedesc"*/)
            {
                employeesByRole = allEmployees.OrderByDescending(x => x.FirstName);
            }
            return employeesByRole;
        }
        internal IOrderedEnumerable<Employee> GetAllEmployees(int order)
        {
            var allEmployees = context.Set<Employee>()
                .Include(x=>x.Role)
                .ToList();

            var employeeList = allEmployees.OrderBy(x => x.LastName);
            if (order == 1/*"lastnameasc"*/)
            {
                employeeList=allEmployees.OrderBy(x => x.LastName);
            }
            if (order == 2/*"firstnameasc"*/)
            {
                employeeList = allEmployees.OrderBy(x => x.FirstName);
            }
            if (order == 3/*lastnamedesc*/)
            {
                employeeList = allEmployees.OrderByDescending(x => x.LastName);
            }
            if (order == 4/*"firstnamedesc"*/)
            {
                employeeList = allEmployees.OrderByDescending(x => x.FirstName);
            }
            return employeeList;
        }
        internal void Print()
        {
            Console.WriteLine("Order List by: ");
            int order = MenuController.Menu("last name. ascending", "first name. ascending", "last name. descending", "first name. descending");
            var employeelist = GetAllEmployees(order);
            Console.WriteLine("--------------------|Employees|--------------------");
            foreach (var item in employeelist)
            {
                Console.WriteLine($"ID: {item.Id} | Name: {item.FirstName} {item.LastName} |\nRole: {item.Role.RoleName} | Employed for: {CalcTimeEmployed(item)} Years |");
                Console.WriteLine("---------------------------------------------------");
            }
        }
        internal void PrintByRole()
        {
            Console.Write("Input Role Name: ");
            string role = Console.ReadLine();
            Console.WriteLine("Order List by: ");
            int order = MenuController.Menu("last name. ascending", "first name. ascending", "last name. descending", "first name. descending");
            var employeeList = GetEmployeesByRole(order,role);
            foreach(var item in employeeList)
            {
                Console.WriteLine("ID: " + item.Id + "| Name: " + item.FirstName+" "+item.LastName+" | Role: "+item.Role.RoleName + " | Salary: " + item.Salary);
            }
        }

        internal void AddEmployee()
        {
            Console.Write("Input First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Input Surname: ");
            string lastName = Console.ReadLine();

            Console.WriteLine("Input starting Salary: ");
            decimal salary;
            decimal.TryParse(Console.ReadLine(), out salary);

            DateTime hireDate = DateTime.Now;
            string personalNumber = SetPersonalNumber();

            Console.WriteLine("Select Role: ");
            int roleId = MenuController.Menu("Teacher", "Principal");

            Employee employee = new Employee(firstName, lastName, salary, personalNumber, hireDate, roleId);
            context.Employees.Add(employee);
            Console.WriteLine("Employee added!");
            context.SaveChanges();
        }
        /// <summary>
        /// Used to calculate a rough ammount of years employed from Employee HireDate
        /// *Does not account for leap years*
        /// </summary>
        /// <param name="emp"></param>
        /// <returns>Double value of years employed rounded to 2 decimals</returns>
        internal double CalcTimeEmployed(Employee emp)
        {
            DateTime hireDate = (DateTime)emp.HireDate; //Value should never be null so no need for nullcheck
            DateTime dateNow = DateTime.Now;
            var yearsEmployed = Math.Round((dateNow.Subtract(hireDate).TotalDays)/365, 2 ,MidpointRounding.ToEven);

            return yearsEmployed;
        }
        internal void CalcDeptSalary()
        {
            Console.WriteLine("Input Role Name: ");
            string role = Console.ReadLine();
            var deptSalary = GetEmployeesByRole(1, role);
            int employees=0;
            decimal totalSalary=0;
            decimal avgSalary=0;
            foreach (var item in deptSalary)
            {
                totalSalary += item.Salary;
                employees++;
            }
            avgSalary = totalSalary / employees;
            Console.WriteLine($"------------------|Role: {role,-9}|------------------");
            Console.WriteLine($"Total Monthly Salary: {totalSalary}\n" +
                $"Average Monthly Salary: {Math.Round(avgSalary, 2)}");
            //add Median salary?
            Console.WriteLine($"-----------------------------------------------------");
        }
        internal void CountEmpInDept()
        {
            var employees = GetAllEmployees(1);
            foreach ( var dept in context.Roles)
            {
                int empCount = 0;
                Console.WriteLine($"----------------|{dept.RoleName,-17}|----------------");
                foreach (var emp in employees.Where(x => x.Role.RoleName==dept.RoleName))
                {
                    Console.WriteLine( emp.LastName);
                    empCount++;
                }
                Console.WriteLine($"----------------|Total: {empCount,-10}|----------------");
            }
        }
    }
}
