using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly APIContext _context;
        public OrderController(APIContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult index()
        {
            var orders = _context.Orders.Include(o => o.AppUser).Include(o => o.Staff)

                .ToList();
            return Ok(orders);
        }

            // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var orders = _context.Orders.Include(o => o.AppUser).Include(o => o.Staff)

                 .FirstOrDefault(o => o.OrderID == id);
            if (orders == null)
            {
                return NotFound(new { message = "Order not found" });
            }
                return Ok(orders);  
        }

        // POST api/<OrderController>
        [HttpPost]
        public ActionResult Post([FromBody] Order order)
        {
            var random = new Random();
            order.InvoiceNo = $"#{random.Next(10000, 999999)}";
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Ok(new {message ="Order Add Successfull"});
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (id != updatedOrder.OrderID)
                return BadRequest(new { message = "Order ID mismatch" });

            var existingOrder = _context.Orders.FirstOrDefault(o => o.OrderID == id);
            if (existingOrder == null)
                return NotFound(new { message = "Order not found" });

            existingOrder.Note = updatedOrder.Note;
            existingOrder.StuffID = updatedOrder.StuffID;
            existingOrder.AppUserID = updatedOrder.AppUserID;
            existingOrder.OrderDate = updatedOrder.OrderDate;

            _context.Orders.Update(existingOrder);
            _context.SaveChanges();

            return Ok(new { message = "Order updated successfully" });
        }

       
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok(new { message = "Order deleted successfully" });
        }
    }
}
