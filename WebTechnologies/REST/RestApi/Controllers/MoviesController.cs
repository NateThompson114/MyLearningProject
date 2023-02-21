using Microsoft.AspNetCore.Mvc;
using RestApi.Application.Models;
using RestApi.Application.Repositories;
using RestApi.Contracts.Request;
using RestApi.Contracts.Responses;
using RestApi.Mapping;

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
        var movie = request.MapToMovie();

        await _movieRepository.CreateAsync(movie);

        var movieResponse = movie.MapToMovieResponse();

        return Created($"/api/movies/{movie.Id}", movieResponse);
    }
}
