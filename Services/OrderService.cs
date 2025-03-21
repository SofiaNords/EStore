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
    }
}
