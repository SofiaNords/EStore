using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Dialogs
{
    public partial class CustomerEditDialog
    {
        [Parameter]
        public CustomerDto Customer { get; set; }

        [Parameter]
        public EventCallback<CustomerForUpdateDto> OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private CustomerForUpdateDto _editedCustomer;

        [Parameter]
        public bool IsVisible { get; set; }

        private string GetModalClass() => IsVisible ? "show d-block" : "fade";

        protected override void OnParametersSet()
        {
            _editedCustomer = Customer != null ? new CustomerForUpdateDto
            {
                FirstName = Customer.FirstName,
                LastName = Customer.LastName,
                Email = Customer.Email,
                Mobile = Customer.Mobile,
                Street = Customer.Street,
                PostalCode = Customer.PostalCode,
                City = Customer.City,
                Country = Customer.Country
            } : new CustomerForUpdateDto();
        }

        private async Task HandleSave()
        {
            await OnSave.InvokeAsync(_editedCustomer);
        }

        private async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
