using System.Collections.Generic;

namespace StarWarsBlazor.Data
{
    public class RootSpeciesObject
    {
        public List<Species> results { get; set; }
        public string next { get; set; }
    }
}
