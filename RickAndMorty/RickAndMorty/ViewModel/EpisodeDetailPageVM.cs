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
                    _currentEpisode = value;
                    OnPropertyChanged(nameof(CurrentEpisode));
            }
        }

        private Character _selectedCharacter;

        public Character SelectedCharacter
        {
            get { return _selectedCharacter; }
            set
            {
                if (_selectedCharacter != value)
                {
                    _selectedCharacter = value;
                    OnPropertyChanged(nameof(SelectedCharacter));
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
            };
        }

	}
}