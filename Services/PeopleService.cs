using StarWarsBlazor.Data;
using StarWarsBlazor.Data.RootObjects;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.IO;
using StarWarsBlazor.Pages;

namespace StarWarsBlazor.Services
{
    public class PeopleService
    {
        private readonly HttpClient _httpClient;
        private List<Person> people = new List<Person>();
        private string peopleJson;

        public PeopleService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<List<Person>> GetPeople()
        {
            if (people.Count == 0)
            {
                await GetAllPeople();
            }
            return people;
        }

        public string GetPeopleJson()
        {
            return peopleJson;
        }

        public Person GetPerson(string url)
        {
            return people.Find(x => x.url == url);
        }

        private async Task GetAllPeople()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                peopleJson = File.ReadAllText("./Data/Json/all_people.json");
                people = JsonConvert.DeserializeObject<List<Person>>(peopleJson);
            }
            else
            {
                var next = "https://swapi.co/api/people/";
                while (next != null)
                {
                    var response = await _httpClient.GetAsync(next);
                    var stream = await response.Content.ReadAsStringAsync();
                    peopleJson += stream;
                    var rootObject = JsonConvert.DeserializeObject<RootSwapiObject<Person>>(stream);
                    people.AddRange(rootObject.results);
                    next = rootObject.next;
                }
            }
            people = people.OrderBy(x => x.name).ToList();
        }
    }
}
