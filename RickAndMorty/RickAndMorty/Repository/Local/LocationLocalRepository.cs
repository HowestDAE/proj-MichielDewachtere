using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using RickAndMorty.Repository.Abstract;

namespace RickAndMorty.Repository.Local
{
    public class LocationLocalRepository : BaseLocationRepository
    {
        private List<Location> _locations { get; set; } = new List<Location>();

        public int AmountOfLocations
        {
            get { return _locations.Count; }
        }

        private static LocationLocalRepository _instance;

        private LocationLocalRepository()
        {
        }

        public static LocationLocalRepository GetInstance()
        {
            if (_instance == null)
                _instance = new LocationLocalRepository();

            return _instance;
        }

        public override async Task<List<Location>> GetLocationsAsync()
        {
            if (_locations.Count != 0)
                return _locations;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "RickAndMorty.Resources.Data.locations.json";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();
                    var pageInfo = JObject.Parse(json);
                    var locations = pageInfo.Value<JArray>("results");
                    var locationList = await DeserializeLocations(locations);
                    if (locationList != null)
                        _locations.AddRange(locationList);

                    return _locations;
                }
            }
        }
    }
}