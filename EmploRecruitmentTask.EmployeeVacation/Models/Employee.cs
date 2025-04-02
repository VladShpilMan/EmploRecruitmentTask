namespace EmploRecruitmentTask.EmployeeVacation.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public int VacationPackageId { get; set; }

        public Team Team { get; set; }
        public VacationPackage VacationPackage { get; set; }
        public List<Vacation> Vacations { get; set; }
    }
}
