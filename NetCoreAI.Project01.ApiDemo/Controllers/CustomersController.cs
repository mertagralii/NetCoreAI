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

        [HttpGet("CustomerList")]
        public IActionResult CustomersList() 
        {
           var customersList = _context.Customers.ToList();
            return Ok(customersList);
        }

        [HttpPost("CreateCustomers")]  
        public IActionResult CreateCustomers(Customer customer) 
        {
             _context.Customers.Add(customer);
            _context.SaveChanges();
            return Ok("Müşteri Ekleme İşlemi Gerçekleştirildi.");
        }

        [HttpDelete("DeleteCustomers")]
        public IActionResult DeleteCustomers(int Id) 
        {
            var deleteCustomers = _context.Customers.Find(Id);

            _context.Customers.Remove(deleteCustomers);
            _context.SaveChanges();
            return Ok("Müşteri Silme İşlemi Gerçekleştirildi.");
        }

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer(int Id) 
        {
            var getCustomer = _context.Customers.Find(Id);
            return Ok(getCustomer);
        }

        [HttpPut("UpdateCustomers")]
        public IActionResult UpdateCustomer(Customer customer) 
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return Ok("Müşteri Güncelleme İşlemi Gerçekleştirildi.");
        }



    }
}
