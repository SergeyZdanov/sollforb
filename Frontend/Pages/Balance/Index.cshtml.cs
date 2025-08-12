using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace Frontend.Pages.Balance
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<BalanceViewModel> Balances { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? FilterResourceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FilterUnitId { get; set; }

        public List<SelectListItem> ResourceOptions { get; set; }
        public List<SelectListItem> UeOptions { get; set; }


        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");

            await LoadFilterOptions(httpClient);
            await LoadBalances(httpClient);
        }

        private async Task LoadBalances(HttpClient httpClient)
        {
            var query = new List<string>();
            if (FilterResourceId.HasValue)
            {
                query.Add($"resourceIds={FilterResourceId.Value}");
            }
            if (FilterUnitId.HasValue)
            {
                query.Add($"ueIds={FilterUnitId.Value}");
            }

            var queryString = query.Any() ? "?" + string.Join("&", query) : "";

            var response = await httpClient.GetAsync($"api/balance{queryString}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Balances = JsonSerializer.Deserialize<List<BalanceViewModel>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        private async Task LoadFilterOptions(HttpClient httpClient)
        {
            var resourceResponse = await httpClient.GetAsync("api/resource?isActive=true");
            if (resourceResponse.IsSuccessStatusCode)
            {
                var jsonString = await resourceResponse.Content.ReadAsStringAsync();
                var resources = JsonSerializer.Deserialize<List<ResourceViewModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ResourceOptions = resources.Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name }).ToList();
            }

            var ueResponse = await httpClient.GetAsync("api/ue?isActive=true");
            if (ueResponse.IsSuccessStatusCode)
            {
                var jsonString = await ueResponse.Content.ReadAsStringAsync();
                var ues = JsonSerializer.Deserialize<List<UeViewModel>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                UeOptions = ues.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).ToList();
            }
        }
    }
}