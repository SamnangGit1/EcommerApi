using Eletronic_Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Eletronic_Api.Data
{
    public class APIContext:DbContext
    {
       
            public APIContext(DbContextOptions<APIContext> options) : base(options) { }

            public DbSet<Customers> Customers { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Brand> Brands { get; set; }

    }
}
