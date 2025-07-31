using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Eletronic_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly APIContext _context;
        public OrderDetailController(APIContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Get()
        {
                var details = _context.OrderDetails.Include(od => od.Item).Include(od => od.Order).ToList();
                  return Ok(details);
        }

        
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var details = _context.OrderDetails.Include(od => od.Item).Include(od => od.Order).FirstOrDefault(od=>od.OrderDetailID==id);
            if (details == null)
            {
                return NotFound(new { message = "Order detail not found" });
            }
            return Ok(details);
        }

        
        [HttpPost]
        public ActionResult Post([FromBody] OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
             _context.SaveChanges();
            return Ok(new { message = "Order detail added successfully" });
        }

    
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] OrderDetail updatedOrderDetail)
        {
            var existingOrderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderDetailID == id);
            if (existingOrderDetail == null)
            {
                return NotFound(new { message = "Order detail not found" });
            }

            existingOrderDetail.OrderID = updatedOrderDetail.OrderID;
            existingOrderDetail.ItemID = updatedOrderDetail.ItemID;
            existingOrderDetail.Quantity = updatedOrderDetail.Quantity;
            existingOrderDetail.Description = updatedOrderDetail.Description;

            _context.SaveChanges();

            return Ok(new { message = "Order detail updated successfully" });
        }

        // DELETE api/<OrderDetailController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderDetailID == id);
            if (orderDetail == null)
            {
                return NotFound(new { message = "Order detail not found" });
            }
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
            return Ok(new { message = "Order detail deleted successfully" });
        }
    }
}
