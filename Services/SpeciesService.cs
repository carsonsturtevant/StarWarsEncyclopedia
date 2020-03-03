using StarWarsBlazor.Data;
using StarWarsBlazor.Data.RootObjects;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;

namespace StarWarsBlazor.Services
{
    public class SpeciesService
    {
        private readonly HttpClient _httpClient;
        private List<Species> species = new List<Species>();
        private string speciesJson;

        public SpeciesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Species>> GetSpecies()
        {
            if (species.Count == 0)
            {
                await GetAllSpecies();
            }
            return species;
        }

        public string GetSpeciesJson()
        {
            return speciesJson;
        }

        public Species GetSpecies(string url)
        {
            return species.Find(x => x.url == url);
        }

        private async Task GetAllSpecies()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                speciesJson = File.ReadAllText("./Data/Json/all_species.json");
                species = JsonConvert.DeserializeObject<List<Species>>(speciesJson);
            }
            else
            {
                var next = "https://swapi.co/api/species/";
                while (next != null)
                {
                    var response = await _httpClient.GetAsync(next);
                    var stream = await response.Content.ReadAsStringAsync();
                    speciesJson += stream;
                    var rootObject = JsonConvert.DeserializeObject<RootSwapiObject<Species>>(stream);
                    species.AddRange(rootObject.results);
                    next = rootObject.next;
                }
            }
            species = species.OrderBy(x => x.name).ToList();
        }
    }
}
