using Moq;
using top.news.stories.api.Services.NewsService;
using top.news.stories.repositories.HackerNewsRepository;
using top.news.stories.repositories.HackerNewsRepository.Models;
using Xunit;

namespace top.news.stories.api.test.Services
{
    public class NewsServiceTest
    {
        private readonly INewsService _newsService;
        private readonly Mock<IHackerNewsRepository> _hackerNewsRepository;
        public NewsServiceTest()
        {
            _hackerNewsRepository = new Mock<IHackerNewsRepository>();
            _newsService = new NewsService(_hackerNewsRepository.Object);
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
            var response= await _newsService.GetTopNews();
            Assert.NotNull(response);
            Assert.True(response.Any());
        }

        [Fact]
        public async Task GetTopNewsShouldRetrunEmptyListWhenGetTopHackerNewsIdsReturnEmpty()
        {
            _hackerNewsRepository.Setup(x => x.GetTopHackerNewsIds()).Returns(Task.FromResult(Enumerable.Empty<int>()));
            
            var response = await _newsService.GetTopNews();
            Assert.NotNull(response);
            Assert.True(!response.Any());
        }
    }
}
