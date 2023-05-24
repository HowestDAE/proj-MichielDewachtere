using Newtonsoft.Json.Linq;
using RickAndMorty.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using RickAndMorty.Repository.Abstract;

namespace RickAndMorty.Repository.API
{
    public class LocationAPIRepository : BaseLocationRepository
    {
        private List<Location> _locations { get; set; } = new List<Location>();
        public Uri NextPage = new Uri("https://rickandmortyapi.com/api/location");

        private static LocationAPIRepository _instance;

        private LocationAPIRepository()
        {
        }
        public static LocationAPIRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LocationAPIRepository();
            }

            return _instance;
        }

        public override async Task<List<Location>> GetLocationsAsync()
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
    }
}