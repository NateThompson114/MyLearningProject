using Microsoft.Extensions.DependencyInjection;
using RestApi.Application.Database;
using RestApi.Application.Repositories;

namespace RestApi.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqlDbConnectionFactory(connectionString));
        return services;
    }
}
