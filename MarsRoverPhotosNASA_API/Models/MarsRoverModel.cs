using System.Text.Json.Serialization;

namespace MarsRoverPhotosNASA_API.Models
{
    public class MarsRoverModel
    {       
        public List<MarsRoverPhoto> Photos { get; set; } = new List<MarsRoverPhoto>(); //list of photos returned from the API

        public class MarsRoverPhoto
        {
            
            public int Id { get; set; }
            public int Sol { get; set; }
            public Camera Camera { get; set; }
            public string ImgSrc { get; set; }
            public string EarthDate { get; set; }
            public Rover Rover { get; set; }
        }

        public class Camera
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int RoverId { get; set; }
            public string FullName { get; set; }
        }

        public class Rover
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LandingDate { get; set; }
            public string LaunchDate { get; set; }
            public string Status { get; set; }
        } 



        //eg request:
        //https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?sol=1000&api_key=DEMO_KEY
        //eg response from API
        //      {
        //"photos": [
        //  {
        //    "id": 102693,
        //    "sol": 1000,
        //    "camera": {
        //      "id": 20,
        //      "name": "FHAZ",
        //      "rover_id": 5,
        //      "full_name": "Front Hazard Avoidance Camera"
        //    },
        //    "img_src": "http://mars.jpl.nasa.gov/msl-raw-images/proj/msl/redops/ods/surface/sol/01000/opgs/edr/fcam/FLB_486265257EDR_F0481570FHAZ00323M_.JPG",
        //    "earth_date": "2015-05-30",
        //    "rover": {
        //      "id": 5,
        //      "name": "Curiosity",
        //      "landing_date": "2012-08-06",
        //      "launch_date": "2011-11-26",
        //      "status": "active"
        //    }
        //  },
    }
}
