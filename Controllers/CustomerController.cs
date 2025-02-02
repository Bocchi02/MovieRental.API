using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.API.Models;

namespace MovieRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MovieRentalDBContext DBcontext;

        public CustomerController(MovieRentalDBContext context)
        {
            DBcontext = context;
        }

        [HttpGet]
        public List<Customer> GetCustomers()
        {
            return DBcontext.Customers.ToList();
        }

        [HttpGet("id={id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            var customer = DBcontext.Customers.FirstOrDefault(c => c.CustomerId == id);

            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> AddStudent([FromBody] Customer customer)
        {
            DBcontext.Customers.Add(customer);
            await DBcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateCustomerDetails(int id, [FromBody] Customer customer)
        {
            if (id != customer.CustomerId)
                return BadRequest();

            DBcontext.Entry(customer).State = EntityState.Modified;

            try
            {
                await DBcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DBcontext.Customers.Any(c => c.CustomerId == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = DBcontext.Customers.Find(id);
            if (customer == null)
                return NotFound();

            DBcontext.Customers.Remove(customer);
            await DBcontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
