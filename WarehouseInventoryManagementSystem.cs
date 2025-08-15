using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace WarehouseInventoryManagementSystem
{
    //Interface for inventory management
    public interface IInventoryManagement
    {
        int ID { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    //Custom exception for inventory management
    public class InventoryException : Exception
    {
        public InventoryException(string message) : base(message) { }
    }

    public class ItemNotFoundException : InventoryException
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : InventoryException
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // Class representing an electronic item in the inventory
    public class ElectronicItem : IInventoryManagement
    {
        public int ID { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            ID = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

    //Class representing a grocery item in the inventory
    public class GroceryItem : IInventoryManagement
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            ID = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    //Generic repository for managing inventory items
    public class InventoryRepository<T> where T : IInventoryManagement
    {
        private readonly Dictionary<int, T> _inventoryItems = new();

        // Add an item to the inventory; throws an exception if the item already exists
        public void AddItem(T item)
        {
            if (_inventoryItems.ContainsKey(item.ID))
            {
                throw new InventoryException($"Item with ID {item.ID} already exists.");
            }
            _inventoryItems.Add(item.ID, item);
        }

        //Get an item by ID; throws an exception if the item does not exist
        public T GetItem(int id)
        {
            if (!_inventoryItems.TryGetValue(id, out var item))
            {
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            }
            return item;
        }

        // Remove an item by ID; throws an exception if the item does not exist
        public void RemoveItem(int id)
        {
            if (!_inventoryItems.Remove(id))
            {
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            }
        }

        //Get all items in the inventory
        public List<T> GetAllItems()
        {
            return new List<T>(_inventoryItems.Values);
        }

        // Update the quantity of an item; throws an exception if the new quantity is invalid
        public void UpdateItemQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidQuantityException("Quantity cannot be negative.");
            }
            var item = GetItem(id);
            item.Quantity = newQuantity;
        }
    }

    // Warehouse Manager class
    public class WareHouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _inventoryRepository = new();
        private readonly InventoryRepository<GroceryItem> _groceryRepository = new();

        public InventoryRepository<ElectronicItem> ElectronicInventory => _inventoryRepository;
        public InventoryRepository<GroceryItem> GroceryInventory => _groceryRepository;

        public void seedData()
        {
            // Seed electronic items
            _inventoryRepository.AddItem(new ElectronicItem(1, "Laptop", 10, "Macbook", 24));
            _inventoryRepository.AddItem(new ElectronicItem(2, "Smartphone", 20, "Tecno", 12));
            _inventoryRepository.AddItem(new ElectronicItem(3, "Tablet", 15, "Apple", 18));

            // Seed grocery items
            _groceryRepository.AddItem(new GroceryItem(1, "Oats", 50, DateTime.Now.AddDays(7)));
            _groceryRepository.AddItem(new GroceryItem(2, "Bacon", 30, DateTime.Now.AddDays(3)));
            _groceryRepository.AddItem(new GroceryItem(3, "Jam", 100, DateTime.Now.AddDays(14)));
        }

        public void PrintAllItems()
        {
            Console.WriteLine("Electronic Items:");
            foreach (var item in _inventoryRepository.GetAllItems())
            {
                Console.WriteLine($"ID: {item.ID}, Name: {item.Name}, Quantity: {item.Quantity}, Brand: {item.Brand}, Warranty: {item.WarrantyMonths} months");
            }

            Console.WriteLine("Grocery Items:");
            foreach (var item in _groceryRepository.GetAllItems())
            {
                Console.WriteLine($"ID: {item.ID}, Name: {item.Name}, Quantity: {item.Quantity}, Expiry Date: {item.ExpiryDate.ToShortDateString()}");
            }
        }

        public void IncreaseStock<T>(InventoryRepository<T> repository, int id, int quantity) where T : IInventoryManagement
        {
            try
            {
                var item = repository.GetItem(id);
                repository.UpdateItemQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Stock increased for item ID {id}. New quantity: {item.Quantity}");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"Quantity error for item ID {id}: {ex.Message}");
            }
        }

        public void RemoveItem<T>(InventoryRepository<T> repository, int id) where T : IInventoryManagement
        {
            try
            {
                repository.RemoveItem(id);
                Console.WriteLine($"Item with ID {id} removed successfully.");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
            }
        }
    }
}
