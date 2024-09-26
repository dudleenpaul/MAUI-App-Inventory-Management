using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;

namespace MyShoppingApp
{
    public class InventoryManager
    {
        private List<Item> inventory; //list that'll stor the inventory items
        private int highestId; //this keeps tracko f the highest id
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "inventory.json"); //this goes to the inventory file where things are being stored

        public InventoryManager() //initializes the inventory n loads fro, file
        {
            inventory = new List<Item>();
            highestId = 0;
            LoadState();
        }

        public void Create(string itemName, string itemDescription, double itemPrice, int itemQuantity, bool isBogo, double markdownAmount)
        {
            highestId++; //incrementing this so that the newly created item has a new id
            int itemId = highestId;

            var newItem = new Item(itemName, itemDescription, itemPrice, itemId, itemQuantity) //new item instance
            {
                IsBogo = isBogo,
                MarkdownAmount = markdownAmount
            };
            inventory.Add(newItem); //into the inventory it goes
            SaveState(); //saviung the current state
        }

        public void Delete(int itemId)
        {
            var item = inventory.FirstOrDefault(i => i.Id == itemId); //find an ite and delete it
            if (item != null)
            {
                inventory.Remove(item);
                SaveState();
            }
        }

        public void Update(int itemId, string itemName, string itemDescription, double itemPrice, int itemQuantity, bool isBogo, double markdownAmount)
        {
            var item = inventory.FirstOrDefault(i => i.Id == itemId); //find by id, and update
            if (item != null)
            {
                item.Name = itemName;
                item.Description = itemDescription;
                item.Price = itemPrice;
                item.CurrentQuantity = itemQuantity;
                item.IsBogo = isBogo;
                item.MarkdownAmount = markdownAmount;
                SaveState();
            }
        }

        public List<Item> GetItems()
        {
            return inventory; //will show the inventory upon request
        }

        public Item? GetItemById(int id)
        {
            return inventory.FirstOrDefault(i => i.Id == id); //finds via id
        }

        public void SaveState()
        {
            var jsonData = JsonConvert.SerializeObject(inventory, Formatting.Indented); //inventory file to json format
            File.WriteAllText(filePath, jsonData);
        }

        public void LoadState()
        {
            if (File.Exists(filePath)) //checking if the ifle exists
            {
                var jsonData = File.ReadAllText(filePath); //read the file
                inventory = JsonConvert.DeserializeObject<List<Item>>(jsonData) ?? new List<Item>(); //deserialize json to a list of items
                if (inventory.Any()) //if the inventory inst empty then change highestId
                {
                    highestId = inventory.Max(i => i.Id);
                }
            }
        }
    }
}