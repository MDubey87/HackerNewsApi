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
    public class StoryControllerTest
    {
        private readonly Mock<IStoryService> _newsServcie;
        private readonly StoryController _controller;
        public StoryControllerTest()
        {
            _newsServcie = new Mock<IStoryService>();
            _controller=  new StoryController(_newsServcie.Object);
        }

        [Fact]
        public async Task GetTopNewsShouldRetunListOfNewsWhenSuccess()
        {
            _newsServcie.Setup(x => x.GetTopNewStories()).Returns(Task.FromResult(MockResponse()));
            var response = await _controller.GetTopNews() as ObjectResult;
            Assert.NotNull(response);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public async Task GetTopNewsShouldThrowExceptionWhenFailed()
        {
            _newsServcie.Setup(x => x.GetTopNewStories()).ThrowsAsync(new Exception());
            await Assert.ThrowsAsync<Exception>(() => _controller.GetTopNews());
            
        }
        [Fact]
        public async Task GetTopNewsShouldReturnNotFoundWhenEmptyResonse()
        {
            _newsServcie.Setup(x => x.GetTopNewStories()).Returns(Task.FromResult(new StoriesResponse { Stories = Enumerable.Empty<Story>() }));
            var response = await _controller.GetTopNews() as NotFoundResult;
            Assert.NotNull(response);
            Assert.Equal(404, response.StatusCode);

        }
        private static StoriesResponse MockResponse()
        {
            return new StoriesResponse
            {
                Stories = new List<Story> { new Story { Id = 1234, Title = "Test News Title", Url = "https://mockurl.com" } }
            };
        }
    }
}
