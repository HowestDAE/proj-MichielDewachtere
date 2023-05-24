using System.Collections.Generic;
using RickAndMorty.Repository.API;
using RickAndMorty.Repository.Local;
using RickAndMorty.ViewModel;

namespace RickAndMorty.Model
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }

        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value.Length != 0)
                {
                    _type = $"({value})";
                }
                else
                {
                    _type = value;
                }
            }

        }
        public string Gender { get; set; }
       
        //Location data
        public int OriginId { get; set; }
        private Location _origin { get; set; }
        public Location Origin
        {
            get
            {
                if (OriginId == -1)
                    return null;

                LoadOriginAsync(OriginId);
                return _origin;
            }
        }

        private async void LoadOriginAsync(int id)
        {
            if (MainVM.UseLocalAPI)
            {
                if (LocationLocalRepository.GetInstance().AmountOfLocations != 0)
                {
                    _origin = await LocationLocalRepository.GetInstance().GetLocationByIdAsync(id);
                }
            }
            else
            {
                if (LocationAPIRepository.GetInstance().NextPage == null)
                {
                    _origin = await LocationAPIRepository.GetInstance().GetLocationByIdAsync(id);
                }
            }
        }

        public int LocationId { get; set; }
        private Location _location { get; set; }
        public Location Location
        {
            get
            {
                if (LocationId == -1)
                    return null;

                LoadLocationAsync(LocationId);
                return _location;
            }
        }
        private async void LoadLocationAsync(int id)
        {
            if (MainVM.UseLocalAPI)
            {
                if (LocationLocalRepository.GetInstance().AmountOfLocations != 0)
                {
                    _location = await LocationLocalRepository.GetInstance().GetLocationByIdAsync(id);
                }
            }
            else
            {
                if (LocationAPIRepository.GetInstance().NextPage == null)
                {
                    _location = await LocationAPIRepository.GetInstance().GetLocationByIdAsync(id);
                }
            }
        }


        public string Image
        {
            get
            {
                return $"https://rickandmortyapi.com/api/character/avatar/{Id}.jpeg";
            }
        }
        
        //Episode data
        public List<int> EpisodesIds {get; set; } = new List<int>();

        private List<Episode> _episodes;
        private bool _isEpisodesLoaded;
        public List<Episode> Episodes
        {
            get
            {
                if (!_isEpisodesLoaded)
                {
                    LoadEpisodesAsync();
                }
                return _episodes;
            }
        }

        private async void LoadEpisodesAsync()
        {
            if (MainVM.UseLocalAPI)
            {
                if (EpisodeLocalRepository.GetInstance().AmountOfEpisodes != 0)
                {
                    _episodes = await EpisodeLocalRepository.GetInstance().GetEpisodesByIdsAsync(EpisodesIds);
                    _isEpisodesLoaded = true;
                }
            }
            else
            {
                if (EpisodeAPIRepository.GetInstance().NextPage == null)
                {
                    _episodes = await EpisodeAPIRepository.GetInstance().GetEpisodesByIdsAsync(EpisodesIds);
                    _isEpisodesLoaded = true;
                }
            }
        }
    }
}
