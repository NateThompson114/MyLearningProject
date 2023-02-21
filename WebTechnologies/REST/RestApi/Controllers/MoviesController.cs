using Microsoft.AspNetCore.Mvc;
using RestApi.Application.Repositories;
using RestApi.Contracts.Request;

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
        return Ok(request);
    }
}
