using EStoreAPI.Models;

namespace EStore.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CustomerDto>> GetCustomersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("api/customers");
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerForCreationDto customer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customers", customer);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerDto>();
            }
            throw new Exception("Något gick fel vid skapandet av produkten.");
        }
    }
}
