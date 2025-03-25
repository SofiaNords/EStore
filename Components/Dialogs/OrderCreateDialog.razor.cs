using EStore.Services;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components;

namespace EStore.Components.Dialogs
{
    public partial class OrderCreateDialog
    {
        [Parameter]
        public OrderForCreationDto Order { get; set; }

        [Parameter]
        public EventCallback<OrderForCreationDto> OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private OrderForCreationDto _createdOrder;

        [Parameter]
        public bool IsVisible { get; set; }

        private List<ProductDto> _products = new List<ProductDto>();
        private List<CustomerDto> _customers = new List<CustomerDto>();

        private string _selectedProductId;
        private int _selectedQuantity = 1;
        private decimal _totalAmount = 0;


        [Inject]
        public ProductService ProductService { get; set; }

        [Inject]
        public CustomerService CustomerService { get; set; }

        private string GetModalClass() => IsVisible ? "show d-block" : "fade";

        protected override async Task OnInitializedAsync()
        {
            _createdOrder = new OrderForCreationDto
            {
                Items = new List<OrderItemForCreationDto>(),
                OrderDate = DateTime.Now 
            }; 
            _products = await ProductService.GetProductsAsync();
            _products = _products.Where(p => !p.IsDiscontinued).ToList();
            _customers = await CustomerService.GetCustomersAsync();
        }

        private void AddItemToOrder()
        {
            var product = _products.FirstOrDefault(p => p.Id == _selectedProductId);
            if (product != null)
            {
                var item = new OrderItemForCreationDto
                {
                    ProductId = product.Id,
                    Quantity = _selectedQuantity,
                    Price = product.Price
                };

                _createdOrder.Items.Add(item);
                _totalAmount += item.Quantity * item.Price;  
            }
        }

        private async Task HandleSave()
        {
            await OnSave.InvokeAsync(_createdOrder);
        }

        private async Task HandleCancel()
        {
            await OnCancel.InvokeAsync();
        }

    }

}
