using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Dialogs
{
    public partial class CustomerCreateDialog
    {
        [Parameter]
        public CustomerForCreationDto Customer { get; set; }

        [Parameter]
        public EventCallback<CustomerForCreationDto> OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private CustomerForCreationDto _createdCustomer;

        [Parameter]
        public bool IsVisible { get; set; }

        private string GetModalClass() => IsVisible ? "show d-block" : "fade";

        protected override void OnParametersSet()
        {
            _createdCustomer = new CustomerForCreationDto();
        }

        private async Task HandleSave()
        {
            await OnSave.InvokeAsync(_createdCustomer);
        }

        private async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
