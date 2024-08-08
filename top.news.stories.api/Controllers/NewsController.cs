using Microsoft.AspNetCore.Mvc;
using top.news.stories.api.Models.Responses;
using top.news.stories.api.Services.NewsService;

namespace top.news.stories.api.Controllers
{
    [ApiController]
    [Route("api")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _hackerNewsRepository;

        public NewsController(INewsService hackerNewsRepository)
        {
            _hackerNewsRepository = hackerNewsRepository;
        }

        [HttpGet]
        [Route("top-news")]
        [ResponseCache(Duration =600)]
        [ProducesResponseType(typeof(IEnumerable<News>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTopNews()
        {
            var result=await _hackerNewsRepository.GetTopNews();
            return Ok(result);
        }
    }
}