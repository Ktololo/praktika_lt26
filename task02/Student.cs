using System;
using System.Collections.Generic;
using System.Linq;

namespace task02
{
    public class Student
    {
        public string? Name { get; set; }
        public string? Faculty { get; set; }
        public List<int>? Grades { get; set; }
        public double AverageGrade => Grades != null && Grades.Count > 0 ? Grades.Average() : 0;
    }
}