namespace MVP.App
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    using Windows.Storage;
    using Windows.Storage.Streams;

    using WinUX.Data.Serialization;

    using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

    public static partial class Extensions
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

        public static async Task SaveDataAsync<T>(this StorageFile storageFile, T data)
        {
            if (storageFile == null)
            {
                throw new ArgumentNullException(nameof(storageFile), "Cannot save data to a null file.");
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Cannot save null data to a file.");
            }

            var encoding = Encoding.UTF8;
            var json = SerializationService.Json.Serialize(data);
            var bytes = encoding.GetBytes(json);
            await FileIO.WriteBytesAsync(storageFile, bytes);
        }

        public static async Task<string> GetDataAsStringAsync(this StorageFile storageFile)
        {
            if (storageFile == null)
            {
                throw new ArgumentNullException(nameof(storageFile), "Cannot retrieve data from a null file.");
            }

            return await FileIO.ReadTextAsync(storageFile, UnicodeEncoding.Utf8);
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