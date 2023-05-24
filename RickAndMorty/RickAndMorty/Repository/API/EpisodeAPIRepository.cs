using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using RickAndMorty.Repository.Abstract;

namespace RickAndMorty.Repository.API
{
    public class EpisodeAPIRepository : BaseEpisodeRepository
    {
        private List<Episode> _episodes { get; set; } = new List<Episode>();
        public Uri NextPage = new Uri("https://rickandmortyapi.com/api/episode");

        private static EpisodeAPIRepository _instance;

        private EpisodeAPIRepository()
        {
        }

        public static EpisodeAPIRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EpisodeAPIRepository();
            }

            return _instance;
        }

        public override async Task<List<Episode>> GetEpisodesAsync()
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
                    var pageInfo = JObject.Parse(responseData);
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

    }
}