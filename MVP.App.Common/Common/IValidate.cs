namespace MVP.App.Common
{
    /// <summary>
    /// Defines an interface for an object that can be validated.
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// Checks whether the object is valid.
        /// </summary>
        /// <returns>
        /// When this method completes, it returns true if valid; else false.
        /// </returns>
        bool IsValid();
    }
}