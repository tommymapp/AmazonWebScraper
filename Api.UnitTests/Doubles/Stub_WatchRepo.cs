using Api.Interfaces;
using Api.Models;

namespace Api.UnitTests.Doubles;

public class Stub_WatchRepo : IWatchRepo
{
    public Watch[] WatchesToReturn { get; set; }
    public Watch[] GetActiveWatches()
    {
        return WatchesToReturn;
    }

    public Task UpdateWatch(Watch watch)
    {
        return Task.CompletedTask;
    }
}