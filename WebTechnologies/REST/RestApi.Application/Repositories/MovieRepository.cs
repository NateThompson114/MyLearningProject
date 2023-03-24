using RestApi.Application.Database;
using RestApi.Application.Models;

namespace RestApi.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        throw new NotImplementedException();
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
