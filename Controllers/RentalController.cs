using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.API.Models;

namespace MovieRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly MovieRentalDBContext _context;

        public RentalController(MovieRentalDBContext context)
        {
            _context = context;
        }

        // Get all RentalHeaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalHeader>>> GetRentalHeaders()
        {
            return await _context.RentalHeaders.Include(r => r.RentalHeaderDetails).ToListAsync();
        }

        // Get RentalHeader by ID with its RentalDetails
        [HttpGet("{id}")]
        public async Task<ActionResult<RentalHeader>> GetRentalHeader(int id)
        {
            var rentalHeader = await _context.RentalHeaders
                .Include(r => r.RentalHeaderDetails)
                .FirstOrDefaultAsync(r => r.RentalHeaderId == id);

            if (rentalHeader == null)
            {
                return NotFound();
            }

            return rentalHeader;
        }

        // Add a RentalHeader
        [HttpPost]
        public async Task<ActionResult<RentalHeader>> PostRentalHeader(RentalHeader rentalHeader)
        {
            // Ensure RentalDetails is initialized to prevent null errors
            if (rentalHeader.RentalHeaderDetails == null || rentalHeader.RentalHeaderDetails.Count == 0)
            {
                return BadRequest("RentalHeader must have at least one RentalHeaderDetail.");
            }

            // Add the RentalHeader to the context
            _context.RentalHeaders.Add(rentalHeader);

            // Add the RentalHeaderDetails to the context
            foreach (var detail in rentalHeader.RentalHeaderDetails)
            {
                // Set the RentalHeaderId to the corresponding RentalHeader for each detail
                detail.RentalHeaderId = rentalHeader.RentalHeaderId;  // This ensures the relationship is set
                _context.RentalHeaderDetails.Add(detail);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRentalHeader), new { id = rentalHeader.RentalHeaderId }, rentalHeader);
        }


        // Update a RentalHeader
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentalHeader(int id, RentalHeader rentalHeader)
        {
            if (id != rentalHeader.RentalHeaderId)
            {
                return BadRequest();
            }

            var existingRental = await _context.RentalHeaders
                .Include(r => r.RentalHeaderDetails) // Ensure related data is included
                .FirstOrDefaultAsync(r => r.RentalHeaderId == id);

            if (existingRental == null)
            {
                return NotFound();
            }

            // Update only necessary properties
            _context.Entry(existingRental).CurrentValues.SetValues(rentalHeader);

            // Optional: If updating related data
            if (rentalHeader.RentalHeaderDetails != null)
            {
                _context.RentalHeaderDetails.RemoveRange(existingRental.RentalHeaderDetails); // Remove old details
                _context.RentalHeaderDetails.AddRange(rentalHeader.RentalHeaderDetails); // Add new details
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // Delete a RentalHeader and its RentalDetails
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentalHeader(int id)
        {
            var rentalHeader = await _context.RentalHeaders
                .Include(r => r.RentalHeaderDetails)
                .FirstOrDefaultAsync(r => r.RentalHeaderId == id);

            if (rentalHeader == null)
            {
                return NotFound();
            }

            _context.RentalHeaders.Remove(rentalHeader);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
