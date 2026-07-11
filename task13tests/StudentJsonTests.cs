using System;
using System.IO;
using System.Collections.Generic;
using task13;
using Xunit;

namespace task13tests
{
    public class StudentJsonTests
    {
        private Student GetTestStudent()
        {
            return new Student
            {
                FirstName = "Абдула",
                LastName = "Абдулович",
                BirthDate = new DateTime(2000, 5, 15),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Математика", Grade = 5 },
                    new Subject { Name = "Физика", Grade = 4 }
                }
            };
        }

        [Fact]
        public void Serialize_ShouldReturnValidJson()
        {
            var student = GetTestStudent();
            var json = StudentJsonHelper.Serialize(student);
            var result = StudentJsonHelper.Deserialize(json);

            Assert.Equal("Абдула", result.FirstName);
            Assert.Equal("Абдулович", result.LastName);
            Assert.Equal(new DateTime(2000, 5, 15), result.BirthDate);
            Assert.Equal(2, result.Grades.Count);
        }

        [Fact]
        public void Deserialize_ShouldReturnCorrectStudent()
        {
            var json = @"
            {
                ""FirstName"": ""Анастасия"",
                ""LastName"": ""Волочкова"",
                ""BirthDate"": ""1999-12-01"",
                ""Grades"": [
                    { ""Name"": ""Химия"", ""Grade"": 5 }
                ]
            }";

            var student = StudentJsonHelper.Deserialize(json);

            Assert.Equal("Анастасия", student.FirstName);
            Assert.Equal("Волочкова", student.LastName);
            Assert.Equal(new DateTime(1999, 12, 1), student.BirthDate);
            Assert.Single(student.Grades);
        }

        [Fact]
        public void Serialize_ShouldIgnoreNullProperties()
        {
            var student = new Student
            {
                FirstName = "Тест",
                LastName = null,
                BirthDate = DateTime.Now,
                Grades = null
            };

            var json = StudentJsonHelper.Serialize(student);

            Assert.DoesNotContain("LastName", json);
            Assert.DoesNotContain("Grades", json);
        }

        [Fact]
        public void Deserialize_InvalidJson_ThrowsException()
        {
            var invalidJson = "{ FirstName: }";
            Assert.Throws<InvalidOperationException>(() => StudentJsonHelper.Deserialize(invalidJson));
        }

        [Fact]
        public void Serialize_NullStudent_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => StudentJsonHelper.Serialize(null));
        }

        [Fact]
        public void SaveAndLoadFile_ShouldWork()
        {
            var student = GetTestStudent();
            var path = Path.GetTempFileName();

            try
            {
                StudentJsonHelper.SaveToFile(student, path);
                var loaded = StudentJsonHelper.LoadFromFile(path);

                Assert.Equal(student.FirstName, loaded.FirstName);
                Assert.Equal(student.LastName, loaded.LastName);
                Assert.Equal(student.BirthDate, loaded.BirthDate);
                Assert.Equal(student.Grades.Count, loaded.Grades.Count);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }

        [Fact]
        public void LoadFromFile_FileNotFound_ThrowsException()
        {
            Assert.Throws<FileNotFoundException>(() => StudentJsonHelper.LoadFromFile("not_exists.json"));
        }
    }
}