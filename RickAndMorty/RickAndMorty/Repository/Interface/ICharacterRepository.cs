using System.Collections.Generic;
using System.Threading.Tasks;
using RickAndMorty.Model;

namespace RickAndMorty.Repository.Interface
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetCharactersAsync();
        Task<List<Character>> GetCharactersByNameAsync(string name);
        Task<List<Character>> GetCharactersByStatusAsync(string status);
        Task<List<Character>> GetCharactersBySpeciesAsync(string species);
        Task<List<Character>> GetCharactersByTypeAsync(string type);
        Task<List<Character>> GetCharactersByGenderAsync(string gender);
        Task<List<Character>> GetCharactersByOriginAsync(string origin);
        Task<List<Character>> GetCharactersByLocationAsync(string location);
        Task<List<Character>> GetCharactersByEpisodeAsync(string episode);
    }
}