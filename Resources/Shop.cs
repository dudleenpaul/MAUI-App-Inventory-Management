namespace MyShoppingApp
{
    public class Shop
    {
        private InventoryManager inventoryManager; //an instance of IM to manage iM
        private ShoppingCart activeCart; //an instance of shoppin to active

        public Shop(InventoryManager inventoryManager)
        {
            this.inventoryManager = inventoryManager; //assign IM to local iM
            this.activeCart = new ShoppingCart("ActiveCart"); //a new shopping cart named activecart
        }

        public Shop(InventoryManager inventoryManager, ShoppingCart cart)
        {
            this.inventoryManager = inventoryManager;
            this.activeCart = cart; //new shopping cart (active cart) to a local var
        }

        public void AddItemToCart(int itemId, int quantity)
        {
            var item = inventoryManager.GetItemById(itemId); //get the item from the inventory via id
            if (item != null) //if not empty then
            {
                if (item.CurrentQuantity >= quantity) //check if there is enough available
                {
                    activeCart.AddItem(item, quantity); //add item to cart
                    item.CurrentQuantity -= quantity; //decrease the quantity in the inventory
                }
                else
                {
                    throw new InvalidOperationException("Not enough quantity available."); //throw an exception stating
                }
            }
            else
            {
                throw new InvalidOperationException("Item not found."); //if the item doesnt exist
            }
        }

        public void RemoveItemFromCart(int itemId, int quantity)
        {
            var item = inventoryManager.GetItemById(itemId); //item grabbed by id
            if (item != null)
            {
                var cartItem = activeCart.Items.FirstOrDefault(i => i.Item.Id == itemId); //searchin for item in the cart
                if (cartItem != null)
                {
                    if (cartItem.Quantity >= quantity) //checking if theres enough in the cart
                    {
                        cartItem.Quantity -= quantity; //decrease the carts quantity
                        item.CurrentQuantity += quantity; //increase the inventorys quantity
                        if (cartItem.Quantity == 0)
                        {
                            activeCart.Items.Remove(cartItem); //take it out the cart if its 0
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot remove more items than present in the cart."); //when trying to take more than available in the cart
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Item not found."); //if the item doesnt exist
            }
        }

        public string Checkout()
        {
            double taxRate = Preferences.Get("TaxRate", 0.07); //default is 0.07, in preferences
            var receipt = activeCart.GetReceipt(taxRate); //creates receipt w/ tax
            activeCart.Items.Clear(); //emptires the cart after checkout
            return receipt;
        }

        public List<CartItem> GetCartItems()
        {
            return activeCart.Items; //returns a list of items (in the cart)
        }

        public double GetCartTotal()
        {
            double total = 0;
            foreach (var cartItem in activeCart.Items) //iterates through items in the cart
            {
                var item = cartItem.Item; //gets an item
                if (item != null)
                {
                    int quantity = cartItem.Quantity; //gets the items quantity
                    double itemPrice = item.Price; //and price

                    int paidQuantity = quantity; //checks if said item is BOGO
                    if (item.IsBogo)
                    {
                        paidQuantity = (quantity / 2) + (quantity % 2); //calculates the paid part of BOGO item
                    }

                    if (item.MarkdownAmount > 0) //checking for makrdowns
                    {
                        itemPrice -= item.MarkdownAmount; //if it has a markdown it is adjusted here
                    }

                    total += itemPrice * paidQuantity;
                }
            }
            return total * 1.07; // Including tax
        }
    }
}