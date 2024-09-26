namespace MyShoppingApp
{
    public partial class InventoryPage : ContentPage
    {
        private InventoryManager inventoryManager; //an instance of IM to manage iM

        public InventoryPage(InventoryManager inventoryManager)
        {
            InitializeComponent(); //initializing things on the page
            this.inventoryManager = inventoryManager; //assigning to inventoryManager
        }

        private async void OnCreateItemClicked(object sender, EventArgs e)
        {
            //prompting the user to enter any details
            string name = await DisplayPromptAsync("Create Item", "Enter item name:");
            string description = await DisplayPromptAsync("Create Item", "Enter item description:");
            string priceStr = await DisplayPromptAsync("Create Item", "Enter item price:");
            string quantityStr = await DisplayPromptAsync("Create Item", "Enter item quantity:");
            bool isBogo = await DisplayAlert("Create Item", "Is this item Buy-One-Get-One-Free?", "Yes", "No");
            string markdownStr = await DisplayPromptAsync("Create Item", "Enter markdown amount (if any):");

            if (double.TryParse(priceStr, out double price) && int.TryParse(quantityStr, out int quantity) && double.TryParse(markdownStr, out double markdown)) //validating any inputs
            {
                try
                {
                    inventoryManager.Create(name, description, price, quantity, isBogo, markdown);
                    await DisplayAlert("Success", "Item created successfully.", "OK"); //make the new item
                }
                catch (InvalidOperationException ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid input values.", "OK");
            }
        }

        private void OnListItemsClicked(object sender, EventArgs e) //this shall list all items
        {
            var items = inventoryManager.GetItems(); //grabbing all items

            if (items.Count == 0) //if empty then relay that
            {
                DisplayAlert("Info", "No items to display.", "OK");
            }
            else
            {
                ItemsListView.ItemsSource = GetItemsAsString(items); //otherwise show the ites
            }
        }

        private List<string> GetItemsAsString(List<Item> items) //this is converting the list to a string to present
        {
            var itemsAsString = new List<string>();
            foreach (var item in items)
            {
                itemsAsString.Add(item.ToString());
            }
            return itemsAsString;
        }

        private void OnShowUpdateFieldsClicked(object sender, EventArgs e) //just prompts user to update
        {
            UpdateSectionLabel.IsVisible = true;
            UpdateItemIdEntry.IsVisible = true;
            UpdateItemNameEntry.IsVisible = true;
            UpdateItemDescriptionEntry.IsVisible = true;
            UpdateItemPriceEntry.IsVisible = true;
            UpdateItemQuantityEntry.IsVisible = true;
            UpdateItemIsBogoSwitch.IsVisible = true;
            UpdateItemIsBogoLabel.IsVisible = true;
            UpdateItemMarkdownEntry.IsVisible = true;
            CommitUpdateButton.IsVisible = true;
        }

        private async void OnCommitUpdateClicked(object sender, EventArgs e)
        {
            if (int.TryParse(UpdateItemIdEntry.Text, out int id)) //validating and parsing item id
            {
                var item = inventoryManager.GetItemById(id); //finding with id
                if (item != null) //if nto empty then update
                {
                    string name = UpdateItemNameEntry.Text;
                    string description = UpdateItemDescriptionEntry.Text;
                    string priceStr = UpdateItemPriceEntry.Text;
                    string quantityStr = UpdateItemQuantityEntry.Text;
                    bool isBogo = UpdateItemIsBogoSwitch.IsToggled;
                    string markdownStr = UpdateItemMarkdownEntry.Text;

                    if (double.TryParse(priceStr, out double price) && int.TryParse(quantityStr, out int quantity) && double.TryParse(markdownStr, out double markdown)) //validating those inputs
                    {
                        inventoryManager.Update(id, name, description, price, quantity, isBogo, markdown);
                        await DisplayAlert("Success", "Item updated successfully.", "OK");
                        ResetUpdateFields(); //resets the update field
                        OnListItemsClicked(sender, e); //refreshes the item list after update
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid input values.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Item not found.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid item ID.", "OK");
            }
        }

        private async void OnDeleteItemClicked(object sender, EventArgs e)
        {
            string idStr = await DisplayPromptAsync("Delete Item", "Enter item ID to delete:");
            if (int.TryParse(idStr, out int id)) //validates the id
            {
                inventoryManager.Delete(id); //bye bye item
                OnListItemsClicked(sender, e); //refresh the item list after deletion
            }
        }

        private void ResetUpdateFields() //update is back to its original field
        {
            UpdateSectionLabel.IsVisible = false;
            UpdateItemIdEntry.IsVisible = false;
            UpdateItemNameEntry.IsVisible = false;
            UpdateItemDescriptionEntry.IsVisible = false;
            UpdateItemPriceEntry.IsVisible = false;
            UpdateItemQuantityEntry.IsVisible = false;
            UpdateItemIsBogoSwitch.IsVisible = false;
            UpdateItemIsBogoLabel.IsVisible = false;
            UpdateItemMarkdownEntry.IsVisible = false;
            CommitUpdateButton.IsVisible = false;

            UpdateItemIdEntry.Text = string.Empty;
            UpdateItemNameEntry.Text = string.Empty;
            UpdateItemDescriptionEntry.Text = string.Empty;
            UpdateItemPriceEntry.Text = string.Empty;
            UpdateItemQuantityEntry.Text = string.Empty;
            UpdateItemIsBogoSwitch.IsToggled = false;
            UpdateItemMarkdownEntry.Text = string.Empty;
        }
    }
}