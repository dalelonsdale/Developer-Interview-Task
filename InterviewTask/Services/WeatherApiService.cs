using InterviewTask.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace InterviewTask.Services
{
    public class WeatherApiService
    {
        #region Structs
        public struct GeoResponse
        {
            public bool Success;
            public string Message;
            public double Longitude;
            public double Lattitude;
        }

        public struct ApiResponse
        {
            public bool Success;
            public string Result;
            public HttpStatusCode StatusCode;
        }

        public struct WeatherForcastResponse
        {
            public WeatherForcast Data;
            public bool Success;
            public string Message;
        }

        #endregion

        #region Members

        public ILoggerService LoggerService { get; set; }

        #endregion

        #region Constructor
        public WeatherApiService()
        {
            LoggerService = new LoggerService();
        }
        #endregion

        #region Methods
        public GeoResponse GetLongAndLatFromCity(string city)
        {
            var response = GetApiResponse(string.Concat("direct?q=", city, ",GB"), ConfigurationManager.AppSettings.Get("GeoApiBaseUrl"));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseObject = JsonConvert.DeserializeObject<List<GeoCode>>(response.Result).First();
                return new GeoResponse { Success = true, Lattitude = responseObject.lat, Longitude = responseObject.lon };
            }
            return new GeoResponse() { Success = false, Message = "Error: StatusCode:" + response.StatusCode };

        }

        public WeatherForcastResponse GetWeatherForCity(string city)
        {
            try
            {
                //Get long and lat for city
                var longAndLatResponse = GetLongAndLatFromCity(city);

                if (longAndLatResponse.Success)
                    //get weather for city using long and lat
                    return GetWeatherForLongAndLat(longAndLatResponse);
                else
                    return new WeatherForcastResponse() { Success = false, Message = "Error Getting Geo Codes: " + longAndLatResponse.Message };
            }
            catch (Exception ex)
            {
                LoggerService.LogError("Error getting weather from city", "WeatherApiService.GetWeatherForCity", ex);
                return new WeatherForcastResponse() { Success = false, Message = "Generic Error: " + ex.Message };
            }
        }

        public WeatherForcastResponse GetWeatherForLongAndLat(GeoResponse longAndLat)
        {
            //Build request string
            var sb = new StringBuilder();
            sb.Append("weather?");
            sb.Append("lat=");
            sb.Append(longAndLat.Lattitude.ToString());
            sb.Append("&lon=");
            sb.Append(longAndLat.Longitude);
            sb.Append("&units=metric");

            //Get json from api
            var weatherApiResponse = GetApiResponse(sb.ToString(), ConfigurationManager.AppSettings.Get("WeatherApiBaseUrl"));

            //If successful then  return data
            if (weatherApiResponse.StatusCode == HttpStatusCode.OK)
            {
                //Convert to Class and return
                var result = JsonConvert.DeserializeObject<WeatherForcast>(weatherApiResponse.Result);
                return new WeatherForcastResponse() { Message = "OK", Success = true, Data = result };
            }
            else
            {
                LoggerService.LogError("There was an error getting weather " + weatherApiResponse.StatusCode, "WeatherApiService.GetWeatherForLongAndLat", null);
                return new WeatherForcastResponse() { Success = false, Message = "Problem returning data status:" + weatherApiResponse.StatusCode };
            }
        }

        private ApiResponse GetApiResponse(string requestString, string baseUrl)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var responseTask = client.GetAsync(string.Concat(requestString, "&appid=", ConfigurationManager.AppSettings.Get("ApiKey")));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    return new ApiResponse() { Success = true, Result = readTask.Result, StatusCode = HttpStatusCode.OK };
                }
                return new ApiResponse() { Success = true, Result = null, StatusCode = result.StatusCode };
            }

        }
        #endregion
    }
}