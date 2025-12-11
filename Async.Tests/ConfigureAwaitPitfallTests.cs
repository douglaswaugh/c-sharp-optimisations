namespace Async.Tests;

[TestFixture]
public class ConfigureAwaitPitfallTests
{
    /// <summary>
    /// This test demonstrates a DEADLOCK caused by not using ConfigureAwait(false).
    ///
    /// What happens:
    /// 1. We set up a single-threaded SynchronizationContext (like a UI thread)
    /// 2. We call GetDataBadAsync().Result, blocking the "UI thread"
    /// 3. Task.Delay completes and tries to resume on the captured context
    /// 4. The continuation is queued to run on the "UI thread"
    /// 5. But the "UI thread" is blocked waiting for .Result
    /// 6. DEADLOCK - the task can't complete because it's waiting for a blocked thread
    ///
    /// The test uses a timeout to detect the deadlock rather than hanging forever.
    /// </summary>
    [Test]
    public void GetDataBadAsync_DeadlocksWithSingleThreadedContext()
    {
        var deadlockDetected = false;

        var thread = new Thread(() =>
        {
            var syncContext = new SingleThreadedSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncContext);

            var sut = new ConfigureAwaitPitfall();

            // .Result blocks the current thread - this will deadlock because:
            // - We're blocking this thread with .Result
            // - The await continuation needs to run on this thread (captured context)
            // - But this thread is blocked waiting for .Result
            var result = sut.GetDataBadAsync().Result;
        });

        thread.Start();

        // If Join times out, the thread is still blocked = deadlock
        var completed = thread.Join(TimeSpan.FromMilliseconds(500));
        deadlockDetected = !completed;

        Assert.That(deadlockDetected, Is.True,
            "Expected deadlock - GetDataBadAsync().Result should deadlock when called from a single-threaded context");
    }

    // TODO: Add GetDataGoodAsync with ConfigureAwait(false)
    // and prove it does NOT deadlock in the same scenario
}
