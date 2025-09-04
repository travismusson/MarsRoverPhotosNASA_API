using MarsRoverPhotosNASA_API.Models;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using static MarsRoverPhotosNASA_API.Models.MarsRoverModel;

namespace MarsRoverPhotosNASA_API.Services
{
    public class MarsRoverService
    {
        private readonly HttpClient _httpClient;        //used to make the web req
        private readonly string _apiKey;               //API key for NASA API

        public MarsRoverService(HttpClient httpClient, IConfiguration config)       // Constructor to initialize the service
        {
            _httpClient = httpClient;
            _apiKey = config["ApiConnection:ApiKey"];           // Get the API key from configuration
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MarsRoverPhotosClient/1.0");        // Set the User-Agent header for the HTTP client
        }

        //call api and return data, via the controller when the web page loads
        public async Task<List<MarsRoverModel.MarsRoverPhoto>> GetMarsRoverModel()      //wrapping in list
        {
            int sol = 1000;        //example sol value, can be adjusted or passed as a parameter
            //string specificCamera = "FHAZ";        //example camera value, can be adjusted or passed as a parameter
            int maxPhotos = 1;        //example max photos value, can be adjusted or passed as a parameter; this is now per camera as im using an array and list for cams
            //so i wanna make it either so that the user can select various parameters to pass through later on
            //for now im gonna just make it specific, also im gonna add a list of all the cameras to get multiple camera angles
            string[] availableCameras = { "FHAZ", "RHAZ", "MAST", "NAVCAM", "PANCAM" };     //this is the front hazard avoidance, rear hazard avoid, navigation cam, and panaromic cam
            var allPhotos = new List<MarsRoverModel.MarsRoverPhoto>();
            //wanna also make it for various rovers.
            //atm its just curiosity, but according to the API DOC only specific rovers have specific cameras       --hence curiosity doesnt have a pan cam but spirit and opportunity do

            /*notes
            //gonna have to loop through it     --so this is quite taxing and relativley slow, im thinking theres gotta be a way to improve it? caching perhaps?
            //currently taking +-45s to load the model, looking at how to improve now
            //with caching its taking +155ms to load a cached response (alottttt better)
            //need to look to see if there is another way to optimize the initial loading
            //https://www.youtube.com/watch?v=0qtWIaHnQ6c       useing http caching and in memory seems to be a good alternative for caching, but still need to figure out how to perhaps precache the api calls?
            //looking up some means seems im needing to use some form of parrallel processing for task? not 100% sure how to do that doe
            */
            foreach (var camera in availableCameras) 
            {
                string url = $"https://api.nasa.gov/mars-photos/api/v1/rovers/opportunity/photos?sol={sol}&camera={camera}&page=1&per_page={maxPhotos}&api_key={_apiKey}";
                //need the response here also
                
                HttpResponseMessage response = await _httpClient.GetAsync(url);        //make the web request
                if (response.IsSuccessStatusCode)        //check if the response is successful
                {
                    var content = await response.Content.ReadAsStringAsync();        //read the content of the response
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,       //set the naming policy to snake case to match the API response
                        PropertyNameCaseInsensitive = true
                    };
                    var roverData = JsonSerializer.Deserialize<MarsRoverModel>(content, options);        //deserialize the content into the MarsRoverModel after some debugging i found the root issue; this is with the deserilization, as the API uses snake case i wasnt receiving the correct valeus
                    if (roverData?.Photos != null && roverData.Photos.Count > 0)
                    {
                        var photos = roverData.Photos.Take(maxPhotos).ToList();        //limit the number of photos to maxPhotos using linq  
                        foreach (var photo in photos)
                        {
                            allPhotos.Add(photo);        //add each photo to the allPhotos list
                        }
                    }
                    //return photos;        //return the list of photos     --instead of returning imediatley im adding a list to this logic
                }
                else
                {
                    throw new Exception("Failed to retrieve data from NASA API");        //throw an exception if the request fails
                }
            }
            return allPhotos;        //return the combined list of all photos from all cameras





            //refactored code:

            //string url = $"https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&api_key={_apiKey}";         //just calling default atm will adjust to enable query adjustment     --this is the given example key given by Nasa API Doc

            //building the URL with parameters (later planning on making it dynamic and adjustable by user input)



            /*if(!string.IsNullOrEmpty(camera))        //if camera is not empty, add it to the URL        //getting issue here returning camera 2c so my logic flawed -- dont need this as already ensuring camera is set via parameters
            {
                url += $"&camera={camera}";
            }*/


        }
    }
}
