using System.Collections.Generic;
using System.Threading.Tasks;
using RickAndMorty.Model;

namespace RickAndMorty.Repository.Interface
{
    public interface ICharacterRepository
    {
        Task<List<Character>> GetCharacterAsync();
    }
}