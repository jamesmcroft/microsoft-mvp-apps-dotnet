namespace MVP.App.Models
{
    /// <summary>
    /// Defines a model for a contributor to the application.
    /// </summary>
    public class Contributor
    {
        /// <summary>
        /// Gets or sets the name of the contributor.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a link to an image of the contributor.
        /// </summary>
        public string ImageUri { get; set; }

        /// <summary>
        /// Gets or sets a link to the contributors webpage of choice.
        /// </summary>
        /// <remarks>
        /// Feel free to use your own personal blog, GitHub profile or another community link (MVP profile).
        /// </remarks>
        public string LinkUri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contributor is an MVP.
        /// </summary>
        public bool IsMvp { get; set; }
    }
}