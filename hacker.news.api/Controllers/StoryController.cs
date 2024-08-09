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
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _newsService;

        /// <summary>
        /// COntroller Constructor
        /// </summary>
        /// <param name="newsService"></param>
        public StoryController(IStoryService newsService)
        {
            _newsService = newsService;
        }


        /// <summary>
        /// Get the list of top news
        /// </summary>
        /// <returns>Returns list of top new stories</returns>
        [HttpGet]
        [Route("top-new-stories")]
        [ProducesResponseType(typeof(StoriesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTopNews()
        {
            var result = await _newsService.GetTopNewStories();
            if(!result.Stories.Any())
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}