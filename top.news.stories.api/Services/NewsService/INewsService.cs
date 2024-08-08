using top.news.stories.api.Models.Responses;

namespace top.news.stories.api.Services.NewsService
{
    /// <summary>
    /// HackerNews Service Interface
    /// </summary>
    public interface INewsService
    {
        /// <summary>
        /// Retrieves Top News
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<News>> GetTopNews();
    }
}
