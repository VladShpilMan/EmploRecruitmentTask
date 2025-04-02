using EmploRecruitmentTask.EmployeeVacation.EntityDbContext;
using EmploRecruitmentTask.EmployeeVacation.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploRecruitmentTask.EmployeeVacation.Services
{
    public class EmployeeService
    {
        private readonly EmployeeDbContext _context;

        public EmployeeService(EmployeeDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetDotNetEmployeesWithVacation2019()
        {
            return _context.Employees
                .Where(e => e.Team.Name == ".NET" &&
                            e.Vacations.Any(v => v.DateSince.Year == 2019))
                .ToList();
        }

        public List<object> GetEmployeesWithUsedVacationDays()
        {
            int currentYear = DateTime.Now.Year;
            return _context.Employees
                .Select(e => new
                {
                    EmployeeName = e.Name,
                    UsedDays = e.Vacations
                        .Where(v => v.DateUntil.Year == currentYear && v.DateUntil < DateTime.Now)
                        .Sum(v => EF.Functions.DateDiffDay(v.DateSince, v.DateUntil) + 1)
                })
                .ToList<object>();
        }

        public List<Team> GetTeamsWithoutVacations2019()
        {
            return _context.Teams
                .Where(t => !t.Employees!.Any(e => e.Vacations.Any(v => v.DateSince.Year == 2019)))
                .ToList();
        }

        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            int currentYear = DateTime.Now.Year;
            int usedDays = vacations
                .Where(v => v.EmployeeId == employee.Id && v.DateUntil.Year == currentYear && v.DateUntil < DateTime.Now)
                .Sum(v => (v.DateUntil - v.DateSince).Days + 1);
            return vacationPackage.GrantedDays - usedDays;
        }


        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            int currentYear = DateTime.Now.Year;
            if (vacationPackage.Year != currentYear)
                return false;

            int freeDays = CountFreeDaysForEmployee(employee, vacations ?? new List<Vacation>(), vacationPackage);
            return freeDays > 0;
        }
    }
}
