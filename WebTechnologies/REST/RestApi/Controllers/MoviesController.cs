using Microsoft.AspNetCore.Mvc;
using RestApi.Application.Repositories;
using RestApi.Contracts.Request;
using RestApi.Mapping;

namespace RestApi.Controllers;

[ApiController]
public class MoviesController : Controller
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpPost(ApiEndPoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody]CreateMovieRequest request)
    {
        var movie = request.MapToMovie();

        await _movieRepository.CreateAsync(movie);

        var movieResponse = movie.MapToMovieResponse();

        return Created($"/{ApiEndPoints.Movies.Create}/{movie.Id}", movieResponse);
    }
}
