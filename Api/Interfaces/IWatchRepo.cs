using Api.Models;

namespace Api.Interfaces;

public interface IWatchRepo
{
    Watch[] GetActiveWatches();
    Task UpdateWatch(Watch watch);
}