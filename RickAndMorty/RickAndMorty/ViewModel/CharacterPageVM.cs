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
                },
                Episodes = new List<Episode>()
            };

            CurrentCharacter.Episodes.Add(new Episode()
            {
                EpisodeNumber = "S01E01",
                Name = "Pilot"
            });
            CurrentCharacter.Episodes.Add(new Episode()
            {
                EpisodeNumber = "S01E02",
                Name = "Lawnmower Dog"
            });
            CurrentCharacter.Episodes.Add(new Episode()
            {
                EpisodeNumber = "S01E03",
                Name = "Anatomy Park"
            });
            CurrentCharacter.Episodes.Add(new Episode()
            {
                EpisodeNumber = "S01E04",
                Name = "M. Night Shaym-Aliens"
            });
            CurrentCharacter.Episodes.Add(new Episode()
            {
                EpisodeNumber = "S01E05",
                Name = "Meeseeks and Destroy"
            });
        }
    }
}
