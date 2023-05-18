using System.Collections.Generic;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RickAndMorty.Model;
using RickAndMorty.Repository.API;
using RickAndMorty.Repository.Interface;
using RickAndMorty.Repository.Local;

namespace RickAndMorty.ViewModel
{
    public class OverViewVM : ObservableObject
    {
        private readonly ICharacterRepository _characterRepository;

        private List<Character> _characters;
        public List<Character> Characters
        {
            get { return _characters;}
            set
            {
                if (_characters != value)
                {
                    _characters = value;
                    OnPropertyChanged(nameof(Characters));
                }
            }

        }

        private Character _selectedCharacter;
        public Character SelectedCharacter
        {
            get { return _selectedCharacter; }
            set { _selectedCharacter = value; }
        }

        public List<string> Filters { get; set; } = new List<string>();
        private string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set { _selectedFilter = value; }
        }


        public RelayCommand<string> SearchCommand { get; private set; }

        public OverViewVM()
        {
            _characterRepository = new CharacterAPIRepository();
            //_characterRepository = new CharacterLocalRepository();
            LoadCharacters();

            LoadFilters();

            SearchCommand = new RelayCommand<string>(LoadFilteredCharacters);
        }

        private async void LoadCharacters()
        {
            Characters = await _characterRepository.GetCharactersAsync();
        }

        private async void LoadFilteredCharacters(string searchId)
        {
            //do not respond if input is empty
            if (string.IsNullOrWhiteSpace(searchId)) 
                return;

            if (_selectedFilter.Equals(Filters[0]))
                Characters = await _characterRepository.GetCharactersByNameAsync(searchId);
            else if (_selectedFilter.Equals(Filters[1]))
                Characters = await _characterRepository.GetCharactersByStatusAsync(searchId);
            else if (_selectedFilter.Equals(Filters[2]))
                Characters = await _characterRepository.GetCharactersBySpeciesAsync(searchId);
            else if (_selectedFilter.Equals(Filters[3]))
                Characters = await _characterRepository.GetCharactersByTypeAsync(searchId);
            else if (_selectedFilter.Equals(Filters[4]))
                Characters = await _characterRepository.GetCharactersByGenderAsync(searchId);
            else if (_selectedFilter.Equals(Filters[5]))
                Characters = await _characterRepository.GetCharactersByOriginAsync(searchId);
            else if (_selectedFilter.Equals(Filters[6]))
                Characters = await _characterRepository.GetCharactersByLocationAsync(searchId);
            else if (_selectedFilter.Equals(Filters[7]))
                Characters = await _characterRepository.GetCharactersByEpisodeAsync(searchId);
        }

        private void LoadFilters()
        {
            Filters.Add("Search By Name");
            Filters.Add("Search By Status");
            Filters.Add("Search By Species");
            Filters.Add("Search By Type");
            Filters.Add("Search By Gender");
            Filters.Add("Search By Origin");
            Filters.Add("Search By Location");
            Filters.Add("Search By Episode Name");
        }
    }
}
