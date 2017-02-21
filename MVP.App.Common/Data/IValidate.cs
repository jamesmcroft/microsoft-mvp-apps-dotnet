namespace MVP.App.Data
{
    public interface IValidate
    {
        /// <summary>
        /// Checks whether the object is valid.
        /// </summary>
        /// <returns>
        /// Returns true if valid; else false.
        /// </returns>
        bool IsValid();
    }
}