using Moq;
using hacker.news.api.Services.NewsService;
using hacker.news.api.repositories.HackerNewsRepository;
using hacker.news.api.repositories.HackerNewsRepository.Models;
using Xunit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace hacker.news.api.test.Services
{
    public class StoryServiceTest
    {
        private readonly IStoryService _newsService;
        private readonly Mock<IHackerNewsRepository> _hackerNewsRepository;
        public StoryServiceTest()
        {
            _hackerNewsRepository = new Mock<IHackerNewsRepository>();
            
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var _memoryCache = serviceProvider.GetService<IMemoryCache>();

            _newsService = new StoryService(_hackerNewsRepository.Object, _memoryCache);
        }
        [Fact]
        public async Task GetTopNewsShouldRetrunListofNewsWhenSuccess()
        {
            var newsIds = new List<int> { 41196696 };
            _hackerNewsRepository.Setup(x => x.GetTopHackerNewsIds()).Returns(Task.FromResult(newsIds.AsEnumerable()));
            var mockResponse = new HackerNewsResponse
            {
                Id = 41196696,
                Title = "Test",
                Url = "http://test.com"
            };
            _hackerNewsRepository.Setup(x => x.GetHackerNewsById(It.IsAny<int>())).Returns(Task.FromResult(mockResponse));
            var response = await _newsService.GetTopNewStories();
            Assert.NotNull(response);
            Assert.True(response.Stories.Any());
        }

        [Fact]
        public async Task GetTopNewsShouldRetrunEmptyListWhenGetTopHackerNewsIdsReturnEmpty()
        {
            _hackerNewsRepository.Setup(x => x.GetTopHackerNewsIds()).Returns(Task.FromResult(Enumerable.Empty<int>()));

            var response = await _newsService.GetTopNewStories();
            Assert.NotNull(response);
            Assert.True(!response.Stories.Any());
        }
    }
}
