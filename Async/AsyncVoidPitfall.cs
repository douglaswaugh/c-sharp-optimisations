namespace Async;

public class AsyncVoidPitfall
{
    public async void ProcessDataBadAsync()
    {
        await Task.Delay(100);
        throw new InvalidOperationException("This exception is lost!");
    }
}
