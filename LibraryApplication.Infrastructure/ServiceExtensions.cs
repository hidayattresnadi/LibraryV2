using LibraryApplication.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleLibraryV2.Context;
using SimpleLibraryV2.NewFolder;

namespace LibraryApplication.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyDbContext>(options => options.UseNpgsql(connection));
            services.AddScoped<IBookRepository,BookRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IBorrowingRepository, BorrowingRepository>();
        }
    }
}