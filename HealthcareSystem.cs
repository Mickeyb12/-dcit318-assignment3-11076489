using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using HealthcareSystem;

namespace HealthcareSystem
{
    
    //    Generic repository interface for any type T
    public class Repository<T>
    {
        private readonly List<T> items = new();
        public void Add(T item) => items.Add(item);

        public List<T> GetAll() => new List<T>(items);

        public T? GetByID(Func<T, bool> predicate) =>
            items.FirstOrDefault(predicate);

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }
  }

// Patient entity
public class Patient
{
    public int ID { get; }
    public string Name { get; }
    public int Age { get; }
    public string Gender { get; }
    public Patient(int id, string name, int age, string gender)
    {
        ID = id;
        Name = name;
        Age = age;
        Gender = gender;
    }

    public override string ToString() => $" Patient: {Name}, {Age}, {Gender}";
}

//Prescription entity
public class Prescription
{
    public int ID { get; }
    public int PatientID { get; }
    public string Medication { get; }
    public DateTime DateIssued { get; }

    public Prescription(int id, int patientId, string medication, DateTime dateIssued)
    {
        ID = id;
        PatientID = patientId;
        Medication = medication;
        DateIssued = dateIssued;
    }

    public override string ToString() => $"Prescription: {ID}, {Medication} issued on {DateIssued:d}";
}

// Main application class
public class HealthcareApp
{
    private readonly Repository<Patient> _patientRepository = new();
    private readonly Repository<Prescription> _prescriptionRepository = new();
    private Dictionary<int, List<Prescription>> _patientPrescriptions = new();

    public void seedData()
    {
        _patientRepository.Add(new Patient(1, "Kevin Heart", 46, "Male"));
        _patientRepository.Add(new Patient(2, "Sarah Connor", 60, "Female"));
        _patientRepository.Add(new Patient(3, "Kylian Mbappé", 26, "Male"));

        _prescriptionRepository.Add(new Prescription(1, 1, "Gentamicin", DateTime.Now.AddDays(-10)));
        _prescriptionRepository.Add(new Prescription(2, 2, "Trisilicate", DateTime.Now.AddDays(-5)));
        _prescriptionRepository.Add(new Prescription(3, 2, "Ibuprofen", DateTime.Now.AddDays(-2)));
        _prescriptionRepository.Add(new Prescription(4, 3, "Paracetamol", DateTime.Now.AddDays(-15)));
        _prescriptionRepository.Add(new Prescription(5, 3, "Metformin", DateTime.Now.AddDays(-7)));
    }

    public void BuildPrescriptions()
    {
        foreach (var prescription in _prescriptionRepository.GetAll())
        {
            if (!_patientPrescriptions.ContainsKey(prescription.PatientID))
            {
                _patientPrescriptions[prescription.PatientID] = new List<Prescription>();
            }
            _patientPrescriptions[prescription.PatientID].Add(prescription);
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int patientId)
    {
        if (_patientPrescriptions.TryGetValue(patientId, out var prescriptions))
        {
            return prescriptions;
        }
        return new List<Prescription>(); 
    }

    public void PrintAllPatients()
    {
        Console.WriteLine("Patients:");
        foreach (var patient in _patientRepository.GetAll())
        {
            Console.WriteLine($"ID: {patient.ID}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
        }
    }

    public void PrintPrescriptionsForPatient(int id)
    {
        var prescriptions = GetPrescriptionsByPatientId(id);
        if (prescriptions.Count == 0)
        {
            Console.WriteLine($"No prescriptions found for patient ID {id}.");
            return;
        }
        Console.WriteLine($"Prescriptions for patient ID {id}:");
        foreach (var prescription in prescriptions)
        {
             Console.WriteLine($"ID: {prescription.ID}, Medication: {prescription.Medication}, Date: {prescription.DateIssued.ToShortDateString()}");
        }
    }
}
           


