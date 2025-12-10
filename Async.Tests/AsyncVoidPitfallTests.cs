namespace Async.Tests;

[TestFixture]
public class AsyncVoidPitfallTests
{
    // Demonstrates that async void exceptions cannot be caught normally
    [Test]
    [Ignore("Do to the unhandled exception, this test can only be run on it's own")]
    public void AsyncVoid_ExceptionIsLost()
    {
        var sut = new AsyncVoidPitfall();

        // This does NOT throw - the exception is lost
        Assert.DoesNotThrow(() => sut.ProcessDataBadAsync());

        // The method returns immediately, exception happens later
        // and cannot be observed by the caller
    }
}
