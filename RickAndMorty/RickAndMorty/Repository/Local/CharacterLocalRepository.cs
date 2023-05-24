using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using RickAndMorty.Repository.Interface;

namespace RickAndMorty.Repository.Local
{
    public class CharacterLocalRepository : ICharacterRepository
    {
        private List<Character> _characters { get; set; } = new List<Character>(); 
        public int AmountOfCharacters
        {
            get { return _characters.Count; }
        }


        private static CharacterLocalRepository _instance;
        private CharacterLocalRepository()
        {
        }

        public static CharacterLocalRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CharacterLocalRepository();
            }

            return _instance;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            if (_characters.Count != 0)
                return _characters;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RickAndMorty.Resources.Data.characters.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var pageInfo = JObject.Parse(json);
                    var characters = pageInfo.Value<JArray>("results");
                    var characterList = DeserializeCharacter(characters);
                    if (characterList != null)
                        _characters.AddRange(characterList);

                    return _characters;
                }
            }
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

                        if (address != null)
                        {
                            int slashIndex = address.IndexOf("/episode/", StringComparison.Ordinal) +
                                             "/episode/".Length;
                            string idString = address.Substring(slashIndex);

                            int id;
                            if (int.TryParse(idString, out id))
                            {
                                episodeIds.Add(id);
                            }
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

            int slashIndex = url.IndexOf("/location/") + "/location/".Length;
            string idString = url.Substring(slashIndex);

            int id = -1;
            if (int.TryParse(idString, out id))
            {
                return id;
            }

            return id;
        }

        public async Task<List<Character>> GetCharactersByIdsAsync(List<int> ids)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => ids.Contains(c.Id)).ToList();

            if (filteredCharacters == null || filteredCharacters.Count == 0)
                return null;

            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByNameAsync(string name)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Name.ToLower().Contains(name.ToLower())).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByStatusAsync(string status)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Status.ToLower().Equals(status.ToLower())).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersBySpeciesAsync(string species)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Species.ToLower().Equals(species.ToLower())).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByTypeAsync(string type)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Type.ToLower().Equals("(" + type.ToLower() + ")")).ToList();
            return filteredCharacters;

        }

        public async Task<List<Character>> GetCharactersByGenderAsync(string gender)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Gender.ToLower().Equals(gender.ToLower())).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByOriginAsync(string origin)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Origin != null && c.Origin.Name != null && c.Origin.Name.ToLower().Contains(origin.ToLower())).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByLocationAsync(string location)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Location != null && c.Location.Name != null && c.Location.Name.ToLower().Contains(location.ToLower())).ToList();
            return filteredCharacters;
        }

        public async Task<List<Character>> GetCharactersByEpisodeAsync(string episode)
        {
            var characters = await GetCharactersAsync();
            var filteredCharacters = characters.Where(c => c.Episodes.Exists(e => e.Name.ToLower().Contains(episode.ToLower()))).ToList();
            return filteredCharacters;
        }
    }
}