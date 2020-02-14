using System.Collections.Generic;

namespace StarWarsBlazor.Data
{
    public class RootPeopleObject
    {
        public List<Person> results { get; set; }
        public string next { get; set; }
    }
}
