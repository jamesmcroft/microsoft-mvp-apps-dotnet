namespace MVP.App
{
    using System;
    using System.Threading.Tasks;

    using Windows.Storage;
    using Windows.Storage.Streams;

    using WinUX.Data.Serialization;

    public static class Extensions
    {
        public static async Task<T> GetDataAsync<T>(this StorageFile storageFile)
        {
            if (storageFile == null)
            {
                throw new ArgumentNullException(nameof(storageFile), "Cannot retrieve data from a null file.");
            }

            var dataString = await storageFile.GetDataAsStringAsync();

            return SerializationService.Json.Deserialize<T>(dataString);
        }

        public static async Task<string> GetDataAsStringAsync(this StorageFile storageFile)
        {
            if (storageFile == null)
            {
                throw new ArgumentNullException(nameof(storageFile), "Cannot retrieve data from a null file.");
            }

            return await FileIO.ReadTextAsync(storageFile);
        }

        public static async Task<IBuffer> GetDataAsBufferAsync(this StorageFile storageFile)
        {
            if (storageFile == null)
            {
                throw new ArgumentNullException(nameof(storageFile), "Cannot retrieve data from a null file.");
            }

            return await FileIO.ReadBufferAsync(storageFile);
        }
    }
}