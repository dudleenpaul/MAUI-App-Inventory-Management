namespace MyShoppingApp
{
    public partial class MainPage : ContentPage
    {
        private InventoryManager inventoryManager; //an instance of IM to manage iM
        private WishList wishList; //an instance of WL to manage wL

        public MainPage()
        {
            InitializeComponent();
            inventoryManager = new InventoryManager();
            wishList = new WishList();
            //initializing all of these
        }

        private async void OnInventoryManagementClicked(object sender, EventArgs e) //navigates to inventory page
        {
            await Navigation.PushAsync(new InventoryPage(inventoryManager));
        }

        private async void OnShopClicked(object sender, EventArgs e) //navigates to Shop
        {
            await Navigation.PushAsync(new ShopPage(inventoryManager, new ShoppingCart("Default")));
        }

        private async void OnConfigurationClicked(object sender, EventArgs e) //navigates to configuration
        {
            await Navigation.PushAsync(new ConfigurationPage());
        }

        private async void OnWishListClicked(object sender, EventArgs e) //navigates tot he wishlist
        {
            await Navigation.PushAsync(new WishListPage(inventoryManager, wishList));
        }
    }
}