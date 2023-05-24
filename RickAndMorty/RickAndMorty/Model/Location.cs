using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RickAndMorty.Repository.API;
using RickAndMorty.Repository.Local;
using RickAndMorty.ViewModel;

namespace RickAndMorty.Model
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Dimension { get; set; }
        public List<int> ResidentIds { get; set; }
        private List<Character> _residents;
        public List<Character> Residents
        {
            get
            {
                LoadCharactersAsync();
                return _residents;
            }
        }
        private async void LoadCharactersAsync()
        {
            if (MainVM.UseLocalAPI)
            {
                if (CharacterLocalRepository.GetInstance().AmountOfCharacters != 0 && ResidentIds != null)
                {
                    _residents = await CharacterLocalRepository.GetInstance().GetCharactersByIdsAsync(ResidentIds);
                }
            }
            else
            {
                if (CharacterAPIRepository.GetInstance().NextPage == null)
                {
                    _residents = await CharacterAPIRepository.GetInstance().GetCharactersByIdsAsync(ResidentIds);
                }
            }
        }
    }
}
