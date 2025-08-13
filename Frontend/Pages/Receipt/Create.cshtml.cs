using Frontend.DTOs;
using Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace Frontend.Pages.Receipt
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public ReceiptViewModel Receipt { get; set; } = new() { Date = DateTime.Today };

        public List<SelectListItem> ResourceOptions { get; set; }
        public List<SelectListItem> UeOptions { get; set; }

        public async Task OnGetAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");

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

        public async Task<IActionResult> OnPostAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("BackendApi");

            var dto = new ReceiptCreateDto
            {
                Number = Receipt.Number,
                Date = Receipt.Date,
                Resources = Receipt.Resources.Select(r => new ReceiptResourceCreateDto
                {
                    ResourceId = r.ResourceId,
                    UnitId = r.UnitId,
                    Quantity = r.Quantity
                }).ToList()
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            await httpClient.PostAsync("api/DocumentReceipt", jsonContent);

            return RedirectToPage("./Index");
        }
    }
}