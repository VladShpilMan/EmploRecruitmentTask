namespace EmploRecruitmentTask.Hierarchy.Models
{
    public class EmployeeStructure
    {
        public int EmployeeId { get; set; }
        public int SuperiorId { get; set; }
        public int Level { get; set; }
    }
}
