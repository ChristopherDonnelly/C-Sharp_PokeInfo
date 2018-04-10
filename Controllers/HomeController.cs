using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokeInfo.Models;

namespace PokeInfo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("pokemon/1");
        }

        [HttpGet]
        [Route("pokemon/{pokeid}")]
        public IActionResult QueryPoke(int pokeid)
        {
            var PokeInfo = new Dictionary<string, object>();
            
            WebRequest.GetPokemonDataAsync(pokeid, ApiResponse =>
                {
                    PokeInfo = ApiResponse;
                    List<string> typeList = new List<string>();

                    ViewData["name"] = PokeInfo["name"];
                    ViewData["height"] = PokeInfo["height"];
                    ViewData["weight"] = PokeInfo["weight"];
                    
                    JArray jArray = PokeInfo["types"] as JArray;
                    
                    foreach(JObject item in jArray)
                    { 
                        typeList.Add((string)item.GetValue("type")["name"]);
                    }

                    ViewData["type"] = string.Join(", ", typeList);
                    ViewData["img"] = ((JObject)PokeInfo["sprites"]).GetValue("front_default");
                }
            ).Wait();

            return View();
        }
    }
}
