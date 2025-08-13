using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace Frontend.Pages.Receipt
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ReceiptViewModel Receipt { get; set; }

        public List<SelectListItem> ResourceOptions { get; set; }
        public List<SelectListItem> UeOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");

            var docResponse = await httpClient.GetAsync($"api/DocumentReceipt/{id}");
            if (!docResponse.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            var jsonString = await docResponse.Content.ReadAsStringAsync();
            Receipt = JsonSerializer.Deserialize<ReceiptViewModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await LoadOptions(httpClient);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");

            var payload = new
            {
                Number = Receipt.Number,
                Date = Receipt.Date,
                Resources = Receipt.Resources
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            await httpClient.PutAsync($"api/DocumentReceipt/{Receipt.Id}", jsonContent);

            return RedirectToPage("./Index");
        }

        private async Task LoadOptions(HttpClient httpClient)
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