namespace MyShoppingApp
{
    public partial class App : Application
    {
        private InventoryManager inventoryManager;
        private WishList wishList;

        public App()
        {
            InitializeComponent();
            inventoryManager = new InventoryManager();
            wishList = new WishList();
            MainPage = new AppShell(inventoryManager, wishList);
        }

        protected override void OnStart()
        {
            inventoryManager.LoadState();
            wishList.LoadState();
        }

        protected override void OnSleep()
        {
            inventoryManager.SaveState();
            wishList.SaveState();
        }

        protected override void OnResume()
        {
            inventoryManager.LoadState();
            wishList.LoadState();
        }
    }
}
