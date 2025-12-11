namespace Async;

/// <summary>
/// Demonstrates the ConfigureAwait(false) pitfall.
///
/// In library code, not using ConfigureAwait(false) can cause:
/// 1. Deadlocks when callers block with .Result or .Wait()
/// 2. Unnecessary performance overhead from context switching
/// </summary>
public class ConfigureAwaitPitfall
{
    // BAD: Captures the SynchronizationContext
    // If called from a single-threaded context and blocked with .Result,
    // this will deadlock
    public async Task<string> GetDataBadAsync()
    {
        await Task.Delay(100);
        return "data";
    }

    public async Task<string> GetDataGoodAsync()
    {
        await Task.Delay(100).ConfigureAwait(false);
        return "data";
    }
}
