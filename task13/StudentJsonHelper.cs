using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13
{
    public static class StudentJsonHelper
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        public static string Serialize(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            try
            {
                return JsonSerializer.Serialize(student, _options);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка сериализации", ex);
            }
        }
        public static Student Deserialize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentNullException(nameof(json));
            try
            {
                return JsonSerializer.Deserialize<Student>(json, _options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Ошибка десериализации JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при десериализации", ex);
            }
        }
        public static void SaveToFile(Student student, string filePath)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            try
            {
                var json = Serialize(student);
                File.WriteAllText(filePath, json);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Ошибка записи файла: {filePath}", ex);
            }
        }

        public static Student LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл не найден: {filePath}");

            try
            {
                var json = File.ReadAllText(filePath);
                return Deserialize(json);
            }
            catch (IOException ex)
            {
                throw new InvalidOperationException($"Ошибка чтения файла: {filePath}", ex);
            }
        }
    }
}