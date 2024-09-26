using Newtonsoft.Json;

namespace MyShoppingApp
{
    public class WishList
    {
        private List<ShoppingCart> carts; //a list to store multiple shopping carts
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "wishlist.json"); //filepath to store wishlist data

        public WishList()
        {
            carts = new List<ShoppingCart>(); //initialize list of carts
            LoadState(); //load the state of file wishlist
        }

        public void AddCart(ShoppingCart cart)
        {
            carts.Add(cart); //add cart to the list
            SaveState(); //save to file
        }

        public void RemoveCart(ShoppingCart cart)
        {
            carts.Remove(cart);//remove cart from list
            SaveState(); //save to file
        }

        public ShoppingCart? GetCart(string name) //find cart by name
        {
            return carts.FirstOrDefault(c => c.Name == name); //finds cart that matches name
        }

        public void SaveState() //savin current state to json file
        {
            var jsonData = JsonConvert.SerializeObject(carts, Formatting.Indented); //serializes to json form
            File.WriteAllText(filePath, jsonData); //writes json data to file
        }

        public void LoadState()
        {
            if (File.Exists(filePath)) //checks if file exists
            {
                var jsonData = File.ReadAllText(filePath); //reads the json data
                carts = JsonConvert.DeserializeObject<List<ShoppingCart>>(jsonData) ?? new List<ShoppingCart>(); //deserializes to a list of shopping carts
            }
        }
    }
}