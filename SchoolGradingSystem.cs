using System;
using System.Collections.Generic;

namespace SchoolGrading
{
    // Custom exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // Student class
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            return Score switch
            {
                >= 80 and <= 100 => "A",
                >= 70 and <= 79 => "B",
                >= 60 and <= 69 => "C",
                >= 50 and <= 59 => "D",
                _ => "F"
            };
        }
    }

    // Student Result Processor
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (var reader = new StreamReader(inputFilePath))
            {
                string? line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line))
                        continue; // skip empty lines

                    var parts = line.Split(',');

                    if (parts.Length != 3)
                        throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields, got {parts.Length}.");

                    if (!int.TryParse(parts[0].Trim(), out int id))
                        throw new FormatException($"Line {lineNumber}: Invalid ID format.");

                    string fullName = parts[1].Trim();

                    if (!int.TryParse(parts[2].Trim(), out int score))
                        throw new InvalidScoreFormatException($"Line {lineNumber}: Score is not a valid integer.");

                    students.Add(new Student(id, fullName, score));
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                }
            }
        }
    }
}
