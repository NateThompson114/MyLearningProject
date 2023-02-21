using RestApi.Application;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
// builder.Services.AddSingleton<IMovieRepository, MovieRepository>(); // This is the normal method, does not encapsulates properly

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
