namespace EmploRecruitmentTask.EmployeeVacation.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<Employee>? Employees { get; set; }
    }
}
