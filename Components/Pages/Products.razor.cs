using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EStore.Components.Pages
{
    public partial class Products
    {
        private List<ProductDto> products = new List<ProductDto>();
        private bool isLoading = true;
        private string errorMessage;

        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Hämtar produkter från API:et
                products = await Http.GetFromJsonAsync<List<ProductDto>>("api/products");
            }
            catch (Exception ex)
            {
                errorMessage = $"Något gick fel: {ex.Message}";
            }
            finally
            {
                isLoading = false;  // När datan har hämtats, sätt isLoading till false
            }
        }

        // Metod för att ta bort produkt
        private async Task DeleteProduct(string productId)
        {
            // Bekräfta borttagning (valfritt)
            var confirm = await Task.FromResult(ConfirmDelete());
            if (!confirm)
            {
                return;
            }

            try
            {
                var response = await Http.DeleteAsync($"api/products/{productId}");
                if (response.IsSuccessStatusCode)
                {
                    // Ta bort produkten från listan
                    products.RemoveAll(p => p.Id == productId);
                }
                else
                {
                    errorMessage = "Något gick fel vid borttagning av produkten.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Något gick fel: {ex.Message}";
            }
        }

        // Bekräftelsefunktion för att ta bort produkt
        private bool ConfirmDelete()
        {
            return true; // För nu, returnera alltid true för att bekräfta borttagning
        }
    }
}
