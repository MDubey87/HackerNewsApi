using Newtonsoft.Json;

namespace hacker.news.api.repositories.Extensions
{
    public static class ExtensionMethods
    {
        public static async Task<T> HttpResponseHandler<T>(this HttpResponseMessage response)
        {
            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseText);
        }
    }
}
