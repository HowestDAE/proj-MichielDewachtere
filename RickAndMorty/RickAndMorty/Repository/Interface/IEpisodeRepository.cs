using RickAndMorty.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RickAndMorty.Repository.Interface
{
    public interface IEpisodeRepository
    {
        Task<List<Episode>> GetEpisodesAsync();
        Task<List<Episode>> GetEpisodesByIdsAsync(List<int> ids);
    }
}