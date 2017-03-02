namespace MVP.App.Common
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using WinUX.Diagnostics.Tracing;

    public class TimeoutAction
    {
        public static async Task<TValue> ExecuteAsync<TValue>(Task<TValue> taskToExecute, CancellationTokenSource cts = null)
        {
            if (cts == null)
            {
                cts = new CancellationTokenSource();
            }

            var token = cts.Token;

            var result = default(TValue);

            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(10), token);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var task = await Task.WhenAny(taskToExecute, timeoutTask);
                    if (task == taskToExecute)
                    {
                        cts.Cancel();

                        if (!task.IsFaulted)
                        {
                            result = taskToExecute.Result;
                        }
                    }

                    if (task == timeoutTask)
                    {
                        // ToDo; Show dialog.
                        cts.Cancel();
                    }
                }
                catch (TaskCanceledException tce)
                {
                    EventLogger.Current.WriteDebug(tce.ToString());
                }
            }

            return result;
        }
    }
}