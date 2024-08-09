using hacker.news.api.Models.Responses;
using hacker.news.api.repositories.HackerNewsRepository;
using hacker.news.api.repositories.HackerNewsRepository.Models;
using Microsoft.Extensions.Caching.Memory;

namespace hacker.news.api.Services.NewsService
{
    /// <summary>
    /// Servcie class to implement INewsServcie
    /// </summary>
    public class StoryService : IStoryService
    {
        private readonly IHackerNewsRepository _hackerNewsRespository;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "NewStories";
        /// <summary>
        /// NewsService Constructor
        /// </summary>
        /// <param name="hackerNewsRepository"></param>
        /// <param name="memoryCache"></param>
        public StoryService(IHackerNewsRepository hackerNewsRepository, IMemoryCache memoryCache)
        {
            _hackerNewsRespository = hackerNewsRepository;
            _memoryCache = memoryCache;
        }

        /// <inheritdoc/>
        public async Task<StoriesResponse> GetTopNewStories()
        {
            var cachedResult = GetCachedData(CacheKey);
            if (cachedResult == null)
            {
                var dataFromSource = await GetStoriesDataFromSource();
                CacheData(CacheKey, dataFromSource);
                cachedResult = dataFromSource;
            }
            return cachedResult;
        }
        private async Task<StoriesResponse> GetStoriesDataFromSource()
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
        /// <summary>
        /// Map Servcie Response 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private StoriesResponse MapTopNewsResponse(HackerNewsResponse[] response)
        {
            var stories = response.Select(r => new Story { Id = r.Id, Title = r.Title, Url = r.Url }).AsEnumerable();
            return new StoriesResponse { Stories = stories };
        }
        /// <summary>
        /// Cache the data
        /// </summary>
        /// <param name="key">Canche Key</param>
        /// <param name="data">Cache Data</param>
        private void CacheData(string key, StoriesResponse data)
        {
            _memoryCache.Set(key, data, TimeSpan.FromMinutes(10));
        }
        /// <summary>
        /// Get the Cache Data
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns></returns>
        private StoriesResponse? GetCachedData(string key)
        {
            if (_memoryCache.TryGetValue(key, out StoriesResponse cachedData))
            {
                return cachedData;
            }
            return null;
        }
    }
}
