﻿namespace EmploRecruitmentTask.Hierarchy.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SuperiorId { get; set; }
        public virtual Employee? Superior { get; set; }
    }
}
