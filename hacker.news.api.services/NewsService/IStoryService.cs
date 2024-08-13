using hacker.news.api.services.Models.Responses;

namespace hacker.news.api.services.NewsService
{
    /// <summary>
    /// HackerNews Service Interface
    /// </summary>
    public interface IStoryService
    {
        /// <summary>
        /// Retrieves List of Top News
        /// </summary>
        /// <returns>Return collection of top news</returns>
        public Task<StoriesResponse> GetTopNewStories();
    }
}
