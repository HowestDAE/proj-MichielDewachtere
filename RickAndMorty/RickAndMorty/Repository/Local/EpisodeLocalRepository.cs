using RickAndMorty.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using RickAndMorty.Repository.Abstract;

namespace RickAndMorty.Repository.Local
{
    public class EpisodeLocalRepository : BaseEpisodeRepository
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

        public override async Task<List<Episode>> GetEpisodesAsync()
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
    }
}