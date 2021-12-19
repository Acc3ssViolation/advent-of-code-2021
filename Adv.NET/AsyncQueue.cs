using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Adv.NET
{
    class AsyncQueue<T>
    {
        private Queue<T> _queue = new Queue<T>();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(0);

        public void Enqueue(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);
            }
            _semaphore.Release();
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            lock (_queue)
            {
                return _queue.Dequeue();
            }
        }
    }
}
