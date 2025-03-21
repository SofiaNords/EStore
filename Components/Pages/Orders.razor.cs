using EStore.Services;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Pages
{
    public partial class Orders
    {
        private bool _isLoading = true;

        private List<OrderDto> _orders = new List<OrderDto>();

        private string _errorMessage;

        [Inject]
        public OrderService OrderService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _orders = await OrderService.GetOrdersAsync();
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
