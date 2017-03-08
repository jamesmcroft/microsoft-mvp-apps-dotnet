namespace MVP.App.Events
{
    /// <summary>
    /// Defines the enumeration values for the data refresh context.
    /// </summary>
    public enum RefreshDataMode
    {
        /// <summary>
        /// Will refresh all context's.
        /// </summary>
        All,

        /// <summary>
        /// Will refresh the user's MVP profile.
        /// </summary>
        Profile,

        /// <summary>
        /// Will refresh the user's contributions.
        /// </summary>
        Contributions
    }
}