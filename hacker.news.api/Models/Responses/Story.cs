namespace hacker.news.api.Models.Responses
{
    /// <summary>
    /// News Response Class
    /// </summary>
    public class Story
    {
        /// <summary>
        /// News Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Title of News
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// News Page Url
        /// </summary>
        public string Url { get; set; }
    }
}
