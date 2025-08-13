using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace Frontend.Pages.Shipment
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ShipmentViewModel Shipment { get; set; } = new() { Date = DateTime.Today };

        public List<SelectListItem> ClientOptions { get; set; }
        public List<SelectListItem> ResourceOptions { get; set; }
        public List<SelectListItem> UeOptions { get; set; }

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            await LoadOptions(httpClient);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");

            var payload = new
            {
                Shipment.Number,
                Shipment.Date,
                Shipment.ClientId,
                Shipment.Resources
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            await httpClient.PostAsync("api/DocumentShipping", jsonContent);

            return RedirectToPage("./Index");
        }

        private async Task LoadOptions(HttpClient httpClient)
        {
            var clientResponse = await httpClient.GetAsync("api/client");
            if (clientResponse.IsSuccessStatusCode)
            {
                var clients = await clientResponse.Content.ReadFromJsonAsync<List<ClientViewModel>>();
                ClientOptions = clients.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            }

            var resourceResponse = await httpClient.GetAsync("api/resource?isActive=true");
            if (resourceResponse.IsSuccessStatusCode)
            {
                var resources = await resourceResponse.Content.ReadFromJsonAsync<List<ResourceViewModel>>();
                ResourceOptions = resources.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList();
            }

            var ueResponse = await httpClient.GetAsync("api/ue?isActive=true");
            if (ueResponse.IsSuccessStatusCode)
            {
                var ues = await ueResponse.Content.ReadFromJsonAsync<List<UeViewModel>>();
                UeOptions = ues.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
            }
        }
    }
}