using Microsoft.AspNetCore.Mvc;
using StudentManagement.UI.Models;
using StudentManagement.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace StudentManagement.UI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DepartmentController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task< IActionResult> Index()
        {
            List<DepartmentDTO> response = new List<DepartmentDTO>();


            //get data from API and pass it to View
            var Client =  httpClientFactory.CreateClient();
            var httpResponse = await Client.GetAsync("https://localhost:7295/api/Department");
            httpResponse.EnsureSuccessStatusCode();

            response.AddRange(await httpResponse.Content.ReadFromJsonAsync<IEnumerable<DepartmentDTO>>());

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add( AddDepartmentViewModel model)
        {
            var Client= httpClientFactory.CreateClient();

            //prepare httprequest to post data to API
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7295/api/Department"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };
            
            //send data request to Api and wait for response
            var httpResponseMessage= await Client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response= httpRequestMessage.Content.ReadFromJsonAsync<DepartmentDTO>();

            if(response is not null)
            {
                return RedirectToAction("Index", "Department");
            }   
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            //Create a client first
            var client= httpClientFactory.CreateClient();
            //tried to fetch data from API
            var response = await client.GetFromJsonAsync<DepartmentDTO>($"https://localhost:7295/api/Department/{Id.ToString()}");
           
            if(response is not null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentDTO model)
        {
            //Create a client
            var client= httpClientFactory.CreateClient();

            //create a request message

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7295/api/Department/{model.DepartmentID}"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            //send request to API to get data
            var httpResponseMessage= await client.SendAsync(httpRequestMessage);
            //checks the status code
            httpResponseMessage.EnsureSuccessStatusCode() ;

            //read data from JSON and store it in DTO's
            var response= await httpResponseMessage.Content.ReadFromJsonAsync<DepartmentDTO>();

            if(response is not null)
            {
                return RedirectToAction("Index", "Department");
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentDTO model)
        {
            try
            {
                //create a client
                var client = httpClientFactory.CreateClient();

                //delete data from API
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7295/api/Department/{model.DepartmentID}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Department");
            }
            catch (Exception ex)
            {
                //console
            }
            return View("Edit");
        }
    }
}
