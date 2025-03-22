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
        private bool _isDetailModalVisible = false;

        private OrderDto _currentDetails;
        private List<OrderDto> _orders = new List<OrderDto>();
        private OrderForCreationDto _orderForCreation = new OrderForCreationDto();
        private string _errorMessage;

        private Dictionary<string, string> _customerNames = new Dictionary<string, string>();
        private Dictionary<string, string> _customerEmails = new Dictionary<string, string>();


        [Inject]
        public OrderService OrderService { get; set; }

        [Inject]
        public CustomerService CustomerService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _orders = await OrderService.GetOrdersAsync();

                // Hämta alla kundnamn för de ordrar vi har.
                foreach (var order in _orders)
                {
                    if (!_customerNames.ContainsKey(order.CustomerId))
                    {
                        var customerName = await GetCustomerNameAsync(order.CustomerId);
                        _customerNames[order.CustomerId] = customerName;

                        var customerEmail = await GetCustomerEmailAsync(order.CustomerId);
                        _customerEmails[order.CustomerId] = customerEmail;
                    }
                }
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

        private void ShowDetails(OrderDto details)
        {
            _currentDetails = details;
            _isDetailModalVisible = true;
        }

        private void CloseDetailsModal()
        {
            _isDetailModalVisible = false;
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

        private async Task<string> GetCustomerNameAsync(string customerId)
        {
            var customer = await CustomerService.GetCustomerByIdAsync(customerId);
            return customer != null ? $"{customer.FirstName} {customer.LastName}" : "Okänd kund";
        }

        private decimal CalculateOrderTotal(OrderDto order)
        {
            return order.Items.Sum(item => item.Price * item.Quantity);
        }

        private async Task<string> GetCustomerEmailAsync(string customerId)
        {
            var customer = await CustomerService.GetCustomerByIdAsync(customerId);
            return customer != null ? $"{customer.Email}" : "Okänd epost";
        }

    }
}
