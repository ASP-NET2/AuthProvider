using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext(options)
    {
    }
}
