using EStore.Services;
using EStoreAPI.Entities;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Pages
{
    public partial class Customers
    {
        private bool _isLoading = true;

        private string _errorMessage;

        private List<CustomerDto> _customers = new List<CustomerDto>();

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
    }
}
