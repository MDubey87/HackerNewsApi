using Newtonsoft.Json;

namespace hacker.news.api.repositories.HackerNewsRepository.Models
{
    /// <summary>
    /// Response Model for Hacker News Story
    /// </summary>
    public class HackerNewsResponse
    {
        /// <summary>
        /// News Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Title of News
        /// </summary>
        public string Title { get; set;}
        /// <summary>
        /// News Page Url
        /// </summary>
        public string Url { get; set;}
    }
}
