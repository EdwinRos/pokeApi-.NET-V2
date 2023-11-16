namespace pokeApi_.NET.Models.Pokemon
{
    public class PokemonComplete
    {
        public int id { get; set; }
        public string? name { get; set; }

        public string? imageUrl { get; set; }

        public List<string> Tipos { get; set; }
    }
}
