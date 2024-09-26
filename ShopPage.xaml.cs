namespace MyShoppingApp
{
    public partial class ShopPage : ContentPage
    {
        private InventoryManager inventoryManager; //an instance of IM to manage iM
        private ShoppingCart activeCart; //an instance of shoppin to active

        public ShopPage(InventoryManager inventoryManager, ShoppingCart activeCart)
        {
            InitializeComponent();
            this.inventoryManager = inventoryManager; //assign IM to local iM
            this.activeCart = activeCart; //a new shopping cart named activecart
            UpdateCartItems(); //cart items list update
        }

        private void OnAddToCartClicked(object sender, EventArgs e) //addin to cart
        {
            if (int.TryParse(ItemIdEntry.Text, out int itemId) && int.TryParse(QuantityEntry.Text, out int quantity)) //parses id n quantity
            {
                var item = inventoryManager.GetItemById(itemId); //finds via id
                if (item != null)
                {
                    if (item.CurrentQuantity >= quantity) //checks on the quantity available
                    {
                        activeCart.AddItem(item, quantity); //adds to cart
                        item.CurrentQuantity -= quantity; //reduces the amount in inventory
                        DisplayAlert("Success", "Item added to cart.", "OK"); //confirmation message
                        UpdateCartItems(); //cart list updated
                    }
                    else
                    {
                        DisplayAlert("Error", "Not enough quantity available.", "OK"); //insuffiencient quantity!!
                    }
                }
                else
                {
                    DisplayAlert("Error", "Item not found.", "OK"); //the item just doesnt exist
                }
            }
            else
            {
                DisplayAlert("Error", "Invalid item ID or quantity.", "OK"); //invalid input
            }
        }

        private void UpdateCartItems()
        {
            CartItemsListView.ItemsSource = null; //clear the current values

            var itemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) }); //colum for name
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); //column for price
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); //colum for quantity

                var nameLabel = new Label();
                var priceLabel = new Label();
                var quantityLabel = new Label();

                nameLabel.SetBinding(Label.TextProperty, "Item.Name"); //binding the item name
                priceLabel.SetBinding(Label.TextProperty, new Binding("Item.Price", stringFormat: "{0:C2}")); //binding item price
                quantityLabel.SetBinding(Label.TextProperty, "Quantity"); //binding quantity

                grid.Children.Add(nameLabel);
                Grid.SetColumn(nameLabel, 0);

                grid.Children.Add(priceLabel);
                Grid.SetColumn(priceLabel, 1);

                grid.Children.Add(quantityLabel);
                Grid.SetColumn(quantityLabel, 2);

                return new ViewCell { View = grid };
            });

            CartItemsListView.ItemTemplate = itemTemplate; //setting templates
            CartItemsListView.ItemsSource = activeCart.Items;
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            try
            {
                double taxRate = Preferences.Get("TaxRate", 0.07);
                string receipt = activeCart.GetReceipt(taxRate);
                await DisplayAlert("Checkout", receipt, "OK");
                activeCart.Items.Clear();
                UpdateCartItems();
                TotalLabel.Text = receipt;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}