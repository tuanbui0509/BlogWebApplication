using Blog.Persistence.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    private readonly IConfiguration _configuration;

    // Constructor for design-time factory usage (e.g., migrations)
    public ApplicationDbContextFactory()
    {
    }

    // Constructor for runtime usage with DI
    public ApplicationDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ApplicationDbContext CreateDbContext(string[] args = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Use configuration for connection string if available
        var connectionString = _configuration?.GetConnectionString("DefaultConnection")
                               ?? "Server=127.0.0.1,1433;Database=WebBlogDb;user id=sa;password=Blog@123;TrustServerCertificate=True"; // Add a fallback or handle appropriately

        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
