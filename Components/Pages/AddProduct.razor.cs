using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EStoreAPI.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EStore.Components.Pages
{
    public partial class AddProduct
    {
        private ProductForCreationDto productForCreationDto = new ProductForCreationDto();
        private string? message;

        [Inject]
        public HttpClient Http { get; set; }

        private async Task HandleValidSubmit()
        {

            try
            {
                var response = await Http.PostAsJsonAsync("/api/products", productForCreationDto);

                if (response.IsSuccessStatusCode)
                {
                    message = "Product added successfully!";
                    productForCreationDto = new ProductForCreationDto(); // Reset form
                }
                else
                {
                    message = "Failed to add product. Please try again.";
                }
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }
        }
    }
}
