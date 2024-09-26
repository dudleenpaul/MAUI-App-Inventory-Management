namespace MyShoppingApp
{
    public partial class WishListPage : ContentPage
    {
        private WishList wishList; //stores the users wishlist of shopping carts
        private InventoryManager inventoryManager; //manages inventory of items

        public WishListPage(InventoryManager inventoryManager, WishList wishList)
        {
            InitializeComponent();
            this.inventoryManager = inventoryManager; //assigns inventory manager
            this.wishList = wishList; //assigns wishlists
            CartPicker.SelectedIndexChanged += OnCartPickerSelectedIndexChanged; //handles when selected cart is changed
        }

        private void OnAddCartClicked(object sender, EventArgs e) //when add event is clicked
        {
            var cartName = CartNameEntry.Text; //gets cart name from input
            if (!string.IsNullOrEmpty(cartName) && wishList.GetCart(cartName) == null)
            {
                var newCart = new ShoppingCart(cartName); //makes cart with name
                wishList.AddCart(newCart); //added to wishlist
                CartPicker.Items.Add(cartName); //joins list
                CartNameEntry.Text = string.Empty; //clears field
            }
        }

        private void OnRemoveCartClicked(object sender, EventArgs e)
        {
            var cartName = CartPicker.SelectedItem?.ToString(); //gets selected cart name from dropdown
            if (cartName != null)
            {
                var cart = wishList.GetCart(cartName); //gets cart from wishlist
                if (cart != null)
                {
                    wishList.RemoveCart(cart); //removes cart from wishlist
                    CartPicker.Items.Remove(cartName); //removes cart from list
                    CartItemsListView.ItemsSource = null;
                    ReceiptLabel.Text = string.Empty;
                }
            }
        }

        private void OnViewCartClicked(object sender, EventArgs e)
        {
            var cartName = CartPicker.SelectedItem?.ToString();
            if (cartName != null)
            {
                var cart = wishList.GetCart(cartName);
                if (cart != null)
                {
                    CartItemsListView.ItemsSource = null; //clears items in list view

                    var itemTemplate = new DataTemplate(() => //template for showing cart items
                    {
                        var grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        var nameLabel = new Label(); 
                        var priceLabel = new Label(); 
                        var quantityLabel = new Label(); 

                        nameLabel.SetBinding(Label.TextProperty, "Item.Name"); //binds name
                        priceLabel.SetBinding(Label.TextProperty, new Binding("Item.Price", stringFormat: "{0:F2}")); //binds price
                        quantityLabel.SetBinding(Label.TextProperty, "Quantity"); //binds quantity

                        grid.Children.Add(nameLabel); //adds nae to grid
                        Grid.SetColumn(nameLabel, 0);

                        grid.Children.Add(priceLabel); //adds price to grid
                        Grid.SetColumn(priceLabel, 1);

                        grid.Children.Add(quantityLabel); //adds quantity to grid
                        Grid.SetColumn(quantityLabel, 2);

                        return new ViewCell { View = grid };
                    });

                    CartItemsListView.ItemTemplate = itemTemplate;
                    CartItemsListView.ItemsSource = cart.Items;
                }
            }
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            var cartName = CartPicker.SelectedItem?.ToString();
            if (cartName != null)
            {
                var cart = wishList.GetCart(cartName);
                if (cart != null)
                {
                    double taxRate = Preferences.Get("TaxRate", 0.07);
                    var receipt = cart.GetReceipt(taxRate);
                    await DisplayAlert("Checkout", receipt, "OK");
                    cart.Items.Clear();
                    CartItemsListView.ItemsSource = null;
                    ReceiptLabel.Text = receipt;
                }
            }
        }

        private void OnCartPickerSelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                OnViewCartClicked(sender, e); //refreshes view bsed oncart
            }
        }


        private async void OnShopForCartClicked(object sender, EventArgs e)
        {
            var cartName = CartPicker.SelectedItem?.ToString();
            if (cartName != null)
            {
                var cart = wishList.GetCart(cartName);
                if (cart != null)
                {
                    await Navigation.PushAsync(new ShopPage(inventoryManager, cart)); //navigates to shop page
                }
                else
                {
                    await DisplayAlert("Error", "Please select a cart first.", "OK");
                }
            }
        }
    }
}