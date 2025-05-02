// Decompiled with JetBrains decompiler
// Type: Extensions.ThreadGroup.ThreadBase
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

namespace Extensions.ThreadGroup
{
    public abstract class ThreadBase : IAsyncDisposable
    {
        private volatile bool _alive;
        private readonly CancellationTokenSource _cts = new();
        private Task? _task;

        protected ThreadBase()
        {
        }

        public void Open()
        {
            if (_alive) return;

            _alive = true;
            _task = Task.Factory.StartNew(
                ThreadProc,
                _cts.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        protected abstract Task OnInitAsync();

        protected abstract Task<bool> OnProcessAsync(CancellationToken cancellationToken);

        private async Task ThreadProc()
        {
            await OnInitAsync();

            while (_alive && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    if (!_alive || !await OnProcessAsync(_cts.Token).ConfigureAwait(false))
                        break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Thread exception: {ex.Message}");
                }
            }
        }

        public async Task CloseAsync()
        {
            if (!_alive) return;

            _alive = false;
            _cts.Cancel();

            if (_task is not null)
            {
                try
                {
                    await _task;
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation occurs
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during thread shutdown: {ex.Message}");
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await CloseAsync();
            _cts.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
