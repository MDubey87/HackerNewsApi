using top.news.stories.api.Models.Responses;
using top.news.stories.repositories.HackerNewsRepository;
using top.news.stories.repositories.HackerNewsRepository.Models;

namespace top.news.stories.api.Services.NewsService
{
    /// <summary>
    /// Servcie class to implement INewsServcie
    /// </summary>
    public class NewsService : INewsService
    {
        private readonly IHackerNewsRepository _hackerNewsRespository;
        /// <summary>
        /// NewsService Constructor
        /// </summary>
        /// <param name="hackerNewsRepository"></param>
        public NewsService(IHackerNewsRepository hackerNewsRepository)
        {
            _hackerNewsRespository = hackerNewsRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<News>> GetTopNews()
        {
            var newsIds = await _hackerNewsRespository.GetTopHackerNewsIds();
            if (newsIds == null || !newsIds.Any())
            {
                return Enumerable.Empty<News>();
            }
            var newsTaskList = new List<Task<HackerNewsResponse>>();
            newsTaskList.AddRange(newsIds.AsEnumerable().Select(newsId => _hackerNewsRespository.GetHackerNewsById(newsId)));
            var newsResponseList = await Task.WhenAll(newsTaskList);            
            return MapTopNewsResponse(newsResponseList);
        }
        private IEnumerable<News> MapTopNewsResponse(HackerNewsResponse[] response)
        {
            return response.Select(r => new News { Id = r.Id, Title = r.Title, Url = r.Url }).AsEnumerable();
        }
    }
}
