using System.Collections.Concurrent;

namespace Async;

/// <summary>
/// A single-threaded SynchronizationContext that processes work on one thread.
/// This simulates UI thread behavior (WinForms/WPF) where all continuations
/// must be processed by the same thread.
///
/// Used to demonstrate ConfigureAwait deadlocks.
/// </summary>
public class SingleThreadedSynchronizationContext : SynchronizationContext
{
    private readonly BlockingCollection<(SendOrPostCallback Callback, object? State)> _queue = new();
    private readonly Thread _thread;

    public SingleThreadedSynchronizationContext()
    {
        _thread = Thread.CurrentThread;
    }

    public override void Post(SendOrPostCallback d, object? state)
    {
        _queue.Add((d, state));
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (Thread.CurrentThread == _thread)
        {
            d(state);
        }
        else
        {
            var done = new ManualResetEventSlim(false);
            Post(_ =>
            {
                d(state);
                done.Set();
            }, null);
            done.Wait();
        }
    }

    /// <summary>
    /// Runs the message pump. Call this from the "UI thread" to process queued work.
    /// </summary>
    public void RunUntilEmpty()
    {
        while (_queue.TryTake(out var item, TimeSpan.FromMilliseconds(10)))
        {
            item.Callback(item.State);
        }
    }

    /// <summary>
    /// Runs with a timeout - used to detect deadlocks.
    /// Returns false if timed out (deadlock detected).
    /// </summary>
    public bool RunWithTimeout(TimeSpan timeout)
    {
        var deadline = DateTime.UtcNow + timeout;
        while (DateTime.UtcNow < deadline)
        {
            if (_queue.TryTake(out var item, TimeSpan.FromMilliseconds(10)))
            {
                item.Callback(item.State);
            }
        }
        return true;
    }

    public void Complete() => _queue.CompleteAdding();
}
