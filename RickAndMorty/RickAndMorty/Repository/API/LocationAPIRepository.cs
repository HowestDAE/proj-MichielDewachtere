using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace RickAndMorty.Repository.API
{
    public class LocationAPIRepository
    {
        private List<Location> _locations { get; set; } = new List<Location>();
        public Uri NextPage = new Uri("https://rickandmortyapi.com/api/location");

        // Singleton instance
        private static LocationAPIRepository _instance;

        // Private constructor to prevent external instantiation
        private LocationAPIRepository()
        {
        }

        // Public static method to access the singleton instance
        public static LocationAPIRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LocationAPIRepository();
            }

            return _instance;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            if (NextPage == null)
                return _locations;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = NextPage;

                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var pageInfo = JObject.Parse(responseData) /*["results"]*/;
                    var locations = pageInfo.Value<JArray>("results");
                    var locationList = await DeserializeLocations(locations);
                    if (locationList != null)
                        _locations.AddRange(locationList);

                    var adress = pageInfo.Value<JObject>("info").Value<string>("next");
                    if (adress != null)
                    {
                        NextPage = new Uri(adress);
                        await GetLocationsAsync();
                    }
                    else
                    {
                        NextPage = null;
                    }

                    return _locations;
                }
            }

            return null;
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

                        // Find the index of the first forward slash after the word "character"
                        int slashIndex = address.IndexOf("/character/") + "/character/".Length;

                        // Extract the substring starting from the index of the first forward slash
                        string idString = address.Substring(slashIndex);

                        // Parse the extracted substring to an integer
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
            var requestedLocation = locations.Where(e => id == e.Id).ToList()[0];
            return requestedLocation;
        }
    }
}