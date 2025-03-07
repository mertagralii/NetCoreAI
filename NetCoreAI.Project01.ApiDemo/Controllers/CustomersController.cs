using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project01.ApiDemo.Context;
using NetCoreAI.Project01.ApiDemo.Entities;

namespace NetCoreAI.Project01.ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomersController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet] // GET ==> Veri çekme işlemi yapacağımız zaman kullanıyoruz.
        public IActionResult CustomersList()
        {
            var customersList = _context.Customers.ToList();
            return Ok(customersList);
        }

        [HttpPost("CreateCustomers")]  // POST ==> Ekleme işlemi yapacağımız zaman kullanıyoruz. 
        public IActionResult CreateCustomers(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok("Müşteri Ekleme İşlemi Gerçekleştirildi.");
        }

        [HttpDelete("DeleteCustomers")] // Delete ==> Silme işlemi yapacağımız zaman kullanıyoruz.
        public IActionResult DeleteCustomers(int Id)
        {
            var deleteCustomers = _context.Customers.Find(Id);

            _context.Customers.Remove(deleteCustomers);
            _context.SaveChanges();
            return Ok("Müşteri Silme İşlemi Gerçekleştirildi.");
        }

        [HttpGet("GetCustomer")] // ÖNEMLİ ==> Yukarda daha önceden get kullandığımız için onu da bununla ayırabilmek için bu sefer geti böyle kullandık. İsimlendirdik.
        public IActionResult GetCustomer(int Id)
        {
            var getCustomer = _context.Customers.Find(Id);
            return Ok(getCustomer);
        }

        [HttpPut("UpdateCustomers")] // PUT ==> Güncelleme yapacağımız zaman kullanıyoruz.
        public IActionResult UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok("Müşteri Güncelleme İşlemi Gerçekleştirildi.");
        }

        // Dip NOT : 
        // Silme İşlemi Yapacağımız zaman HttpDelete, Güncelleme İşlemi yapacağımız zaman HttpPUT, Veri çekeceğimiz zaman HTTPGet, Veri EKleyeceğimiz zaman HttpPost kullanıyoruz.
        // Api'de daha öncesinde bir yerde HTTPGET kullanmışsak, diğer yerlerde de HTTPGET kullanmamız gerekiyor. Aynı şekilde diğerleri içinde geçerlidir.
        // Bunun sebebi, Api'nin çalışma mantığıdır.
        // Eğer başka bir yerde HTTPGET kullanmışsak, diğer yerlerde de kullanmamız gerekiyorsa o zaman [HttpGet("İsim")] yazmamız gerekiyor.Böylelikle metotları ayırabiliriz.

    }
}
