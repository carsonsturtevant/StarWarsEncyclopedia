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
    public class FilmService
    {
        private readonly HttpClient _httpClient;
        private List<Film> films = new List<Film>();
        private string filmsJson;

        public FilmService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<List<Film>> GetFilms()
        {
            if (films.Count == 0)
            {
                await GetAllFilms();
            }
            return films;
        }

        public string GetFilmsJson()
        {
            return filmsJson;
        }

        private async Task GetAllFilms()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                filmsJson = File.ReadAllText("./Data/Json/all_films.json");
                films = JsonConvert.DeserializeObject<List<Film>>(filmsJson);
            }
            else
            {
                var next = "https://swapi.co/api/films/";
                while (next != null)
                {
                    var response = await _httpClient.GetAsync(next);
                    var stream = await response.Content.ReadAsStringAsync();
                    filmsJson += stream;
                    var rootObject = JsonConvert.DeserializeObject<RootSwapiObject<Film>>(stream);
                    films.AddRange(rootObject.results);
                    next = rootObject.next;
                }
            }
            films = films.OrderBy(x => x.url).ToList();
        }
    }
}
