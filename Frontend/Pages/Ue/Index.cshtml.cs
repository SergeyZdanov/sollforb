using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Frontend.Pages.Ue
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<UeViewModel> Ues { get; set; } = new();

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var response = await httpClient.GetAsync("api/ue?isActive=false");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Ues = JsonSerializer.Deserialize<List<UeViewModel>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }
    }
}
