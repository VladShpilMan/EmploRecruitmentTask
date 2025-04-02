using EmploRecruitmentTask.EmployeeVacation.EntityDbContext;
using EmploRecruitmentTask.EmployeeVacation.Models;
using EmploRecruitmentTask.EmployeeVacation.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace EmploRecruitmentTask.EmployeeVacation
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<EmployeeDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<EmployeeService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
                context.Database.EnsureCreated();
                SeedData(context);
            }

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                EmployeeService service = scope.ServiceProvider.GetRequiredService<EmployeeService>();
                EmployeeDbContext context = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();

                Console.WriteLine("Zadanie 2a: Pracownicy .NET korzystający z urlopu w 2019 r.:");
                List<Employee> dotNetEmployees = service.GetDotNetEmployeesWithVacation2019();
                foreach (var emp in dotNetEmployees)
                    Console.WriteLine($"- {emp.Name}");

                Console.WriteLine("\nZadanie 2b: Dni urlopowe wykorzystane w 2025 r.:");
                List<object> usedDays = service.GetEmployeesWithUsedVacationDays();
                foreach (var item in usedDays)
                    Console.WriteLine($"- {item}");

                Console.WriteLine("\nZadanie 2c: Zespoły bez świąt w 2019 r.:");
                List<Team> teams = service.GetTeamsWithoutVacations2019();
                foreach (var team in teams)
                    Console.WriteLine($"- {team.Name}");

                Employee jan = context.Employees.Include(e => e.Vacations).Include(e => e.VacationPackage)
                    .First(e => e.Name == "Jan Kowalski");
                int freeDays = service.CountFreeDaysForEmployee(jan, jan.Vacations, jan.VacationPackage);
                bool canRequest = service.IfEmployeeCanRequestVacation(jan, jan.Vacations, jan.VacationPackage);

                Console.WriteLine($"\nZadanie 3: Dni wolne dla Jana Kowalskiego w 2025 roku: {freeDays}");
                Console.WriteLine($"Zadanie 4: Czy Jan Kowalski może poprosić o urlop? {canRequest}");
            }
        }

        static void SeedData(EmployeeDbContext context)
        {
            if (!context.Teams.Any())
            {
                context.Teams.AddRange(
                    new Team { Name = ".NET" },
                    new Team { Name = "Java" },
                    new Team { Name = "DevOps" }
                );
                context.VacationPackages.AddRange(
                    new VacationPackage { Name = "Standard", GrantedDays = 20, Year = 2019 },
                    new VacationPackage { Name = "Standard 2025", GrantedDays = 20, Year = 2025 }
                );
                context.SaveChanges();

                context.Employees.AddRange(
                    new Employee { Name = "Jan Kowalski", TeamId = 1, PositionId = 1, VacationPackageId = 2 },
                    new Employee { Name = "Kamil Nowak", TeamId = 1, PositionId = 2, VacationPackageId = 1 },
                    new Employee { Name = "Anna Wiśniewska", TeamId = 2, PositionId = 1, VacationPackageId = 1 }
                );
                context.SaveChanges();

                context.Vacations.AddRange(
                    new Vacation { DateSince = new DateTime(2019, 6, 1), DateUntil = new DateTime(2019, 6, 5), NumberOfHours = 40, IsPartialVacation = false, EmployeeId = 1 },
                    new Vacation { DateSince = new DateTime(2019, 7, 10), DateUntil = new DateTime(2019, 7, 12), NumberOfHours = 24, IsPartialVacation = true, EmployeeId = 2 },
                    new Vacation { DateSince = new DateTime(2025, 3, 1), DateUntil = new DateTime(2025, 3, 3), NumberOfHours = 24, IsPartialVacation = false, EmployeeId = 1 }
                );
                context.SaveChanges();
            }
        }
    }
}
