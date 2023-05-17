using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RickAndMorty.Model;
using RickAndMorty.Repository.Interface;

namespace RickAndMorty.Repository.Local
{
    public class CharacterLocalRepository : ICharacterRepository
    {
        private List<Character> _characters { get; set; }

        public async Task<List<Character>> GetCharacterAsync()
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
                    _characters = JsonConvert.DeserializeObject<List<Character>>(json);
                    
                    return _characters;
                }
            }

        }
    }
}
