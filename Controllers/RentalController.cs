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
            _context.RentalHeaders.Add(rentalHeader);
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

            _context.Entry(rentalHeader).State = EntityState.Modified;
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

        // -------------------------
        // CRUD for RentalHeaderDetail
        // -------------------------

        // Get all RentalDetails for a specific RentalHeader
        [HttpGet("{rentalHeaderId}/details")]
        public async Task<ActionResult<IEnumerable<RentalHeaderDetail>>> GetRentalDetails(int rentalHeaderId)
        {
            return await _context.RentalHeaderDetails
                .Where(rd => rd.RentalHeaderId == rentalHeaderId)
                .ToListAsync();
        }

        // Add a RentalDetail to a RentalHeader
        [HttpPost("{rentalHeaderId}/details")]
        public async Task<ActionResult<RentalHeaderDetail>> PostRentalDetail(int rentalHeaderId, RentalHeaderDetail rentalDetail)
        {
            rentalDetail.RentalHeaderId = rentalHeaderId;
            _context.RentalHeaderDetails.Add(rentalDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRentalDetails), new { rentalHeaderId = rentalDetail.RentalHeaderId }, rentalDetail);
        }

        // Delete a RentalDetail
        [HttpDelete("{rentalHeaderId}/details/{rentalDetailId}")]
        public async Task<IActionResult> DeleteRentalDetail(int rentalHeaderId, int rentalDetailId)
        {
            var rentalDetail = await _context.RentalHeaderDetails
                .FirstOrDefaultAsync(rd => rd.RentalHeaderId == rentalHeaderId && rd.RentalHeaderDetailId == rentalDetailId);

            if (rentalDetail == null)
            {
                return NotFound();
            }

            _context.RentalHeaderDetails.Remove(rentalDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
