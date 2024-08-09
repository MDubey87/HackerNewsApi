using hacker.news.api.Models.Responses;
using hacker.news.api.repositories.HackerNewsRepository;
using hacker.news.api.repositories.HackerNewsRepository.Models;

namespace hacker.news.api.Services.NewsService
{
    /// <summary>
    /// Servcie class to implement INewsServcie
    /// </summary>
    public class StoryService : IStoryService
    {
        private readonly IHackerNewsRepository _hackerNewsRespository;
        /// <summary>
        /// NewsService Constructor
        /// </summary>
        /// <param name="hackerNewsRepository"></param>
        public StoryService(IHackerNewsRepository hackerNewsRepository)
        {
            _hackerNewsRespository = hackerNewsRepository;
        }

        /// <inheritdoc/>
        public async Task<StoriesResponse> GetTopNewStories()
        {
            var newsIds = await _hackerNewsRespository.GetTopHackerNewsIds();
            if (newsIds == null || !newsIds.Any())
            {
                return new StoriesResponse { Stories = Enumerable.Empty<Story>() };
            }
            var newsTaskList = new List<Task<HackerNewsResponse>>();
            newsTaskList.AddRange(newsIds.AsEnumerable().Select(newsId => _hackerNewsRespository.GetHackerNewsById(newsId)));
            var newsResponseList = await Task.WhenAll(newsTaskList);            
            return MapTopNewsResponse(newsResponseList);
        }
        private StoriesResponse MapTopNewsResponse(HackerNewsResponse[] response)
        {
            var stories= response.Select(r => new Story { Id = r.Id, Title = r.Title, Url = r.Url }).AsEnumerable();
            return new StoriesResponse { Stories = stories };
        }
    }
}
