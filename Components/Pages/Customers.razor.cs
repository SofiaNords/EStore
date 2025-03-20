using EStore.Services;
using EStoreAPI.Entities;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Pages
{
    public partial class Customers
    {
        private bool _isLoading = true;
        private bool _isCreationModalVisible = false;

        private string _errorMessage;

        private List<CustomerDto> _customers = new List<CustomerDto>();

        private CustomerForCreationDto _customerForCreation = new CustomerForCreationDto();

        [Inject]
        public CustomerService CustomerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _customers = await CustomerService.GetCustomersAsync();
            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void PrepareCreate()
        {
            _customerForCreation = new CustomerForCreationDto();
            _isCreationModalVisible = true;
        }

        private async Task HandleCreateSave(CustomerForCreationDto newCustomer)
        {
            try
            {
                var createdCustomer = await CustomerService.CreateCustomerAsync(newCustomer);

                _customers.Add(createdCustomer);

                _isCreationModalVisible = false;
            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
        }

        private void HandleCreateCancel()
        {
            _isCreationModalVisible = false;
        }
    }
}
