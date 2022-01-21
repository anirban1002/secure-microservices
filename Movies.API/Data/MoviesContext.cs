using Microsoft.EntityFrameworkCore;
using Movies.API.Models;

namespace Movies.API.Data
{
    public class MoviesContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }

        public MoviesContext(DbContextOptions<MoviesContext> options) : base(options)
        {
        }
    }
}
