namespace EmploRecruitmentTask.EmployeeVacation.Models
{
    public class VacationPackage
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int GrantedDays { get; set; }
        public int Year { get; set; }

        public List<Employee>? Employees { get; set; }
    }
}
