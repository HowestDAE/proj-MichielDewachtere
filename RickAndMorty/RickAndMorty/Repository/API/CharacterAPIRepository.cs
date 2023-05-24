using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using RickAndMorty.Repository.Abstract;

namespace RickAndMorty.Repository.API
{
    public class CharacterAPIRepository : BaseCharacterRepository
    {
        private List<Character> _characters { get; set; } = new List<Character>();
        public Uri NextPage = new Uri("https://rickandmortyapi.com/api/character");

        private static CharacterAPIRepository _instance;

        private CharacterAPIRepository()
        {
        }

        public static CharacterAPIRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CharacterAPIRepository();
            }

            return _instance;
        }

        public override async Task<List<Character>> GetCharactersAsync()
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
                    var pageInfo = JObject.Parse(responseData);
                    var characters = pageInfo.Value<JArray>("results");
                    var characterList = DeserializeCharacter(characters);
                    if (characterList != null)
                        _characters.AddRange(characterList);

                    var adress = pageInfo.Value<JObject>("info")?.Value<string>("next");
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
    }
}