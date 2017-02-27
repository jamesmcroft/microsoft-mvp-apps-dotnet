namespace MVP.App
{
    using System.Threading.Tasks;

    using WinUX.Data.Serialization;

    using XamarinApiToolkit.Storage;

    public static partial class Extensions
    {
        public static async Task<T> GetDataAsync<T>(this IAppFile storageFile)
        {
            var dataString = await storageFile.ReadTextAsync();
            return SerializationService.Json.Deserialize<T>(dataString);
        }

        public static async Task SaveDataAsync<T>(this IAppFile storageFile, T data)
        {
            var json = SerializationService.Json.Serialize(data);
            await storageFile.WriteTextAsync(json);
        }
    }
}