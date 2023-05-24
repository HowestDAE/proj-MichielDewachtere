using System.Collections.Generic;
using System.Threading.Tasks;
using RickAndMorty.Model;

namespace RickAndMorty.Repository.Interface
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetLocationsAsync();
        Task<Location> GetLocationByIdAsync(int id);
    }
}