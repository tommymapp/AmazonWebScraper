using Api.Models;

namespace Api.Interfaces;

public interface IWatchRepo
{
    Watch[] GetActiveWatches();
}