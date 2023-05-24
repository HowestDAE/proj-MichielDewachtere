using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RickAndMorty.Repository.Abstract
{
    public abstract class BaseEpisodeRepository
    {
        public abstract Task<List<Episode>> GetEpisodesAsync();

        protected async Task<List<Episode>> DeserializeEpisodes(JArray array)
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