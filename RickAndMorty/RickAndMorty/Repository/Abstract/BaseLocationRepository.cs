using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RickAndMorty.Repository.Abstract
{
    public abstract class BaseLocationRepository
    {
        public abstract Task<List<Location>> GetLocationsAsync();

        protected async Task<List<Location>> DeserializeLocations(JArray array)
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

                        int slashIndex = address.IndexOf("/character/") + "/character/".Length;
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