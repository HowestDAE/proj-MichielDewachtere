using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net;

namespace RickAndMorty.Repository.API
{
    public class EpisodeAPIRepository
    {
        private List<Episode> _episodes { get; set; } = new List<Episode>();
        public Uri NextPage = new Uri("https://rickandmortyapi.com/api/episode");

        // Singleton instance
        private static EpisodeAPIRepository _instance;

        // Private constructor to prevent external instantiation
        private EpisodeAPIRepository()
        {
        }

        // Public static method to access the singleton instance
        public static EpisodeAPIRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EpisodeAPIRepository();
            }

            return _instance;
        }

        public async Task<List<Episode>> GetEpisodesAsync()
        {
            if (NextPage == null)
                return _episodes;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = NextPage;

                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var pageInfo = JObject.Parse(responseData)/*["results"]*/;
                    var episodes = pageInfo.Value<JArray>("results");
                    var episodeList = await DeserializeEpisodes(episodes);
                    if (episodeList != null)
                        _episodes.AddRange(episodeList);

                    var adress = pageInfo.Value<JObject>("info").Value<string>("next");
                    if (adress != null)
                    {
                        NextPage = new Uri(adress);
                        await GetEpisodesAsync();
                    }
                    else
                    {
                        NextPage = null;
                    }

                    return _episodes;
                }
            }

            return null;
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

                        // Find the index of the first forward slash after the word "character"
                        int slashIndex = address.IndexOf("/character/") + "/character/".Length;

                        // Extract the substring starting from the index of the first forward slash
                        string idString = address.Substring(slashIndex);

                        // Parse the extracted substring to an integer
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