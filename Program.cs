using System;
using System.Collections.Generic;
using System.IO;
using FinanceManagement;
using HealthcareSystem;
using WarehouseInventoryManagementSystem;
using SchoolGrading;
using InventorySystem;

namespace MainApplication
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // ============================
            // Question 1: Finance Management System
            // ============================
            Console.WriteLine("Question 1");
            Console.WriteLine("Welcome to the Finance Management System!");
            var financeApp = new Account.FinanceApp();
            financeApp.Run();


            // ============================
            // Question 2: Healthcare System
            // ============================
            Console.WriteLine("\nQuestion 2");
            Console.WriteLine("Welcome to the Healthcare System!");
            var healthcareApp = new HealthcareApp();
            healthcareApp.seedData();
            healthcareApp.BuildPrescriptions();
            healthcareApp.PrintAllPatients();

            Console.WriteLine("Enter Patient ID to view prescriptions:");
            if (int.TryParse(Console.ReadLine(), out int patientIdQ2))
            {
                var prescriptions = healthcareApp.GetPrescriptionsByPatientId(patientIdQ2);
                if (prescriptions.Count > 0)
                {
                    healthcareApp.PrintPrescriptionsForPatient(patientIdQ2);
                }
                else
                {
                    Console.WriteLine($"No prescriptions found for Patient ID {patientIdQ2}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }


            // ============================
            // Question 3: Warehouse Inventory Management System
            // ============================
            Console.WriteLine("\nQuestion 3");
            Console.WriteLine("Welcome to the Warehouse Inventory Management System!");
            var manager1 = new WareHouseManager();
            manager1.seedData();

            Console.WriteLine("\n--- All Items ---");
            manager1.PrintAllItems();

            Console.WriteLine("\n--- Testing Exceptions ---");
            try
            {
                manager1.ElectronicInventory.AddItem(new ElectronicItem(1, "Tablet", 5, "Apple", 12));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            manager1.RemoveItem(manager1.GroceryInventory, 99);
            manager1.IncreaseStock(manager1.ElectronicInventory, 2, -50);


            // ============================
            // Question 4: School Grading System
            // ============================
            Console.WriteLine("\nQuestion 4");
            Console.WriteLine("Welcome to the School Grading System!");
            var processor = new StudentResultProcessor();
            string inputFilePath = "input.txt";
            string outputFilePath = "report.txt";

            try
            {
                List<Student> students = processor.ReadStudentsFromFile(inputFilePath);
                processor.WriteReportToFile(students, outputFilePath);
                Console.WriteLine($"Report successfully written to {outputFilePath}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The input file '{inputFilePath}' was not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Score format error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Format error: {ex.Message}");
            }
            catch (System.MissingFieldException ex)
            {
                Console.WriteLine($"Missing field error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }


            // ============================
            // Question 5: Inventory System (New Session Simulation)
            // ============================
            Console.WriteLine("\nQuestion 5");
            Console.WriteLine("Welcome to the Inventory System!");
            string filePath = "inventory.json";

            // First session
            var app = new InventoryApp(filePath);
            app.SeedSampleData();
            app.SaveData();

            // Simulate new session (clear from memory)
            Console.WriteLine("\n--- New Session ---");
            var newApp = new InventoryApp(filePath);
            newApp.LoadData();
            newApp.PrintAllItems();
        }
    }
}
