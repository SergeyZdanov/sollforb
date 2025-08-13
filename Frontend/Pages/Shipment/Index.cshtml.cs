using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Web;

namespace Frontend.Pages.Shipment
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ShipmentViewModel> Shipments { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterClientId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterResourceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterUnitId { get; set; }

        public List<SelectListItem> ClientOptions { get; set; }
        public List<SelectListItem> ResourceOptions { get; set; }
        public List<SelectListItem> UeOptions { get; set; }

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            await LoadFilterOptions(httpClient);
            await LoadShipments(httpClient);
        }

        public async Task<IActionResult> OnPostSignAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            await httpClient.PostAsync($"api/DocumentShipping/{id}/sign", null);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRevertSignAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");
            await httpClient.PostAsync($"api/DocumentShipping/{id}/revert-sign", null);
            return RedirectToPage();
        }

        private async Task LoadShipments(HttpClient httpClient)
        {
            var builder = new UriBuilder(httpClient.BaseAddress + "api/DocumentShipping");
            var query = HttpUtility.ParseQueryString(builder.Query);

            if (StartDate.HasValue) query["startDate"] = StartDate.Value.ToString("o");
            if (EndDate.HasValue) query["endDate"] = EndDate.Value.ToString("o");
            if (FilterClientId.HasValue) query["clientIds"] = FilterClientId.Value.ToString();
            if (FilterResourceId.HasValue) query["resourceIds"] = FilterResourceId.Value.ToString();
            if (FilterUnitId.HasValue) query["ueIds"] = FilterUnitId.Value.ToString();

            builder.Query = query.ToString();

            var response = await httpClient.GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Shipments = JsonSerializer.Deserialize<List<ShipmentViewModel>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        private async Task LoadFilterOptions(HttpClient httpClient)
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