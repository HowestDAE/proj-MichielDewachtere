using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public Uri NextPage = new Uri("https://rickandmortyapi.com/api/character");

        // Singleton instance
        private static CharacterAPIRepository _instance;

        // Private constructor to prevent external instantiation
        private CharacterAPIRepository()
        {
        }

        // Public static method to access the singleton instance
        public static CharacterAPIRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CharacterAPIRepository();
            }

            return _instance;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            if (NextPage == null)
                return _characters;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = NextPage;

                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var pageInfo = JObject.Parse(responseData)/*["results"]*/;
                    var characters = pageInfo.Value<JArray>("results");
                    //var characterList = JsonConvert.DeserializeObject<List<Character>>(characters.ToString());
                    var characterList = DeserializeCharacter(characters);
                    if (characterList != null) 
                        _characters.AddRange(characterList);

                    var adress = pageInfo.Value<JObject>("info").Value<string>("next");
                    if (adress != null)
                    {
                        NextPage = new Uri(adress);
                        await GetCharactersAsync();
                    }
                    else
                    {
                        NextPage = null;
                    }

                    return _characters;
                }
            }

            return null;
        }

        public async Task<List<Character>> GetCharactersByIdsAsync(List<int> ids)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => ids.Contains(c.Id)).ToList();
            return filteredCharacters;
        }

        public List<Character> DeserializeCharacter(JArray characterList)
        {
            var characters = new List<Character>();

            foreach (var character in characterList)
            {
                var newCharacter = new Character()
                {
                    Id = character.Value<int>("id"),
                    Name = character.Value<string>("name"),
                    Status = character.Value<string>("status"),
                    Species = character.Value<string>("species"),
                    Type = character.Value<string>("type"),
                    Gender = character.Value<string>("gender"),
                    OriginId = GetIdFromLocationURL(character.Value<JObject>("origin")?.Value<string>("url")),
                    LocationId = GetIdFromLocationURL(character.Value<JObject>("location")?.Value<string>("url")),
                    EpisodesIds = new List<int>()
                };

                var episodes = character.Value<JArray>("episode");
                if (episodes != null)
                {
                    var episodeIds = new List<int>();
                    foreach (var episode in episodes)
                    {
                        var address = episode.Value<string>();

                        // Find the index of the first forward slash after the word "character"
                        int slashIndex = address.IndexOf("/episode/") + "/episode/".Length;

                        // Extract the substring starting from the index of the first forward slash
                        string idString = address.Substring(slashIndex);

                        // Parse the extracted substring to an integer
                        int id;
                        if (int.TryParse(idString, out id))
                        {
                            episodeIds.Add(id);
                        }
                    }

                    newCharacter.EpisodesIds.AddRange(episodeIds);
                }
                characters.Add(newCharacter);
            }

            return characters;
        }

        private int GetIdFromLocationURL(string url)
        {
            if (url == "") 
                return -1;

            // Find the index of the first forward slash after the word "character"
            int slashIndex = url.IndexOf("/location/") + "/location/".Length;

            // Extract the substring starting from the index of the first forward slash
            string idString = url.Substring(slashIndex);

            // Parse the extracted substring to an integer
            int id = -1;
            if (int.TryParse(idString, out id))
            {
                return id;
            }

            return id;
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
            //var filteredCharacters = characters.Where(c => c.Origin.Name.Equals(origin)).ToList();
            //return filteredCharacters;
            return null;
        }

        public async Task<List<Character>> GetCharactersByLocationAsync(string location)
        {
            var characters = await GetCharactersAsync();
            //var filteredCharacters = characters.Where(c => c.Location.Name.Equals(location)).ToList();
            //return filteredCharacters;
            return null;
        }

        public async Task<List<Character>> GetCharactersByEpisodeAsync(string episode)
        {
            var characters = await GetCharactersAsync();
            //var filteredCharacters = characters.Where(c => c.Episodes.Exists(e => e.Name.Equals(episode))).ToList();
            //return filteredCharacters;
            return null;
        }
    }
}