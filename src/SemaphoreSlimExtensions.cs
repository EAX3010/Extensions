namespace Extensions
{
    public static class SemaphoreSlimExtensions
    {
        public static async Task<SemaphoreReleaser> WaitAsyncDisposable(this SemaphoreSlim semaphore, CancellationToken cancellationToken = default)
        {
            await semaphore.WaitAsync(cancellationToken);
            return new SemaphoreReleaser(semaphore);
        }

        public readonly struct SemaphoreReleaser : IAsyncDisposable
        {
            private readonly SemaphoreSlim _semaphore;

            public SemaphoreReleaser(SemaphoreSlim semaphore)
            {
                _semaphore = semaphore;
            }

            public ValueTask DisposeAsync()
            {
                _semaphore.Release();
                return ValueTask.CompletedTask;
            }
        }
    }
}
