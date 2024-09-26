namespace MyShoppingApp
{
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Id { get; set; }
        public int CurrentQuantity { get; set; }
        public bool IsBogo { get; set; }
        public double MarkdownAmount { get; set; }

        public Item(string itemName, string itemDescription, double itemPrice, int itemId, int itemQuantity) //had to flip the Name to the LHS not RHS, and making sure that the itemName and itemDescription are not null or empty
        {
            Name = itemName;
            Description = itemDescription;
            Price = itemPrice;
            Id = itemId;
            CurrentQuantity = itemQuantity; //setting up the inigial quantity
        }

        public override string ToString()  //provide a readable representation
        {
            return $"ID: {Id}, Name: {Name}, Description: {Description}, Price: {Price:C2}, Quantity: {CurrentQuantity}, BOGO: {IsBogo}, Markdown: {MarkdownAmount:C2}";
        }
    }
}