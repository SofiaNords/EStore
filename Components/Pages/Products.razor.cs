using EStore.Services;
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
        private bool isCreationModalVisible = false;

        private string _searchQuery;
        private string _currentDescription;
        private ProductDto _productToEdit;
        private ProductForCreationDto _productForCreation = new ProductForCreationDto();
        private string _productIdToDelete;
        private List<ProductDto> _products = new List<ProductDto>();
        private string _errorMessage;

        [Inject]
        public ProductService ProductService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _products = await ProductService.GetProductsAsync();
            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
            finally
            {
                isLoading = false; 
            }
        }

        private async Task SearchProducts()
        {
            isLoading = true;
            try
            {
                _products = await ProductService.GetProductsAsync(null, _searchQuery, null);
            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        private void ShowDescription(string description)
        {
            _currentDescription = description;
            isDescriptionModalVisible = true;
        }

        private void CloseDescriptionModal()
        {
            isDescriptionModalVisible = false;
        }

        private void PrepareCreate()
        {
            _productForCreation = new ProductForCreationDto();
            isCreationModalVisible = true;
        }

        private async Task HandleCreateSave(ProductForCreationDto newProduct)
        {
            try
            {
                var createdProduct = await ProductService.CreateProductAsync(newProduct);

                _products.Add(createdProduct);

                isCreationModalVisible = false;
            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
        }

        private void HandleCreateCancel()
        {
            isCreationModalVisible = false;
        }

        private void PrepareDelete(string productId)
        {
            _productIdToDelete = productId;
            isConfirmDialogVisible = true;
        }


        private async Task HandleDeleteConfirmation(bool isConfirmed)
        {
            if (isConfirmed && !string.IsNullOrEmpty(_productIdToDelete))
            {
                try
                {
                    await ProductService.DeleteProductAsync(_productIdToDelete);

                    _products.RemoveAll(p => p.Id == _productIdToDelete);
                }
                catch (Exception ex)
                {
                    _errorMessage = $"Något gick fel: {ex.Message}";
                }
            }
            isConfirmDialogVisible = false;
        }

        private void PrepareEdit(ProductDto product)
        {
            _productToEdit = product;
            isEditDialogVisible = true;
        }

        private async Task HandleEditSave(ProductForUpdateDto updatedProduct)
        {
            try
            {
                await ProductService.UpdateProductAsync(_productToEdit.Id, updatedProduct);

                var index = _products.FindIndex(p => p.Id == _productToEdit.Id);

                if (index >= 0)
                {
                    _products[index].ProductNumber = updatedProduct.ProductNumber;
                    _products[index].Name = updatedProduct.Name;
                    _products[index].Description = updatedProduct.Description;
                    _products[index].Price = updatedProduct.Price;
                    _products[index].Category = updatedProduct.Category;
                    _products[index].IsDiscontinued = updatedProduct.IsDiscontinued;
                }
                isEditDialogVisible = false; 

            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
        }

        private void HandleEditCancel()
        {
            isEditDialogVisible = false;  
        }
    }


}
