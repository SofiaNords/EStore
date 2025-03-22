using EStoreAPI.Models;
using ZstdSharp.Unsafe;

namespace EStore.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/products");
        }

        public async Task<List<ProductDto>> GetProductsAsync(string? name = null, string? searchQuery = null, string? productNumber = null)
        {
            var url = "api/products?";
            if (!string.IsNullOrEmpty(name))
                url += $"name={name}&";
            if (!string.IsNullOrEmpty(searchQuery))
                url += $"searchQuery={searchQuery}&";
            if (!string.IsNullOrEmpty(productNumber))
                url += $"productNumber={productNumber}&";
            url = url.TrimEnd('&');

            return await _httpClient.GetFromJsonAsync<List<ProductDto>>(url);
        }


        public async Task<ProductDto> CreateProductAsync(ProductForCreationDto product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            throw new Exception("Något gick fel vid skapandet av produkten.");
        }

        public async Task DeleteProductAsync(string productId)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Något gick fel vid borttagning av produkten.");
            }
        }

        public async Task UpdateProductAsync(string productId, ProductForUpdateDto updatedProduct)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{productId}", updatedProduct);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Något gick fel vid uppdatering av produkten.");
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(string productId)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"api/products/{productId}");
        }
    }
}
