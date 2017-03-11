namespace MVP.App
{
    using Android.Views;
    using Android.Widget;

    using GalaSoft.MvvmLight.Helpers;

    public partial class InitializingActivity
    {
        private ProgressBar loadingProgressRing;

        private Binding<ViewStates, ViewStates> loadingProgressRingVisibilityBinding;

        private TextView loadingProgress;

        private Binding<string, string> loadingProgressTextBinding;

        private Binding<ViewStates, ViewStates> loadingProgressVisibilityBinding;

        private Button signinButton;

        private Binding<ViewStates, ViewStates> signinButtonVisibilityBinding;
        
        public ProgressBar LoadingProgressRing
            =>
                this.loadingProgressRing
                ?? (this.loadingProgressRing = this.FindViewById<ProgressBar>(Resource.Id.loading_progressring));

        public TextView LoadingProgress
            =>
                this.loadingProgress
                ?? (this.loadingProgress = this.FindViewById<TextView>(Resource.Id.loading_progress));

        public Button SigninButton
            => this.signinButton ?? (this.signinButton = this.FindViewById<Button>(Resource.Id.signin_button));

        private void SetupBindings()
        {
            this.loadingProgressRingVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadingState,
                () => this.LoadingProgressRing.Visibility);

            this.loadingProgressTextBinding = this.SetBinding(
                () => this.ViewModel.LoadingProgress,
                () => this.LoadingProgress.Text);

            this.loadingProgressVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadingState,
                () => this.LoadingProgress.Visibility);

            this.signinButtonVisibilityBinding = this.SetBinding(
                () => this.ViewModel.LoadedState,
                () => this.SigninButton.Visibility);
        }
    }
}