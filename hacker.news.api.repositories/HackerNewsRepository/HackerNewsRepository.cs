using hacker.news.api.repositories.Extensions;
using hacker.news.api.repositories.HackerNewsRepository.Models;

namespace hacker.news.api.repositories.HackerNewsRepository
{
    public class HackerNewsRepository : IHackerNewsRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string httpBaseUrl="https://hacker-news.firebaseio.com/v0/";
        /// <summary>
        /// HackerNewsRepository Constructor
        /// </summary>
        /// <param name="httpClientFactory"></param>
        public HackerNewsRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        /// <inheritdoc/>
        public async Task<HackerNewsResponse> GetHackerNewsById(int storyId)
        {
            var httpClient = GetHttpClient();
            var response = await httpClient.GetAsync($"item/{storyId}.json");
            response.EnsureSuccessStatusCode();
            return await response.HttpResponseHandler<HackerNewsResponse>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetTopHackerNewsIds()
        {
            var httpClient = GetHttpClient();
            var response= await httpClient.GetAsync("newstories.json");
            response.EnsureSuccessStatusCode();
            return await response.HttpResponseHandler<List<int>>();
        }
        private HttpClient GetHttpClient()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(httpBaseUrl);
            return httpClient;
        }
    }
}
