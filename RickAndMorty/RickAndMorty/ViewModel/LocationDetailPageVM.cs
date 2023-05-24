using CommunityToolkit.Mvvm.ComponentModel;
using RickAndMorty.Model;

namespace RickAndMorty.ViewModel
{
    public class LocationDetailPageVM : ObservableObject
    {
        private Location _currentLocation;
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                if (_currentLocation != value)
                {
                    _currentLocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private Character _selectedResident;

        public Character SelectedResident
        {
            get { return _selectedResident; }
            set
            {
                if (_selectedResident != value)
                {
                    _selectedResident = value;
                    OnPropertyChanged(nameof(_selectedResident));
                }
            }
        }


        public LocationDetailPageVM()
        {
            CurrentLocation = new Location()
            {
                Id = 1,
                Name = "Earth (C-137)",
                Type = "Planet",
                Dimension = "Dimension C-137",
            };
        }
    }
}