var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();



/// REST Course
/// (RE)presentational (S)tate (T)ransfer
/// Used to build communication system for distristributed services
///
/// 6 contraints
/// - Uniform Interface: Clear defined interface between client and server
/// - - Identification of resources
/// - - Manipulation of resources through respresentations
/// - - Self-descriptive messages
/// - - Hypermedia as the engine of application state <--Heavily debated
///
/// - Stateless: The server in a single call should have everything it needs to process the call and not require a previous call
/// - Cacheable: Should explicitly state to the client if and how long the server will cache, its up to the client to bypass the cache
/// - Client-Server: Client and Server can change independly so long as the contract is the same
/// - Layered System: The client should not know where the request goes and how it processes
/// - Cod on demand (Optional): The server can send literal code to the client to run
///
///
/// Resource naming and routing: Naming should be plural
/// - /movies -- To get all movies
/// - /movies/1 -- To get one movie
/// - /movies/1/ratings -- To get the ratings for the movie
/// - /ratings/me -- Would be to get the resource for you
