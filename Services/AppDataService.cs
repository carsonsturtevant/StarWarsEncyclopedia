using StarWarsBlazor.Data;
using StarWarsBlazor.Pages;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace StarWarsBlazor.Services
{
    public class AppDataService
    {
        private readonly HttpClient _httpClient;
        private List<Person> people;

        public AppDataService(HttpClient httpClient) {
            _httpClient = httpClient;
            _ = getAllPeople();
        }

        public List<Person> getPeople()
        {
            return people;
        }

        private async Task getAllPeople()
        {
            var next = "https://swapi.co/api/people/";
            while (next != null)
            {
                var response = await _httpClient.GetAsync(next);
                var stream = await response.Content.ReadAsStringAsync();
                var rootObject = JsonConvert.DeserializeObject<RootPeopleObject>(stream);
                people.AddRange(rootObject.results);
                next = rootObject.next;
            }
            people = people.OrderBy(x => x.name).ToList();
        }
    }
}
