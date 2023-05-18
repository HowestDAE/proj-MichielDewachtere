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
                    //var characters = JObject.Parse(json)["results"];
                    _characters = JsonConvert.DeserializeObject<List<Character>>(characters.ToString());

                    for (int i = 0; i < _characters.Count; ++i)
                    {
                        var episodes = characters[i].Value<JArray>("episode");
                        //_characters[i].Episodes = JsonConvert.DeserializeObject<List<Episode>>(episodes.ToString());
                        if (episodes != null)
                            for (int j = 0; j < episodes.Count; ++j)
                            {
                                //_characters[i].Episodes.Add(new Episode());
                                //_characters[i].Episodes[j].Id = episodes[j].Value<int>("id");
                                //_characters[i].Episodes[j].Name = episodes[j].Value<string>("name");
                                //_characters[i].Episodes[j].AirDate = episodes[j].Value<string>("air_date");
                                //_characters[i].Episodes[j].EpisodeNumber = episodes[j].Value<string>("episode");
                                //_characters[i].Episodes[j].Characters = new List<Character>();
                            }
                        //var episodes = characters[i]["episode"].ToObject<JArray>();
                        //var episodes = characters["episode"].ToObject<List<JObject>>().Select(e => e["name"].ToString()).ToList();
                        //character.Episodes.AddRange();
                    }

                    return _characters;
                }
            }
        }

        public Task<List<Character>> GetCharactersByIdsAsync(List<int> ids)
        {
            throw new System.NotImplementedException();
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