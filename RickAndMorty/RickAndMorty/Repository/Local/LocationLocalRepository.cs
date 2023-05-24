using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using RickAndMorty.Repository.Interface;

namespace RickAndMorty.Repository.Local
{
    public class LocationLocalRepository : ILocationRepository
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

        public async Task<List<Location>> GetLocationsAsync()
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
        public async Task<List<Location>> DeserializeLocations(JArray array)
        {
            var locations = new List<Location>();

            foreach (var location in array)
            {
                var newLocation = new Location()
                {
                    Id = location.Value<int>("id"),
                    Name = location.Value<string>("name"),
                    Type = location.Value<string>("type"),
                    Dimension = location.Value<string>("dimension"),
                    ResidentIds = new List<int>()
                };

                var residents = location.Value<JArray>("residents");
                if (residents != null)
                {
                    var residentIds = new List<int>();
                    foreach (var resident in residents)
                    {
                        var address = resident.Value<string>();

                        int slashIndex =  address.IndexOf("/character/") + "/character/".Length;
                        string idString = address.Substring(slashIndex);

                        int id;
                        if (int.TryParse(idString, out id))
                        {
                            residentIds.Add(id);
                        }
                    }

                    newLocation.ResidentIds.AddRange(residentIds);
                }


                locations.Add(newLocation);
            }

            return locations;
        }

        public async Task<Location> GetLocationByIdAsync(int id)
        {
            var locations = await GetLocationsAsync();
            var requestedLocation = locations.Where(e => id == e.Id).ToList();

            if (requestedLocation.Count == 0)
            {
                return new Location()
                {
                    Name = "Unknown"
                };
            }

            return requestedLocation[0];
        }
    }
}