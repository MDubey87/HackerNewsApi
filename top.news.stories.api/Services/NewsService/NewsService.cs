using top.news.stories.api.Models.Responses;
using top.news.stories.repositories.HackerNewsRepository;
using top.news.stories.repositories.HackerNewsRepository.Models;

namespace top.news.stories.api.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly IHackerNewsRepository _hackerNewsRespository;
        public NewsService(IHackerNewsRepository hackerNewsRepository)
        {
            _hackerNewsRespository = hackerNewsRepository;
        }
        public async Task<IEnumerable<News>> GetTopNews()
        {
            var stories = await _hackerNewsRespository.GetTopHackerNews();
            var newsTaskList = new List<Task<HackerNewsResponse>>();
            if(stories != null && stories.Count > 0)
            {
                newsTaskList.AddRange(stories.AsEnumerable().Select(newsId=>_hackerNewsRespository.GetHackerNewsById(newsId)));
            }
            var newsResponseList = await Task.WhenAll(newsTaskList);
            return MapTopNewsResponse(newsResponseList);
        }
        private IEnumerable<News> MapTopNewsResponse(HackerNewsResponse[] response)
        {
            return response.Select(r => new News { Id = r.Id, Title = r.Title, Url = r.Url }).AsEnumerable();
        }
    }
}
