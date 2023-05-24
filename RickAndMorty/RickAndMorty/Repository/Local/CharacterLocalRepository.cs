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
using RickAndMorty.Repository.Abstract;
using RickAndMorty.Repository.Interface;

namespace RickAndMorty.Repository.Local
{
    public class CharacterLocalRepository : BaseCharacterRepository
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

        public override async Task<List<Character>> GetCharactersAsync()
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
    }
}