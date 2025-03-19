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
        private bool isConfirmDialogVisible = false;
        private string productIdToDelete;

        [Inject]
        public HttpClient Http { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                products = await Http.GetFromJsonAsync<List<ProductDto>>("api/products");
            }
            catch (Exception ex)
            {
                errorMessage = $"Något gick fel: {ex.Message}";
            }
            finally
            {
                isLoading = false; 
            }
        }

        private void PrepareDelete(string productId)
        {
            productIdToDelete = productId;
            isConfirmDialogVisible = true;
        }


        private async Task HandleDeleteConfirmation(bool isConfirmed)
        {
            if (isConfirmed && !string.IsNullOrEmpty(productIdToDelete))
            {
                try
                {
                    var response = await Http.DeleteAsync($"api/products/{productIdToDelete}");
                    if (response.IsSuccessStatusCode)
                    {
                        products.RemoveAll(p => p.Id == productIdToDelete);
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
            isConfirmDialogVisible = false;
        }
    }
}
