using Microsoft.Extensions.DependencyInjection;
using RestApi.Application.Repositories;

namespace RestApi.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MovieRepository>();
        return services;
    }
}
