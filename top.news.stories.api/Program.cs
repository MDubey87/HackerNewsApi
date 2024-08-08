using top.news.stories.api.Middlewares;
using top.news.stories.api.Services.NewsService;
using top.news.stories.repositories.HackerNewsRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddResponseCaching();
builder.Services.AddScoped<IHackerNewsRepository, HackerNewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddelware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
