using EStore.Services;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace EStore.Components.Pages
{
    public partial class Orders : ComponentBase
    {
        private bool _isLoading = true;
        private bool _isCreationModalVisible = false;

        private List<OrderDto> _orders = new List<OrderDto>();
        private OrderForCreationDto _orderForCreation = new OrderForCreationDto();
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

        private void PrepareCreate()
        {
            _orderForCreation = new OrderForCreationDto();
            _isCreationModalVisible = true;
        }

        private async Task HandleCreateSave(OrderForCreationDto newOrder)
        {
            try
            {
                var createdOrder = await OrderService.CreateOrderAsync(newOrder);

                _orders.Add(createdOrder);

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
