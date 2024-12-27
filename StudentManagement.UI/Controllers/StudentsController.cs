using Microsoft.AspNetCore.Mvc;
using StudentManagement.UI.Models;

//using StudentManagement.API.Models.DTO;
using StudentManagement.UI.Models.DTO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace StudentManagement.UI.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public StudentsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task< IActionResult> Index()
        {
            List<StudentsDTO> response = new List<StudentsDTO>();

            //get Data from Web API and pass it on view
            var Client = httpClientFactory.CreateClient();
            var HttpresponseMessage = await Client.GetAsync("https://localhost:7295/api/Students");
            HttpresponseMessage.EnsureSuccessStatusCode();

            response.AddRange(await HttpresponseMessage.Content.ReadFromJsonAsync<IEnumerable<StudentsDTO>>());
            

            return View(response);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel model)
        {
            var client= httpClientFactory.CreateClient();

            var HttpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri= new Uri("https://localhost:7295/api/Students"),
                Content= new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8,"application/json")
            };

            var httpresponseMessage= await client.SendAsync(HttpRequestMessage);
            httpresponseMessage.EnsureSuccessStatusCode();

            var response = httpresponseMessage.Content.ReadFromJsonAsync<StudentsDTO>();

            if (response is not null)
            {
                return RedirectToAction("Index","Students");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client=  httpClientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<StudentsDTO>($"https://localhost:7295/api/Students/{id.ToString()}");
            
            if(response is not null)
            {
                return View(response);
            }
            return View(null);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(StudentsDTO model)
        {
            var client= httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7295/api/Students/{model.StudentID}"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<StudentsDTO>();

            if(response is not null)
            {
                return RedirectToAction("Edit", "Students");
            }
            return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(StudentsDTO model)
        {
            try
            {

                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7295/api/Students/{model.StudentID.ToString()}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Edit", "Students");
            }
            catch (Exception ex)
            {
                //Console
            }
            return View("Edit");
        }
    }
}
