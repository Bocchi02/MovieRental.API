using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieRental.API.Models;

namespace MovieRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieRentalDBContext DBcontext;

        public MovieController(MovieRentalDBContext context)
        {
            DBcontext = context;
        }

        [HttpGet]
        public List<Movie> GetMovies()
        {
            return DBcontext.Movies.ToList();
        }

        [HttpGet("id={id}")]
        public ActionResult<Movie> GetMovieById(int id)
        {
            var movie = DBcontext.Movies.FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
                return NotFound();

            return movie;
        }

        [HttpPost]
        public ActionResult<Movie> AddMovie(Movie movie)
        {
            string response = string.Empty;
            DBcontext.Movies.Add(movie);
            DBcontext.SaveChanges();
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.MovieId }, movie);
        }

        [HttpPut("id={id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie movie)
        {
            if (id != movie.MovieId)
                return BadRequest();

            DBcontext.Entry(movie).State = EntityState.Modified;

            try
            {
                await DBcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DBcontext.Movies.Any(m => m.MovieId == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("id={id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = DBcontext.Movies.Find(id);
            if (movie == null)
                return NotFound();

            DBcontext.Movies.Remove(movie);
            await DBcontext.SaveChangesAsync();

            return NoContent();

        }
    }
}
