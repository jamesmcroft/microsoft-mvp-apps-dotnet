using System;
using System.Collections.ObjectModel;

using Java.Lang;
using Java.Util;

namespace MVP.App.Common.Charting
{
    public class ChartCollection<T> : ObservableCollection<T>, IIterable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IntPtr Handle { get; }
        public IIterator Iterator()
        {
            throw new NotImplementedException();
        }
    }
}