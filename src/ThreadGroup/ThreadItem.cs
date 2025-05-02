// Decompiled with JetBrains decompiler
// Type: Extensions.ThreadGroup.ThreadItem
// Assembly: Extensions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 635FAC6D-CDE5-48E5-A8FB-F155E2899BE4
// Assembly location: C:\Users\x3010\Desktop\LordsUpdate\LordsSource\bin\Debug\Extensions.dll

namespace Extensions.ThreadGroup
{
    public class ThreadItem : ThreadBase
    {
        private readonly int _maxProcessInterval;
        private readonly SemaphoreSlim _semaphore;
        private readonly Func<CancellationToken, Task> _eventHandler;
        private readonly Func<int, CancellationToken, Task>? _sleepHandler;

        public ThreadItem(
            int interval,
            string semaphoreName,
            Func<CancellationToken, Task> eventHandler,
            Func<int, CancellationToken, Task>? sleepHandler = null)
        {
            _maxProcessInterval = interval;
            _semaphore = new SemaphoreSlim(1, 1);
            _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
            _sleepHandler = sleepHandler;
        }

        protected override Task OnInitAsync() => Task.CompletedTask;

        protected override async Task<bool> OnProcessAsync(CancellationToken cancellationToken)
        {
            await using var _ = await _semaphore.WaitAsyncDisposable(cancellationToken).ConfigureAwait(false);

            try
            {
                var now = Time32.Now;
                try
                {
                    await _eventHandler(cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    int elapsed = Time32.Now.AllMilliseconds - now.AllMilliseconds;
                    int sleepTime = _maxProcessInterval - elapsed;

                    if (sleepTime is >= 0 && sleepTime <= _maxProcessInterval)
                    {
                        if (_sleepHandler is not null)
                            await _sleepHandler(sleepTime, cancellationToken).ConfigureAwait(false);
                        else
                            await Task.Delay(sleepTime, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return true;
        }
    }
}
