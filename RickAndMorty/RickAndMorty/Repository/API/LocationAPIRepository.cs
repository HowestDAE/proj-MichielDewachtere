using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace RickAndMorty.Repository.API
{
    public class LocationAPIRepository
    {
        private List<Location> _locations { get; set; } = new List<Location>();
        private Uri _nextPage = new Uri("https://rickandmortyapi.com/api/location");

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
            if (_nextPage == null)
                return _locations;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = _nextPage;

                HttpResponseMessage response = await client.GetAsync("");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var pageInfo = JObject.Parse(responseData)/*["results"]*/;
                    var locations = pageInfo.Value<JArray>("results");
                    var locationList = JsonConvert.DeserializeObject<List<Location>>(locations.ToString());
                    if (locationList != null)
                        _locations.AddRange(locationList);

                    var adress = pageInfo.Value<JObject>("info").Value<string>("next");
                    if (adress != null)
                    {
                        _nextPage = new Uri(adress);
                        await GetLocationsAsync();
                    }

                    return _locations;
                }
            }

            return null;
        }
    }
}