using Microsoft.AspNetCore.Mvc;
using hacker.news.api.Models.Responses;
using hacker.news.api.Services.NewsService;

namespace hacker.news.api.Controllers
{
    /// <summary>
    /// News Controller 
    /// </summary>
    [ApiController]
    [Route("api")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        /// <summary>
        /// COntroller Constructor
        /// </summary>
        /// <param name="newsService"></param>
        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }


        /// <summary>
        /// Get the list of top news
        /// </summary>
        /// <returns>Returns list of top news</returns>
        [HttpGet]
        [Route("top-news-list")]
        [ResponseCache(Duration = 600)]
        [ProducesResponseType(typeof(IEnumerable<News>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTopNews()
        {
            var result = await _newsService.GetTopNews();
            if(!result.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}