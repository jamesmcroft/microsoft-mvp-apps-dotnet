namespace MVP.App.Models.Common
{
    using GalaSoft.MvvmLight;

    using MVP.App.Common;

    public abstract class ItemViewModelBase<TModel> : ViewModelBase, IValidate
    {
        /// <summary>
        /// Populates the item view model with a model.
        /// </summary>
        /// <param name="model">
        /// The model to populate with.
        /// </param>
        public abstract void Populate(TModel model);

        /// <summary>
        /// Saves the item view model as the model.
        /// </summary>
        /// <returns>
        /// Returns the model representing the item view model.
        /// </returns>
        public abstract TModel Save();

        /// <inheritdoc />
        public abstract bool IsValid();
    }
}