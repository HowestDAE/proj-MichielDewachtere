using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using RickAndMorty.Model;

namespace RickAndMorty.ViewModel
{
    public class EpisodeDetailPageVM : ObservableObject
    {
		private Episode _currentEpisode;

		public Episode CurrentEpisode
		{
			get { return _currentEpisode; }
            set
            {
                if (_currentEpisode != value)
                {
                    _currentEpisode = value;
                    OnPropertyChanged(nameof(_currentEpisode));
                }
            }
        }

        public EpisodeDetailPageVM()
        {
            CurrentEpisode = new Episode()
            {
                Id = 1,
                Name = "Pilot",
                AirDate = "December 2, 2013",
                EpisodeNumber = "S01E01",
                Characters = new List<Character>()
            };

            CurrentEpisode.Characters.Add(new Character()
            {
                Id = 1,
                Name = "Rick Sanchez"
            });
            CurrentEpisode.Characters.Add(new Character()
            {
                Id = 2,
                Name = "Morty Smith"
            });
            CurrentEpisode.Characters.Add(new Character()
            {
                Id = 3,
                Name = "Beth"
            });
            CurrentEpisode.Characters.Add(new Character()
            {
                Id = 4,
                Name = "Summer Smith"
            });
            CurrentEpisode.Characters.Add(new Character()
            {
                Id = 5,
                Name = "Jerry Smith"
            });
            CurrentEpisode.Characters.Add(new Character()
            {
                Id = 6,
                Name = "Someone Else idk"
            });
        }

	}
}