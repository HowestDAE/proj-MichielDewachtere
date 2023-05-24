using RickAndMorty.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using RickAndMorty.Repository.Interface;
using System.Linq;
using Newtonsoft.Json.Linq;
using RickAndMorty.Repository.API;
using System;
using System.IO;
using System.Reflection;

namespace RickAndMorty.Repository.Local
{
    public class EpisodeLocalRepository : IEpisodeRepository
    {
        private List<Episode> _episodes { get; set; } = new List<Episode>();
        public int AmountOfEpisodes
        {
            get { return _episodes.Count; }
        }

        private static EpisodeLocalRepository _instance;
        private EpisodeLocalRepository()
        {
        }
        public static EpisodeLocalRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EpisodeLocalRepository();
            }

            return _instance;
        }

        public async Task<List<Episode>> GetEpisodesAsync()
        {
            if (_episodes.Count != 0)
                return _episodes;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RickAndMorty.Resources.Data.episodes.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var pageInfo = JObject.Parse(json);
                    var episodes = pageInfo.Value<JArray>("results");
                    var episodeLit = await DeserializeEpisodes(episodes);
                    if (episodeLit != null)
                        _episodes.AddRange(episodeLit);

                    return _episodes;
                }
            }
        }

        public async Task<List<Episode>> DeserializeEpisodes(JArray array)
        {
            var episodes = new List<Episode>();

            foreach (var episode in array)
            {
                var newEpisode = new Episode()
                {
                    Id = episode.Value<int>("id"),
                    Name = episode.Value<string>("name"),
                    AirDate = episode.Value<string>("air_date"),
                    EpisodeNumber = episode.Value<string>("episode"),
                    CharacterIds = new List<int>()
                };

                var characters = episode.Value<JArray>("characters");
                if (characters != null)
                {
                    var characterIds = new List<int>();
                    foreach (var character in characters)
                    {
                        var address = character.Value<string>();

                        int slashIndex = address.IndexOf("/character/") + "/character/".Length;
                        string idString = address.Substring(slashIndex);

                        int id;
                        if (int.TryParse(idString, out id))
                        {
                            characterIds.Add(id);
                        }
                    }

                    newEpisode.CharacterIds.AddRange(characterIds);
                }


                episodes.Add(newEpisode);
            }

            return episodes;
        }


        public async Task<List<Episode>> GetEpisodesByIdsAsync(List<int> ids)
        {
            var episodes = await GetEpisodesAsync();
            var filteredEpisodes = episodes.Where(e => ids.Contains(e.Id)).ToList();
            return filteredEpisodes;
        }
    }
}