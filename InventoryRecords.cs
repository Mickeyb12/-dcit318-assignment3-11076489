using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventorySystem
{
    // Marker interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Immutable record implementing the interface
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // Generic inventory logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log = new();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item) => _log.Add(item);

        public List<T> GetAll() => new List<T>(_log);

        public void SaveToFile()
        {
            try
            {
                string json = JsonSerializer.Serialize(_log, new JsonSerializerOptions { WriteIndented = true });
                using var writer = new StreamWriter(_filePath);
                writer.Write(json);
                Console.WriteLine($"Data saved to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("No saved file found.");
                    return;
                }

                using var reader = new StreamReader(_filePath);
                string json = reader.ReadToEnd();
                var items = JsonSerializer.Deserialize<List<T>>(json);

                _log = items ?? new List<T>();
                Console.WriteLine($"Loaded {_log.Count} items from {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }
    }

    // InventoryApp integration
    public class InventoryApp
    {
        private readonly InventoryLogger<InventoryItem> _logger;

        public InventoryApp(string filePath)
        {
            _logger = new InventoryLogger<InventoryItem>(filePath);
        }

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Laptop", 5, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Game Console", 15, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Desktop", 10, DateTime.Now));
            _logger.Add(new InventoryItem(4, "Monitor", 7, DateTime.Now));
            _logger.Add(new InventoryItem(5, "Pojector", 50, DateTime.Now));
        }

        public void SaveData() => _logger.SaveToFile();

        public void LoadData() => _logger.LoadFromFile();

        public void PrintAllItems()
        {
            foreach (var item in _logger.GetAll())
            {
                Console.WriteLine($"{item.Name} (ID: {item.Id}) - Qty: {item.Quantity}, Added: {item.DateAdded:g}");
            }
        }
    }
}
