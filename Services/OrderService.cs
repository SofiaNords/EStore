using EStoreAPI.Models;

namespace EStore.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;


        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<OrderDto>>("api/orders");
        }

        public async Task<OrderDto> CreateOrderAsync(OrderForCreationDto order)
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", order);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderDto>();
            }
            throw new Exception("Något gick fel vid skapandet av ordern.");
        }
    }
}
