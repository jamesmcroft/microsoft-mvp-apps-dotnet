namespace MVP.App.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Windows.Storage;
    using Windows.Storage.Streams;

    using WinUX.Security.Data;

    public class StorageService
    {
        private IDataEncryptionService encryptionService;

        public StorageService(IDataEncryptionService encryptionService)
        {
            if (encryptionService == null)
            {
                throw new ArgumentNullException(nameof(encryptionService), "The data encryption service cannot be null.");
            }

            this.encryptionService = encryptionService;
        }

        public async Task<T> GetDataFromFileAsync<T>(StorageFolder storageFolder, string fileName)
        {
            var encryptedBuffer = await this.GetDataBufferFromFileAsync(storageFolder, fileName);

        }

        private async Task<IBuffer> GetDataBufferFromFileAsync(StorageFolder storageFolder, string fileName)
        {
            if (storageFolder == null)
            {
                throw new ArgumentNullException(nameof(storageFolder), "Cannot retrieve data from a null folder.");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(
                    nameof(fileName),
                    "Cannot retrieve data from a file without a file name.");
            }

            var storageFile = await storageFolder.GetFileAsync(fileName);
            if (storageFile == null)
            {
                throw new FileNotFoundException("Cannot retrieve data from a file that does not exist.", fileName);
            }

            return await FileIO.ReadBufferAsync(storageFile);
        }
    }
}