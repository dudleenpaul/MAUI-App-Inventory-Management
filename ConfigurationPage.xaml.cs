namespace MyShoppingApp
{
    public partial class ConfigurationPage : ContentPage
    {
        public ConfigurationPage()
        {
            InitializeComponent();

            // Load the current tax rate when the page is opened
            TaxRateEntry.Text = Preferences.Get("TaxRate", 0.07).ToString();
        }

        private void OnSaveTaxRateClicked(object sender, EventArgs e)
        {
            if (double.TryParse(TaxRateEntry.Text, out double taxRate))
            {
                Preferences.Set("TaxRate", taxRate);
                DisplayAlert("Success", "Tax rate updated successfully.", "OK");
            }
            else
            {
                DisplayAlert("Error", "Invalid tax rate value.", "OK");
            }
        }
    }
}