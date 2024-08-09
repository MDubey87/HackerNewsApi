using hacker.news.api.Models.Responses;

namespace hacker.news.api.Services.NewsService
{
    /// <summary>
    /// HackerNews Service Interface
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Retrieves List of Top News
        /// </summary>
        /// <returns>Return collection of top news</returns>
        public Task<IEnumerable<News>> GetTopNews();
    }
}
