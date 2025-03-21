using EStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<List<CustomerDto>> GetCustomersAsync([FromQuery] string? searchQuery)
        {
            var url = "api/customers?";
            if (!string.IsNullOrEmpty(searchQuery))
                url += $"searchQuery={searchQuery}&";
            url = url.TrimEnd('&');

            return await _httpClient.GetFromJsonAsync<List<CustomerDto>>(url);
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerForCreationDto customer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customers", customer);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CustomerDto>();
            }
            throw new Exception("Något gick fel vid skapandet av kunden.");
        }

        public async Task UpdateCustomerAsync(string customerId, CustomerForUpdateDto updatedCustomer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/customers/{customerId}", updatedCustomer);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Något gick fel vid uppdatering av kunden.");
            }
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(string customerId)
        {
            return await _httpClient.GetFromJsonAsync<CustomerDto>($"api/customers/{customerId}");
        }
    }
}

