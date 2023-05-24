using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RickAndMorty.Model;

namespace RickAndMorty.ViewModel
{
    public class CharacterDetailPageVM : ObservableObject
    {
        private Character _currentCharacter;
        public Character CurrentCharacter
        {
            get { return _currentCharacter; }
            set
            {
                _currentCharacter = value;
                OnPropertyChanged();
            }
        }

        private Episode _selectedEpisode;
        public Episode SelectedEpisode
        {
            get { return _selectedEpisode; }
            set
            {
                OriginSelected = false;
                LocationSelected = false;
                _selectedEpisode = value;
            }
        }

        private bool _originSelected;
        public bool OriginSelected
        {
            get { return _originSelected; }
            set
            {
                if (_originSelected != value)
                {
                    _originSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _locationSelected;
        public bool LocationSelected
        {
            get { return _locationSelected;}
            set
            {
                if (_locationSelected != value)
                {
                    _locationSelected = value;
                    OnPropertyChanged();
                }
            }
        }
        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set { _selectedLocation = value; }
        }

        public RelayCommand SelectOriginCommand { get; set; }
        public RelayCommand SelectLocationCommand { get; set; }

        public CharacterDetailPageVM()
        {
            SelectOriginCommand = new RelayCommand(SelectOrigin);
            SelectLocationCommand = new RelayCommand(SelectLocation);

            CurrentCharacter = new Character()
            {
                Id = 1,
                Name = "Rick Sanchez",
                Status = "Alive",
                Species = "Human",
                Type = "",
                Gender = "Male",
            };
        }

        private void SelectOrigin()
        {
            _selectedLocation = CurrentCharacter.Origin;
            OriginSelected = true;
            LocationSelected = false;
        }
        private void SelectLocation()
        {
            _selectedLocation = CurrentCharacter.Location;
            LocationSelected = true;
            OriginSelected = false;
        }
    }
}
