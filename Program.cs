using System;
using FinanceManagement;
using HealthcareSystem;
using WarehouseInventoryManagementSystem;

namespace FinanceManagement
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Question 1
            Console.WriteLine("Question 1");
            Console.WriteLine("Welcome to the Finance Management System!");
            var app = new Account.FinanceApp();
            app.Run();

            // Question 2
            Console.WriteLine("Question 2");
            Console.WriteLine("Welcome to the Healthcare System!");
            var healthcareApp = new HealthcareApp();
            healthcareApp.seedData();
            healthcareApp.BuildPrescriptions();
            healthcareApp.PrintAllPatients();

            Console.WriteLine("Enter Patient ID to view prescriptions:");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                var prescriptions = healthcareApp.GetPrescriptionsByPatientId(patientId);
                if (prescriptions.Count > 0)
                {
                    healthcareApp.PrintPrescriptionsForPatient(patientId);
                }
                else
                {
                    Console.WriteLine($"No prescriptions found for Patient ID {patientId}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Patient ID.");
            }

            // Question 3
            Console.WriteLine("Question 3");
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
        }
    }
}
