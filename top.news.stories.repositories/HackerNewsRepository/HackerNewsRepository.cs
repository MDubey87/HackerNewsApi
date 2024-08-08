using top.news.stories.repositories.Extensions;
using top.news.stories.repositories.HackerNewsRepository.Models;

namespace top.news.stories.repositories.HackerNewsRepository
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
        public async Task<List<int>> GetTopHackerNews()
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
