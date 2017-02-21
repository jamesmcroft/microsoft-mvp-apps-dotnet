namespace MVP.App.Models.Common
{
    using GalaSoft.MvvmLight;

    using MVP.App.Data;

    public abstract class ItemViewModelBase<TModel> : ViewModelBase, IValidate
    {
        public abstract void Populate(TModel model);

        public abstract bool IsValid();
    }
}