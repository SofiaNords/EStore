namespace EStore.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;


        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    }
}
