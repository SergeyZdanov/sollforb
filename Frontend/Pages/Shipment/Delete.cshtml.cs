using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Frontend.Pages.Shipment
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ShipmentViewModel Shipment { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            var response = await httpClient.GetAsync($"api/DocumentShipping/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Shipment = JsonSerializer.Deserialize<ShipmentViewModel>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            await httpClient.DeleteAsync($"api/DocumentShipping/{Shipment.Id}");

            return RedirectToPage("./Index");
        }
    }
}