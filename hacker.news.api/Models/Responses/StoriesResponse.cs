namespace hacker.news.api.Models.Responses
{
    /// <summary>
    /// Response Object
    /// </summary>
    public class StoriesResponse
    {
        /// <summary>
        /// Colletion of Stories
        /// </summary>
        public IEnumerable<Story> Stories { get; set; }
    }
}
