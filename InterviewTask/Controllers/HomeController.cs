using InterviewTask.Models;
using InterviewTask.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace InterviewTask.Controllers
{
    public class HomeController : Controller
    {
        /*
         * Prepare your opening times here using the provided HelperServiceRepository class.       
         */
        public ILoggerService Logger = new LoggerService();
        public WeatherApiService WeatherApiService = new WeatherApiService();
        public IHelperServiceRepository Repo = new HelperServiceRepository();

        public HomeController()
        {
            Logger = new LoggerService();
            WeatherApiService = new WeatherApiService();
            Repo = new HelperServiceRepository();
        }


        public ActionResult Index()
        {
            if (Session["Models"] == null)
                Session["Models"] = Repo.Get();
            return View(Repo.Get());
        }

        public ActionResult UpdateWeather()
        {
            List<HelperServiceModel> models = (List<HelperServiceModel>)Session["Models"];
            foreach (var model in models)
            {
                try
                {
                    //Get Weather Info
                    var weatherResult = WeatherApiService.GetWeatherForCity(
                        model.Title.Remove(model.Title.IndexOf(" Helper Service")).Trim());

                    //If success set weather otherwise show error message
                    if (weatherResult.Success)
                    {
                        var sb = new StringBuilder();
                        sb.Append("Weather: ");
                        sb.Append(weatherResult.Data.Weather[0].Main);
                        sb.Append(" ");
                        sb.Append(weatherResult.Data.Weather[0].Description);
                        sb.Append("\r\n Temp:");
                        sb.Append(weatherResult.Data.Main.Temp);
                        sb.Append("c");
                        model.WeatherInfo = sb.ToString();
                    }
                    else
                        model.WeatherInfo = "Sorry no weather available";
                }     
                catch(Exception e)
                {
                    Logger.LogError(e.Message, "HomeController.UpdateWeatherText", e);
                }
                
            }
            
            Session["Models"] = models;
            return View("Index",models);
        }
    }
}