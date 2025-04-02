using EmploRecruitmentTask.EmployeeVacation.Models;
using Microsoft.EntityFrameworkCore;

namespace EmploRecruitmentTask.EmployeeVacation.EntityDbContext
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<VacationPackage> VacationPackages { get; set; }
        public DbSet<Vacation> Vacations { get; set; }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }
    }
}
