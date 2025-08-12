using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Frontend.Pages.Client
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ClientViewModel Client { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var response = await httpClient.GetAsync($"api/client/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Client = JsonSerializer.Deserialize<ClientViewModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Page();
            }
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            await httpClient.DeleteAsync($"api/client/{Client.Id}");
            return RedirectToPage("./Index");
        }
    }
}