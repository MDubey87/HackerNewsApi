using top.news.stories.repositories.HackerNewsRepository.Models;

namespace top.news.stories.repositories.HackerNewsRepository
{
    /// <summary>
    /// Repository Interface
    /// </summary>
    public interface IHackerNewsRepository
    {
        /// <summary>
        /// Retrieves news details by news id
        /// </summary>
        /// <param name="storyId">Id of story</param>
        /// <returns>Return HackerNewsResponse</returns>
        public Task<HackerNewsResponse> GetHackerNewsById(int storyId);
        /// <summary>
        /// Retrieves top news
        /// </summary>
        /// <returns>Return List of top news ids</returns>
        public Task<List<int>> GetTopHackerNews();
    }
}
