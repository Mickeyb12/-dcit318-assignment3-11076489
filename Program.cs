using System;
using FinanceManagement;
using HealthcareSystem;

namespace FinanceManagement
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // Question 1
            Console.WriteLine("Question 1");
            Console.WriteLine("Welcome to the Finance Management System!");
            var app = new Account.FinanceApp(); // create a variable called 'app'
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
        }
    }
}
    