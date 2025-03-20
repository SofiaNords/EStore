using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Dialogs
{
    public partial class ProductCreateDialog
    {
        [Parameter]
        public ProductForCreationDto Product { get; set; }

        [Parameter]
        public EventCallback<ProductForCreationDto> OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private ProductForCreationDto createdProduct;

        [Parameter]
        public bool IsVisible { get; set; }

        private string GetModalClass() => IsVisible ? "show d-block" : "fade";

        protected override void OnParametersSet()
        {
            createdProduct = new ProductForCreationDto();
        }

        private async Task HandleSave()
        {
            await OnSave.InvokeAsync(createdProduct);
        }

        private async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
