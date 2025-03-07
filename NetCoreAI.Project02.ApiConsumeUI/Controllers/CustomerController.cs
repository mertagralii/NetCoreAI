using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project02.ApiConsumeUI.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace NetCoreAI.Project02.ApiConsumeUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory; // Bunun üzerinden bir istemci oluşturucaz ve bu istemci üzerinden GET, POST, PULL, DELETE gibi işlemler yapacağız.

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CustomersList() // Bu metodda apiye istek atıp gelen veriyi döneceğiz. (Apiler asekron ile çalıştıkları için metodumuzu async'e çevirdik.) 
        {
            var client = _httpClientFactory.CreateClient(); // Client oluşturduk (İstemci oluşturduk).
            var responseMessage = await client.GetAsync("https://localhost:7180/api/Customers"); // İstek attık.(GetAsync metodu ile bir adrese istekde bulucam (Request URL)) await ise bu asekron metodu çağırmam için bir keyword
            if (responseMessage.IsSuccessStatusCode) // Eğer başarılı bir şekilde dönerse (200 kodu dönerse)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // Gelen veriyi okuduk ve jsonData'ya kayıt ettik.
                var values = JsonConvert.DeserializeObject<List<ResultCustomerDto>>(jsonData); // Okunan veriyi Deserialize ederek gelen Json verisini string ifadeye çevirdik sonrasında APİ'den gelen müşterileri ResultCustomerDto ile eşledik.(Mapladik) (İstek attığımız yerin modeli ile bizim ResultCustomerDto modelimizin içindeki proplar aynı olmalı yoksa hata verir)
                return View(values); // View'e döndük.
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateCustomer() 
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto) 
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createCustomerDto); // Gelen String ifadeyi Json'a çevirdik.
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json"); // Json verisini StringContent'e çevirdik.StirngContent(Gönderilecek Json türü,Encoding.UTF8 ==> Türkçe karakter olabilir buna göre yap, gönderdiğim türün formatı)
            var responseMessage = await client.PostAsync("https://localhost:7180/api/Customers/CreateCustomers", content); // PostAsync metodu ile bir adrese istekde bulunduk ve içerik olarak content'i gönderdik.
            if (responseMessage.IsSuccessStatusCode) // Eğer başarılı bir şekilde dönerse (200 kodu dönerse)
            {
                return RedirectToAction("CustomersList"); // CustomersList metoduna yönlendirdik.
            }
            return View();
        }
        
        public async Task<IActionResult> DeleteCustomer(int Id) 
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7180/api/Customers/DeleteCustomers/{Id}"); // DeleteAsync metodu ile bir adrese istekde bulunduk ve içerik olarak content'i gönderdik.
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("CustomersList");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7180/api/Customers/GetCustomer?Id=" + id); // APİ'deki girilen Id'ye göre veri getiren APİ'ye istek attık.
            if (responseMessage.IsSuccessStatusCode) // Eğer başarılı bir şekilde dönerse (200 Kodu Dönerse)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); //Gelen veriyi okuduk ve jsonData değişkenine ekledik.
                var values = JsonConvert.DeserializeObject<GetByIdCustomerDto>(jsonData); // Gelen veri Json formatında olduğunu için bu gelen ifadeyi String hale getirdik.
                return View(values); // Ve gelen veriyi sayfaya yönlendirdik.
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(GetByIdCustomerDto getByIdCustomerDto) 
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(getByIdCustomerDto); // Gelen String ifadeyi Json'a çevirdik.
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json"); // Json verisini StringContent'e çevirdik.StirngContent(Gönderilecek Json türü,Encoding.UTF8 ==> Türkçe karakter olabilir buna göre yap, gönderdiğim türün formatı)
            var responseMessage = await client.PutAsync("https://localhost:7180/api/Customers/UpdateCustomers", content); // PutAsync metodu ile bir adrese istekde bulunduk ve içerik olarak content'i gönderdik.
            if (responseMessage.IsSuccessStatusCode) // Eğer başarılı bir şekilde dönerse (200 kodu dönerse)
            {
                return RedirectToAction("CustomersList"); // CustomersList metoduna yönlendirdik.
            }
            return View();
        }

    }
}
