using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using RickAndMorty.Repository.Interface;

namespace RickAndMorty.Repository.Local
{
    public class CharacterLocalRepository : ICharacterRepository
    {
        private List<Character> _characters { get; set; }

        public async Task<List<Character>> GetCharactersAsync()
        {
            if (_characters != null)
                return _characters;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RickAndMorty.Resources.Data.characters.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var characters = JObject.Parse(json)["results"];
                    _characters = JsonConvert.DeserializeObject<List<Character>>(characters.ToString());
                    
                    return _characters;
                }
            }
        }

        public async Task<List<Character>> GetCharactersByNameAsync(string name)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Name.Equals(name)).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByStatusAsync(string status)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Status.Equals(status)).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersBySpeciesAsync(string species)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Species.Equals(species)).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByTypeAsync(string type)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Type.Equals(type)).ToList();
            return filteredCharacters;

        }

        public async Task<List<Character>> GetCharactersByGenderAsync(string gender)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Gender.Equals(gender)).ToList();
            return filteredCharacters;

        }

        public async Task<List<Character>> GetCharactersByOriginAsync(string origin)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Origin.Name.Equals(origin)).ToList();
            return filteredCharacters;

        }

        public async Task<List<Character>> GetCharactersByLocationAsync(string location)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Location.Name.Equals(location)).ToList();
            return filteredCharacters;

        }

        public async Task<List<Character>> GetCharactersByEpisodeAsync(string episode)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Episodes.Exists(e => e.Name.Equals(episode))).ToList();
            return filteredCharacters;

        }
    }
}