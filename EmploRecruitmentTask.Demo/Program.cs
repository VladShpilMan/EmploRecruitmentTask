using EmploRecruitmentTask.Hierarchy.Models;
using EmploRecruitmentTask.Hierarchy.Services;

namespace EmploRecruitmentTask.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task1();
        }

        private static void Task1()
        {
            List<Employee> employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "Jan", SuperiorId = null },
                new Employee { Id = 2, Name = "Kamil", SuperiorId = 1 },
                new Employee { Id = 3, Name = "Maria", SuperiorId = null },
                new Employee { Id = 4, Name = "Andrzej", SuperiorId = 2 },
                new Employee { Id = 5, Name = "Piotr", SuperiorId = 3 },
                new Employee { Id = 6, Name = "Tomasz", SuperiorId = 2 },
                new Employee { Id = 7, Name = "Krzysztof", SuperiorId = 6 },
                new Employee { Id = 8, Name = "Barbara", SuperiorId = 3 },
                new Employee { Id = 9, Name = "Zofia", SuperiorId = 8 }
            };

            EmployeesStructure structure = new EmployeesStructure();
            structure.FillEmployeesStructure(employees);

            int? row1 = structure.GetSuperiorRowOfEmployee(2, 1);
            int? row2 = structure.GetSuperiorRowOfEmployee(4, 3);
            int? row3 = structure.GetSuperiorRowOfEmployee(4, 1);
            int? row4 = structure.GetSuperiorRowOfEmployee(7, 1);
            int? row5 = structure.GetSuperiorRowOfEmployee(9, 3);
            int? row6 = structure.GetSuperiorRowOfEmployee(9, 1);

            Console.WriteLine($"Row1: {row1}");
            Console.WriteLine($"Row2: {row2}");
            Console.WriteLine($"Row3: {row3}");
            Console.WriteLine($"Row4: {row4}");
            Console.WriteLine($"Row5: {row5}");
            Console.WriteLine($"Row6: {row6}");

        }
    }
}
