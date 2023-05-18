using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using RickAndMorty.Repository.Interface;

namespace RickAndMorty.Repository.API
{
    public class CharacterAPIRepository : ICharacterRepository
    {
        private List<Character> _characters { get; set; } = new List<Character>();
        private readonly Uri _baseAdress = new Uri("https://rickandmortyapi.com/api/character");
        private Uri _nextPage;

        public CharacterAPIRepository()
        {
            _nextPage = _baseAdress;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            if (_nextPage == null)
                return _characters;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _nextPage;

                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var pageInfo = JObject.Parse(responseData)/*["results"]*/;
                    var characters = pageInfo.Value<JArray>("results");
                    var characterList = JsonConvert.DeserializeObject<List<Character>>(characters.ToString());
                    if (characterList != null) 
                        _characters.AddRange(characterList);

                    var adress = pageInfo.Value<JObject>("info").Value<string>("next");
                    if (adress != null)
                    {
                        _nextPage = new Uri(adress);
                        await GetCharactersAsync();
                    }

                    return _characters;
                }
            }

            return null;
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