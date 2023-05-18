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
                //Origin = new Location()
                //{
                //    Name = "Earth (C-137)"
                //},
                //Location = new Location()
                //{
                //    Name = "Citadel of Ricks"
                //},
                //Episodes = new List<Episode>()
            };
            
            //CurrentCharacter.Episodes.Add(new Episode()
            //{
            //    EpisodeNumber = "S01E01",
            //    Name = "Pilot"
            //});
            //CurrentCharacter.Episodes.Add(new Episode()
            //{
            //    EpisodeNumber = "S01E02",
            //    Name = "Lawnmower Dog"
            //});
            //CurrentCharacter.Episodes.Add(new Episode()
            //{
            //    EpisodeNumber = "S01E03",
            //    Name = "Anatomy Park"
            //});
            //CurrentCharacter.Episodes.Add(new Episode()
            //{
            //    EpisodeNumber = "S01E04",
            //    Name = "M. Night Shaym-Aliens"
            //});
            //CurrentCharacter.Episodes.Add(new Episode()
            //{
            //    EpisodeNumber = "S01E05",
            //    Name = "Meeseeks and Destroy"
            //});
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
