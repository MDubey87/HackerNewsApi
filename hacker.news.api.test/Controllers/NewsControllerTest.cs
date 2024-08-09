using Microsoft.AspNetCore.Mvc;
using Moq;
using hacker.news.api.Controllers;
using hacker.news.api.Models.Responses;
using hacker.news.api.Services.NewsService;
using Xunit;

namespace hacker.news.api.test.Controllers
{
    /// <summary>
    /// Test class for NewsController
    /// </summary>
    public class NewsControllerTest
    {
        private readonly Mock<INewsService> _newsServcie;
        private readonly NewsController _controller;
        public NewsControllerTest()
        {
            _newsServcie = new Mock<INewsService>();
            _controller=  new NewsController(_newsServcie.Object);
        }

        [Fact]
        public async Task GetTopNewsShouldRetunListOfNewsWhenSuccess()
        {
            _newsServcie.Setup(x => x.GetTopNews()).Returns(Task.FromResult(MockResponse()));
            var response = await _controller.GetTopNews() as ObjectResult;
            Assert.NotNull(response);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetTopNewsShouldThrowExceptionWhenFailed()
        {
            _newsServcie.Setup(x => x.GetTopNews()).ThrowsAsync(new Exception());
            await Assert.ThrowsAsync<Exception>(() => _controller.GetTopNews());
            
        }
        [Fact]
        public async Task GetTopNewsShouldReturnNotFoundWhenEmptyResonse()
        {
            _newsServcie.Setup(x => x.GetTopNews()).Returns(Task.FromResult(Enumerable.Empty<News>()));
            var response = await _controller.GetTopNews() as NotFoundResult;
            Assert.NotNull(response);
            Assert.Equal(404, response.StatusCode);

        }
        private static IEnumerable<News> MockResponse()
        {
            return new List<News> { new News { Id = 1234, Title = "Test News Title", Url = "https://mockurl.com" } };
        }
    }
}
