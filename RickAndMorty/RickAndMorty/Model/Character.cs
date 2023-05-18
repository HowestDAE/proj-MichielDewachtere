﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RickAndMorty.Repository.API;

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
        public Location Origin { get; set; }
        public Location Location { get; set; }
        public string Image
        {
            get
            {
                return $"https://rickandmortyapi.com/api/character/avatar/{Id}.jpeg";
            }
        }
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
            if (EpisodeAPIRepository.GetInstance().NextPage == null)
            {
                _episodes = await EpisodeAPIRepository.GetInstance().GetEpisodesByIdsAsync(EpisodesIds);
                _isEpisodesLoaded = true;
            }
        }
    }
}
