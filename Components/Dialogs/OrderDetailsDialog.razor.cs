using EStore.Services;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Dialogs
{
    public partial class OrderDetailsDialog
    {
        [Parameter]
        public OrderDto Order { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        private OrderDto _order;

        private Dictionary<string, string> _productNames = new Dictionary<string, string>();
        private Dictionary<string, string> _customerNames = new Dictionary<string, string>();
        private Dictionary<string, string> _customerEmails = new Dictionary<string, string>();

        [Inject]
        public ProductService ProductService { get; set; }

        [Inject]
        public CustomerService CustomerService { get; set; }

        private string GetModalClass() => IsVisible ? "modal show d-block" : "modal fade";

        protected override async Task OnParametersSetAsync()
        {
            if (Order != null && !string.IsNullOrEmpty(Order.CustomerId))
            {
                _order = new OrderDto
                {
                    Id = Order.Id,
                    CustomerId = Order.CustomerId,
                    OrderDate = Order.OrderDate,
                    Items = Order.Items != null ? new List<OrderItemDto>(Order.Items) : new List<OrderItemDto>()
                };

                // Hämta kundens namn om det inte redan finns i _customerNames
                if (!_customerNames.ContainsKey(Order.CustomerId))
                {
                    var customer = await CustomerService.GetCustomerByIdAsync(Order.CustomerId);
                    if (customer != null)
                    {
                        _customerNames[Order.CustomerId] = $"{customer.FirstName} {customer.LastName}";
                        _customerEmails[Order.CustomerId] = customer.Email; // Spara kundens e-postadress
                    }
                    else
                    {
                        _customerNames[Order.CustomerId] = "Okänd kund";
                        _customerEmails[Order.CustomerId] = "Okänd e-post"; // Om kunden inte hittas, sätt en default e-post
                    }
                }

                // Ladda produktnamnen för varje produkt i ordern
                foreach (var item in _order.Items)
                {
                    if (!_productNames.ContainsKey(item.ProductId))
                    {
                        var productName = await GetProductNameAsync(item.ProductId);
                        _productNames[item.ProductId] = productName;
                    }
                }

            }
            
        }
        private async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }

        private decimal CalculateOrderTotal(OrderDto order)
        {
            return order.Items.Sum(item => item.Price * item.Quantity);
        }

        private async Task<string> GetProductNameAsync(string productId)
        {
            var product = await ProductService.GetProductByIdAsync(productId);

            return product != null ? $"{product.Name}" : "Okänt namn";
          
        }
    }
}
