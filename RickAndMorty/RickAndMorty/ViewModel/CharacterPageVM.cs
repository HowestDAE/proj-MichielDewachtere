using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RickAndMorty.Model;

namespace RickAndMorty.ViewModel
{
    public class CharacterPageVM : ObservableObject
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

        public CharacterPageVM()
        {
            CurrentCharacter = new Character()
            {
                Id = 1,
                Name = "Rick Sanchez",
                Status = "Alive",
                Species = "Human",
                Type = "",
                Gender = "Male",
                Origin = new Location()
                {
                    Name = "Earth (C-137)"
                },
                Location = new Location()
                {
                    Name = "Citadel of Ricks"
                }
            };
        }
    }
}
