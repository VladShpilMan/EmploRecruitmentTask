using EmploRecruitmentTask.EmployeeVacation.EntityDbContext;
using EmploRecruitmentTask.EmployeeVacation.Models;
using EmploRecruitmentTask.EmployeeVacation.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace EmploRecruitmentTask.NUnit
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            DbContextOptions<EmployeeDbContext> options = new DbContextOptions<EmployeeDbContext>();
            Mock<EmployeeDbContext> mockContext = new Mock<EmployeeDbContext>(options);
            _service = new EmployeeService(mockContext.Object);
        }

        [Test]
        public void employee_can_request_vacation()
        {
            // Arrange
            Employee employee = new Employee { Id = 1, Name = "Jan Kowalski" };
            VacationPackage vacationPackage = new VacationPackage { GrantedDays = 20, Year = 2025 };
            List<Vacation> vacations = new List<Vacation>
            {
                new Vacation
                {
                    EmployeeId = 1,
                    DateSince = new DateTime(2025, 1, 1),
                    DateUntil = new DateTime(2025, 1, 5)
                } 
            };

            // Act
            bool result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            // Assert
            Assert.IsTrue(result, "Employee should be able to request vacation with free days remaining.");
        }

        [Test]
        public void employee_cant_request_vacation()
        {
            // Arrange
            Employee employee = new Employee { Id = 1, Name = "Jan Kowalski" };
            VacationPackage vacationPackage = new VacationPackage { GrantedDays = 20, Year = 2025 };
            List<Vacation> vacations = new List<Vacation>
            {
                new Vacation
                {
                    EmployeeId = 1,
                    DateSince = new DateTime(2025, 1, 1),
                    DateUntil = new DateTime(2025, 1, 20)
                } 
            };

            // Act
            bool result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            // Assert
            Assert.IsFalse(result, "Employee should not be able to request vacation with no free days.");
        }
    }
}
