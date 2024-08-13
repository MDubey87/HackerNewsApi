using hacker.news.api.Middlewares;
using hacker.news.api.services.NewsService;
using hacker.news.api.repositories.HackerNewsRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("*")
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IHackerNewsRepository, HackerNewsRepository>();
builder.Services.AddScoped<IStoryService, StoryService>();
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddelware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
