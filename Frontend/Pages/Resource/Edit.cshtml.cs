using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Frontend.Pages.Resource
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ResourceViewModel Resource { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var response = await httpClient.GetAsync($"api/resource/{id}");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Resource = JsonSerializer.Deserialize<ResourceViewModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Page();
            }
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var jsonContent = new StringContent(JsonSerializer.Serialize(Resource), Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"api/resource?id={Resource.Id}", jsonContent);
            return RedirectToPage("./Index");
        }
    }
}