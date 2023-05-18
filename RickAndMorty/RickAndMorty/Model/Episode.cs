using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RickAndMorty.Repository.API;

namespace RickAndMorty.Model
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AirDate { get; set; }
        public string EpisodeNumber { get; set; }
        public List<int> CharacterIds { get; set; }

        private List<Character> _characters;
        public List<Character> Characters
        {
            get
            {
                LoadCharactersAsync();
                return _characters;
            }
        }
        private async void LoadCharactersAsync()
        {
            if (CharacterAPIRepository.GetInstance().NextPage == null)
            {
                _characters = await CharacterAPIRepository.GetInstance().GetCharactersByIdsAsync(CharacterIds);
            }
        }
    }
}
