using Frontend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Frontend.Pages.Client
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ClientViewModel> Clients { get; set; } = new();

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var response = await httpClient.GetAsync("api/client");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Clients = JsonSerializer.Deserialize<List<ClientViewModel>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}