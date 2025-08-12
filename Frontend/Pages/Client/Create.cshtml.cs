using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Frontend.Pages.Client
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ClientViewModel Client { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var jsonContent = new StringContent(JsonSerializer.Serialize(Client), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/client", jsonContent);
            return RedirectToPage("./Index");
        }
    }
}