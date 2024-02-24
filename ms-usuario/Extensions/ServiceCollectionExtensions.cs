using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;
using ms_usuario.Interface;
using ms_usuario.Repositories;

namespace ms_usuario.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Conta>), typeof(Repository<Conta>));
        }

        public static void SetupDbContext(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ContaDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ContaDbContext).Assembly.FullName)),
                ServiceLifetime.Transient, ServiceLifetime.Transient
                );
        }
    }
}
