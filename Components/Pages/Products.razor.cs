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
    }
}
