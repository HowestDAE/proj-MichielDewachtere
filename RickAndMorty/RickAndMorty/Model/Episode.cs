using System.Collections.Generic;
using RickAndMorty.Repository.API;
using RickAndMorty.Repository.Local;
using RickAndMorty.ViewModel;

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
            if (MainVM.UseLocalAPI)
            {
                if (CharacterLocalRepository.GetInstance().AmountOfCharacters != 0 && CharacterIds != null)
                {
                    _characters = await CharacterLocalRepository.GetInstance().GetCharactersByIdsAsync(CharacterIds);
                }
            }
            else
            {
                if (CharacterAPIRepository.GetInstance().NextPage == null)
                {
                    _characters = await CharacterAPIRepository.GetInstance().GetCharactersByIdsAsync(CharacterIds);
                }
            }
        }
    }
}
