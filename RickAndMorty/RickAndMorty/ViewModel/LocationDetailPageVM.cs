using System.Collections.Generic;
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
                //Residents = new List<Character>()
            };

            //CurrentLocation.Residents.Add(new Character()
            //{
            //    Id = 1,
            //    Name = "Rick Sanchez"
            //});
            //CurrentLocation.Residents.Add(new Character()
            //{
            //    Id = 2,
            //    Name = "Morty Smith"
            //});
            //CurrentLocation.Residents.Add(new Character()
            //{
            //    Id = 3,
            //    Name = "Beth"
            //});
            //CurrentLocation.Residents.Add(new Character()
            //{
            //    Id = 4,
            //    Name = "Summer Smith"
            //});
            //CurrentLocation.Residents.Add(new Character()
            //{
            //    Id = 5,
            //    Name = "Jerry Smith"
            //});
            //CurrentLocation.Residents.Add(new Character()
            //{
            //    Id = 6,
            //    Name = "Someone Else idk"
            //});
        }
    }
}