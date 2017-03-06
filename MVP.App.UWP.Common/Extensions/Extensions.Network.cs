namespace MVP.App
{
    using WinUX.Networking;

    public static partial class Extensions
    {
        /// <summary>
        /// Gets a value indicating whether a network connection is available.
        /// </summary>
        /// <param name="networkStatusManager">
        /// The network status manager.
        /// </param>
        /// <returns>
        /// Returns true if connected; else false.
        /// </returns>
        public static bool IsConnected(this NetworkStatusManager networkStatusManager)
        {
            return networkStatusManager != null
                   && networkStatusManager.CurrentConnectionType != NetworkConnectionType.Disconnected
                   && networkStatusManager.CurrentConnectionType != NetworkConnectionType.Unknown;
        }
    }
}