namespace MyShoppingApp
{
    public partial class AppShell : Shell
    {
        private InventoryManager inventoryManager;
        private WishList wishList;

        public AppShell(InventoryManager inventoryManager, WishList wishList)
        {
            InitializeComponent();
            this.inventoryManager = inventoryManager;
            this.wishList = wishList;
        }

        private async void OnInventoryManagementClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InventoryPage(inventoryManager));
        }

        private async void OnShopClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShopPage(inventoryManager, new ShoppingCart("Default")));
        }

        private async void OnConfigurationClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ConfigurationPage());
        }

        private async void OnWishListClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WishListPage(inventoryManager, wishList));
        }
    }
}