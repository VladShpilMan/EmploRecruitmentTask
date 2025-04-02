using EmploRecruitmentTask.Hierarchy.Models;

namespace EmploRecruitmentTask.Hierarchy.Services
{
    public class EmployeesStructure
    {
        private readonly Dictionary<int, List<EmployeeStructure>> _relations = new Dictionary<int, List<EmployeeStructure>>();

        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            _relations.Clear();

            List<int> duplicates = employees.GroupBy(e => e.Id)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key)
                                      .ToList();
            if (duplicates.Any())
                throw new ArgumentException($"Duplicate IDs: {string.Join(", ", duplicates)}");

            Dictionary<int, List<Employee>> subordinatesDict = employees
                .Where(e => e.SuperiorId.HasValue)
                .GroupBy(e => e.SuperiorId!.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            IEnumerable<Employee> roots = employees.Where(e => !e.SuperiorId.HasValue);
            foreach (Employee root in roots)
            {
                DFS(root, new List<Employee>(), subordinatesDict);
            }

            foreach (Employee employee in employees)
            {
                if (!_relations.ContainsKey(employee.Id))
                    _relations[employee.Id] = new List<EmployeeStructure>();
            }

            return _relations.SelectMany(r => r.Value).ToList();
        }

        private void DFS(Employee employee, List<Employee> ancestors, Dictionary<int, List<Employee>> subordinatesDict)
        {
            if (ancestors.Any(a => a.Id == employee.Id))
                throw new InvalidOperationException($"Cycle detected in hierarchy for ID {employee.Id}");

            List<EmployeeStructure> relations = new List<EmployeeStructure>();
            int level = 1;

            for (int i = ancestors.Count - 1; i >= 0; i--)
            {
                relations.Add(new EmployeeStructure
                {
                    EmployeeId = employee.Id,
                    SuperiorId = ancestors[i].Id,
                    Level = level
                });
                level++;
            }
            _relations[employee.Id] = relations;

            if (subordinatesDict.TryGetValue(employee.Id, out List<Employee>? subordinates))
            {
                ancestors.Add(employee);
                foreach (Employee sub in subordinates)
                {
                    DFS(sub, ancestors, subordinatesDict);
                }
                ancestors.RemoveAt(ancestors.Count - 1);
            }
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            if (_relations.TryGetValue(employeeId, out List<EmployeeStructure>? relations))
            {
                EmployeeStructure? relation = relations.FirstOrDefault(r => r.SuperiorId == superiorId);
                return relation?.Level;
            }
            return null;
        }
    }
}
