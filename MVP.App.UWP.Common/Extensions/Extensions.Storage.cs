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
            var dataString = await storageFile.GetDataAsStringAsync();

            return SerializationService.Json.Deserialize<T>(dataString);
        }

        public static async Task SaveDataAsync<T>(this StorageFile storageFile, T data)
        {
            var encoding = Encoding.UTF8;
            var json = SerializationService.Json.Serialize(data);
            var bytes = encoding.GetBytes(json);
            await FileIO.WriteBytesAsync(storageFile, bytes);
        }

        public static async Task<string> GetDataAsStringAsync(this StorageFile storageFile)
        {
            return await FileIO.ReadTextAsync(storageFile, UnicodeEncoding.Utf8);
        }

        public static async Task<IBuffer> GetDataAsBufferAsync(this StorageFile storageFile)
        {
            return await FileIO.ReadBufferAsync(storageFile);
        }
    }
}