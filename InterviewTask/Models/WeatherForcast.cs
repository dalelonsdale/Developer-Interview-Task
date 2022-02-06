using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTask.Models
{
    public class WeatherForcast
    {
        [JsonProperty("coord")]
        public CoordInfo Coord { get; set; }
        [JsonProperty("weather")]
        public List<WeatherInfo> Weather { get; set; }

        [JsonProperty("@base")]
        public string Base { get; set; }

        [JsonProperty("main")]
        public MainInfo Main { get; set; }

        [JsonProperty("Visibility")]
        public int Visibility { get; set; }

        [JsonProperty("wind")]
        public WindInfo Wind { get; set; }

        [JsonProperty("clouds")]
        public CloudsInfo Clouds { get; set; }

        [JsonProperty("dt")]
        public int Dt { get; set; }

        [JsonProperty("sys")]
        public SysInfo Sys { get; set; }

        [JsonProperty("timezone")]
        public int Timezone { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cod")]
        public int Cod { get; set; }

        public class CoordInfo
        {
            [JsonProperty("lon")]
            public double Longitude { get; set; }
            [JsonProperty("lat")]
            public double Lattitude { get; set; }
        }

        public class WeatherInfo
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("main")]
            public string Main { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("icon")]
            public string Icon { get; set; }
        }

        public class MainInfo
        {
            [JsonProperty("temp")]
            public double Temp { get; set; }
            [JsonProperty("feels_like")]
            public double Feelslike { get; set; }
            [JsonProperty("temp_min")]
            public double TempMin { get; set; }
            [JsonProperty("temp_max")]
            public double TempMax { get; set; }
            [JsonProperty("pressure")]
            public int Pressure { get; set; }
            [JsonProperty("humidity")]
            public int Humidity { get; set; }
        }

        public class WindInfo
        {
            [JsonProperty("speed")]
            public double Speed { get; set; }
            [JsonProperty("deg")]
            public int Deg { get; set; }
            [JsonProperty("gust")]
            public double Gust { get; set; }
        }

        public class CloudsInfo
        {
            [JsonProperty("all")]
            public int All { get; set; }            
        }

        public class SysInfo
        {
            [JsonProperty("type")]
            public int type { get; set; }
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("country")]
            public string Country { get; set; }
            [JsonProperty("sunrise")]
            public int Sunrise { get; set; }
            [JsonProperty("sunset")]
            public int Sunset { get; set; }
        }
    }
}