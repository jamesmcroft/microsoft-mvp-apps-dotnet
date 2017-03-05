namespace MVP.App.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading;
    using System.Threading.Tasks;

    using Windows.Foundation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    using WinUX.Diagnostics.Tracing;
    using WinUX.Xaml;

    public class LazyLoadItemCollection<TItem, TDataContainer> : ObservableCollection<TItem>, ISupportIncrementalLoading
        where TDataContainer : IItemLoader<TItem>
    {
        private CancellationToken cancellationToken;

        private readonly uint increment;

        public LazyLoadItemCollection()
            : this(Activator.CreateInstance<TDataContainer>(), 20)
        {
        }

        public LazyLoadItemCollection(TDataContainer container, uint increment)
        {
            this.Container = container;
            this.increment = increment;
            this.HasMoreItems = true;
        }

        public TDataContainer Container { get; }

        public bool HasMoreItems { get; private set; }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(this.LoadMoreItemsAsync);
        }

        public void Reset()
        {
            this.Clear();
            this.HasMoreItems = true;
        }

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken ct)
        {
            uint resultCount = 0;

            this.cancellationToken = ct;

            if (!this.cancellationToken.IsCancellationRequested)
            {
                IEnumerable<TItem> containerItems = null;

                try
                {
                    containerItems = await this.Container.GetMoreItemsAsync(
                                         (uint)this.Items.Count,
                                         this.increment,
                                         this.cancellationToken);
                }
                catch (OperationCanceledException)
                {
                }
                catch (HttpRequestException hre) when (hre.Message.Contains("401"))
                {
                    // Work out way to inform user.
                    Application.Current.Exit();
                }
                catch (Exception ex)
                {
                    EventLogger.Current.WriteDebug(ex.Message);
                }

                var items = containerItems as IList<TItem> ?? containerItems.ToList();
                if (items != null && items.Any() && !this.cancellationToken.IsCancellationRequested)
                {
                    resultCount = (uint)items.Count;

                    await UIDispatcher.RunAsync(
                        () =>
                            {
                                foreach (var item in items)
                                {
                                    this.Add(item);
                                }
                            });
                }
                else
                {
                    this.HasMoreItems = false;
                }
            }

            return new LoadMoreItemsResult { Count = resultCount };
        }
    }
}