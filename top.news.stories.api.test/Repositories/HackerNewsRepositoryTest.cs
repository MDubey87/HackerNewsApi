using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using top.news.stories.repositories.HackerNewsRepository;
using top.news.stories.repositories.HackerNewsRepository.Models;
using Xunit;

namespace top.news.stories.api.test.Repositories
{
    public class HackerNewsRepositoryTest
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly IHackerNewsRepository _hackerNewsRepository;
        public HackerNewsRepositoryTest()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _hackerNewsRepository = new HackerNewsRepository(_httpClientFactory.Object);
        }

        [Fact]
        public async Task GetHackerNewsById_Success()
        {
            var responseContent = JsonConvert.SerializeObject(new HackerNewsResponse { Id = 1, Title = "Test", Url = "http://test.com" });
            var mockClient = GetMockHttpClient(HttpStatusCode.OK, responseContent);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockClient);
            var response = await _hackerNewsRepository.GetHackerNewsById(1);
            Assert.NotNull(response);
            Assert.IsType<HackerNewsResponse>(response);
        }

        [Fact]
        public async Task GetHackerNewsById_ThrowException_WhenFailed()
        {            
            var mockClient = GetMockHttpClient(HttpStatusCode.NotFound, string.Empty);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockClient);
            await Assert.ThrowsAsync<HttpRequestException>(() =>  _hackerNewsRepository.GetHackerNewsById(1));            
        }

        [Fact]
        public async Task GetTopHackerNewsIds_Success()
        {
            int[] content = { 12345, 56789 };
            var responseContent = JsonConvert.SerializeObject(content);
            var mockClient = GetMockHttpClient(HttpStatusCode.OK, responseContent);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockClient);
            var response = await _hackerNewsRepository.GetTopHackerNewsIds();
            Assert.NotNull(response);
            Assert.True(response.Any());
        }

        [Fact]
        public async Task GetTopHackerNewsIds_ThrowException_WhenFailed()
        {
            var mockClient = GetMockHttpClient(HttpStatusCode.NotFound, string.Empty);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(mockClient);
            await Assert.ThrowsAsync<HttpRequestException>(() => _hackerNewsRepository.GetTopHackerNewsIds());
        }

        private HttpClient GetMockHttpClient(HttpStatusCode statusCode, string content)
        {
            var clientHandlerMock = new Mock<DelegatingHandler>();
            clientHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                }).Verifiable();
            clientHandlerMock.As<IDisposable>().Setup(s => s.Dispose());
            return new HttpClient(clientHandlerMock.Object);
        }
    }
}
