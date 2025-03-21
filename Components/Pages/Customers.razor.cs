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
        private bool _isEditDialogVisible = false;

        private string _searchQuery;
        private string _errorMessage;
        private CustomerDto _customerToEdit;
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

        private async Task SearchCustomers()
        {
            _isLoading = true;
            try
            {
                _customers = await CustomerService.GetCustomersAsync(_searchQuery);
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

        private void PrepareEdit(CustomerDto customer)
        {
            _customerToEdit = customer;
            _isEditDialogVisible = true;
        }

        private async Task HandleEditSave(CustomerForUpdateDto updatedCustomer)
        {
            try
            {
                await CustomerService.UpdateCustomerAsync(_customerToEdit.Id, updatedCustomer);

                var index = _customers.FindIndex(c => c.Id == _customerToEdit.Id);

                if (index >= 0)
                {
                    _customers[index].FirstName = updatedCustomer.FirstName;
                    _customers[index].LastName = updatedCustomer.LastName;
                    _customers[index].Email = updatedCustomer.Email;
                    _customers[index].Mobile = updatedCustomer.Mobile;
                    _customers[index].Street = updatedCustomer.Street;
                    _customers[index].PostalCode = updatedCustomer.PostalCode;
                    _customers[index].City = updatedCustomer.City;
                    _customers[index].Country = updatedCustomer.Country;
                }
                _isEditDialogVisible = false;
            }
            catch (Exception ex)
            {
                _errorMessage = $"Något gick fel: {ex.Message}";
            }
        }
        private void HandleEditCancel()
        {
            _isEditDialogVisible = false;
        }
    }
}
