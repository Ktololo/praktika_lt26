using System;
using System.Collections.Generic;
using System.Linq;
using task02;
using Xunit;

namespace task02tests
{
    public class StudentServiceTests
    {
        private readonly List<Student> _testStudents;
        private readonly StudentService _service;
        public StudentServiceTests()
        {
            _testStudents = new List<Student>
            {
                new() { Name = "Леша", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
                new() { Name = "Аня", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
                new() { Name = "Абдула", Faculty = "МО", Grades = new List<int> { 5, 5, 5 } },
                new() { Name = "Мария", Faculty = "МО", Grades = new List<int> { 4, 4, 4 } },
                new() { Name = "Сергей", Faculty = "ИФМО", Grades = new List<int> { 2, 3, 2 } }
            };
            _service = new StudentService(_testStudents);
        }
        [Fact]
        public void GetStudentsByFaculty_WhenFacultyExists_ReturnsCorrectStudents()
        {
            var result = _service.GetStudentsByFaculty("ФИТ").ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, s => Assert.Equal("ФИТ", s.Faculty));
        }
        [Fact]
        public void GetStudentsByFaculty_WhenFacultyDoesNotExist_ReturnsEmpty()
        {
            var result = _service.GetStudentsByFaculty("Информатика").ToList();
            Assert.Empty(result);
        }
        [Fact]
        public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
        {
            var result = _service.GetStudentsWithMinAverageGrade(4.0).ToList();
            Assert.Equal(3, result.Count);
            Assert.All(result, s => Assert.True(s.AverageGrade >= 4.0));
        }
        [Fact]
        public void GetStudentsOrderedByName_ReturnsStudentsInAlphabeticalOrder()
        {
            var result = _service.GetStudentsOrderedByName().ToList();
            var expectedNames = new List<string> { "Абдула", "Аня", "Леша", "Мария", "Сергей" };

            Assert.Equal(expectedNames, result.Select(s => s.Name));
        }
        [Fact]
        public void GroupStudentsByFaculty_ReturnsCorrectGroups()
        {
            var result = _service.GroupStudentsByFaculty();

            Assert.Equal(3, result.Count);
            Assert.Equal(2, result["ФИТ"].Count());
            Assert.Equal(2, result["МО"].Count());
            Assert.Single(result["ИФМО"]);
        }
        [Fact]
        public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
        {
            var result = _service.GetFacultyWithHighestAverageGrade();
            Assert.Equal("МО", result);
        }
        [Fact]
        public void Constructor_WithNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new StudentService(null));
        }
    }
}