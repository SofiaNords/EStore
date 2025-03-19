using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace EStore.Components.Pages
{
    public partial class Products
    {
        private bool isLoading = true;
        private bool isEditDialogVisible = false;
        private bool isConfirmDialogVisible = false;
        private bool isDescriptionModalVisible = false;

        private string currentDescription;
        private ProductDto productToEdit;
        private string productIdToDelete;
        private List<ProductDto> products = new List<ProductDto>();
        private string errorMessage;

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

        private void ShowDescription(string description)
        {
            currentDescription = description;
            isDescriptionModalVisible = true;
        }

        private void CloseDescriptionModal()
        {
            isDescriptionModalVisible = false;
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

        private void PrepareEdit(ProductDto product)
        {
            productToEdit = product;
            isEditDialogVisible = true;
        }

        private async Task HandleEditSave(ProductForUpdateDto updatedProduct)
        {
            try
            {
                var response = await Http.PutAsJsonAsync($"api/products/{productToEdit.Id}", updatedProduct);
                if (response.IsSuccessStatusCode)
                {
                    var index = products.FindIndex(p => p.Id == productToEdit.Id);
                    if (index >= 0)
                    {
                        products[index].ProductNumber = updatedProduct.ProductNumber;
                        products[index].Name = updatedProduct.Name;
                        products[index].Price = updatedProduct.Price;
                        products[index].IsDiscontinued = updatedProduct.IsDiscontinued;
                    }
                    isEditDialogVisible = false; 
                }
                else
                {
                    errorMessage = "Något gick fel vid uppdatering av produkten.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Något gick fel: {ex.Message}";
            }
        }

        private void HandleEditCancel()
        {
            isEditDialogVisible = false;  
        }
    }


}
