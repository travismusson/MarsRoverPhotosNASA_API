using MarsRoverPhotosNASA_API.Models;
using MarsRoverPhotosNASA_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace MarsRoverPhotosNASA_API.Controllers
{
    public class MarsRoverController : Controller
    {
        private readonly MarsRoverService _marsRoverService;
        //instantiate the memory cache
        private readonly IMemoryCache _memoryCache;
        public MarsRoverController(MarsRoverService marsRoverService, IMemoryCache memoryCache)
        {
            _marsRoverService = marsRoverService;       //map service to controller
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://www.youtube.com/watch?v=0qtWIaHnQ6c
        //refactored - as we are now utilizing in memory cache, seems to be more effective not 100% sure on the logistics
        //seems response caching is more inline with client operations within html pages, not really used for api req
        //memory caching allows for accomodation of expensive api req

        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any, VaryByHeader ="User-Agent")]       //improves loadtimes drastically for cached responses, this is for 30s the cache will remain before its removed
       
        //there is also something called redis caching? need to research some more      --introduced with .net8
        //maybe i could preload the api req within background somehow?





        public async Task<IActionResult> MarsPhotos()
        {
            //define cache key
            const string cacheKey = "roverResult";

            if(!_memoryCache.TryGetValue(cacheKey, out IEnumerable<MarsRoverModel.MarsRoverPhoto>? marsRoverModel))      //may be nullable
            {
                //cache miss get from API
                marsRoverModel = await _marsRoverService.GetMarsRoverModel();        //call the service to get the Mars Rover model    
                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(30)

                };
                //store within cache
                _memoryCache.Set(cacheKey, marsRoverModel, cacheOptions);
                //debug addin some loggin to ensure cache miss
                Console.WriteLine("Cache miss - fetched from API");
            }
            else
            {
                //cache hit
                Console.WriteLine("Cache hit - fetched from cache");
            }

                return View(marsRoverModel);        //return the view with the model
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
