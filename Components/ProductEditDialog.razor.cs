using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components
{
    public partial class ProductEditDialog
    {
        [Parameter]
        public ProductDto Product { get; set; }

        [Parameter]
        public EventCallback<ProductForUpdateDto> OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private ProductForUpdateDto editedProduct;

        [Parameter]
        public bool IsVisible { get; set; }

        private string GetModalClass() => IsVisible ? "show d-block" : "fade";

        protected override void OnParametersSet()
        {
            editedProduct = Product != null ? new ProductForUpdateDto
            {
                ProductNumber = Product.ProductNumber,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Category = Product.Category,
                IsDiscontinued = Product.IsDiscontinued
            } : new ProductForUpdateDto();
        }

        private async Task HandleSave()
        {
            await OnSave.InvokeAsync(editedProduct);
        }

        private async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
