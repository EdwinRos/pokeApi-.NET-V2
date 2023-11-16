using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pokeApi_.NET.Models;
using pokeApi_.NET.Models.Pokemon;
using System.Diagnostics;

namespace pokeApi_.NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HttpClient _httpClient;

        private const int itemCounts = 25;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int offset = (page - 1) * itemCounts;

            List<PokemonComplete> pokemonCompletes = new List<PokemonComplete>();
            //Logica para cargar la poke api
            //primero obtener los pokemon haciendo uso de httpCLient
            HttpResponseMessage response = await _httpClient.GetAsync($"pokemon?offset={offset}&limit={itemCounts}");

            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();

                    var pokemonList = serializer.Deserialize<PokemonFirstList>(jsonReader);

                    //Modificar la lista para agregar todo lo demas no solo el nombre
                    foreach (var item in pokemonList.results)
                    {
                        //vamos a hacer una nueva lista con cada pokemon ya completo.
                        PokemonComplete pokemon = new PokemonComplete();
                        pokemon.name = item.name;

                        //Consumir la segunda ruta para el llenado
                        var IdFromUrl = ObtenerId(item.url);
                        HttpResponseMessage complementResponse = await _httpClient.GetAsync($"pokemon-form/{IdFromUrl}");
                        if (complementResponse.IsSuccessStatusCode)
                        {
                            //agregar imagen
                            pokemon.imageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{IdFromUrl}.png";
                            pokemon.id = IdFromUrl;

                            //agregar los tipos
                            using (var typeStream = await complementResponse.Content.ReadAsStreamAsync())
                            using (var typeReader = new StreamReader(typeStream))
                            using (var typeJsonReader = new JsonTextReader(typeReader))
                            {
                                var TypeInfo = serializer.Deserialize<Tpes>(typeJsonReader);

                                pokemon.Tipos = TypeInfo.types.Select(t => t.type.name).ToList();

                            }
                        }  
                        //agregar cada elemento a la lista
                        pokemonCompletes.Add(pokemon);
                    }
                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = (int)Math.Ceiling((double)pokemonList.count / itemCounts);

                    return View(pokemonCompletes);

                }
            }
            return View(Error);
        }

        public IActionResult GetPokemonDetail(string pokemon)
        {
            //peetiticion mas corta
            string objString = HttpContext.Request.Form["pokemonObj"];

            ViewBag.PokemonDetail = objString;
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //metodo para obtener el id de la url
        private int ObtenerId(String id)
        {
            string[] parts = id.Split('/');
            
            return int.Parse(parts[6]);
        }

    }
}