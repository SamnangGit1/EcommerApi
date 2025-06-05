using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly APIContext _dbcontext;
        public CustomerController(APIContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var customer=_dbcontext.Customers.ToList();
            return Ok(customer);
        }
     
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var customer = _dbcontext.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

       
        [HttpPost]
        public IActionResult Post([FromBody] Customers customers)
        {
           _dbcontext.Customers.Add(customers);
            _dbcontext.SaveChanges();
            return Ok(new { massage = "Customer Added Successfully" });
        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = _dbcontext.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customers customer)
        {
            var existingCustomer = _dbcontext.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (existingCustomer == null)
            {
                return NotFound();
            }
            existingCustomer.CustomerName = customer.CustomerName;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Email = customer.Email;
            existingCustomer.Address = customer.Address;
            existingCustomer.AddressType = customer.AddressType;
            existingCustomer.HouseNo = customer.HouseNo;
            _dbcontext.Customers.Update(existingCustomer);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Customer updated successfully" });
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customer = _dbcontext.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }
            _dbcontext.Customers.Remove(customer);
            _dbcontext.SaveChanges();
            return Ok(new { message = "Customer deleted successfully" });
        }
    }
}
