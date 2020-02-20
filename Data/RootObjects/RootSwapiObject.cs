using System.Collections.Generic;

namespace StarWarsBlazor.Data.RootObjects
{
    public class RootSwapiObject<T>
    {
        public List<T> results { get; set; }
        public string next { get; set; }
    }
}
