using Microsoft.AspNetCore.Mvc;
using RestApi.Application.Models;
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
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
    }

    [HttpGet(ApiEndPoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie is null)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndPoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();

        var moviesResponse = movies.MapToMoviesResponse();

        return Ok(moviesResponse);
    }

    [HttpPut(ApiEndPoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updated = await _movieRepository.UpdateAsync(movie);
        if (!updated)
        {
            return NotFound();
        }

        var response = movie.MapToMovieResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndPoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _movieRepository.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
