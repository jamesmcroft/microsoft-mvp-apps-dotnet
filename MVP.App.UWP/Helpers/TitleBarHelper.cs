namespace MVP.App.Helpers
{
    using System;

    using Windows.ApplicationModel.Core;
    using Windows.Foundation.Metadata;
    using Windows.UI;
    using Windows.UI.ViewManagement;

    using WinUX;

    /// <summary>
    /// Defines a helper class for handling title bar functionality.
    /// </summary>
    public static class TitleBarHelper
    {
        /// <summary>
        /// Extends the content of the application into the title bar by the given parameter.
        /// </summary>
        /// <param name="shouldExtend">
        /// A value indicating whether the app should extend into the title bar.
        /// </param>
        public static void ExtendToTitleBar(bool shouldExtend)
        {
            if (!ApiInformation.IsTypePresent("Windows.ApplicationModel.Core.CoreApplication"))
            {
                return;
            }

            CoreApplicationView coreApplicationView = CoreApplication.GetCurrentView();
            if (coreApplicationView?.TitleBar != null)
            {
                coreApplicationView.TitleBar.ExtendViewIntoTitleBar = shouldExtend;
            }

            if (!shouldExtend)
            {
                return;
            }

            // We're extending into the title bar so the button colors need to update to be transparent.
            if (!ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                return;
            }

            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            if (applicationView?.TitleBar != null)
            {
                applicationView.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                applicationView.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            }
        }

        /// <summary>
        /// Initializes the title bar with the application's theme.
        /// </summary>
        public static void InitializeTitleBar()
        {
            ExtendToTitleBar(false);

            if (!ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                return;
            }

            ApplicationView applicationView = ApplicationView.GetForCurrentView();
            if (applicationView?.TitleBar == null)
            {
                return;
            }

            applicationView.TitleBar.ForegroundColor = Colors.White;
            applicationView.TitleBar.BackgroundColor = "#00467A".ToColor();

            applicationView.TitleBar.InactiveForegroundColor = Colors.Black;
            applicationView.TitleBar.InactiveBackgroundColor = "#C6E7FF".ToColor();

            applicationView.TitleBar.ButtonForegroundColor = Colors.White;
            applicationView.TitleBar.ButtonBackgroundColor = "#00467A".ToColor();

            applicationView.TitleBar.ButtonHoverForegroundColor = Colors.Black;
            applicationView.TitleBar.ButtonHoverBackgroundColor = "#60BCFF".ToColor();

            applicationView.TitleBar.ButtonPressedForegroundColor = Colors.White;
            applicationView.TitleBar.ButtonPressedBackgroundColor = "#001A2D".ToColor();

            applicationView.TitleBar.ButtonInactiveForegroundColor = Colors.Black;
            applicationView.TitleBar.ButtonInactiveBackgroundColor = "#C6E7FF".ToColor();
        }
    }
}