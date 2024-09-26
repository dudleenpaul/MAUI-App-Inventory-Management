using System.Text;

namespace MyShoppingApp
{
    public class ShoppingCart
    {
        public string Name { get; set; } //name of the cart
        public List<CartItem> Items { get; set; } //list whats inside

        public ShoppingCart(string name) //initializes with a name
        {
            Name = name; //sets the cart name
            Items = new List<CartItem>();
        }

        public void AddItem(Item item, int quantity)
        {
            var cartItem = Items.FirstOrDefault(i => i.Item.Id == item.Id);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem(item, quantity));
            }
        }

        public void RemoveItem(Item item, int quantity)
        {
            var cartItem = Items.FirstOrDefault(i => i.Item.Id == item.Id);
            if (cartItem != null)
            {
                cartItem.Quantity -= quantity;
                if (cartItem.Quantity <= 0)
                {
                    Items.Remove(cartItem);
                }
            }
        }

        public double GetTotal()
        {
            double total = 0;
            foreach (var cartItem in Items)
            {
                double price = cartItem.Item.IsBogo && cartItem.Quantity >= 2
                    ? cartItem.Item.Price * (cartItem.Quantity / 2 + cartItem.Quantity % 2)
                    : cartItem.Item.Price * cartItem.Quantity;
                total += price - cartItem.Item.MarkdownAmount * cartItem.Quantity;
            }
            return total;
        }

        public string GetReceipt(double taxRate) //generates receipt
        {
            StringBuilder receipt = new StringBuilder(); //builds teh receipt
            receipt.AppendLine("Receipt:"); //title
            receipt.AppendLine("Name\tPrice\tQuantity\tTotal"); //header

            double subtotal = 0;
            foreach (var cartItem in Items)
            {
                double price = cartItem.Item.IsBogo && cartItem.Quantity >= 2 //calculates item price with bogo
                    ? cartItem.Item.Price * (cartItem.Quantity / 2 + cartItem.Quantity % 2)
                    : cartItem.Item.Price * cartItem.Quantity;
                double itemTotal = price - cartItem.Item.MarkdownAmount * cartItem.Quantity; //total for current item
                subtotal += itemTotal; //adds to subtotal
                receipt.AppendLine($"{cartItem.Item.Name}\t{cartItem.Item.Price:C2}\t{cartItem.Quantity}\t{itemTotal:C2}");
            }

            //calulates tax n total
            double tax = subtotal * taxRate;
            double total = subtotal + tax;

            //appends subtotal tax n total
            receipt.AppendLine($"Subtotal: {subtotal:C2}");
            receipt.AppendLine($"Tax ({taxRate:P2}): {tax:C2}");
            receipt.AppendLine($"Total: {total:C2}");

            return receipt.ToString(); //returns as string
        }
    }

    public class CartItem
    {
        public Item Item { get; set; } //item thats being bought
        public int Quantity { get; set; } //amount of item

        public CartItem(Item item, int quantity)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item)); //makes sure it isn't null
            Quantity = quantity; //sets quantity
        }
    }
}