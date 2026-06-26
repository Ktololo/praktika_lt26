using System;
using System.Collections.Generic;
using System.Linq;

namespace task02
{
    public class StudentService
    {
        private readonly List<Student> _students;
        public StudentService(List<Student> students)
        {
            _students = students ?? throw new ArgumentNullException(nameof(students));
        }

        public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        {
            if (string.IsNullOrEmpty(faculty))
                return Enumerable.Empty<Student>();

            return _students.Where(s => s.Faculty != null && s.Faculty.Equals(faculty, StringComparison.OrdinalIgnoreCase));
        }
        public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
        {
            return _students.Where(s => s.AverageGrade >= minAverageGrade);
        }
        public IEnumerable<Student> GetStudentsOrderedByName()
        {
            return _students.Where(s => s.Name != null).OrderBy(s => s.Name);
        }
        public ILookup<string, Student> GroupStudentsByFaculty()
        {
            return _students.Where(s => s.Faculty != null).ToLookup(s => s.Faculty!);
        }
        public string? GetFacultyWithHighestAverageGrade()
        {
            if (_students.Count == 0)
                return null;
            var facultyAverages = _students
                .Where(s => s.Faculty != null)
                .GroupBy(s => s.Faculty!)
                .Select(g => new
                {
                    Faculty = g.Key,
                    AverageGrade = g.Average(s => s.AverageGrade)
                });
            if (!facultyAverages.Any())
                return null;
            var maxAverage = facultyAverages.Max(f => f.AverageGrade);
            return facultyAverages
                .First(f => Math.Abs(f.AverageGrade - maxAverage) < 0.0001)
                .Faculty;
        }
    }
}