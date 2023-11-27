using Amazon_Data;
using Amazon_Domain;
using Microsoft.EntityFrameworkCore;

namespace AmazonSuperApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            // Create a db connection
            // Create the db if it's not created 

            //using (AmazonContext context = new AmazonContext())
            //{
            //    context.Database.EnsureCreated();
            //}


            //var firstTask = new EmployeeTask()
            //{
            //    Name = "Doing the first task",
            //    CreatedDate = DateTime.Now,
            //    ClosedDate = DateTime.Now.AddDays(1)
            //};

            //var secondTask = new EmployeeTask()
            //{
            //    Name = "Doing the second task",
            //    CreatedDate = DateTime.Now,
            //    ClosedDate = DateTime.Now.AddDays(1)
            //};

            //var samir = new Employee()
            //{
            //    FirstName = "Ahmad",
            //    LastName = "Abo humid",
            //    Tasks = new List<EmployeeTask>() { firstTask, secondTask }
            //};


            //AddEmployee(samir);

            //AddBulkEmployees();

            //UpdateEmployeeByName("Samer");

            //DeleteEmployeeByName("Samir"); // DO NOOOOOOOOOOT REMOVE => SOFT DELETE 

            GetEmployees();

        }

        private static void DeleteEmployeeByName(string name)
        {
            using (AmazonContext context = new AmazonContext())
            {
                var employee = context.Employees.FirstOrDefault(e => e.FirstName == name);

                context.Employees.Remove(employee);

                context.SaveChanges();
            }

            //using (AmazonContext context = new AmazonContext())
            //{
            //    var employee = context.Employees.FirstOrDefault(e => e.FirstName == name);
            //    employee.IsActive = false;

            //    context.SaveChanges();
            //}
        }

        private static void UpdateEmployeeByName(string name)
        {

            using (AmazonContext context = new AmazonContext())
            {
                var employee = context.Employees.FirstOrDefault(e => e.FirstName == name);
                employee.LastName = "Abo Samra";

                //context.Employees.Update(employee);
                context.SaveChanges();
            }



        }

        private static void AddBulkEmployees()
        {
            using (AmazonContext context = new AmazonContext())
            {
                context.Employees.AddRange(
                    new Employee() { FirstName = "Ayman", LastName="Abo Alyemn"},
                    new Employee() { FirstName = "Samer", LastName="Abo Alyemn"},
                    new Employee() { 
                        FirstName = "ahmad",
                        LastName = "Abo Alyemn",
                        Address = new Address() { 
                            City = "Egypt",
                            Phone = "52543252356",
                            Country = "Cairo",
                            Fax = "544545454",
                            PostalCode = "45454",
                            Region = "ME"} 
                        }
                    );

                context.SaveChanges();
            }
        }

        private static void AddEmployee(Employee employee)
        {
            if (employee == null) return;

            using (AmazonContext context = new AmazonContext())
            {
                context.Employees.Add(employee);

                context.SaveChanges();
            }
        }

        private static void GetEmployees()
        {
            using (AmazonContext context = new AmazonContext())
            {
                // Get the employees and their tasks 


                //var samer = context.Employees
                //    //.Include(e => e.Tasks) // left join
                //    .FirstOrDefault<Employee>(e => e.FirstName == "Samer");

                //// Explicit loading

                //context.Entry(samer).Collection(e => e.Tasks).Load();

                //var employeeSummary = 
                //     context
                //    .Employees
                //    .Select(e => 
                //                new { Id = e.Id, FullName = e.FirstName + " " + e.LastName}
                //            ).ToList();

                //var employees = context.Employees.FromSqlRaw("Select * from Employees").ToList();

                //var samername = "Samer";

                //context.Employees.FromSqlInterpolated($"Select * from emmployees where FirstName = '{samername}'").ToList();


                var employees = context.Employees
                    .Include(e => e.Tasks.Where(t => t.ClosedDate > DateTime.Now)) // left join  -- Eager loading
                        .ThenInclude(t => t.Categories)
                                           //.Where(e => e.FirstName.ToLower() == "Samir".ToLower())
                    .Where(e => EF.Functions.Like(e.FirstName, "%am%"))
                    .TagWith("Reading the employees and their tasks")
                    .ToList();

                foreach (var employee in employees)
                {
                    Console.WriteLine(employee.FirstName + " " + employee.LastName);

                    foreach (var task in employee.Tasks)
                    {
                        Console.WriteLine(task.Name);
                    }
                }
            }
        }
    }
}