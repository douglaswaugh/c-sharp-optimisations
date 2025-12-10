namespace Async;

public class AsyncVoidPitfall
{
    public async void ProcessDataBadAsync()
    {
        await Task.Delay(100);
        throw new InvalidOperationException("This exception is lost!");
    }

    public async Task ProcessDataGoodAsync()
    {
        await Task.Delay(10);
        throw new InvalidOperationException("This exception can be caught!");
    }
}
