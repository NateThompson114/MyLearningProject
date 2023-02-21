using Microsoft.AspNetCore.Mvc;
using RestApi.Application.Models;
using RestApi.Application.Repositories;
using RestApi.Contracts.Request;
using RestApi.Contracts.Responses;

namespace RestApi.Controllers;

[ApiController]
[Route("api")]
public class MoviesController : Controller
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost("movies")]
    public async Task<IActionResult> Create([FromBody]CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };

        await _movieRepository.CreateAsync(movie);

        var movieResponse = new MovieResponse
        {
            Id = movie.Id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres
        };

        return Created($"/api/movies/{movie.Id}", movieResponse);
    }
}
