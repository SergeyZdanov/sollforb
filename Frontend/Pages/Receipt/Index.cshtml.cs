using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Web;

namespace Frontend.Pages.Receipt
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ReceiptViewModel> Receipts { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

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
            await LoadReceipts(httpClient);
        }

        private async Task LoadReceipts(HttpClient httpClient)
        {
            var builder = new UriBuilder(httpClient.BaseAddress + "api/DocumentReceipt");
            var query = HttpUtility.ParseQueryString(builder.Query);

            if (StartDate.HasValue) query["startDate"] = StartDate.Value.ToString("o");
            if (EndDate.HasValue) query["endDate"] = EndDate.Value.ToString("o");
            if (FilterResourceId.HasValue) query["resourceIds"] = FilterResourceId.Value.ToString();
            if (FilterUnitId.HasValue) query["ue"] = FilterUnitId.Value.ToString();

            builder.Query = query.ToString();

            var response = await httpClient.GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Receipts = JsonSerializer.Deserialize<List<ReceiptViewModel>>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        private async Task LoadFilterOptions(HttpClient httpClient)
        {
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