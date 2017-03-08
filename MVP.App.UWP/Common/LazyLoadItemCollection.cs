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

    /// <summary>
    /// Defines an observable item collection which has lazy loading capabilities.
    /// </summary>
    /// <typeparam name="TItem">
    /// The type of item contained in the collection.
    /// </typeparam>
    /// <typeparam name="TDataContainer">
    /// The type of <see cref="IItemLoader{TItem}"/> to load data from.
    /// </typeparam>
    public class LazyLoadItemCollection<TItem, TDataContainer> : ObservableCollection<TItem>, ISupportIncrementalLoading
        where TDataContainer : IItemLoader<TItem>
    {
        private CancellationToken cancellationToken;

        private readonly uint increment;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyLoadItemCollection{TItem,TDataContainer}"/> class.
        /// </summary>
        public LazyLoadItemCollection()
            : this(20)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyLoadItemCollection{TItem,TDataContainer}"/> class.
        /// </summary>
        /// <param name="increment">
        /// The number of items to retrieve on each load.
        /// </param>
        public LazyLoadItemCollection(uint increment)
            : this(Activator.CreateInstance<TDataContainer>(), increment)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyLoadItemCollection{TItem,TDataContainer}"/> class.
        /// </summary>
        /// <param name="container">
        /// The container to use to get data from.
        /// </param>
        /// <param name="increment">
        /// The number of items to retrieve on each load.
        /// </param>
        public LazyLoadItemCollection(TDataContainer container, uint increment)
        {
            this.Container = container;
            this.increment = increment;
            this.HasMoreItems = true;
        }

        /// <summary>
        /// Gets the container to get data from.
        /// </summary>
        public TDataContainer Container { get; }

        /// <inheritdoc />
        public bool HasMoreItems { get; private set; }

        /// <inheritdoc />
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(this.LoadMoreItemsAsync);
        }

        /// <summary>
        /// Resets the items in the collection and begins lazy loading again.
        /// </summary>
        public void Reset()
        {
            this.Clear();
            this.HasMoreItems = true;
        }

        /// <summary>
        /// Loads items from the container into the collection asynchronously.
        /// </summary>
        /// <param name="ct">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// When this method completes, it returns a <see cref="LoadMoreItemsResult"/> object.
        /// </returns>
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